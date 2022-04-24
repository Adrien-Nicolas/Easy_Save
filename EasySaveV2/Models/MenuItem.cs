using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Models
{
    /// <summary>
    /// MenuItem class, allowing to create and options for one menu
    /// </summary>
    class MenuItem
    {
        public string Texte { get; set; }
        public Action Action { get; set; }
        
        /// <summary>
        /// Constructor of menuItem
        /// </summary>
        /// <param name="Texte">string</param>
        /// <param name="Action">Action</param>
        public MenuItem(string Texte, Action Action)
        {
            this.Texte = Texte;
            this.Action = Action;
        }
    }
}
