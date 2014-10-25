using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClaimsTodoApp.Models
{
    public class Todo
    {
        public int TodoId { get; set; }
        public string Owner { get; set; }
        public string Text { get; set; }
        public bool IsComplete { get; set; }
    }
}