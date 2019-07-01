using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace progressbar
{
    public partial class Form1 : Form
    {
        private string path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"version.xml");
        
        // private int thisVersion = Convert.ToInt32(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
        private int thisVersion = Convert.ToInt32(Application.ProductVersion.Replace(".", ""));
        private string thisVersionS = Application.ProductVersion;

        public Form1()
        {

            InitializeComponent();

            //MessageBox.Show(thisVersionS);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Download();


        }
        private void download_Completed()
        {
            try
            {
                Process.Start("updater.exe", "update.exe progressbar.exe");
                Process.GetCurrentProcess().Kill();

            }
            catch (Exception) { }
        }

        public void cleanUpdateFile()
        {

        }
        public void Download()
        {

            string url = "https://raw.githubusercontent.com/KAleksandr/testUpdate/master/version.xml";
            string url2 = "https://github.com/KAleksandr/testUpdate/blob/master/progressbar.exe?raw=true";
            string url3 = "https://github.com/KAleksandr/testUpdate/blob/master/updater.exe?raw=true";
             //string url3 = "https://github.com/KAleksandr/PayConverter2/blob/master/PayConverter.exe?raw=true";

        
           
            // DownloadManyFiles(dict);
            int ver;
            string version;
            XmlDocument doc = new XmlDocument();
            try
            {
                using (WebClient wc = new WebClient())
                {

                    wc.DownloadProgressChanged += (s, te) => { progressBar1.Value = te.ProgressPercentage; };

                    wc.DownloadFile(new Uri(url), path2);
                    Download(new Uri(url2), "launcher.update");
                    bool exists = System.IO.Directory.Exists(path2);
                    if (!exists)
                    {
                        doc.Load(path2);
                        version = doc.GetElementsByTagName("myprogram")[0].InnerText;
                        ver = Convert.ToInt32(version.Replace(".", ""));
                        //File.Delete("version.xml");
                    }
                    else
                    {
                        ver = thisVersion;
                        version = thisVersionS;
                    }



                }
                if (thisVersion < ver)
                {
                    
                    Download(new Uri(url2), "launcher.update");
                    Download(new Uri(url3), "updater.exe");

                    Thread.Sleep(300);

                    if (File.Exists("launcher.update") && File.Exists("updater.exe"))
                    {
                        MessageBox.Show(this, "Виявлено нову версію (" +
                                              doc.GetElementsByTagName("myprogram")[0].InnerText + ")" +
                                              Environment.NewLine +
                                              "Додаток буде автоматично оновлено і перезапуститься.",
                            Application.ProductName + " v" + Application.ProductVersion, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        checkUpdates();

                    }


                }
            }
            catch (System.Net.WebException) { }
        }
        public void Download(Uri uri, string path2)
        {
            using (WebClient wc = new WebClient())
            {



                wc.DownloadFile(uri, path2);
            }
        }
        //public async Task DownloadManyFiles(Dictionary<Uri, string> files)
        //{
        //    using (WebClient wc = new WebClient())
        //    {
        //        wc.DownloadProgressChanged += (s, e) => progressBar1.Value = e.ProgressPercentage;
        //        foreach (KeyValuePair<Uri, string> pair in files) { await wc.DownloadFileTaskAsync(pair.Key, pair.Value); }
        //        wc.Dispose();
        //    }
            

        //}
        public void checkUpdates()
        {
            try
            {
                int newVersion = Convert.ToInt32(new Version(FileVersionInfo.GetVersionInfo("launcher.update").FileVersion)
                    .ToString().Replace(".", ""));
                int oldVersion =
                    Convert.ToInt32(new Version(Application.ProductVersion)
                        .ToString().Replace(".", ""));

                MessageBox.Show("" + newVersion + " " + oldVersion);

                //if (File.Exists("launcher.update"))
                if (File.Exists("launcher.update") && newVersion > oldVersion)
                {
                    Process.Start("updater.exe", "progressbar.exe  launcher.update");
                    Process.GetCurrentProcess().CloseMainWindow();
                    MessageBox.Show("Jr");
                }
                // else
                {
                    //if (File.Exists("launcher.update")) { File.Delete("launcher.update"); }
                    //Download();
                }

            }
            catch (Exception)
            {
                //if (File.Exists("launcher.update")) { File.Delete("launcher.update"); }
                //Download();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text += " ver." + thisVersionS; //Application.ProductVersion;
        }
    }

}

