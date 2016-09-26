using System;
using System.Collections.Generic;
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
        public bool IsApproved { get; set; }
    }
}
