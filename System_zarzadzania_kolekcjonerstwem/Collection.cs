using System.Collections.Generic;

namespace System_zarzadzania_kolekcjonerstwem
{
    public struct Collection
    {
        public string Name { get; set; }
        public List<string> Items { get; set; }

        public Collection(string name)
        {
            Name = name;
            Items = new List<string>();
        }
    }
}
