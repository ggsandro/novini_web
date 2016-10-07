using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Novini.Models
{
    public class NewsModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
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
