using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompare2._0.Models
{
    class FCompare
    { 
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Hash { get; set; }
        public long Sise { get; set; }
        public DateTime Date { get; set; }
        public bool exist { get; set; }
        public bool delite { get; set; }
        public bool renamed { get; set; }
        public bool replased { get; set; }
        public bool changed { get; set; }
    }
}
