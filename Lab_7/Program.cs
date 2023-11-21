using Microsoft.VisualBasic.FileIO;
using System;
using System.Diagnostics;
using System.IO;

namespace Lab_7
{
    public class Program
    {
        
        public static void Main()
        {
            Folder root = Folder.GetRootFolder(DriveInfo.GetDrives());

            DirectoryWork.OpenDirectories(root, root.Open());
        }
    }
}