using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Model
{
    public class News
    {
        [Key]
        public long Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
