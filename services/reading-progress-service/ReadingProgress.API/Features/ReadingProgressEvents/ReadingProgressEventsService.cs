using ReadingProgress.Data;

namespace ReadingProgress.API.Features.ReadingProgressEvents
{
    public class ReadingProgressEventsService
    {
        private readonly AppDbContext _dbContext;
        public ReadingProgressEventsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // add new reading progress event
        public async Task<bool> AddReadingProgressEvent(string userId, string manhwaId, decimal chapterNumber)
        {
            var readingProgressEvent = new Data.Models.ReadingProgressEvent
            {
                UserId = userId,
                ManhwaId = manhwaId,
                Chapter = chapterNumber,
                EventDate = DateTime.UtcNow
            };
            _dbContext.ReadingProgressEvents.Add(readingProgressEvent);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
