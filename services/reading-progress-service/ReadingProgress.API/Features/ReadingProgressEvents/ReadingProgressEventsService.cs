using ReadingProgress.API.Features.Shared;
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
        public async Task<ServiceResult> AddReadingProgressEvent(string userId, string manhwaId, decimal chapterNumber)
        {

            // check if the same event already exists (same userId, manhwaId, chapterNumber)
            var existingEvent = 
                _dbContext.ReadingProgressEvents
                .FirstOrDefault(e => e.UserId == userId && e.ManhwaId == manhwaId && e.Chapter == chapterNumber);

            if (existingEvent != null) 
                return ServiceResult.Failure("Reading Progress Event for chapter already exists", ErrorType.BadRequest);

            var readingProgressEvent = new Data.Models.ReadingProgressEvent
            {
                UserId = userId,
                ManhwaId = manhwaId,
                Chapter = chapterNumber,
                EventDate = DateTime.UtcNow
            };
            _dbContext.ReadingProgressEvents.Add(readingProgressEvent);
            var result = await _dbContext.SaveChangesAsync();

            var serviceResult = new ServiceResult
            {
                IsSuccess = result > 0
            };

            return serviceResult;
        }
    }
}
