using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveV2.Models
{
    class PriorityFile
    {
        public string name { get; set; }
        public PriorityFile()
        {
            this.name = "";
        }
        public override string ToString()
        {
            return $"{name}";
        }
    }
}
