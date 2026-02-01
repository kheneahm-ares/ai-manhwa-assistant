using Microsoft.EntityFrameworkCore;
using ReadingProgress.Data;
using ReadingProgress.Data.Models;

namespace ReadingProgress.API.Features.ReadingLists
{
    public class ReadingListsService
    {
        private readonly AppDbContext _dbContext;

        public ReadingListsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // add new reading list
        public async Task<bool> AddReadingList(string userId, string manhwaId)
        {

            // check if exists, if so just return true
            var existing = await _dbContext.ReadingLists
                .FirstOrDefaultAsync(rl => rl.UserId == userId && rl.ManhwaId == manhwaId);

            if (existing != null) return true;

            var readingList = new ReadingList
            {
                UserId = userId,
                ManhwaId = manhwaId,
                StartDate = DateTime.UtcNow,
                Status = ReadingStatus.Reading
            };

            _dbContext.ReadingLists.Add(readingList);

            var result = await _dbContext.SaveChangesAsync();

            return result > 0;

        }
    }
}
