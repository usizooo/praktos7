using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;

namespace Lab_7
{
    public class Folder : DirectoryItem
    {
        public readonly bool IsRoot;
        public readonly bool IsDrive;
        public string Name { get; private set; } = string.Empty;
        public DateTime CreationTime { get; private set; }
        public List<DirectoryItem> CurrentDirectory { get; private set; }

        public Folder(string fullName, bool isRoot = false, bool isDrive = false, List<DirectoryItem>? directory = null)
            : base(fullName)
        {
            IsRoot = isRoot;
            IsDrive = isDrive;
            CurrentDirectory = directory ?? new List<DirectoryItem>();
            if (IsRoot)
                return;

            if (IsDrive)
            {
                Name = FullName;
                return;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(FullName);
            if (directoryInfo.Exists)
            {
                Name = directoryInfo.Name;
                CreationTime = directoryInfo.CreationTime;
            }
            else
                throw new ArgumentException();
        }

        public void SetDirectory()
        {
            foreach (var folder in Directory.GetDirectories(FullName))
                CurrentDirectory.Add(new Folder(folder));
            foreach (var file in Directory.GetFiles(FullName))
                CurrentDirectory.Add(new File(file));
        }

        public override List<DirectoryItem>? Open()
        {
            if (CurrentDirectory.Count == 0)
                SetDirectory();

            return CurrentDirectory;
        }

        public static Folder GetRootFolder(DriveInfo[] drives)
        {
            List<DirectoryItem> rootDirectory = new List<DirectoryItem>();
            foreach (var drive in drives)
            {
                rootDirectory.Add(new Folder(drive.Name, false, true));
            }

            return new Folder(string.Empty, true, false, rootDirectory);
        }

        public override string ToString()
        {
            double totalSize = -1;
            double totalFreeSpace = -1;
            if (IsDrive)
            {
                DriveInfo driveInfo = new DriveInfo(FullName);
                totalSize = driveInfo.TotalSize / Math.Pow(2, 30);
                totalFreeSpace = driveInfo.TotalFreeSpace / Math.Pow(2, 30);
            }

            return IsDrive
                ? $"{Name,-5}свободно {string.Format("{0:f2}", totalFreeSpace)} Гб из {string.Format("{0:f2}", totalSize)} Гб"
                : $"{Name,-40}{CreationTime,-25}";
        }

        public override void Add(DirectoryItem component) => CurrentDirectory.Add(component);
        public override void Remove(DirectoryItem component) => CurrentDirectory.Remove(component);
    }
}