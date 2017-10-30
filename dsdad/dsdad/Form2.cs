using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace dsdad
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] exe = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            textBox1.Text = exe[0];

            if (exe.Length > 1) MessageBox.Show("Only one file is expected.");
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            string path;

            if (textBox1.Text != "")
            {
                path = textBox1.Text;

                if (File.Exists(path))
                {
                    if (Path.GetFileName(path) == "DARKSOULS.exe")
                    {
                        try
                        {
                            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\settings.config"), path);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            MessageBox.Show("Could not create \"\\Workspace\\settings.config\" as current directory is protected.");
                        }
                        catch
                        {
                            MessageBox.Show("An error occured while trying to create/write \"\\Workspace\\settings.config\"");
                        }

                        //createBak(path);
                        
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Specified file is not \"DARKSOULS.exe\"!");
                    }
                }
                else
                {
                    MessageBox.Show("Specified file does not exist!");
                }

            }
            else
            {
                MessageBox.Show("Please specify the location of \"DARKSOULS.exe\".");
            }

        }

        private void Form2_Activated(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\settings.config")))
                textBox1.Text = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\settings.config"));
            else File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\settings.config")).Dispose();
        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        /*
        public void createBak(string path)
        {
            string dsdir = Path.GetDirectoryName(path);
            bool l = false;

            for (int i = 0; i < 4; i++)
            {
                string dvdbnd = Path.Combine(dsdir, "dvdbnd" + i);

                l |= !File.Exists(dvdbnd + ".bhd5.bak") | !File.Exists(dvdbnd + ".bdt.bak");
            }

            if (l)
            {
                DialogResult dialogResult = MessageBox.Show("DSDAD would like to create back up files (.bak) for the dvdbnd libraries.\nThis may take a while.\n\nAllow backing up?", "", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        string dvdbnd = Path.Combine(dsdir, "dvdbnd" + i);

                        if (!File.Exists(dvdbnd + ".bhd5.bak"))
                        {
                            try
                            {
                                File.Copy(dvdbnd + ".bhd5", dvdbnd + ".bhd5.bak");
                            }
                            catch
                            {
                                MessageBox.Show("Could not create backup files, as \"" + dsdir + "\" is protected.");
                                return;
                            }
                        }
                        if (!File.Exists(dvdbnd + ".bdt.bak"))
                        {
                            try
                            {
                                File.Copy(dvdbnd + ".bdt", dvdbnd + ".bdt.bak");
                            }
                            catch
                            {
                                MessageBox.Show("Could not create backup files, as \"" + dsdir + "\" is protected.");
                                return;
                            }
                        }
                    }
                }
            }
        }
        */
    }
}
