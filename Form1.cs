using System;
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
        private int thisVersion = Convert.ToInt32(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".",""));
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (WebClient wc = new WebClient())
            {
                string url= "https://raw.githubusercontent.com/KAleksandr/testUpdate/master/version.xml";
                wc.DownloadProgressChanged += (s, te) => { progressBar1.Value = te.ProgressPercentage; };
                //wc.DownloadFileAsync(new Uri( "https://download.virtualbox.org/virtualbox/6.0.8/VirtualBox-6.0.8-130520-Win.exe"), $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\VirtualBox 6.0.8.exe");
                //wc.DownloadFileAsync(new Uri(url),path2);
                wc.DownloadFile(new Uri(url),path2);
                var doc = new XmlDocument();
                doc.Load(path2);
                string version = doc.GetElementsByTagName("myprogram")[0].InnerText;
                int ver = Convert.ToInt32(version.Replace(".", ""));
                if (thisVersion < ver)
                {
                    MessageBox.Show("new version " + version);
                    MessageBox.Show(this, "Виявлено нову версію (" + doc.GetElementsByTagName("myprogram")[0].InnerText + ")" + Environment.NewLine +
                                          "Додаток буде автоматично оновлено і перезапуститься.", Application.ProductName + " v" + Application.ProductVersion, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
        }

        


    }

}

