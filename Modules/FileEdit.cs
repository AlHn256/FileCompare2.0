using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace FileCompare2._0
{
    class FileEdit
    {
        string MainDir = "";
        private string StrException = "";
        private List<CopyList> CList;
        public class CopyList
        {
            public string File { get; set; }
            public string Hesh { get; set; }
            public int Copy { get; set; }

            public CopyList(string File, string Hesh, int Copy)
            {
                this.File = File;
                this.Hesh = Hesh;
                this.Copy = Copy;
            }
        }
        
        public string AutoLoade()
        {
            string LoadeInfo ="";
            string[] FiletoLoad = GetAutoSaveFilesList();

            foreach (string LFile in FiletoLoad)
            {
                if (File.Exists(LFile) )
                {
                    try
                    {
                        StreamReader sr = new StreamReader(LFile);
                        LoadeInfo=sr.ReadToEnd();
                        sr.Close();
                    }
                    catch  { };
                }
            }
            return LoadeInfo;
        }

        public bool AutoSave(string[] Info)
        {
            string[] FiletoSave = GetAutoSaveFilesList();
            if (Info.Length == 0 || FiletoSave.Length == 0)
                return false;

            string str = "";
            foreach (string txt in Info) str += txt + "\r";
            foreach (string FtoSave in FiletoSave)
            {
                if (Directory.Exists(Path.GetDirectoryName(FtoSave)))
                {
                    try
                    {
                        using (FileStream fs = File.Create(FtoSave))
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes(str);
                            fs.Write(info, 0, info.Length);
                        }
                        return true;
                    }
                    catch { };
                }
            }
            return false;
        }

        public bool ChkDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                DirectoryInfo tmpdir = new DirectoryInfo(dir);
                try
                {
                    tmpdir.Create();
                }
                catch { }
                if (Directory.Exists(dir)) return true;
            }
            else return true;
            return false;
        }

        public bool ChkFile(string file)
        {
            if (!File.Exists(file))
            {
                try
                {
                    using (FileStream fs = File.Create(file))
                    {
                        if (File.Exists(file)) return true;
                    }
                }
                catch { }

                return false;
            }

            return true;
        }

        public string ComputeMD5Checksum(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                byte[] checkSum = md5.ComputeHash(fileData);
                return BitConverter.ToString(checkSum);
            }
        }

        public void CopyDel(string dir,int Lv)
        {
            CList = new List<CopyList>();
            if (Directory.Exists(dir) == true)
            {
                if (Lv == 0)
                {
                    string[] files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
                    if (files.Length != 0) { foreach (string file in files) { string md5 = ComputeMD5Checksum(file); CopyList elm = new CopyList(file, md5, -1); CList.Add(elm); } }
                }
                else
                {
                    string[] files = Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly);
                    if (files.Length != 0) { foreach (string file in files) { string md5 = ComputeMD5Checksum(file); CopyList elm = new CopyList(file, md5, -1); CList.Add(elm); } }
                }

                if(CList.Count()!=0)
                { 
                    int i = 0, j = 0;
                    for (i = 0; i < CList.Count() - 1; i++)
                    {
                        string HeshI = CList[i].Hesh;
                        for (j = i + 1; j < CList.Count(); j++)
                        {
                            if (CList[j].Copy == -1 && HeshI == CList[j].Hesh)
                            {
                                CList[i].Copy = i;
                                CList[j].Copy = i;
                            }
                        }
                    }

                    for (i = 0; i < CList.Count; i++)if (CList[i].Copy != -1 && CList[i].Copy != i)File.Delete(CList[i].File);
                }
            }
        }

        public string DirFile(string Dir, string File)
        {
            if (Dir[Dir.Length-1] == '\\') Dir = Dir.Substring(0, Dir.Length - 1);
            if (File[0] == '\\') File = File.Substring(1);
            MainDir = Dir + "\\" + File;
            return MainDir;
        }
        public bool DirRename(string Dir, string NewDir)
        {
            DirectoryInfo CorDir = new DirectoryInfo(Dir);
            CorDir.MoveTo(NewDir);
            if (CorDir.Exists) return true;
            else return false;
        }

        internal bool IsSameDisk(string Dir, string Dir2)
        {
            if (Dir != null && Dir2 != null)
            {
                if (Dir.Length > 3 && Dir2.Length > 3)
                {
                    if (Dir.IndexOf(@":\") == 1 && Dir2.IndexOf(@":\") == 1)
                    {
                        Dir = Dir.ToLower();
                        Dir2 = Dir2.ToLower();
                        if (Dir[0] == Dir2[0]) return true;
                    }
                }
            }
            return false;
        }


        internal bool IsSameDir(string DirFrom, string DirTo)
        {
            if (DirFrom != null && DirTo != null)
            {
                if (DirFrom.Length > 1 && DirTo.Length > 1)
                {
                    if (DirFrom.IndexOf(DirTo) != -1 || DirTo.IndexOf(DirFrom) != -1) return true;
                }
            }
            return false;
        }

        internal string GetAutoLoadeFirstFile()
        {
            string LoadeFile = "";
            string[] FiletoLoad = GetAutoSaveFilesList();

            foreach (string LFile in FiletoLoad)
            {
                if (File.Exists(LFile))
                {
                    LoadeFile = LFile;
                    break;
                }
            }
            return LoadeFile;
        }
        private string[] GetAutoSaveFilesList()
        {

            string test = AppDomain.CurrentDomain.BaseDirectory;
            string ApplicationFileName = Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Split('\\').Last()) + ".inf";
            string[] AutoSaveFiles = new string[] { @"C:\Windows\Temp", @"D:", @"E:", @"C:"};

            string[] AutoSaveFilesList = new string[AutoSaveFiles.Count() + 1];
            int i = 0;
            AutoSaveFilesList[i++]= Directory.GetCurrentDirectory() + "\\" + ApplicationFileName;
            foreach(string elem in AutoSaveFiles)
            {
                AutoSaveFilesList[i++] = elem + "\\" + ApplicationFileName;
            }

            return AutoSaveFilesList;
        }
        
        public bool FileRename(string File, string NewFile)
        {
            bool Fl = false;
            FileInfo CorFile = new FileInfo(File);
            CorFile.MoveTo(NewFile);
            if (CorFile.Exists) Fl = true;
            else Fl = false;
            return Fl;
        }
        public string GetExeption() { return StrException; }
        public void SetExeption(Exception e) { StrException = e.Message.ToString(); }
        public List<string> GetFileList(string file)
        {
            List<string> FileList = new List<string>();

            if (File.Exists(file))
                FileList = File.ReadAllLines(file).ToList();
            return FileList;
        }

        public List<string> GetFileList(string file, int nEncoding)
        {
            string encoding = "utf-8";
            if (nEncoding == 1)
                encoding = "windows-1251";

            if (encoding == null || encoding.Length == 0) return GetFileList(file);

            List<string> FileList = new List<string>();
            if (File.Exists(file))
                FileList = File.ReadAllLines(file, Encoding.GetEncoding(encoding)).ToList();
            return FileList;
        }

        public List<string> GetFileList(string file, string encoding)
        {
            if (encoding == null || encoding.Length == 0) return GetFileList(file);

            List<string> FileList = new List<string>();
            if (File.Exists(file))
                FileList = File.ReadAllLines(file, Encoding.GetEncoding(encoding)).ToList();
            return FileList;
        }

        public bool SetFileList(string file, List<string> fileList)
        {
            FileStream f1 = new FileStream(file, FileMode.Truncate, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(f1, Encoding.UTF8);
            foreach (string txt in fileList) sw.WriteLine(txt);
            sw.Dispose();
            return true;
        }

        public bool SetFileList(string file, List<string> fileList, int nEncoding)
        {
            if (nEncoding == 1)
                return SetFileList(file, fileList, "windows-1251");
            else
                return SetFileList(file, fileList);
        }

        public bool SetFileList(string file, List<string> fileList, string encoding = "utf-8")
        {
            FileStream f1 = new FileStream(file, FileMode.Truncate, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(f1, Encoding.GetEncoding(encoding));
            foreach (string txt in fileList) sw.WriteLine(txt);
            sw.Dispose();

            return true;
        }

        public bool SetFileString(string file, string text)
        {
            using (StreamWriter writetext = new StreamWriter(file))
            {
                writetext.WriteLine(text);
            }
            return true;
        }
    }
}