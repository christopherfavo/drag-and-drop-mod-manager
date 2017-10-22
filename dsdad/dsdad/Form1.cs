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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void eXELocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 settings = new Form2();
            settings.ShowDialog();
        }

        private void clearLogBtn_Click(object sender, EventArgs e)
        {
            log.Text = "";
        }

        private void log_TextChanged(object sender, EventArgs e)
        {
            log.SelectionStart = log.Text.Length;
            log.ScrollToCaret();
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            panel1.Visible = true;
        }

        private void removeBtn_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem mod = listView1.SelectedItems[0];
                listView1.Items.Remove(mod);
            }

            if (listView1.Items.Count == 0)
                panel1.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace")))
            {
                MessageBox.Show("DSDAD will perform a first time run installation.");
                firstTimeRun();
            }

            Form2 settings = new Form2();
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\settings.config"))) settings.ShowDialog();

            /*
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\log.txt")))
            {
                string[] lines = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\log.txt"));
                if (lines.Length > 1)
                {
                    if (lines[lines.Length - 1].Substring(9) != "Program terminated successfully.")
                    {
                        MessageBox.Show("DSDAD crashed while DS was running. DSDAD will attempt to restore the dvdbnd libraries from backup files.\n\nThis operation will take a while.");

                        restoreCrashBackup();
                    }
                }
            }
            */
        }


        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            string[] mods = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (string mod in mods)
            {
                if (Path.GetExtension(mod) == ".stp")
                {
                    string[] setup = File.ReadAllLines(mod);

                    foreach (string line in setup)
                    {
                        listView1.Items.Add(line);
                    }

                    log.Text += "Loaded mods from Mod Setup: \"" + Path.GetFileName(mod) + "\"\n";
                }
                else
                {
                    listView1.Items.Add(mod);
                }
            }

            panel1.Visible = false;
        }

        private void launchBtn_Click(object sender, EventArgs e)
        {
            disableBtns();

            Log("line");
            Log("Initiating launching sequence.\r\n");
            Log("line");

            string dsdir = "";

            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\settings.config")))
            {
                dsdir = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\settings.config"));
            }

            if (File.Exists(dsdir) & Path.GetFileName(dsdir) == "DARKSOULS.exe")
            {
                BinaryReader exeReader = new BinaryReader(File.Open(dsdir, FileMode.Open), System.Text.Encoding.Unicode);
                EXE.exeCheck(exeReader, EXE.debCheck(exeReader));
                exeReader.Close();
                bool l = false;

                foreach (bool mod in EXE.isModified)
                {
                    l |= mod;
                }

                if (l)
                {
                    DialogResult dialogresult = MessageBox.Show("One or more of the main directories are loaded from unpacked libraries. Some mods might not show up in-game.\n\nTo change this, go to \"Settings -> Modify EXE\"\n\nDo you wish to continue?","Launch Dark Souls w/ mods", MessageBoxButtons.OKCancel);
                    if (dialogresult != DialogResult.OK)
                    {
                        Log("User chose to modify \"DARKSOULS.exe\".\r\n");
                        Log("line");
                        Log("Program terminated successfully.\r\n");
                        enableBtns();
                        return;
                    }
                }

                dsdir = Path.GetDirectoryName(dsdir);
            }
            else
            {
                MessageBox.Show("Please specify the loaction of \"DARKSOULS.exe\" in \"Settings -> EXE Loaction\" to be able to install mods to the game.");
                Log("Path to \"DARKSOULS.exe\" was not specifed.\r\n");
                Log("line");
                Log("Program terminated successfully.\r\n");

                enableBtns();

                return;
            }

            if (!canBeOpened(dsdir))
            {
                MessageBox.Show("The dvdbnd libraries of Dark Souls cannot be opened, as it's either opened in another program, or protected.");
                Log("The dvdbnd libraries cannot be opened.\r\n");
                Log("line");
                Log("Program terminated successfully.\r\n");

                enableBtns();

                return;
            }

            List<byte[]> backup = new List<byte[]>
            {
                File.ReadAllBytes(dsdir + "\\dvdbnd0.bhd5"),
                File.ReadAllBytes(dsdir + "\\dvdbnd1.bhd5"),
                File.ReadAllBytes(dsdir + "\\dvdbnd2.bhd5"),
                File.ReadAllBytes(dsdir + "\\dvdbnd3.bhd5")
            };

            List<BHD5.Bhd5Struct> dvdbnd = new List<BHD5.Bhd5Struct>
            {
                BHD5.GetBhd5Data(dsdir + "\\dvdbnd0.bhd5"),
                BHD5.GetBhd5Data(dsdir + "\\dvdbnd1.bhd5"),
                BHD5.GetBhd5Data(dsdir + "\\dvdbnd2.bhd5"),
                BHD5.GetBhd5Data(dsdir + "\\dvdbnd3.bhd5")
            };

            List<UInt64> initArchiveSize = new List<UInt64>
            {
                (UInt64)new FileInfo(dsdir + "\\dvdbnd0.bdt").Length,
                (UInt64)new FileInfo(dsdir + "\\dvdbnd1.bdt").Length,
                (UInt64)new FileInfo(dsdir + "\\dvdbnd2.bdt").Length,
                (UInt64)new FileInfo(dsdir + "\\dvdbnd3.bdt").Length
            };

            List<bool> modifiedBhd5 = new List<bool>(new bool[4]);

            StringReader reader = new StringReader(Properties.Resources.filenames);
            string[] filenames = EnumerateLines(reader).ToArray();
            string text = "";

            int matching = -1;

            if (listView1.Items.Count == 0) loadFolder(dsdir);

            Log("Preparing to install " + listView1.Items.Count + " mods.\r\n");
            Log("line");

            foreach (ListViewItem mod in listView1.Items)
            {
                bool l = false;
                int id = 0;

                if (File.Exists(mod.Text))
                {
                    while (!l & id < filenames.Length)
                    {
                        if (Path.GetFileName(mod.Text) == Path.GetFileName(filenames[id]))
                        {
                            Log("Found \"" + Path.GetFileName(mod.Text) + "\" in the filenames list at line " + id + ".\r\n");
                            matching = id;
                            l = true;
                        }
                        else id++;
                    }

                    if (!l)
                    {
                        text = "Did not find \"" + Path.GetFileName(mod.Text) + "\" in the filenames list. Ignoring file.\r\n";
                        Log(text);
                        log.Text += text;
                        matching = -1;
                    }

                    if (matching != -1)
                    {
                        UInt32 hash = BHD5.GetHash(filenames[matching]);
                        Log("Hash for \"" + Path.GetFileName(filenames[matching]) + "\" is: " + hash + "\r\n");

                        int bucket = 0;
                        int entry = 0;
                        int bhd5 = 3;
                        l = false;

                        while (!l & bhd5 > -1)
                        {
                            l = BHD5.FindHash(dvdbnd[bhd5], hash, ref bucket, ref entry);
                            if (!l)
                            {
                                Log("Did not find \"" + Path.GetFileName(mod.Text) + "\" in dvdbnd" + bhd5 + ".bhd5\r\n");
                                bhd5--;
                            }
                        }

                        if (!l)
                        {
                            text = "Could not find hash in any of the bhd5 files (should be impossible).\r\n";
                            log.Text += text;
                            Log(text);
                        }
                        else
                        {
                            Log("Found \"" + Path.GetFileName(mod.Text) + "\" in \"dvdbnd" + bhd5 + ".bhd5\", in bucket " + bucket + " at entry " + entry + "\r\n");
                        }

                        BHD5.EntryStruct modEntry = new BHD5.EntryStruct();
                        BHD5.EntryStruct oldEntry = dvdbnd[bhd5].buckets[bucket].entries[entry];


                        modEntry.hash = dvdbnd[bhd5].buckets[bucket].entries[entry].hash;
                        modEntry.size = (UInt32)new FileInfo(mod.Text).Length;
                        modEntry.offset = (UInt64)new FileInfo(dsdir + "\\dvdbnd" + bhd5 + ".bdt").Length;

                        Log("Modifying \"dvdbnd" + bhd5 + ".bhd5\", bucket " + bucket + ", entry " + entry + "\r\n" +
                                "Old size: " + oldEntry.size + ", old offset: " + oldEntry.offset + "\r\n" +
                                "New size: " + modEntry.size + ", new offset: " + modEntry.offset + "\r\n");

                        dvdbnd[bhd5].buckets[bucket].entries[entry] = modEntry;

                        BinaryWriter bdt = new BinaryWriter(File.Open(dsdir + "\\dvdbnd" + bhd5 + ".bdt", FileMode.Append));
                        byte[] modFile = File.ReadAllBytes(mod.Text);

                        bdt.Write(modFile);

                        bdt.Close();

                        modifiedBhd5[bhd5] |= l;

                        Log("Installed \"" + mod.Text + "\" to \"dvdbnd" + bhd5 + ".bdt\"\r\n");
                        Log("line");
                        log.Text += "Installed \"" + Path.GetFileName(mod.Text) + "\".\n";
                    }
                }
                else
                {
                    text = "Could not install file \"" + mod.Text + "\" as it does not exist.";
                    Log(text);
                    log.Text += text;
                }

            }

            for (int i = 0; i < 4; i++)
            {
                if (modifiedBhd5[i])
                {
                    BHD5.RewriteBhd5(dvdbnd[i], dsdir + "\\dvdbnd" + i + ".bhd5");
                    Log("Rewritten \"dvdbnd" + i + ".bhd5\"\r\n");
                }
            }

            bool noModified = false;
            foreach (bool item in modifiedBhd5) noModified |= item;

            if (!noModified) Log("No mods were installed.\r\n");

            Log("line");

            runDS();

            Log("Restoring backups.\r\n");

            for (int i = 0; i < 4; i++)
            {
                if (modifiedBhd5[i])
                {
                    BinaryWriter bhd5 = new BinaryWriter(File.Open(dsdir + "\\dvdbnd" + i + ".bhd5", FileMode.Create));
                    bhd5.Write(backup[i]);

                    bhd5.Close();

                    Log("Restored \"dvdbnd" + i + ".bhd5\"\r\n");

                    FileStream bdt = new FileStream(dsdir + "\\dvdbnd" + i + ".bdt", FileMode.Open);
                    bdt.SetLength((Int64)initArchiveSize[i]);

                    bdt.Close();

                    Log("Restored \"dvdbnd" + i + ".bdt\"\r\n");
                }
                else
                {
                    Log("No need to restore \"dvdbnd" + i + ".bhd5\" and \"dvdbnd" + i + ".bdt\" as they were not modified.\r\n");
                }
            }

            GC.Collect();

            enableBtns();
            Log("line");
            Log("Program terminated successfully.\r\n");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK )
            {
                foreach (string mod in openFileDialog2.FileNames)
                {
                    if (Path.GetExtension(mod) == ".stp")
                    {
                        string[] setup = File.ReadAllLines(mod);

                        foreach (string line in setup)
                        {
                            listView1.Items.Add(line);
                        }

                        log.Text += "Loaded mods from Mod Setup: \"" + Path.GetFileName(mod) + "\"\n";
                    }
                    else
                    {
                        listView1.Items.Add(mod);
                    }
                }
                
            }

            panel1.Visible = listView1.Items.Count == 0;
        }

        private IEnumerable<string> EnumerateLines(StringReader readl)
        {
            string line;

            while ((line = readl.ReadLine()) != null)
            {
                yield return line.Substring(1, line.Length - 1);
            }
        }

        private void runDS()
        {
            string text = "Launching Dark Souls...\r\n";
            log.Text += text;
            Log(text);

            System.Diagnostics.Process dslink = new System.Diagnostics.Process();
            dslink.StartInfo.FileName = "steam://rungameid/211420"; //System.IO.File.ReadAllText(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Workspace\\settings.config"));
            dslink.Start();

            bool running = false;
            int attempt = 0;
            System.Diagnostics.Process[] processes = { };

            Log("Attempting to latch onto the Dark Souls process\r\n");

            while (!running & attempt < 5)
            {
                Log("Attempt " + (attempt + 1) + "\r\n");
                System.Threading.Thread.Sleep(1000);
                processes = System.Diagnostics.Process.GetProcessesByName("DARKSOULS");
                running = processes.Length != 0;
                attempt++;
            }

            if (running)
            {
                text = "Dark Souls is running.\r\n";
                log.Text += text + "Please do not close DSDAD until you quit Dark Souls!\r\n";
                Log(text + "Waiting for Dark Souls to exit...\r\n");

                processes[0].WaitForExit();

                text = "Dark Souls has finished running.\r\n";
                log.Text += text;
                Log(text);
                Log("line");
            }
            else
            {
                MessageBox.Show("An error occured while trying to launch Dark Souls.");

                text = "Could not launch Dark Souls.\r\n";
                log.Text += text;
                Log(text);
            }
        }

        private void Log(string text)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\log.txt");

            if (!File.Exists(path)) File.Create(path).Dispose();

            StreamWriter logFile = File.AppendText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\log.txt"));
            if (text != "line")
            {
                logFile.Write(DateTime.Now.ToString("HH:mm:ss ") + text);
            }
            else
            {
                logFile.Write("-------------------------------------\r\n");
            }
            logFile.Close();
        }

        private void disableBtns()
        {

            foreach (Button button in this.Controls.OfType<Button>())
            {
                button.Enabled = false;
            }
            menuStrip1.Enabled = false;

        }

        private void enableBtns()
        {

            foreach (Button button in this.Controls.OfType<Button>())
            {
                button.Enabled = true;
            }
            menuStrip1.Enabled = true;

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            crashDetect();

            string[] args = Environment.GetCommandLineArgs();

            if (args.Length != 1)
            {
                for (int i = 1; i < args.Length; i++)
                {
                    if (Path.GetExtension(args[i]) == ".stp")
                    {
                        string[] setup = File.ReadAllLines(args[i]);

                        foreach (string line in setup)
                        {
                            listView1.Items.Add(line);
                        }

                        log.Text += "Loaded mods from Mod Setup: \"" + Path.GetFileName(args[i]) + "\"\n";
                    }
                    else
                    {
                        listView1.Items.Add(args[i]);
                        log.Text += "Loaded mod \"" + Path.GetFileName(args[i]) + "\"\n";
                    }
                }
            }

            panel1.Visible = listView1.Items.Count == 0;
        }

        private void crashDetect()
        {
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\log.txt")))
            {
                string[] lines = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\log.txt"));
                if (lines.Length > 1)
                {
                    if (lines[lines.Length - 1].Substring(9) != "Program terminated successfully.")
                    {
                        log.Text += "Attempting to restore dvdbnd libraries...\nPlease wait.\n";
                        MessageBox.Show("DSDAD crashed while DS was running. DSDAD will attempt to restore the dvdbnd libraries from backup files.\n\nThis operation will take a while.");

                        restoreCrashBackup();
                    }
                }
            }
        }

        private void restoreCrashBackup()
        {
            disableBtns();
            bool corrupt = false;

            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\settings.config")))
            {
                string dsdir = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\settings.config"));

                if (File.Exists(dsdir) & Path.GetFileName(dsdir) == "DARKSOULS.exe")
                {
                    dsdir = Path.GetDirectoryName(dsdir);
                    string cannot = "Cannot restore the following:\n";
                    bool l = false;

                    for (int i = 0; i < 4; i++)
                    {
                        string dvdbnd = Path.Combine(dsdir, "dvdbnd" + i);

                        if (File.Exists(dvdbnd + ".bhd5.bak"))
                        {
                            try
                            {
                                File.Delete(dvdbnd + ".bhd5");
                                File.Copy(dvdbnd + ".bhd5.bak", dvdbnd + ".bhd5");
                            }
                            catch
                            {
                                MessageBox.Show("Could not restore backup files. Restoration has to be done manually.");
                                return;
                            }

                            log.Text += "Restored \"" + Path.GetFileName(dvdbnd + ".bhd5") + "\"\n";
                        }
                        else
                        {
                            cannot += "-\"" + Path.GetFileName(dvdbnd + ".bhd5") + "\"\n";
                            l = true;
                        }

                        if (File.Exists(dvdbnd + ".bdt.bak"))
                        {
                            try
                            {
                                File.Delete(dvdbnd + ".bdt");
                                File.Copy(dvdbnd + ".bdt.bak", dvdbnd + ".bdt");
                            }
                            catch
                            {
                                MessageBox.Show("Could not restore backup files. Restoration has to be done manually.");
                                return;
                            }

                            log.Text += "Restored \"" + Path.GetFileName(dvdbnd + ".bdt") + "\"\n";
                        }
                        else
                        {
                            cannot += "-\"" + Path.GetFileName(dvdbnd + ".bdt") + "\"\n";
                            l = true;
                        }
                    }
                    if (l) MessageBox.Show(cannot + "\nas backup files do not exist for these files.");
                    else
                    {
                        MessageBox.Show("Backup files restored successfully");
                        Log("line");
                        Log("Restore after crash complete. Preventing CrashDetect from tripping falsely:\r\n");
                        Log("Program terminated successfully.\r\n");
                    }

                }
                else corrupt = true;
            }
            else corrupt = true;

            if (corrupt) MessageBox.Show("\"settings.config\" got corrupted. DSDAD cannot restore the dvdbnd libraries. Restoration has to be done manually.");
            enableBtns();
        }

        private void loadFolder(string path)
        {
            Log("No files were added to the program. Loading mods from \"\\dadmod\".\r\n");
            log.Text += "Loading mods from \"\\dadmod.\"\n";

            panel1.Visible = false;

            string[] mods = { };

            if (Directory.Exists(path + "\\dadmod"))
            {
                mods = Directory.GetFiles(path + "\\dadmod");
            }
            else
            {
                Log("\"" + path + "\\dadmod\" does not exist, and could not load any mods from it.\r\n");
                Log("Attempting to create \"\\dadmod\"\r\n");
                try
                {
                    Directory.CreateDirectory(path + "\\dadmod");
                }
                catch(UnauthorizedAccessException)
                {
                    Log("Could not create \"\\dadmod\", as \"" + path + "\" is protected.\r\n");
                }
                catch
                {
                    Log("Could not create \"\\dadmod\".\r\n");
                }

                if (Directory.Exists(path + "\\dadmod")) Log("Successfully created \"" + path + "\\dadmod\"\r\n");
            }

            foreach (string mod in mods)
            {
                if (Path.GetExtension(mod) == ".stp")
                {
                    string[] setup = File.ReadAllLines(mod);

                    foreach (string line in setup)
                    {
                        listView1.Items.Add(line);
                    }
                    Log("Loading Mod Setup\"" + Path.GetFileName(mod) + "\"\r\n");
                }
                else
                {
                    listView1.Items.Add(mod);
                }
                Log("Loading \"" + mod + "\"\r\n");
            }

            if (listView1.Items.Count == 0)
            {
                string text = "No mods were loaded, as \"\\dadmod\" is an empty folder.\r\n";
                Log(text);
                log.Text += text;

                panel1.Visible = true;
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter save = new StreamWriter(File.Open(saveFileDialog1.FileName, FileMode.Create));

                foreach (ListViewItem mod in listView1.Items)
                {
                    save.Write(mod.Text + "\n");
                }
                save.Close();

                log.Text += "Saved mod setup \"" + Path.GetFileName(saveFileDialog1.FileName) + "\".\n";
            }
        }

        private void openModSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count != 0)
            {

                DialogResult dialogResult = MessageBox.Show("You are about to open a new Setup. Current Setup will be closed.\n\nDo you wish to continue?", "Open Mod Setup", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    listView1.Items.Clear();
                }
                else
                {
                    return;
                }
            }

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] lines = File.ReadAllLines(openFileDialog1.FileName);

                foreach (string line in lines)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = line;

                    listView1.Items.Add(item);
                }

                log.Text += "Opened mod setup \"" + Path.GetFileName(openFileDialog1.FileName) + "\".\n";
            }

            if (listView1.Items.Count == 0) panel1.Visible = true;
            else panel1.Visible = false;

        }

        private void clearSteupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            panel1.Visible = true;
        }

        private void modifyEXEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 modify = new Form3();
            modify.ShowDialog();
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form4 help = new Form4();
            help.Show();
        }

        private void firstTimeRun()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                Directory.CreateDirectory(Path.Combine(path, "Workspace"));
                File.Create(Path.Combine(path, "Workspace\\settings.config")).Dispose();
                File.Create(Path.Combine(path, "Workspace\\log.txt")).Dispose();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Could not create the following: \"\\Workspace\", \"\\Workspace\\settings.config\" and \"\\Workspace\\log.txt\" as current directory is protected.");
                this.Close();
            }
            catch
            {
                MessageBox.Show("An error has occured.\nCould not create the following: \"\\Workspace\", \"\\Workspace\\settings.config\" and \"\\Workspace\\log.txt\"");
                this.Close();
            }

            Form2 settings = new Form2();
            settings.ShowDialog();

            string dsdir = File.ReadAllText(Path.Combine(path, "Workspace\\settings.config"));

            if (File.Exists(dsdir))
            {
                settings.createBak(dsdir);

                dsdir = Path.GetDirectoryName(dsdir);
                if (!Directory.Exists(Path.Combine(dsdir, "dadmod")))
                {
                    try
                    {
                        Directory.CreateDirectory(Path.Combine(dsdir, "dadmod"));
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Could not create \"\\dadmod\" as \"" + dsdir + "\" is protected.");
                    }
                    catch
                    {
                        MessageBox.Show("An error occured while creating \"" + Path.Combine(dsdir, "dadmod") + "\".");
                    }
                }
            }
            else
            {
                log.Text += "The location of \"DARKSOULS.exe\" was not specified.\n";
            }
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form5 about = new Form5();
            about.Show();
        }

        private void restoreDarkSoulsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            log.Text += "Attempting to restore dvdbnd libraries...\nPlease wait.\n";

            DialogResult dialogresult = MessageBox.Show("DSDAD will attempt to restore the dvdbnd libraries.\n\nThis operation will take a while. Continue?","Restore Dark Souls",MessageBoxButtons.OKCancel);
            if (dialogresult == DialogResult.OK)
            {
                restoreCrashBackup();
            }
            else
            {
                log.Text += "Operation cancelled.\n";
            }
        }

        private bool canBeOpened(string path)
        {
            bool l = true;

            for (int i = 0; i < 4; i++)
            {
                try
                {
                    BinaryWriter file = new BinaryWriter(File.Open(path + "\\dvdbnd" + i + ".bhd5",FileMode.Open));
                    file.Close();
                    BinaryWriter file2 = new BinaryWriter(File.Open(path + "\\dvdbnd" + i + ".bdt", FileMode.Open));
                    file2.Close();
                }
                catch
                {
                    l = false;
                }
            }

            return l;
        }
    }
}
