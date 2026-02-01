using System;
using System.Collections.Generic;
using System.Text;

namespace ReadingProgress.Data.Models
{

    public enum ReadingStatus
    {
        NotStarted = 0,
        Reading,
        Completed,
        OnHold,
        Dropped
    }

    public class ReadingList
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ManhwaId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        public decimal? Score { get; set; }

        public ReadingStatus Status { get; set; }
    }
}
