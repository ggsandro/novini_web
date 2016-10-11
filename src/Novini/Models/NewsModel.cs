using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Novini.Models
{
    public class NewsModel
    {
        public int Id { get; set; }
        [Required,MaxLength(80)]
        public string Title { get; set; }
        [Required, MaxLength(130)]
        public string Content { get; set; }
        [Required, MaxLength(200),DataType(DataType.Url)]
        public string Url { get; set; }
        public DateTime TimeStamp { get; set; }
        public string TimeStampString
        {
            get
            {
                return TimeStamp.ToString("dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
            }
        }
        public bool IsApproved { get; set; }
    }
}
