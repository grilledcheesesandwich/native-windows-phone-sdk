using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esmann.WP.Common.Json
{
    public class JsonNameAttribute : Attribute
    {
        public string Name { get; private set; }
        public JsonNameAttribute()
            : this(null)
        {

        }
        public JsonNameAttribute(string name)
        {
            Name = name;
        }
    }
}
