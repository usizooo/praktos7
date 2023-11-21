using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public static class DirectoryWork
    {
        public static void OpenDirectories(DirectoryItem parent, List<DirectoryItem>? currentDirectory)
        {
            if (currentDirectory == null)
            {
                return;
            }

            int currentDirectoryItem = 0;
            bool window = true;

            while (window)
            {
                Console.Clear();
                DirectoryWork.PrintDirectoryItems(parent, currentDirectory, currentDirectoryItem);

                var action = Console.ReadKey();
                switch (action.Key)
                {
                    case ConsoleKey.UpArrow:
                        currentDirectoryItem = (currentDirectoryItem - 1) % currentDirectory.Count < 0
                            ? currentDirectory.Count - 1
                            : (currentDirectoryItem - 1) % currentDirectory.Count;
                        break;
                    case ConsoleKey.DownArrow:
                        currentDirectoryItem = (currentDirectoryItem + 1) % currentDirectory.Count;
                        break;
                    case ConsoleKey.Enter:
                        OpenDirectories(currentDirectory[currentDirectoryItem], currentDirectory[currentDirectoryItem].Open());
                        break;
                    case ConsoleKey.Escape:
                        window = false;
                        break;
                    case ConsoleKey.X:
                        CreateNewFolder((Folder)parent, currentDirectory);
                        break;
                    case ConsoleKey.Y:
                        CreateNewFile((Folder)parent, currentDirectory);
                        break;
                    case ConsoleKey.Z:
                        DeleteFolder((Folder)parent, currentDirectory);
                        break;
                    case ConsoleKey.W:
                        DeleteFile((Folder)parent, currentDirectory);
                        break;
                    default:
                        DirectoryWork.ErrorMassage("Некорректный ввод, попытайтесь снова", currentDirectory.Count);
                        break;
                }
            }
        }

        public static void CreateNewFolder(Folder parent, List<DirectoryItem> currentDirectory)
        {
            if (parent.IsRoot || parent.IsDrive)
            {
                DirectoryWork.ErrorMassage("Здесь нельзя создать папку", currentDirectory.Count);
                return;
            }
            string? folderName = SetValueForAction("Введите название папки", currentDirectory.Count);
            if (folderName == null)
                throw new ArgumentException("Folder name is null");
            DirectoryInfo newFolder = Directory.CreateDirectory(parent.FullName + "\\" + folderName);
            parent.Add(new Folder(newFolder.FullName));
        }

        public static void DeleteFolder(Folder parent, List<DirectoryItem> currentDirectory)
        {
            if (parent.IsRoot || parent.IsDrive)
            {
                DirectoryWork.ErrorMassage("Здесь нельзя удалять элементы этой директории", currentDirectory.Count);
                return;
            }
            string? folderName = SetValueForAction("Введите название элемента директории", currentDirectory.Count);
            if (folderName == null)
                throw new ArgumentException("Folder name is null or not found");

            string fullFolderName = parent.FullName + "\\" + folderName;

            DirectoryItem? deletedFolder = currentDirectory
                .Where(x => x.FullName == fullFolderName)
                .FirstOrDefault();

            if (deletedFolder == null)
                throw new ArgumentException("Folder is null");

            parent.Remove(deletedFolder);

            Directory.Delete(fullFolderName, true);
        }

        public static void CreateNewFile(Folder parent, List<DirectoryItem> currentDirectory)
        {
            if (parent.IsRoot || parent.IsDrive)
            {
                DirectoryWork.ErrorMassage("Здесь нельзя создать файл", currentDirectory.Count);
                return;
            }
            string? fileName = SetValueForAction("Введите название файла", currentDirectory.Count);
            if (fileName == null)
                throw new ArgumentException("File name is null");
            string fullFileName = parent.FullName + "\\" + fileName;
            using (FileStream newFile = System.IO.File.Create(fullFileName))
            {
                parent.Add(new File(newFile.Name));
            }
        }

        public static void DeleteFile(Folder parent, List<DirectoryItem> currentDirectory)
        {
            if (parent.IsRoot || parent.IsDrive)
            {
                DirectoryWork.ErrorMassage("Здесь нельзя удалять элементы этой директории", currentDirectory.Count);
                return;
            }
            string? fileName = SetValueForAction("Введите название файла директории", currentDirectory.Count);
            if (fileName == null)
                throw new ArgumentException("File name is null or not found");
            string fullFileName = parent.FullName + "\\" + fileName;
            DirectoryItem? deletedFolder = currentDirectory
                .Where(x => x.FullName == fullFileName)
                .FirstOrDefault();

            if (deletedFolder == null)
                throw new ArgumentException("File is null");
            parent.Remove(deletedFolder);
            System.IO.File.Delete(fullFileName);
        }

        public static void PrintDirectoryItems(DirectoryItem parent, List<DirectoryItem> directoryItems, int currentItem)
        {
            if (directoryItems == null)
            {
                return;
            }

            PrintActions();

            if (((Folder)parent).IsRoot)
            {
                Console.WriteLine("\tЭтот компьютер");
            }
            else if (((Folder)parent).IsDrive)
            {
                Console.WriteLine("\tДиск " + ((Folder)parent).Name);
            }
            else
            {
                Console.WriteLine("\tПапка " + ((Folder)parent).Name);
            }

            Console.WriteLine(new string('-', Console.WindowWidth));
            for (int i = 0; i < directoryItems.Count; i++)
            {
                if (i == currentItem)
                {
                    Console.WriteLine($"->\t{directoryItems[i]}");
                }
                else
                {
                    Console.WriteLine($"\t{directoryItems[i]}");
                }
            }
            Console.SetCursorPosition(0, currentItem + 8);
        }

        public static string? SetValueForAction(string actionInfo, int cursorShift)
        {
            Console.SetCursorPosition(0, cursorShift + 9);
            Console.WriteLine(new string('-', Console.WindowWidth));
            Console.WriteLine("\t" + actionInfo);
            string? value = Console.ReadLine();
            Console.WriteLine(new string('-', Console.WindowWidth));
            return value;
        }

        public static void PrintActions()
        {
            Console.WriteLine(new string('-', Console.WindowWidth));
            Console.WriteLine("\tX - создать папку");
            Console.WriteLine("\tY - создать файл");
            Console.WriteLine("\tZ - удалить папку");
            Console.WriteLine("\tW - удалить файл");
            Console.WriteLine(new string('-', Console.WindowWidth));
        }

        public static void ErrorMassage(string errorInfo, int cursorShift)
        {
            Console.SetCursorPosition(0, cursorShift + 9);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n" + errorInfo);
            Console.ReadKey();
            Console.ResetColor();
        }
    }
}