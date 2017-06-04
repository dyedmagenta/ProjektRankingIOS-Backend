using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Model
{
    public class Tournament
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
