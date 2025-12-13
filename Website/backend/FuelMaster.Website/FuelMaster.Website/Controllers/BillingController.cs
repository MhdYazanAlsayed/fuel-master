using FuelMaster.Website.Attributes;
using FuelMaster.Website.Core.Interfaces;
using FuelMaster.Website.DTOs.Requests;
using FuelMaster.Website.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FuelMaster.Website.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[RequireActiveTenantAndSubscription]
public class BillingController : ControllerBase
{
    private readonly IBillingService _billingService;
    private readonly ILogger<BillingController> _logger;

    public BillingController(
        IBillingService billingService,
        ILogger<BillingController> logger)
    {
        _billingService = billingService;
        _logger = logger;
    }

    [HttpPost("payment-cards")]
    public async Task<IActionResult> AddPaymentCard([FromBody] AddPaymentCardRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var cardInfo = new PaymentCardInfo
            {
                CardNumber = request.CardNumber,
                ExpiryMonth = request.ExpiryMonth.ToString("D2"),
                ExpiryYear = request.ExpiryYear.ToString(),
                Cvv = request.Cvv,
                CardholderName = request.CardholderName
            };

            var paymentCard = await _billingService.AddPaymentCardAsync(userId, cardInfo);

            var response = new PaymentCardResponse
            {
                Id = paymentCard.Id,
                CardLastFour = paymentCard.CardLastFour,
                CardBrand = paymentCard.CardBrand,
                ExpiryMonth = paymentCard.ExpiryMonth,
                ExpiryYear = paymentCard.ExpiryYear,
                IsDefault = paymentCard.IsDefault,
                CreatedAt = paymentCard.CreatedAt
            };

            _logger.LogInformation("User {UserId} added payment card {CardId}", userId, paymentCard.Id);

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding payment card");
            return StatusCode(500, new { message = "An error occurred while adding the payment card" });
        }
    }

    [HttpGet("payment-cards")]
    public async Task<IActionResult> GetPaymentCards()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var cards = await _billingService.GetUserPaymentCardsAsync(userId);

            var response = cards.Select(c => new PaymentCardResponse
            {
                Id = c.Id,
                CardLastFour = c.CardLastFour,
                CardBrand = c.CardBrand,
                ExpiryMonth = c.ExpiryMonth,
                ExpiryYear = c.ExpiryYear,
                IsDefault = c.IsDefault,
                CreatedAt = c.CreatedAt
            });

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment cards");
            return StatusCode(500, new { message = "An error occurred while retrieving payment cards" });
        }
    }

    [HttpDelete("payment-cards/{cardId}")]
    public async Task<IActionResult> DeletePaymentCard(Guid cardId)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var result = await _billingService.DeletePaymentCardAsync(userId, cardId);
            if (!result)
            {
                return NotFound(new { message = "Payment card not found" });
            }

            _logger.LogInformation("User {UserId} deleted payment card {CardId}", userId, cardId);

            return Ok(new { message = "Payment card deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting payment card {CardId}", cardId);
            return StatusCode(500, new { message = "An error occurred while deleting the payment card" });
        }
    }

    [HttpPut("payment-cards/{cardId}/set-default")]
    public async Task<IActionResult> SetDefaultCard(Guid cardId)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var card = await _billingService.SetDefaultCardAsync(userId, cardId);
            if (card == null)
            {
                return NotFound(new { message = "Payment card not found" });
            }

            var response = new PaymentCardResponse
            {
                Id = card.Id,
                CardLastFour = card.CardLastFour,
                CardBrand = card.CardBrand,
                ExpiryMonth = card.ExpiryMonth,
                ExpiryYear = card.ExpiryYear,
                IsDefault = card.IsDefault,
                CreatedAt = card.CreatedAt
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting default card {CardId}", cardId);
            return StatusCode(500, new { message = "An error occurred while setting the default card" });
        }
    }

    [HttpPost("process/{subscriptionId}")]
    public async Task<IActionResult> ProcessBilling(Guid subscriptionId)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var billingHistory = await _billingService.ProcessBillingAsync(subscriptionId);

            var response = new BillingHistoryResponse
            {
                Id = billingHistory.Id,
                UserSubscriptionId = billingHistory.UserSubscriptionId,
                Amount = billingHistory.Amount,
                Status = billingHistory.Status.ToString(),
                PaymentDate = billingHistory.PaymentDate,
                TransactionId = billingHistory.TransactionId,
                CreatedAt = billingHistory.CreatedAt
            };

            _logger.LogInformation("Processed billing for subscription {SubscriptionId}", subscriptionId);

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing billing for subscription {SubscriptionId}", subscriptionId);
            return StatusCode(500, new { message = "An error occurred while processing billing" });
        }
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetBillingHistory()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var history = await _billingService.GetBillingHistoryAsync(userId);

            var response = history.Select(h => new BillingHistoryResponse
            {
                Id = h.Id,
                UserSubscriptionId = h.UserSubscriptionId,
                Amount = h.Amount,
                Status = h.Status.ToString(),
                PaymentDate = h.PaymentDate,
                TransactionId = h.TransactionId,
                CreatedAt = h.CreatedAt
            });

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving billing history");
            return StatusCode(500, new { message = "An error occurred while retrieving billing history" });
        }
    }

    [HttpPost("refund/{billingHistoryId}")]
    public async Task<IActionResult> RefundBilling(Guid billingHistoryId)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var result = await _billingService.RefundBillingAsync(billingHistoryId);
            if (!result)
            {
                return BadRequest(new { message = "Unable to process refund" });
            }

            _logger.LogInformation("Refund processed for billing history {BillingHistoryId}", billingHistoryId);

            return Ok(new { message = "Refund processed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing refund for billing history {BillingHistoryId}", billingHistoryId);
            return StatusCode(500, new { message = "An error occurred while processing the refund" });
        }
    }
}

