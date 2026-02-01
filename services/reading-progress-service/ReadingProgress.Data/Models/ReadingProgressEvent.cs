using System;
using System.Collections.Generic;
using System.Text;

namespace ReadingProgress.Data.Models
{
    public class ReadingProgressEvent
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ManhwaId { get; set; }

        public decimal Chapter { get; set; } // chapter they're on not chapters read

        public DateTime EventDate { get; set; }

    }
}
