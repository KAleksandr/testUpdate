using System;

using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace progressbar
{
    public partial class Form1 : Form
    {
        private string path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"version.xml");
        private string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"update.exe");
        private int thisVersion = Convert.ToInt32(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
        private string thisVersionS = (Assembly.GetExecutingAssembly().GetName().Version.ToString());

        public Form1()
        {
           
            InitializeComponent();
            
            MessageBox.Show(thisVersionS);
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

        public void Download()
        {
            string url = "https://raw.githubusercontent.com/KAleksandr/testUpdate/master/version.xml";
            string url2 = "https://github.com/KAleksandr/testUpdate/blob/master/progressbar.exe?raw=true";


            int ver;
            string version;
            var doc = new XmlDocument();
            using (WebClient wc = new WebClient())
            {

                wc.DownloadProgressChanged += (s, te) => { progressBar1.Value = te.ProgressPercentage; };
                //wc.DownloadFileAsync(new Uri( "https://download.virtualbox.org/virtualbox/6.0.8/VirtualBox-6.0.8-130520-Win.exe"), $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\VirtualBox 6.0.8.exe");
                //wc.DownloadFileAsync(new Uri(url),path2);
                wc.DownloadFile(new Uri(url), path2);
                bool exists = System.IO.Directory.Exists(path2);
                if (!exists)
                {
                    doc.Load(path2);
                    version = doc.GetElementsByTagName("myprogram")[0].InnerText;
                    ver = Convert.ToInt32(version.Replace(".", ""));
                }
                else
                {
                    ver = thisVersion;
                    version = thisVersionS;
                }



            }
            if (thisVersion < ver)
            {
                //if (File.Exists("launcher.update")) { File.Delete("launcher.update"); }
                //MessageBox.Show("new version " + version);
                //MessageBox.Show(this, "Виявлено нову версію (" + doc.GetElementsByTagName("myprogram")[0].InnerText + ")" + Environment.NewLine +
                //                      "Додаток буде автоматично оновлено і перезапуститься.", Application.ProductName + " v" + Application.ProductVersion, MessageBoxButtons.OK, MessageBoxIcon.Information);
                using (WebClient wc = new WebClient())
                {
                    // wc.DownloadProgressChanged += (s, te) => { progressBar1.Value = te.ProgressPercentage; };
                    wc.DownloadFile(new Uri(url2), "launcher.update");
                    if (File.Exists("launcher.update"))
                    {
                        MessageBox.Show(this, "Виявлено нову версію (" +
                                              doc.GetElementsByTagName("myprogram")[0].InnerText + ")" +
                                              Environment.NewLine +
                                              "Додаток буде автоматично оновлено і перезапуститься.",
                            Application.ProductName + " v" + Application.ProductVersion, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        if (File.Exists("version.xml")) { File.Delete("version.xml"); }
                        checkUpdates();
                    }
                }


                
            }
        }
        public void checkUpdates()
        {
            try
            {
                if (File.Exists("launcher.update"))
                // if (File.Exists("launcher.update") && new Version(FileVersionInfo.GetVersionInfo("launcher.update").FileVersion) > new Version(Application.ProductVersion))
                {
                    Process.Start("updater.exe", "progressbar.exe  launcher.update");
                    Process.GetCurrentProcess().CloseMainWindow();
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
            this.Text += " ver." +  Application.ProductVersion;
        }
    }

}

