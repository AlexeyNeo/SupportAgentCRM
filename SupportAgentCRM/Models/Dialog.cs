﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportAgentCRM.Models
{
    public class Dialog
    {
        public string ID { get; set; }
        public string state { get; set; }
        public DateTimeOffset begin { get; set; }
        public DateTimeOffset end { get; set; }
        public string error { get; set; }
    }
}