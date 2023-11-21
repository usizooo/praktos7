using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public abstract class DirectoryItem
    {
        public string FullName { get; }
        public DirectoryItem(string fullName)
        {
            FullName = fullName;
        }

        public abstract List<DirectoryItem>? Open();
        public abstract void Add(DirectoryItem component);
        public abstract void Remove(DirectoryItem component);
    }
}
