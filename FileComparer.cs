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

namespace FileCompare2._0
{
    public partial class FileComparer : Form
    {
        FileEdit fileEdit = new FileEdit();
        List<Files> FileList2 = new List<Files>();
        List<FCompare> FCompareFileList1 = new List<FCompare>();
        List<FCompare> FCompareFileList2 = new List<FCompare>();
        List<FCompare> MissingFiles = new List<FCompare>();
        
        public FileComparer()
        {
            InitializeComponent();

            //ChkFile();
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


        public Task Worker(string searchDir, string saveFile)
        {
            _worker = new Worker();
            _worker.WorkCompleted += _worker_WorkCompleted;
            _worker.ProcessChanged += _worker_ProcessChanged;
            _worker.SendMessag += _worker_SendMessag;
            _worker.SerchDir(searchDir);
            _worker.SaveFile(saveFile);

            StartButton.Enabled = false;

            //Thread thread = new Thread(_worker.SaveFile);
            //thread.Start(_context);

            Task T = Task.Run(() => { _worker.SaveFile(_context); });
            return T;
            //Task.Run(() => { _worker.SaveFile(_context); });
        }

        private void _worker_SendMessag(string text)
        {
            RTB.Text += text;
        }

        private void _worker_WorkCompleted(bool cancelled)
        {
            RTB.Text += cancelled ? " Process canseled\n" : "Process finished\n";
            //ChkFile();
            StartButton.Enabled = true;
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


        async private void StartButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SecondDirTextBox.Text) && !string.IsNullOrEmpty(FirstDirTextBox.Text))
            {
                try
                {
                    string File1 = GetDefaultFile(1);
                    string File2 = GetDefaultFile(2);
                    await Worker(FirstDirTextBox.Text, File1);
                    if (TextBoxFile1.Text != File1) TextBoxFile1.Text = File1;
                    else F1Label.Text = LoadFile(TextBoxFile1.Text, out FCompareFileList1);
                    await Worker(SecondDirTextBox.Text, File2);
                    if (TextBoxFile2.Text == File2) F2Label.Text = LoadFile(TextBoxFile2.Text, out FCompareFileList2);
                    else TextBoxFile2.Text = File2;
                }
                catch (AggregateException err)
                {
                    foreach (var ie in err.InnerExceptions) RTB.Text += "Err " + ie.GetType().Name + " : " + ie.Message + "\n";
                }
            }
        }

        private void ComparButton_Click(object sender, EventArgs e)
        {
            if (ComparButton.Text == "Compare")
            {   
                FindDifference();
                ComparButton.Text = "Apply";
            }
            else
            {
                ApplyChanges(FCompareFileList1, FCompareFileList2);
                ComparButton.Text = "Compare";
            }
        }

        private void FindDifference()
        {
            if (FCompareFileList1.Count() != 0)
            {
                string txt = string.Empty;

                for (int i = 0; i < FCompareFileList1.Count() - 1; i++)
                {
                    if (FCompareFileList1[i].Copy != null) continue;

                    string hashI = FCompareFileList1[i].Hash;
                    for (int j = i + 1; j < FCompareFileList1.Count(); j++)
                    {
                        if (FCompareFileList1[j].Copy != null) continue;
                        if (hashI == FCompareFileList1[j].Hash)
                        {
                            FCompareFileList1[i].Copy = i;
                            FCompareFileList1[j].Copy = i;
                        }
                    }
                }

                var copyFile = (from x in FCompareFileList1
                                where (x.Copy != null)
                                select new { x.Hash, x.Copy }).Distinct().OrderBy(x => x.Copy).ToList();

                for (int i = 0; i < FCompareFileList2.Count(); i++)
                {
                    foreach (var elem in copyFile)
                    {
                        if (FCompareFileList2[i].Hash == elem.Hash) FCompareFileList2[i].Copy = elem.Copy;
                    }
                }



                for (int i = FCompareFileList1.Count() - 1; i > -1; i--)
                {
                    if (FCompareFileList1[i].Copy != null) continue;

                    bool exist = false;
                    for (int y = FCompareFileList2.Count() - 1; y > -1; y--)
                    {
                        if (FCompareFileList2[y].Copy != null) continue;
                        if (FCompareFileList1[i].Hash == FCompareFileList2[y].Hash)
                        {
                            var f1 = FCompareFileList1[i];
                            var f2 = FCompareFileList2[y];

                            var s1 = f1.Name.LastOf(FirstDirTextBox.Text);
                            var s2 = f2.Name.LastOf(SecondDirTextBox.Text);
                            if (s1 != s2 && FCompareFileList1[i].ShortName == FCompareFileList2[y].ShortName)
                            {
                                FCompareFileList1[i].replased = true;
                                FCompareFileList2[y].replased = true;
                                FCompareFileList2[y].moveTo = SecondDirTextBox.Text + s1;
                            }
                            else
                            {
                                FCompareFileList2.RemoveAt(y);
                            }
                            exist = true;
                            break;
                        }
                    }

                    FCompareFileList1[i].exist = exist;
                    if (!exist) txt += FCompareFileList1[i].Name + " - to copy\n";
                }

                var FReplased = FCompareFileList2.Where(x => x.replased == true && x.Copy==null).ToList();
                var FCopy1 = FCompareFileList1.Where(x =>  x.Copy != null).ToList();
                var FCopy2 = FCompareFileList2.Where(x => x.Copy != null).ToList();

                FCompareFileList1 = FCompareFileList1.Where(x => x.Copy == null && x.exist == false).ToList();
                FCompareFileList2 = FCompareFileList2.Where(x => x.Copy == null && x.replased == false).ToList();

                if (FCompareFileList2.Count() != 0)
                    foreach (var elem in FCompareFileList2)
                        txt += elem.Name + " - for Delete\n";

                RTB.Text = FCompareFileList2.Count() + " - Files For Delete \\ " + FCopy1.Count() + " - FCopy1 \\ " + FCopy2.Count() + " - FCopy2 \\ "
                    + FReplased.Count() + " - FReplased \\ " + FCompareFileList1.Count() + " - Files To Copy\n\n" + txt;
            }
        }

        private void ApplyChanges(List<FCompare> FilesToCopy, List<FCompare> FilesForDelete)
        {
            string txt = string.Empty;

            

            var FilesForReplas = FilesForDelete.Where(x => x.replased).ToList();
            foreach (var file in FilesForReplas)
            {
                if (File.Exists(file.Name))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(file.Name) && !string.IsNullOrEmpty(file.moveTo))
                        {
                            string Dir = Path.GetDirectoryName(file.moveTo);
                            if (fileEdit.ChkDir(Dir))File.Move(file.Name, file.moveTo);
                        }
                    }
                    catch (Exception ex)
                    {
                        txt += file.Name + " - !!!ERRRR!!!  - " + ex.Message + "\n";
                    }

                    if (File.Exists(file.moveTo))
                    {
                        txt += file.moveTo + " - moved!\n";
                    }
                }
            }

            FilesForDelete = FilesForDelete.Where(x => !x.replased).ToList();
            foreach (var file in FilesForDelete)
            {
                if (File.Exists(file.Name))
                {
                    try
                    {
                        File.SetAttributes(file.Name, FileAttributes.Normal);
                        File.Delete(file.Name);
                    }
                    catch (Exception ex)
                    {
                        txt += file.Name + " - !!!ERRRR!!!  - " + ex.Message + "\n";
                    }

                    if (!File.Exists(file.Name))
                    {
                        txt += file.Name + " - deleted!\n";
                    }
                }
            }

            foreach (var file in FilesToCopy)
            {
                string targetFile = fileEdit.DirFile(SecondDirTextBox.Text, file.Name.LastOf(FirstDirTextBox.Text));

                //CopyFile(file.Name, targetFile);
                if (!string.IsNullOrEmpty(file.Name) && !string.IsNullOrEmpty(targetFile) && File.Exists(file.Name))
                {
                    string Dir = Path.GetDirectoryName(targetFile);
                    if (fileEdit.ChkDir(Dir)) File.Copy(file.Name, targetFile);
                }

                if (File.Exists(file.Name))txt += file.Name + " - file copied\n";
                else txt += file.Name + " - !!!COPYERRRR!!!\n";
            }

            RTB.Text = txt;
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
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK) FirstDirTextBox.Text = FBD.SelectedPath;
        }

        private void SecondDirBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK) SecondDirTextBox.Text = FBD.SelectedPath;
        }

        private void ChangeBut_Click(object sender, EventArgs e)
        {
            string tmp = TextBoxFile2.Text;
            TextBoxFile2.Text = TextBoxFile1.Text;
            TextBoxFile1.Text = tmp;
        }

        private string LoadFile(string file, out List<FCompare> fCompare)
        {
            fCompare = new List<FCompare>();
            if (File.Exists(file))
            {
                List<Files> FileList = new List<Files>();
                string readText = File.ReadAllText(file);
                FileList = JsonSerializer.Deserialize<List<Files>>(readText);
                fCompare = mapFCompare(FileList);

                if (FileList.Count > 0) return FileList.Count.ToString();
                else return "0";
            }
            return "0";
        }

        private void TextBoxFile1_TextChanged(object sender, EventArgs e)
        {
            F1Label.Text = LoadFile(TextBoxFile1.Text, out FCompareFileList1);
        }

        private void TextBoxFile2_TextChanged(object sender, EventArgs e)
        {
            F2Label.Text = LoadFile(TextBoxFile2.Text, out FCompareFileList2);
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
                int minelem = FileList2[0].Name.QuantityOf('\\');
                foreach (var elem in FileList2)
                {
                    int x = elem.Name.QuantityOf('\\');
                    if (x < minelem) minelem = x;
                }

                string[,] stmas = new string[FileList2.Count(), minelem];

                var resultDirMass = FileList2[0].Name.Split('\\').Take(minelem).ToArray();

                for (int i = 0; i < FileList2.Count(); i++)
                {
                    var mas = FileList2[i].Name.Split('\\').Take(minelem).ToArray();
                    for (int y = 0; y < resultDirMass.Count(); y++)
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
            string[] searchRsh = new string[] { "*.*" };
            DirectoryInfo DI = new DirectoryInfo(FirstDirTextBox.Text);
            string[] files = Directory.GetFiles(FirstDirTextBox.Text);
            string[] dirs = Directory.GetDirectories(FirstDirTextBox.Text);
            FileInfo[] FI = searchRsh.SelectMany(fi => DI.GetFiles(fi, SearchOption.AllDirectories)).Distinct().ToArray();
            FileInfo[] FI2 =  DI.GetFiles("*",SearchOption.AllDirectories).Distinct().ToArray();

        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (_worker != null) _worker.Cansel();
        }

        private void GetAllRsh()
        {
            //List<Files> FileList = new List<Files>();
            //string serchDir = string.IsNullOrEmpty(FirstDirTextBox.Text) ? MainDir : FirstDirTextBox.Text;
            //RTB.Text = "SerchDir - " + serchDir + "\n";

            //if (Directory.Exists(serchDir))
            //{
            //    DirectoryInfo DI = new DirectoryInfo(serchDir);
            //    FileInfo[] FI = DI.GetFiles("*", SearchOption.AllDirectories);
            //    var rsh = FI.Select(f => f.Extension.ToLower()).Distinct().ToArray();

            //    foreach (var elem in rsh)
            //    {
            //        RTB.Text += "\"*" + elem + ", ";
            //    }
            //}
        }

        private void CopyFile(string copyFile, string targetFile)
        {
            if (!string.IsNullOrEmpty(copyFile) && !string.IsNullOrEmpty(targetFile)&& File.Exists(copyFile))
            {
                string Dir = Path.GetDirectoryName(targetFile);
                if (fileEdit.ChkDir(Dir))File.Copy(copyFile, targetFile);
            }
        }
    }
}
