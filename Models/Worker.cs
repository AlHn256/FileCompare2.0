using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace FileCompare2._0.Models
{
    class Worker
    {
        private bool _canselled = false;
        private string serchDir = string.Empty;
        private string saveFile = string.Empty;
        private FileEdit fileEdit = new FileEdit();
        public void Cansel()
        {
            _canselled = true;
        }

        public void SerchDir(string dir)
        {
            serchDir = dir;
        }
        public void SaveFile(string file)
        {
            saveFile = file;
        }

        public void SaveFile(object param)
        {
            SynchronizationContext context = (SynchronizationContext)param;

            List<Files> FileList = new List<Files>();
            string[] rsh = new string[] { "*.*" };
            //string[] rsh = new string[] { "*.m3u", "*.jpg", "*.exe", "*.txt", "*.mp4", "*.mp3", "*.asf", "*.mpg", "*.avi", "*.webm", "*.gpx", "*.pdf", "*.png", "*.wav", "*.jpeg", "*.mpeg", "*.flv", "*.wma", "*.bmp", "*.doc", "*.gif", "*.tif", "*.htm", "*.html", "*.rtf", "*.ogg", "*.ttf", "*.dat", "*.wmv" }; //All
            //string[] rsh = new string[] { "*.mp4", "*.mp3", "*.wav", "*.webm", "*.ogg", "*.wma", "*.mpg", "*.avi", "*.mpeg", "*.wmv", "*.dat", "*.asf" }; // Media rsh
            //string[] rsh = new string[] { "*.mp4", "*.mp3", "*.wav", "*.webm", "*.ogg", "*.wma" }; // Audio rsh
            //string[] rsh = new string[] { "*.mpg", "*.avi", "*.mpeg", "*.wmv", "*.dat", "*.asf" }; // Video rsh
            if (Directory.Exists(serchDir))
            {
                //string FN = "";
                DirectoryInfo DI = new DirectoryInfo(serchDir);
                FileInfo[] FI = rsh.SelectMany(fi => DI.GetFiles(fi, SearchOption.AllDirectories)).Distinct().ToArray();

                if (FI.Length > 0)
                    for(int i =0; i< FI.Length; i++)
                    {
                        if (_canselled) break;
                        FileList.Add(new Files
                        {
                            Name = FI[i].FullName,
                            ShortName = FI[i].Name,
                            Date = FI[i].CreationTime,
                            Hash = fileEdit.ComputeMD5Checksum(FI[i].FullName),
                            Sise = FI[i].Length
                        });
                        context.Send(OnProgressChanged, (i+1)*100 / FI.Length);
                    }
                
                string json = JsonSerializer.Serialize(FileList);
                if (string.IsNullOrEmpty(saveFile)) saveFile = Directory.GetCurrentDirectory() + "\\" + Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Split('\\').Last()) + ".cmp";
                lock (object1)
                {
                    File.WriteAllText(saveFile, json);
                }
                string text = "Saved to " + saveFile + "\n";
                context.Send(OnSendMessag, text);
            }

            context.Send(OnWorkCompleted, _canselled);
        }

        static object object1 = new object();

        public void OnProgressChanged(object i)
        {
            if (ProcessChanged != null) ProcessChanged((int)i);
        }

        public void OnWorkCompleted(object cancelled)
        {
            if (WorkCompleted != null) WorkCompleted((bool)cancelled);
        }

        public void OnSendMessag(object text)
        {
            SendMessag((string)text);
        }

        public event Action<int> ProcessChanged;
        public event Action<bool> WorkCompleted;
        public event Action<string> SendMessag;
    }
}
