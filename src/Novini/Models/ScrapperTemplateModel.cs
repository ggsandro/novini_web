﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Novini.Models
{
    public class ScrapperTemplateModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string TitleHtmlSelector { get; set; }
        public string LinkHtmlSelector { get; set; }
    }
}
