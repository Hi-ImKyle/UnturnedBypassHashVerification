using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string contentPath;

        private void TextBox1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            TextBox1.Text = fbd.SelectedPath;
            ScanContentFolder(fbd.SelectedPath);
        }

        public void ScanContentFolder(string path)
        {
            if (!path.Contains("Workshop\\Content"))
            {
                MessageBox.Show(this, "This doesn't look like the correct directory \nPlease check again", "Check again", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                return;
            }
            string[] modsF = Directory.GetDirectories(path);
            foreach(string folder in modsF)
            {
                ListBox1.Items.Add("< " + folder.Replace(path + "\\", "") + " > " + File.ReadLines(folder + "\\mod.usoinfo").First());
            }
            if(ListBox1.Items.Count > 0)
            {
                button1.Enabled = true;
                contentPath = path;
            }
        }

        public static bool IsStringInFile(string fileName, string searchString)
        {
            return File.ReadAllText(fileName).Contains(searchString);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(var listBoxItem in ListBox1.Items)
            {
                string modID = listBoxItem.ToString().Substring(2, 9);
                //MessageBox.Show(contentPath + "\\" + modID);
                string[] modFiles = Directory.GetDirectories(contentPath + "\\" + modID);
                foreach(string cate in modFiles)
                {
                    string[] cateFiles = Directory.GetDirectories(cate);
                    foreach (string item in cateFiles)
                    {
                        string[] itemFiles = Directory.GetFiles(item);
                        foreach (string unityAssets in itemFiles)
                        {
                            if (unityAssets.Replace(item + "\\", "").Contains(item.Replace(cate + "\\", "")) && unityAssets.Replace(item + "\\", "").Contains("unity3d") == false && unityAssets.Replace(item + "\\", "").Contains("English") == false)
                            {
                                if (IsStringInFile(unityAssets, "Bypass_Hash_Verification"))
                                    continue;
                                toolStripStatusLabel1.Text = unityAssets.Replace(item + "\\", "");
                                using (StreamWriter sw = File.AppendText(unityAssets))
                                {
                                    sw.WriteLine(Environment.NewLine);
                                    sw.WriteLine("Bypass_Hash_Verification");
                                }
                                Thread.Sleep(50);
                                //MessageBox.Show(item.Replace(cate + "\\", "") + Environment.NewLine + Environment.NewLine + unityAssets + Environment.NewLine + Environment.NewLine + unityAssets.Replace(item + "\\", ""));
                            }else
                            {
                                continue;
                            }
                        }
                    }
                }
            }

            toolStripStatusLabel1.Text = "Done!";
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            //ScanContentFolder(TextBox1.Text);
        }
    }
}
