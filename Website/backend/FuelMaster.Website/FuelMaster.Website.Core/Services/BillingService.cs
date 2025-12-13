using FuelMaster.Website.Core.Entities;
using FuelMaster.Website.Core.Enums;
using FuelMaster.Website.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FuelMaster.Website.Core.Services;

public class BillingService : IBillingService
{
    private readonly IPaymentCardRepository _paymentCardRepository;
    private readonly IBillingHistoryRepository _billingHistoryRepository;
    private readonly IUserSubscriptionRepository _subscriptionRepository;
    private readonly IPaymentProviderService _paymentProviderService;
    private readonly ILogger<BillingService> _logger;

    public BillingService(
        IPaymentCardRepository paymentCardRepository,
        IBillingHistoryRepository billingHistoryRepository,
        IUserSubscriptionRepository subscriptionRepository,
        IPaymentProviderService paymentProviderService,
        ILogger<BillingService> logger)
    {
        _paymentCardRepository = paymentCardRepository;
        _billingHistoryRepository = billingHistoryRepository;
        _subscriptionRepository = subscriptionRepository;
        _paymentProviderService = paymentProviderService;
        _logger = logger;
    }

    public async Task<PaymentCard> AddPaymentCardAsync(string userId, PaymentCardInfo cardInfo)
    {
        // Validate card
        var isValid = await _paymentProviderService.ValidateCardAsync(cardInfo);
        if (!isValid)
        {
            throw new ArgumentException("Invalid card information");
        }

        // Create payment token
        var token = await _paymentProviderService.CreatePaymentTokenAsync(cardInfo);

        // Extract last 4 digits
        var lastFour = cardInfo.CardNumber.Length > 4
            ? cardInfo.CardNumber.Substring(cardInfo.CardNumber.Length - 4)
            : "****";

        // Determine card brand (simplified - in production, use a proper card detection library)
        var cardBrand = DetermineCardBrand(cardInfo.CardNumber);

        // Check if this should be the default card (first card for user)
        var existingCards = await _paymentCardRepository.GetCardsByUserIdAsync(userId);
        var isDefault = !existingCards.Any();

        var paymentCard = new PaymentCard
        {
            UserId = userId,
            CardLastFour = lastFour,
            CardBrand = cardBrand,
            ExpiryMonth = int.Parse(cardInfo.ExpiryMonth),
            ExpiryYear = int.Parse(cardInfo.ExpiryYear),
            IsDefault = isDefault,
            Token = token, // Encrypted token from payment provider
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await _paymentCardRepository.AddAsync(paymentCard);
    }

    public async Task<IEnumerable<PaymentCard>> GetUserPaymentCardsAsync(string userId)
    {
        return await _paymentCardRepository.GetCardsByUserIdAsync(userId);
    }

    public async Task<bool> DeletePaymentCardAsync(string userId, Guid cardId)
    {
        var card = await _paymentCardRepository.GetByIdAsync(cardId);
        if (card == null || card.UserId != userId)
        {
            return false;
        }

        await _paymentCardRepository.DeleteAsync(card);

        // If this was the default card, set another card as default
        if (card.IsDefault)
        {
            var remainingCards = await _paymentCardRepository.GetCardsByUserIdAsync(userId);
            var firstCard = remainingCards.FirstOrDefault();
            if (firstCard != null)
            {
                await _paymentCardRepository.SetDefaultCardAsync(userId, firstCard.Id);
            }
        }

        return true;
    }

    public async Task<PaymentCard?> SetDefaultCardAsync(string userId, Guid cardId)
    {
        var card = await _paymentCardRepository.GetByIdAsync(cardId);
        if (card == null || card.UserId != userId)
        {
            return null;
        }

        await _paymentCardRepository.SetDefaultCardAsync(userId, cardId);
        card.IsDefault = true;
        card.UpdatedAt = DateTime.UtcNow;
        await _paymentCardRepository.UpdateAsync(card);

        return card;
    }

    public async Task<BillingHistory> ProcessBillingAsync(Guid subscriptionId)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId);
        if (subscription == null)
        {
            throw new ArgumentException("Subscription not found", nameof(subscriptionId));
        }

        // Check if subscription is free
        if (subscription.Plan.IsFree)
        {
            throw new InvalidOperationException("Cannot process billing for free plan");
        }

        // Get default payment card
        var defaultCard = await _paymentCardRepository.GetDefaultCardByUserIdAsync(subscription.UserId);
        if (defaultCard == null)
        {
            throw new InvalidOperationException("No payment card found for user");
        }

        // Process payment
        var paymentRequest = new PaymentRequest
        {
            Amount = subscription.Plan.Price,
            Currency = "USD",
            PaymentToken = defaultCard.Token,
            Description = $"Subscription payment for {subscription.Plan.Name}",
            Metadata = new Dictionary<string, string>
            {
                { "subscriptionId", subscription.Id.ToString() },
                { "planId", subscription.PlanId.ToString() },
                { "userId", subscription.UserId }
            }
        };

        var paymentResult = await _paymentProviderService.ProcessPaymentAsync(paymentRequest);

        // Create billing history
        var billingHistory = new BillingHistory
        {
            UserSubscriptionId = subscriptionId,
            Amount = subscription.Plan.Price,
            Status = paymentResult.Success ? BillingStatus.Completed : BillingStatus.Failed,
            PaymentDate = DateTime.UtcNow,
            TransactionId = paymentResult.TransactionId,
            PaymentProviderResponse = paymentResult.ProviderResponse != null
                ? JsonSerializer.Serialize(paymentResult.ProviderResponse)
                : null,
            CreatedAt = DateTime.UtcNow
        };

        // Update subscription next billing date if payment succeeded
        if (paymentResult.Success)
        {
            subscription.NextBillingDate = CalculateNextBillingDate(subscription.Plan);
            subscription.UpdatedAt = DateTime.UtcNow;
            await _subscriptionRepository.UpdateAsync(subscription);
        }

        return await _billingHistoryRepository.AddAsync(billingHistory);
    }

    public async Task<IEnumerable<BillingHistory>> GetBillingHistoryAsync(string userId)
    {
        return await _billingHistoryRepository.GetBillingHistoryByUserIdAsync(userId);
    }

    public async Task<bool> RefundBillingAsync(Guid billingHistoryId)
    {
        var billingHistory = await _billingHistoryRepository.GetByIdAsync(billingHistoryId);
        if (billingHistory == null || billingHistory.Status != BillingStatus.Completed)
        {
            return false;
        }

        if (string.IsNullOrEmpty(billingHistory.TransactionId))
        {
            return false;
        }

        var refundResult = await _paymentProviderService.RefundPaymentAsync(
            billingHistory.TransactionId,
            billingHistory.Amount);

        if (refundResult)
        {
            billingHistory.Status = BillingStatus.Refunded;
            await _billingHistoryRepository.UpdateAsync(billingHistory);
        }

        return refundResult;
    }

    private string DetermineCardBrand(string cardNumber)
    {
        // Simplified card brand detection
        // In production, use a proper library like CreditCardValidator
        if (string.IsNullOrWhiteSpace(cardNumber))
            return "Unknown";

        var firstDigit = cardNumber[0];
        return firstDigit switch
        {
            '4' => "Visa",
            '5' => "Mastercard",
            '3' => "American Express",
            '6' => "Discover",
            _ => "Unknown"
        };
    }

    private DateTime? CalculateNextBillingDate(SubscriptionPlan plan)
    {
        if (plan.IsFree)
        {
            return null;
        }

        return plan.BillingCycle == BillingCycle.Monthly
            ? DateTime.UtcNow.AddMonths(1)
            : DateTime.UtcNow.AddYears(1);
    }
}

