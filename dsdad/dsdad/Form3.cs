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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private List<CheckBox> checkboxes = new List<CheckBox> { };

        private void uncheckAll(object sender, MouseEventArgs e)
        {
            uncheckAllChB();
        }

        private void uncheckAllChB()
        {
            bool l = true;

            foreach (TabPage page in tabControl1.TabPages)
            {
                foreach (CheckBox box in page.Controls.OfType<CheckBox>())
                {
                    l &= box.Checked;
                }
            }

            allChB.Checked = l;
        }

        private void allCheck(object sender, MouseEventArgs e)
        {
            foreach (TabPage page in tabControl1.TabPages)
            {
                foreach (CheckBox box in page.Controls.OfType<CheckBox>())
                {
                    box.Checked = allChB.Checked;
                }
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            string path = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\settings.config"));

            if (!File.Exists(path))
            {
                MessageBox.Show("Set the location of \"DARKSOULS.exe\" in \"Settings -> EXE Location\" to be able to modify it.");
                this.Close();
            }
            else
            {
                BinaryReader exeReader = new BinaryReader(File.Open(path, FileMode.Open), System.Text.Encoding.Unicode);

                debugChB.Checked = EXE.debCheck(exeReader);
                EXE.exeCheck(exeReader, debugChB.Checked);
                dcxChB.Checked = EXE.dcxCheck(exeReader, debugChB.Checked);

                exeReader.Close();

                addCheckboxes();
                int j = 0;

                for (int i = 0; i < checkboxes.Count; i++)
                {
                    switch (i)
                    {
                        default:
                            checkboxes[i].Checked = EXE.isModified[j];
                            j++;
                            break;
                        case 8:
                            checkboxes[i].Checked = EXE.isModified[j];
                            j += 3;
                            break;
                        case 17:
                            checkboxes[i].Checked = EXE.isModified[j];
                            j += 2;
                            break;
                    };
                }

                if (dcxChB.Checked)
                {
                    uncheckAllChB();
                }
                else
                {
                    uncheckDcxChB();
                }
            }
        }

        private void addCheckboxes()
        {
            checkboxes.Add(menu);
            checkboxes.Add(shader);
            checkboxes.Add(font);
            checkboxes.Add(msg);
            checkboxes.Add(chr);
            checkboxes.Add(obj);
            checkboxes.Add(parts);
            checkboxes.Add(map);
            checkboxes.Add(remo);
            checkboxes.Add(mtd);
            checkboxes.Add(param);
            checkboxes.Add(paramdef);
            checkboxes.Add(facegen);
            checkboxes.Add(sound);
            checkboxes.Add(other);
            checkboxes.Add(script);
            checkboxes.Add(sfx);
            checkboxes.Add(dvdbndEvent);
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void modifyBtn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("This action will modify \"DARKSOULS.exe\". Installing any mods originating from the checked directories via DSDAD will not show up in-game.\n\nDo you wish to continue?", "", MessageBoxButtons.OKCancel);
            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            if (!dcxChB.Checked)
            {
                MessageBox.Show("Non DCX-compression mode is only allowed on unpacked libraries. Make sure all files are uncompressed, otherwise the game will crash.");
            }
            else
            {
                if (allChB.Checked)
                {
                    MessageBox.Show("Warning!\n\nSince all files are being loaded from the unpacked libraries, any mods installed via DSDAD will not show up in-game.");
                }
            }


            modifyBtn.Enabled = false;
            cancelBtn.Enabled = false;
            tabControl1.Enabled = false;
            allChB.Enabled = false;

            int j = 0;

            for (int i = 0; i < EXE.isModified.Count; i++)
            {
                switch (i)
                {
                    default:
                        EXE.isModified[i] = checkboxes[j].Checked;
                        j++;
                        break;
                    case 9:
                    case 10:
                        EXE.isModified[i] = checkboxes[j-2].Checked;
                        break;
                    case 18:
                        EXE.isModified[i] = checkboxes[j-1].Checked;
                        break;
                }
            }

            EXE.modifyExe(debugChB.Checked, dcxChB.Checked);

            this.Close();
        }

        private void dcxChB_CheckedChanged(object sender, EventArgs e)
        {
            uncheckDcxChB();
        }

        private void uncheckDcxChB()
        {
            foreach (TabPage page in tabControl1.TabPages)
            {
                foreach (CheckBox box in page.Controls.OfType<CheckBox>())
                {
                    box.Checked = !dcxChB.Checked;
                    box.Enabled = dcxChB.Checked;
                }
            }

            allChB.Checked = !dcxChB.Checked;
            allChB.Enabled = dcxChB.Checked;
        }
    }
}
