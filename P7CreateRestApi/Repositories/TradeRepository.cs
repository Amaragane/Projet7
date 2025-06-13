﻿using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Repositories.Interfaces;

namespace P7CreateRestApi.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        private readonly LocalDbContext _context;

        public TradeRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trade>> GetAllAsync()
        {
            return await _context.Trades.ToListAsync();
        }

        public async Task<Trade?> GetByIdAsync(int id)
        {
            return await _context.Trades.FindAsync(id);
        }

        public async Task<Trade> CreateAsync(Trade trade)
        {
            trade.CreationDate = DateTime.Now;
            _context.Trades.Add(trade);
            await _context.SaveChangesAsync();
            return trade;
        }

        public async Task<Trade> UpdateAsync(Trade trade)
        {
            trade.RevisionDate = DateTime.Now;
            _context.Entry(trade).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return trade;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var trade = await _context.Trades.FindAsync(id);
            if (trade == null) return false;

            _context.Trades.Remove(trade);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Trades.AnyAsync(e => e.TradeId == id);
        }
    }
}
