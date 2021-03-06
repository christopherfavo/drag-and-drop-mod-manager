﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

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

            string[] initArchiveSize = new string[4]
            {
                new FileInfo(dsdir + "\\dvdbnd0.bdt").Length.ToString(),
                new FileInfo(dsdir + "\\dvdbnd1.bdt").Length.ToString(),
                new FileInfo(dsdir + "\\dvdbnd2.bdt").Length.ToString(),
                new FileInfo(dsdir + "\\dvdbnd3.bdt").Length.ToString()
            };

            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\backup")))
            {
                try
                {
                    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\backup"));
                }
                catch
                {
                    Log("\"Workspace\\backup\" is missing, and could not create it. Could not create backup files for crashDetect to restore from.\r\n");
                    Log("line");
                }
            }

            if (Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\backup")))
            {
                File.WriteAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\backup\\initarchivesize.txt"), initArchiveSize);

                for (int i = 0; i < 4; i++)
                {
                    File.WriteAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\backup\\dvdbnd" + i + ".bhd5"), backup[i]);
                }
            }

            List<bool> modifiedBhd5 = new List<bool>(new bool[4]);

            StringReader reader = new StringReader(Properties.Resources.filenames);
            string[] filenames = EnumerateLines(reader).ToArray();
            string text = "";

            int matching = -1;

            if (listView1.Items.Count == 0 || checkBox1.Checked) loadFolder(dsdir);

            specialCases();

            if (listView1.Items.Count == 1) text = "mod.\r\n";
            else text = "mods.\r\n";

            Log("Preparing to install " + listView1.Items.Count + text);
            Log("line");

            List<UInt32> special = new List<UInt32>(new UInt32[7]);
            List<string> possibleDSFixSpecial = new List<string> { };

            foreach (ListViewItem mod in listView1.Items)
            {
                bool l = false;
                int id = 0;
                bool dcx = true;

                if (File.Exists(mod.Text))
                {
                    while (!l & id < filenames.Length)
                    {
                        if (Path.GetFileName(mod.Text) == Path.GetFileName(filenames[id]))
                        {
                            checkIfSpecialCase(Path.GetFileName(mod.Text), ref special, ref id);

                            Log("Found \"" + Path.GetFileName(mod.Text) + "\" in the filenames list at line " + id + ".\r\n");
                            matching = id;
                            l = true;                            
                        }
                        else if (Path.GetFileName(mod.Text) + ".dcx" == Path.GetFileName(filenames[id]))
                        {
                            checkIfSpecialCase(Path.GetFileName(mod.Text) + ".dcx", ref special, ref id);

                            Log("Found DCX version of \"" + Path.GetFileName(mod.Text) + "\" in the filenames list at line " + id + ".\r\n");
                            matching = id;
                            l = true;
                            dcx = false;
                        }
                        else id++;
                    }

                    if (!l)
                    {
                        text = "Did not find \"" + Path.GetFileName(mod.Text) + "\" in the filenames list. ";
                        string ext = Path.GetExtension(mod.Text);

                        if (ext == ".png" || ext == ".dds" || ext == ".jpg" || ext == ".tga" || ext == ".bmp")
                        {
                            possibleDSFixSpecial.Add(mod.Text);
                            text += "Handling as possible DSFix texture mod.\r\n";
                        }
                        else
                        {
                            text += "Ignoring file.\r\n";
                        }

                        Log(text);
                        log.Text += text;
                        Log("line");
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

                        BinaryWriter bdt = new BinaryWriter(File.Open(dsdir + "\\dvdbnd" + bhd5 + ".bdt", FileMode.Append));
                        byte[] modFile;

                        if (!dcx)
                        {
                            Log("Compressing \"" + Path.GetFileName(mod.Text) + "\" to \"" + Path.GetFileName(mod.Text) + ".dcx\".\r\n");
                            modFile = rebuildDCX(File.ReadAllBytes(mod.Text));
                        }
                        else
                        {
                            modFile = File.ReadAllBytes(mod.Text);
                        }

                        modEntry.hash = dvdbnd[bhd5].buckets[bucket].entries[entry].hash;
                        modEntry.size = (UInt32)modFile.Length;
                        modEntry.offset = (UInt64)new FileInfo(dsdir + "\\dvdbnd" + bhd5 + ".bdt").Length;

                        Log("Modifying \"dvdbnd" + bhd5 + ".bhd5\", bucket " + bucket + ", entry " + entry + "\r\n" +
                                "Old size: " + oldEntry.size + ", old offset: " + oldEntry.offset + "\r\n" +
                                "New size: " + modEntry.size + ", new offset: " + modEntry.offset + "\r\n");

                        dvdbnd[bhd5].buckets[bucket].entries[entry] = modEntry;
                        
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
                    text = "Could not install file \"" + mod.Text + "\" as it does not exist.\r\n";
                    Log(text);
                    log.Text += text;
                }
            }

            foreach (string dsfixmod in possibleDSFixSpecial)
            {
                string path = Path.Combine(dsdir, "dsfix\\tex_override\\" + Path.GetFileName(dsfixmod));

                if (File.Exists(path))
                {
                    if (!File.Exists(path + ".bak"))
                    {
                        File.Copy(path, path + ".bak");
                    }
                    File.Delete(path);
                }
                if (File.Exists(dsfixmod))
                {
                    File.Copy(dsfixmod, path);
                    Log("Placed possible DSFix texture mod \"" + dsfixmod + "\" into \"dsfix\\tex_override\".\r\n");
                }
                else
                {
                    Log("\"" + dsfixmod + "\" does not exist, could not be installed.\r\n");
                }               
            }

            if (possibleDSFixSpecial.Count != 0)
            {
                text = "DSFix mods installed. Note that for these mods to show up in-game, you still have to manually set \"enableTextureOverride\" in \"DSFix.ini\".\r\n";

                Log("line");
                Log(text);
                log.Text += text;
                Log("line");
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
                    bdt.SetLength(long.Parse(initArchiveSize[i]));

                    bdt.Close();

                    Log("Restored \"dvdbnd" + i + ".bdt\"\r\n");
                }
                else
                {
                    Log("No need to restore \"dvdbnd" + i + ".bhd5\" and \"dvdbnd" + i + ".bdt\" as they were not modified.\r\n");
                }
            }

            if (possibleDSFixSpecial.Count != 0) Log("line");

            foreach (string dsfixmod in possibleDSFixSpecial)
            {
                string path = Path.Combine(dsdir, "dsfix\\tex_override\\" + Path.GetFileName(dsfixmod));

                if (File.Exists(path)) File.Delete(path);
                if (File.Exists(path + ".bak")) File.Copy(path + ".bak", path);                

                Log("Deleted possible DSFix texture mod \"" + Path.GetFileName(dsfixmod) + "\" from \"dsfix\\tex_override\".\r\n");
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
                        DialogResult dialogResult = MessageBox.Show("DSDAD crashed while DS was running. DSDAD will attempt to restore the dvdbnd libraries from backup files.\n\nThis operation will take a while. Continue?", "Crash Detected", MessageBoxButtons.OKCancel);
                        if (dialogResult == DialogResult.OK)  restoreCrashBackup();
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
                    string appdir = "";

                    if (Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\backup\\")))
                    {
                        appdir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\backup\\dvdbnd");

                        if (File.Exists(appdir + 0 + ".bhd5") && File.Exists(appdir + 1 + ".bhd5") && File.Exists(appdir + 2 + ".bhd5") && File.Exists(appdir + 3 + ".bhd5"))
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                string dvdbnd = Path.Combine(dsdir, "dvdbnd" + i);
                                byte[] bhd5 = new byte[] { };

                                try
                                {
                                    bhd5 = File.ReadAllBytes(appdir + i + ".bhd5");
                                    File.WriteAllBytes(dvdbnd + ".bhd5", bhd5);
                                }
                                catch
                                {
                                    MessageBox.Show("Could not restore backup files. Either the files are open in another program, or \"DATA\" is protected. Restoration has to be done manually.");
                                    enableBtns();
                                    return;
                                }
                                log.Text += "Restored \"" + Path.GetFileName(dvdbnd + ".bhd5") + "\"\n";
                            }
                        }
                        else
                        {
                            MessageBox.Show("Cannot restore dvdbnd.bhd5 header files as their backup files are missing from \"Workspace\\backup\"");
                            enableBtns();
                            return;
                        }

                        if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\backup\\initarchivesize.txt")))
                        {
                            string[] initSize = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\backup\\initarchivesize.txt"));

                            for (int i = 0; i < 4; i++)
                            {
                                string dvdbnd = Path.Combine(dsdir, "dvdbnd" + i);

                                try
                                {
                                    FileStream bdt = new FileStream(dsdir + "\\dvdbnd" + i + ".bdt", FileMode.Open);
                                    bdt.SetLength(long.Parse(initSize[i]));

                                    bdt.Close();
                                }
                                catch
                                {
                                    MessageBox.Show("Could not restore backup files. Either the files are open in another program, or \"DATA\" is protected. Restoration has to be done manually.");
                                    enableBtns();
                                    return;
                                }

                                log.Text += "Restored \"" + Path.GetFileName(dvdbnd + ".bdt") + "\"\n";
                            }
                        }
                        else
                        {
                            MessageBox.Show("Cannot uninstall mods from the dvdbnd.bdt libraries, as one of the key backup files (\"Workspace\\backup\\initarchivesize.txt\") is missing.");
                            enableBtns();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("\"Workspace\\backup\" does not exist. Cannot restore backups.\nAttempting to create \"Workspace\\backup\"");
                        try
                        {
                            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace\\backup\\"));
                        }
                        catch (UnauthorizedAccessException)
                        {
                            MessageBox.Show("Could not create \"Workspace\\backup\" as \"Workspace\" is protected.");
                        }
                        catch
                        {
                            MessageBox.Show("Could not create \"Workspace\\backup\".");
                        }
                        enableBtns();
                        return;
                    }                    

                    MessageBox.Show("Backup files restored successfully");
                    Log("line");
                    Log("Restore after crash complete. Preventing CrashDetect from tripping falsely:\r\n");
                    Log("Program terminated successfully.\r\n");
                }
                else corrupt = true;
            }
            else corrupt = true;

            if (corrupt) MessageBox.Show("\"settings.config\" got corrupted. DSDAD cannot restore the dvdbnd libraries. Restoration has to be done manually.");
            enableBtns();
        }

        private void loadFolder(string path)
        {
            if (!checkBox1.Checked) Log("No files were added to the program. Loading mods from \"\\dadmod\".\r\n");
            else Log("Loading extra mod files from \"\\dadmod\".\r\n");
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
                        foreach (ListViewItem setupmod in listView1.Items)
                        {
                            if (Path.GetFileName(line) == Path.GetFileName(setupmod.Text)) setupmod.Remove();
                        }
                        listView1.Items.Add(line);
                    }
                    Log("Loading Mod Setup\"" + Path.GetFileName(mod) + "\"\r\n");
                }
                else
                {
                    foreach (ListViewItem setupmod in listView1.Items)
                    {
                        if (Path.GetFileName(mod) == Path.GetFileName(setupmod.Text)) setupmod.Remove();
                    }
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
                Directory.CreateDirectory(Path.Combine(path, "Workspace\\backup"));
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
                //settings.createBak(dsdir);

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

        private void specialCases()
        {
            List<string> item, menu, local, talkccm, talktpf, dsccm, dstpf;

            item = new List<string> { };
            menu = new List<string> { };
            local = new List<string> { };
            talkccm = new List<string> { };
            talktpf = new List<string> { };
            dsccm = new List<string> { };
            dstpf = new List<string> { };

            foreach (ListViewItem mod in listView1.Items)
            {
                string name = Path.GetFileName(mod.Text);
                if (name.Substring(name.Length - 4) == ".dcx")
                {
                    name = name.Remove(name.Length - 4);
                }

                switch (name)
                {
                    /*
                    case "item.msgbnd.dcx": item.Add(mod.Text);
                        break;
                    case "menu.msgbnd.dcx": menu.Add(mod.Text);
                        break;
                    case "DSFont24.ccm.dcx": dsccm.Add(mod.Text);
                        break;
                    case "DSFont24.tpf.dcx": dstpf.Add(mod.Text);
                        break;
                    case "TalkFont24.ccm.dcx": talkccm.Add(mod.Text);
                        break;
                    case "TalkFont24.tpf.dcx": talktpf.Add(mod.Text);
                        break;
                    case "menu_local.tpf.dcx": local.Add(mod.Text);
                        break;
                        */
                    case "item.msgbnd": item.Add(mod.Text);
                        break;
                    case "menu.msgbnd": menu.Add(mod.Text);
                        break;
                    case "DSFont24.ccm": dsccm.Add(mod.Text);
                        break;
                    case "DSFont24.tpf": dstpf.Add(mod.Text);
                        break;
                    case "TalkFont24.ccm": talkccm.Add(mod.Text);
                        break;
                    case "TalkFont24.tpf": talktpf.Add(mod.Text);
                        break;
                    case "menu_local.tpf": local.Add(mod.Text);
                        break;
                    default:
                        break;
                }
            }
            string text = "";

            int count = item.Count;
            if (item.Count != 0 && item.Count < 10)
            {                
                for (int i = count; i < 10; i++)
                {
                    listView1.Items.Add(item[0]);
                }
                text = "Special case found: \"item.msgbnd.dcx\" requires multiple installs. (10 total)\r\n";
                Log(text);
                log.Text += text;
            }
            
            count = menu.Count;
            if (menu.Count != 0 && menu.Count < 10)
            {                
                for (int i = count; i < 10; i++)
                {
                    listView1.Items.Add(menu[0]);
                }
                text = "Special case found: \"menu.msgbnd.dcx\" requires multiple installs. (10 total)\r\n";
                Log(text);
                log.Text += text;
            }
            
            count = local.Count;
            if (local.Count != 0 && local.Count < 9)
            {
                for (int i = count; i < 9; i++)
                {
                    listView1.Items.Add(local[0]);
                }
                text = "Special case found: \"menu_local.tpf.dcx\" requires multiple installs. (9 total)\r\n";
                Log(text);
                log.Text += text;
            }

            count = talkccm.Count;
            if (talkccm.Count != 0 && talkccm.Count < 5)
            {                
                for (int i = count; i < 5; i++)
                {
                    listView1.Items.Add(talkccm[0]);
                }
                text = "Special case found: \"TalkFont24.ccm.dcx\", requires multiple installs. (5 total)\r\n";
                Log(text);
                log.Text += text;
            }

            count = talktpf.Count;
            if (talktpf.Count != 0 && talktpf.Count < 5)
            {                
                for (int i = count; i < 5; i++)
                {
                    listView1.Items.Add(talktpf[0]);
                }
                text = "Special case found: \"TalkFont24.tpf.dcx\", requires multiple installs. (5 total)\r\n";
                Log(text);
                log.Text += text;
            }

            count = dsccm.Count;
            if (dsccm.Count != 0 && dsccm.Count < 5)
            {
                for (int i = count; i < 5; i++)
                {
                    listView1.Items.Add(dsccm[0]);
                }
                text = "Special case found: \"DSFont24.ccm.dcx\", requires multiple installs. (5 total)\r\n";
                Log(text);
                log.Text += text;
            }

            count = dstpf.Count;
            if (dstpf.Count != 0 && dstpf.Count < 5)
            {                
                for (int i = count; i < 5; i++)
                {
                    listView1.Items.Add(dstpf[0]);
                }
                text = "Special case found: \"DSFont24.tpf.dcx\", requires multiple installs. (5 total)\r\n";
                Log(text);
                log.Text += text;
            }
        }

        private void checkIfSpecialCase(string name, ref List<UInt32> list, ref int matching)
        {
            switch (name)
            {
                case "item.msgbnd.dcx":
                    if (list[0] < 20)
                    {
                        matching += (int)list[0];
                        list[0] += 2;
                    }
                    break;
                case "menu.msgbnd.dcx":
                    if (list[1] < 20)
                    {
                        matching += (int)list[1];
                        list[1] += 2;
                    }
                    break;
                case "DSFont24.ccm.dcx":
                    if (list[2] < 25)
                    {
                        matching += (int)list[2];
                        list[2] += 4;
                    }
                    break;
                case "DSFont24.tpf.dcx":
                    if (list[3] < 25)
                    {
                        matching += (int)list[3];
                        list[3] += 4;
                    }
                    break;
                case "TalkFont24.ccm.dcx":
                    if (list[4] < 25)
                    {
                        matching += (int)list[4];
                        list[4] += 4;
                    }
                    break;
                case "TalkFont24.tpf.dcx":
                    if (list[5] < 25)
                    {
                        matching += (int)list[5];
                        list[5] += 4;
                    }
                    break;
                case "menu_local.tpf.dcx":
                    matching += (int)list[6];

                    switch (list[6])
                    {
                        case 0: list[6] = 4;
                            break;
                        case 4: list[6] = 5;
                            break;
                        case 5: list[6] = 8;
                            break;
                        case 8: list[6] = 9;
                            break;
                        case 9: list[6] = 22;
                            break;
                        case 22: list[6] = 23;
                            break;
                        case 23: list[6] = 24;
                            break;
                        case 24: list[6] = 25;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        private byte[] rebuildDCX(byte[] mod)
        {
            byte[] dcx;

            Stream result = new MemoryStream();

            DeflateStream zip = new DeflateStream(result, System.IO.Compression.CompressionMode.Compress, true);

            zip.Write(mod, 0, mod.Length);
            zip.Close();

            byte[] compressed = new byte[result.Length];

            result.Position = 0;
            result.Read(compressed, 0, (int)result.Length);
            result.Close();

            dcx = new byte[78 + compressed.Length + 2];

            Array.Copy(System.Text.Encoding.ASCII.GetBytes("DCX"), 0, dcx, 0, 3);
            Array.Copy(toBigEndian(0x10000), 0, dcx, 4, 4);
            Array.Copy(toBigEndian(0x18), 0, dcx, 8, 4);
            Array.Copy(toBigEndian(0x24), 0, dcx, 12, 4);
            Array.Copy(toBigEndian(0x24), 0, dcx, 16, 4);
            Array.Copy(toBigEndian(0x2C), 0, dcx, 20, 4);
            Array.Copy(System.Text.Encoding.ASCII.GetBytes("DCS"), 0, dcx, 24, 3);
            Array.Copy(toBigEndian(mod.Length), 0, dcx, 28, 4);
            Array.Copy(toBigEndian(compressed.Length + 4), 0, dcx, 32, 4);
            Array.Copy(System.Text.Encoding.ASCII.GetBytes("DCP"), 0, dcx, 36, 3);
            Array.Copy(System.Text.Encoding.ASCII.GetBytes("DFLT"), 0, dcx, 40, 4);
            Array.Copy(toBigEndian(0x20), 0, dcx, 44, 4);
            Array.Copy(toBigEndian(0x9000000), 0, dcx, 48, 4);
            Array.Copy(toBigEndian(0x10100), 0, dcx, 64, 4);
            Array.Copy(System.Text.Encoding.ASCII.GetBytes("DCA"), 0, dcx, 68, 3);
            Array.Copy(toBigEndian(0x8), 0, dcx, 72, 4);
            Array.Copy(toBigEndian(0x78DA0000), 0, dcx, 76, 4);

            Array.Copy(compressed, 0, dcx, 78, compressed.Length);

            return dcx;
        }

        private byte[] toBigEndian(int number)
        {
            byte[] bytes = new byte[4];

            bytes = BitConverter.GetBytes(number);
            Array.Reverse(bytes);

            return bytes;
        }
    }
}
