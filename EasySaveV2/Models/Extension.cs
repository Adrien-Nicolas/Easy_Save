using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveV2.Models
{
    /// <summary>
    /// Stock the extensions
    /// </summary>
    class Extension
    {
        public string name { get; set; }
        public Extension()
        {
            this.name = "";
        }
        public override string ToString()
        {
            return $"{name}";

        }
    }
}
