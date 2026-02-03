using ReadingProgress.Data.Models;

namespace ReadingProgress.API.Features.ReadingLists.DTOs
{
    public class UpdateReadingListRequest
    {
        public string ManhwaId { get; set; }
        public ReadingStatus Status { get; set; }
    }
}
