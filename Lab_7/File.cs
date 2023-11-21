using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class File : DirectoryItem
    {
        public string Name { get; private set; } = string.Empty;
        public DateTime CreationTime { get; private set; }
        public string FileExtensions { get; private set; } = string.Empty;
        public long Size { get; private set; }

        public File(string fullName) : base(fullName)
        {
            FileInfo fileInfo = new FileInfo(FullName);
            if (fileInfo.Exists)
            {
                Name = fileInfo.Name;
                CreationTime = fileInfo.CreationTime;
                try
                {
                    FileExtensions = fileInfo.Name.Substring(fileInfo.Name.LastIndexOf('.'));
                }
                catch
                {
                    FileExtensions = ".";
                }
                Size = fileInfo.Length;
            }
            else
                throw new ArgumentException();
        }

        public override List<DirectoryItem>? Open()
        {

            Process.Start(new ProcessStartInfo { FileName = FullName, UseShellExecute = true });
            return null;
        }

        public override string ToString()
        {
            return $"{Name, -40}{CreationTime, -25}{GetFormatSize(), -10}{FileExtensions, -10}";
        }

        private string GetFormatSize()
        {
            List<string> formats = new List<string>() { "Б", "Кб", "Мб", "Гб" };
            int currentFormat = 0;
            double size = Size;
            
            while (size >= 1024)
            {
                size /= 1024;
                currentFormat++;
            }

            return string.Format("{0:f2} {1}", size, formats[currentFormat]);
        }

        public override void Add(DirectoryItem component)
        {
            throw new InvalidOperationException();
        }

        public override void Remove(DirectoryItem component)
        {
            throw new InvalidOperationException();
        }
    }
}