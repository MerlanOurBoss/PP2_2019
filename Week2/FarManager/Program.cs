﻿using System;
using System.Collections.Generic;
using System.IO;

namespace FarManager
{
    class Folder
    {
        int selectedIndex;
        FileSystemInfo[] contents;

        public Folder(FileSystemInfo[] fileSystemInfos)
        {
            selectedIndex = 0;
            contents = fileSystemInfos;
        }

        public void Up()
        {
            if (selectedIndex == 0)
            {
                selectedIndex = contents.Length - 1;
            } else
            {
                selectedIndex--;
            }
        }

        public void Down()
        {
            if (selectedIndex == contents.Length - 1)
            {
                selectedIndex = 0;
            } 
            else
            {
                selectedIndex++;
            }   
        }

        public FileSystemInfo GetSelectedObj()
        {
            return contents[selectedIndex];
        }

        public void PrintFolder()
        {
            Console.Clear();

            for (int i =0; i < contents.Length; ++i)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Cyan;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                }

                // Setup colors
                if (contents[i].GetType() == typeof(DirectoryInfo))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.WriteLine(contents[i].Name);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo dir = new DirectoryInfo(@"C:\Users\daule\source\repos\programming-principles-2-2019\Week2");
            Folder folder = new Folder(dir.GetFileSystemInfos());


            Stack<Folder> dirs = new Stack<Folder>();
            dirs.Push(folder);

            //DirectoryInfo dir2 = new DirectoryInfo(@"C:\Users\daule\source\repos\programming-principles-2-2019");
            //Folder f2 = new Folder(dir.GetFileSystemInfos());
            //f2.PrintFolder();

            bool run = true;
            bool directoryMode = true; // true = directory, false = file

            while (run)
            {
                if (directoryMode)
                {
                    dirs.Peek().PrintFolder();
                }
                
                //folder.PrintFolder();
                ConsoleKeyInfo pressedKey = Console.ReadKey();

                //if (pressedKey.Key == ConsoleKey.A)
                //{
                //    Console.WriteLine("pressed A");
                //}
                //else if (pressedKey.Key == ConsoleKey.B)
                //{
                //    Console.WriteLine("pressed B");
                //}


                switch (pressedKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        dirs.Peek().Up();
                        break;
                    case ConsoleKey.DownArrow:
                        dirs.Peek().Down();
                        break;

                    case ConsoleKey.Enter:

                        FileSystemInfo selected = dirs.Peek().GetSelectedObj();

                        if (selected.GetType() == typeof(DirectoryInfo))
                        {
                            var fInfos = (selected as DirectoryInfo).GetFileSystemInfos();
                            dirs.Push(new Folder(fInfos));
                        }
                        else
                        {
                            string fullPath = (selected as FileInfo).FullName;
                            FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                            StreamReader sr = new StreamReader(fileStream);

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;

                            Console.Clear();
                            Console.Write(sr.ReadToEnd());

                            directoryMode = false;

                            sr.Close();
                            fileStream.Close();
                        }

                        break;

                    case ConsoleKey.Escape:
                        if (directoryMode)
                        {
                            run = false;
                        }
                        else
                        {
                            directoryMode = true;
                        }
                        break;

                    case ConsoleKey.Backspace:
                        if (dirs.Count > 1)
                        {
                            dirs.Pop();
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
