using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoApi.Models
{
    public class Todo
    {
        public int TodoId { get; set; }
        public string Owner { get; set; }
        public string Text { get; set; }
    }
}