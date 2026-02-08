using ReadingProgress.Data.Models;

namespace ReadingProgress.API.Features.ReadingLists.DTOs
{

    public class GetReadingList
    {
        public string ManhwaId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        public decimal? Score { get; set; }

        public ReadingStatus Status { get; set; }
    }
}
