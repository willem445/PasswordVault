using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Models
{
    class Category
    {
        public List<string> Categories { get; set; }

        public Category()
        {
            Categories = new List<string>();
        }
    }
}
