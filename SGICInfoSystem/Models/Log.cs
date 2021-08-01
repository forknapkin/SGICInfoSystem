using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGICInfoSystem.Models
{
    public class Log
    {
        public int Id { get;  set; }
        public string Action { get; set; }
        public DateTime DateTimeAction { get; set; }
        public string User { get; set; }
    }
}