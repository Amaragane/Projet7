using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Repositories.Interfaces;

namespace P7CreateRestApi.Repositories
{
    public class BidListRepository : IBidListRepository
    {
        private readonly LocalDbContext _context;

        public BidListRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BidList>> GetAllAsync()
        {
            return await _context.BidLists.ToListAsync();
        }

        public async Task<BidList> GetByIdAsync(int id)
        {
            if(await _context.BidLists.AnyAsync(e => e.BidListId == id))
            {

                return await _context.BidLists.FindAsync(id);
            }
            return null!; // Return null if not found
        }

        public async Task<BidList> CreateAsync(BidList bidList)
        {
            bidList.CreationDate = DateTime.Now;
            _context.BidLists.Add(bidList);
            await _context.SaveChangesAsync();
            return bidList;
        }

        public async Task<BidList> UpdateAsync(BidList bidList)
        {
            bidList.RevisionDate = DateTime.Now;
            _context.Entry(bidList).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return bidList;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bidList = await _context.BidLists.FindAsync(id);
            if (bidList == null) return false;

            _context.BidLists.Remove(bidList);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.BidLists.AnyAsync(e => e.BidListId == id);
        }
    }
}
