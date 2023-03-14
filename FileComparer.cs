using FileCompare2._0.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
//using static System.Net.WebRequestMethods;

namespace FileCompare2._0
{
    public partial class FileComparer : Form
    {
        FileEdit fileEdit = new FileEdit();
        List<Files> FileList1 = new List<Files>();
        List<Files> FileList2 = new List<Files>();
        List<FCompare> MissingFiles = new List<FCompare>();
        string MainDir = @"C:\Test\Music\";
        public FileComparer()
        {
            InitializeComponent();
            FirstDirTextBox.Text = MainDir;
            ChkFile();
            Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _context = SynchronizationContext.Current;
        }

        private void ChkFile()
        {
            string file = GetDefaultFile(1);
            if (File.Exists(file))
            {
                TextBoxFile1.Text = file;
            }
            file = GetDefaultFile(2);
            if (File.Exists(file))
            {
                TextBoxFile2.Text = file;
            }
        }
        private Worker _worker;
        private object _context;


        public void Worker(string searchDir, string saveFile)
        {
            _worker = new Worker();
            _worker.WorkCompleted += _worker_WorkCompleted;
            _worker.ProcessChanged += _worker_ProcessChanged;
            _worker.SendMessag += _worker_SendMessag;
            _worker.SerchDir(searchDir);
            _worker.SaveFile(saveFile);

            //StartButton.Enabled = false;

            Thread thread = new Thread(_worker.SaveFile);
            thread.Start(_context);
        }

        private void _worker_SendMessag(string text)
        {
            RTB.Text += text;
        }

        private void _worker_WorkCompleted(bool cancelled)
        {
            RTB.Text += cancelled ? " Process canseled\n" : "Process finished\n";
            //StartButton.Enabled = true;
        }

        private void _worker_ProcessChanged(int progress)
        {
            progressBar1.Value = progress;
        }
        private string GetDefaultFile(int N)
        {
            string file = Directory.GetCurrentDirectory() + "\\" + Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Split('\\').Last());
            if (N == 1) file += ".cmp";
            else file += " (" + N + ").cmp";
            return file;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(Task.Run(() =>
            {
                Worker(string.IsNullOrEmpty(FirstDirTextBox.Text) ? MainDir : FirstDirTextBox.Text, TextBoxFile1.Text);
            }));

            if (!string.IsNullOrEmpty(SecondDirTextBox.Text))
            {
                tasks.Add(Task.Run(() =>
                {
                    if (string.IsNullOrEmpty(TextBoxFile2.Text))
                        TextBoxFile2.Text = Directory.GetCurrentDirectory() + "\\" + Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Split('\\').Last()) + " (2).cmp";
                    Worker(SecondDirTextBox.Text, TextBoxFile2.Text);
                }));
            }

            try
            {
                Task.WaitAll(tasks.ToArray());
                //RTB.Text += "\nFinish";
            }
            catch (AggregateException err)
            {
                foreach (var ie in err.InnerExceptions) RTB.Text += "Err " + ie.GetType().Name + " : " + ie.Message + "\n";
            }
        }

        private void GetAllRsh()
        {
            List<Files> FileList = new List<Files>();
            string serchDir = string.IsNullOrEmpty(FirstDirTextBox.Text) ? MainDir : FirstDirTextBox.Text;
            RTB.Text = "SerchDir - " + serchDir + "\n";

            if (Directory.Exists(serchDir))
            {
                DirectoryInfo DI = new DirectoryInfo(serchDir);
                FileInfo[] FI = DI.GetFiles("*", SearchOption.AllDirectories);
                var rsh = FI.Select(f => f.Extension.ToLower()).Distinct().ToArray();

                foreach (var elem in rsh)
                {
                    RTB.Text += "\"*" + elem + ", ";
                }
            }
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            if (FileList1.Count() != 0 && FileList2.Count() != 0)
            {

                MissingFiles = mapFCompare(FileList1).ToList();// FileList1.Take(10).ToList();
                var mas2 = mapFCompare(FileList2).ToList();

                for (int i = MissingFiles.Count() - 1; i > -1; i--)
                {
                    for (int y = mas2.Count() - 1; y > -1; y--)
                    {
                        if (MissingFiles[i].Hash == mas2[y].Hash)
                        {
                            MissingFiles[i].exist = true;
                            mas2.RemoveAt(y);
                            break;
                        }
                    }
                }

                string text = string.Empty;
                int z = 0;
                MissingFiles = MissingFiles.Where(x => !x.exist).ToList();
                for (int i = MissingFiles.Count() - 1; i > -1; i--)
                    text += z++ + " " + MissingFiles[i].Name + "\n";

                MissFilesLab.Text = "Missing Files " + MissingFiles.Count().ToString();
                RTB.Text = text;

                if(string.IsNullOrEmpty(CopyDirTextBox.Text))GetMainSaveDir();
            }
        }

        private void GetFileList()
        {
            RTB.Text = string.Empty;
            List<Files> FileList = new List<Files>();

            //string[] rsh = new string[] { "*.m3u", "*.jpg", "*.exe", "*.txt", "*", "*.mp4", "*.mp3", "*.asf", "*.mpg", "*.avi", "*.webm", "*.gpx", "*.pdf", "*.png", "*.wav", "*.jpeg", "*.mpeg", "*.flv", "*.wma", "*.bmp", "*.doc", "*.gif", "*.tif", "*.htm", "*.html", "*.rtf", "*.ogg", "*.ttf", "*.dat", "*.wmv" }; //All
            string[] rsh = new string[] { "*.mp4", "*.mp3", "*.wav", "*.webm", "*.ogg", "*.wma", "*.mpg", "*.avi", "*.mpeg", "*.wmv", "*.dat", "*.asf" }; // Media rsh
            //string[] rsh = new string[] { "*.mp4", "*.mp3", "*.wav", "*.webm", "*.ogg", "*.wma" }; // Audio rsh
            //string[] rsh = new string[] { "*.mpg", "*.avi", "*.mpeg", "*.wmv", "*.dat", "*.asf" }; // Video rsh

            string serchDir = string.IsNullOrEmpty(FirstDirTextBox.Text) ? MainDir : FirstDirTextBox.Text;

            if (Directory.Exists(serchDir))
            {
                string FN = "";
                DirectoryInfo DI = new DirectoryInfo(serchDir);
                FileInfo[] FI = rsh.SelectMany(fi => DI.GetFiles(fi, SearchOption.AllDirectories)).Distinct().ToArray();

                if (FI.Length > 0)
                    Parallel.ForEach(FI, f =>
                    {
                        FileList.Add(new Files
                        {
                            Name = f.FullName,
                            ShortName = f.Name,
                            Date = f.CreationTime,
                            Hash = fileEdit.ComputeMD5Checksum(f.FullName),
                            Sise = f.Length
                        });
                    });

                string json = JsonSerializer.Serialize(FileList);
                if (string.IsNullOrEmpty(TextBoxFile1.Text)) TextBoxFile1.Text = Directory.GetCurrentDirectory() + "\\" + Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Split('\\').Last()) + ".cmp";
                File.WriteAllText(TextBoxFile1.Text, json);
            }
        }

        private void FirstDirBtn_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "(*.cmp)|*.cmp|All files (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    TextBoxFile1.Text = dialog.FileName;
                }
            }
        }

        private void FirstDirBtn_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog()
            {
                RootFolder = Environment.SpecialFolder.CommonDesktopDirectory,
                ShowNewFolderButton = true
            };

            if (FBD.ShowDialog() == DialogResult.OK)FirstDirTextBox.Text = FBD.SelectedPath;
        }

        private void SecondDirBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)SecondDirTextBox.Text = FBD.SelectedPath;
        }

        private void ChangeBut_Click(object sender, EventArgs e)
        {
            string tmp = TextBoxFile2.Text;
            TextBoxFile2.Text = TextBoxFile1.Text;
            TextBoxFile1.Text=tmp;
        }

        private string LoadFile(string file, out List<Files> FileList)
        {
            FileList = new List<Files>();
            if (File.Exists(file))
            {
                string readText = File.ReadAllText(file);
                FileList = JsonSerializer.Deserialize<List<Files>>(readText);
                if (FileList.Count > 0)  return FileList.Count.ToString();
                else return "0";
            }
            else return "0";
        }

        private void TextBoxFile1_TextChanged(object sender, EventArgs e)
        {
            F1Label.Text = LoadFile(TextBoxFile1.Text, out FileList1);
        }

        private void TextBoxFile2_TextChanged(object sender, EventArgs e)
        {
            F2Label.Text =  LoadFile(TextBoxFile2.Text, out FileList2);
        }

        private List<FCompare> mapFCompare(List<Files> filesList)
        {
            List<FCompare> fCompares = new List<FCompare>();
            foreach (var elem in filesList)
            {
                fCompares.Add(new FCompare()
                {
                    Name = elem.Name,
                    ShortName = elem.ShortName,
                    Hash = elem.Hash,
                    Sise = elem.Sise,
                    Date = elem.Date,
                });
            }

            return fCompares;
        }

        protected void GetMainSaveDir()
        {
            string resultDir = string.Empty;
            if (FileList2.Count() > 0)
            {
                int minelem= FileList2[0].Name.QuantityOf('\\');
                foreach (var elem in FileList2)
                {
                    int x = elem.Name.QuantityOf('\\');
                    if (x < minelem) minelem = x;
                }

                string[,] stmas = new string[FileList2.Count(), minelem];

                var resultDirMass = FileList2[0].Name.Split('\\').Take(minelem).ToArray();

                for (int i=0; i< FileList2.Count(); i++)
                {
                    var mas = FileList2[i].Name.Split('\\').Take(minelem).ToArray();
                    for(int y = 0; y< resultDirMass.Count(); y++)
                    {
                        if (resultDirMass[y] != mas[y])
                        {
                            resultDirMass = resultDirMass.Take(y).ToArray();
                            break;
                        }
                    }
                }

                foreach (var elem in resultDirMass)
                {
                    resultDir += elem + "\\";
                }
            }
            CopyDirTextBox.Text = resultDir;
        }

        private void CopyFilesButton_Click(object sender, EventArgs e)
        {
            if(MissingFiles.Count>0)
            {
                RTB.Text = string.Empty;
                string aasd = "MUSIK";

                for(int i = 0; i< MissingFiles.Count; i++)
                {
                    string To = CopyDirTextBox.Text + MissingFiles[i].Name.Substring(MissingFiles[i].Name.IndexOf(aasd) + aasd.Length + 1);

                    var dir = To.FirstOf('\\', 88);
                    fileEdit.ChkDir(dir);


                    RTB.Text += "From " + MissingFiles[i].Name + "\nTo - " + To +"\n";
                    if (File.Exists(MissingFiles[i].Name))
                    {
                        File.Copy(MissingFiles[i].Name, To, true);
                    }
                }

            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (_worker != null) _worker.Cansel();
        }

        private void Test_Click(object sender, EventArgs e)
        {
            GetAllRsh();
        }

    }
}
