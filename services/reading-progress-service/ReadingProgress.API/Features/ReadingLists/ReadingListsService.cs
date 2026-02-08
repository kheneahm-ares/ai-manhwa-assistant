using Microsoft.EntityFrameworkCore;
using ReadingProgress.API.Features.ReadingLists.DTOs;
using ReadingProgress.API.Features.Shared;
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

        public async Task<ServiceResult<List<GetReadingList>>> GetReadingList(string userId)
        {
            var userReadingLists = await _dbContext.ReadingLists
                .Where(rl => rl.UserId == userId)
                .ToListAsync();

            var response = userReadingLists.Select(rl => new GetReadingList 
            {
                ManhwaId = rl.ManhwaId,
                StartDate = rl.StartDate,
                CompletedDate = rl.CompletedDate,
                Score = rl.Score,
                Status = rl.Status
            }).ToList();

            return ServiceResult<List<GetReadingList>>.Success(response);

        }

        // add new reading list
        public async Task<ServiceResult> AddReadingList(string userId, string manhwaId)
        {

            // check if exists, if so just return true
            var existing = await _dbContext.ReadingLists
                .FirstOrDefaultAsync(rl => rl.UserId == userId && rl.ManhwaId == manhwaId);

            if (existing != null) return ServiceResult.Failure("Reading List exists", ErrorType.BadRequest);

            var readingList = new ReadingList
            {
                UserId = userId,
                ManhwaId = manhwaId,
                StartDate = DateTime.UtcNow,
                Status = ReadingStatus.Reading
            };

            _dbContext.ReadingLists.Add(readingList);

            var result = await _dbContext.SaveChangesAsync();

            var serviceResult = new ServiceResult
            {
                IsSuccess = result > 0
            };

            return serviceResult;

        }

        public async Task<ServiceResult> UpdateReadingList(string userId, string manhwaId, ReadingStatus status)
        {
            var existing = await _dbContext.ReadingLists
                            .FirstOrDefaultAsync(rl => rl.UserId == userId && rl.ManhwaId == manhwaId);


            // if not exists 
            if (existing == null) return ServiceResult.Failure("Reading List does not exist", ErrorType.NotFound);
            // if no change, return success
            if (existing.Status == status)
            {
                return new ServiceResult { IsSuccess = true };
            }

            if (status == ReadingStatus.Completed)
            {
                existing.CompletedDate = DateTime.UtcNow;
            }

            // if changing from completed to something else, remove completed date
            if (existing.Status == ReadingStatus.Completed && status != ReadingStatus.Completed)
            {
                existing.CompletedDate = null;
            }

            existing.Status = status;
            var result = await _dbContext.SaveChangesAsync();

            var serviceResult = new ServiceResult
            {
                IsSuccess = result > 0
            };

            return serviceResult;
        }
    }
}
