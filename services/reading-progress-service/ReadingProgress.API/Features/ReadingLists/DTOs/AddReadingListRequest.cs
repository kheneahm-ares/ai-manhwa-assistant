using System.ComponentModel.DataAnnotations;

namespace ReadingProgress.API.Features.ReadingLists.DTOs
{
    public class AddReadingListRequest
    {
        [Required]
        public string ManhwaId { get; set; }
    }
}
