using FuelMaster.Website.Core.Entities;
using FuelMaster.Website.Core.Interfaces;
using FuelMaster.Website.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.Website.Infrastructure.Repositories;

public class PaymentCardRepository : Repository<PaymentCard>, IPaymentCardRepository
{
    public PaymentCardRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PaymentCard>> GetCardsByUserIdAsync(string userId)
    {
        return await _dbSet
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.IsDefault)
            .ThenByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<PaymentCard?> GetDefaultCardByUserIdAsync(string userId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.UserId == userId && c.IsDefault);
    }

    public async Task SetDefaultCardAsync(string userId, Guid cardId)
    {
        // Remove default from all cards
        var allCards = await _dbSet
            .Where(c => c.UserId == userId)
            .ToListAsync();

        foreach (var card in allCards)
        {
            card.IsDefault = card.Id == cardId;
            card.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }
}

