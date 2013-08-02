using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FixAdria {
    class Program {
        static void Main(string[] args) {
            bool menu_loop = true;
            string menu_answer;
            string filename;
            string[] tmp;
            string bakname;

            Console.WriteLine("FixAdria v1.0.0.0 by drag0n(@masendav.org)");
            Console.WriteLine("======================================\n");
            Patches Patch = new Patches();
            Console.WriteLine("Which file(s) to patch:");
            Console.WriteLine("a) HFFsp.exe (Single Player Edition)");
            Console.WriteLine("b) HellfireFixed.exe (Multiplayer Version)");
            Console.WriteLine("c) Both");
            Console.WriteLine("d) Custom");
            Console.WriteLine("e) Exit");
            while (menu_loop) {
                Console.Write(": ");
                menu_answer = Convert.ToString(Console.ReadKey().KeyChar).ToUpper();
                Console.WriteLine();
                switch (menu_answer) {
                    case "A":
                        Console.WriteLine("-> HFFsp.exe");
                        Patch.AdriaWelcome("HFFsp.exe", "HFFsp.bak", false);
                        menu_loop = false;
                        break;
                    case "B":
                        Console.WriteLine("-> HellfireFixed.exe");
                        Patch.AdriaWelcome("HellfireFixed.exe", "HellfireFixed.bak", false);
                        menu_loop = false;
                        break;
                    case "C":
                        Console.WriteLine("-> HFFsp.exe");
                        Patch.AdriaWelcome("HFFsp.exe", "HFFsp.bak", false);
                        Console.WriteLine();
                        Console.WriteLine("-> HellfireFixed.exe");
                        Patch.AdriaWelcome("HellfireFixed.exe", "HellfireFixed.bak", false);
                        menu_loop = false;
                        break;
                    case "D":
                        Console.Write("Filename: ");
                        filename = Console.ReadLine().TrimEnd();
                        tmp = filename.Split(Convert.ToChar("."));
                        bakname = filename.Substring(0,filename.Length - tmp[tmp.GetUpperBound(0)].Length) + "bak";
                        Console.WriteLine();
                        Console.WriteLine("-> {0}", filename);
                        Patch.AdriaWelcome(filename, bakname, false);
                        menu_loop = false;
                        break;
                    case "E":
                        menu_loop = false;
                        break;
                    default:
                        break;
                }
            }
            Console.WriteLine("\nPress any key to continue..");
            Console.ReadKey(true);
        }
        class Patches {
            public void AdriaWelcome(string filename, string bakname, bool silence) {
                int buf_size = 23;
                int start_offset = 635060;
                int length = 23;
                byte[] buf = new byte[buf_size];
                string tmp;
                string oldstring = "Sfx\\Towners\\cow1.wav\0\0\0";
                string newstring = "Sfx\\Towners\\Witch38.wav";
                bool overwrite_loop = true;
                string overwrite_answer;

                if (silence == false) Console.WriteLine("Checking file..");
                if (!File.Exists(filename)) { if (silence == false) Console.WriteLine("ERROR! File does not exist! ({0})", filename); Console.Beep(); }
                else {
                    FileStream s = File.OpenRead(filename);
                    s.Position = start_offset;
                    s.Read(buf, 0, length);
                    s.Close();

                    tmp = Encoding.ASCII.GetString(buf);
                    if (tmp == oldstring) {
                        if (silence == false) Console.WriteLine("..seems to be alright");
                        if (silence == false) Console.WriteLine("Making backup..");
                        if (!File.Exists(bakname)) { File.Copy(filename, bakname); }
                        else {
                            Console.WriteLine("ERROR! File with backup file's name already exists!"); Console.Beep();
                            while (overwrite_loop) {
                                Console.Write("Overwrite? [y/n] ");
                                overwrite_answer = Convert.ToString(Console.ReadKey().KeyChar).ToUpper();
                                switch (overwrite_answer) {
                                    case "Y":
                                        File.Copy(filename, bakname, true);
                                        overwrite_loop = false;
                                        Console.WriteLine();
                                        break;
                                    case "N":
                                        Console.WriteLine();
                                        return;
                                    default:
                                        Console.WriteLine();
                                        break;
                                }
                            }
                        }
                        if (silence == false) Console.WriteLine("..backup done ({0})", bakname);
                        //Commiting replacement
                        if (silence == false) Console.WriteLine("Patching..");
                        try {
                            s = File.OpenWrite(filename);
                            s.Position = start_offset;
                            buf = Encoding.ASCII.GetBytes(newstring);
                            s.Write(buf, 0, length);
                            s.Close();
                            if (silence == false) Console.WriteLine("..done.");
                        }
                        catch (IOException) {
                            if (silence == false) Console.WriteLine("ERROR! File in use?"); Console.Beep();
                        }
                    }
                    else if (tmp == newstring) { if (silence == false) Console.WriteLine("ERROR! Patching not needed. Already patched or unsupported version."); Console.Beep(); }
                    else { if (silence == false) Console.WriteLine("ERROR! Unsupported version?"); Console.Beep(); }
                }
            }
        }
    }
}
