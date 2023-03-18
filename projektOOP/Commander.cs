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
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Xml.Linq;

namespace projektOOP
{
    public partial class Commander : Form
    {
        private DriveInfo[] allDrives;
        private const string upDirName = "[..]";

        private string leftP = "";
        private string rightP = "";

        private ListViewColumnSorter leftColumnSorter = new ListViewColumnSorter();
        private ListViewColumnSorter rightColumnSorter = new ListViewColumnSorter();

        private ListView.SelectedListViewItemCollection copy = null;

        private temporaryTextBox tBox = null;

        public Commander()
        {
            InitializeComponent();
        }
        //executed on program load
        //here all setup is handled
        private void Form1_Load(object sender, EventArgs e)
        {
            allDrives = DriveInfo.GetDrives(); //get data about drives
            //add items to the combo boxes
            foreach (DriveInfo drive in allDrives)
            {
                if (drive.IsReady)
                {
                    leftDrive.Items.Add(drive.Name);
                    rightDrive.Items.Add(drive.Name);
                    //set default items because it's fun
                    leftDrive.SelectedItem = drive.Name;
                    rightDrive.SelectedItem = drive.Name;
                }
            }

            leftBox.ListViewItemSorter = leftColumnSorter;
            rightBox.ListViewItemSorter = rightColumnSorter;
            leftBox.FullRowSelect = true;
            rightBox.FullRowSelect = true;
            //set icons
            systemIcon folderIcon = new systemIcon(0x4, systemIcon.SHGSI_LARGEICON);
            iconList.Images.Add(folderIcon.icon);
            systemIcon fileIcon = new systemIcon(0x0, systemIcon.SHGSI_LARGEICON);
            iconList.Images.Add(fileIcon.icon);
            systemIcon linkIcon = new systemIcon(0x39, systemIcon.SHGSI_LARGEICON);
            iconList.Images.Add(linkIcon.icon);
            leftBox.SmallImageList = iconList;
            rightBox.SmallImageList = iconList;
            systemIcon mainIcon = new systemIcon(0x39, systemIcon.SHGSI_LARGEICON);
            this.Icon = mainIcon.icon;
            //menustrip icons
            ToolStripItemCollection options = boxMenu.Items;
            systemIcon deleteIcon = new systemIcon(0x54, systemIcon.SHGSI_LARGEICON);
            options[0].Image = deleteIcon.icon.ToBitmap();
            options[1].Image = folderIcon.icon.ToBitmap();
            systemIcon copyIcon = new systemIcon(0x37, systemIcon.SHGSI_LARGEICON);
            options[2].Image = copyIcon.icon.ToBitmap();
            systemIcon pasteIcon = new systemIcon(0x1, systemIcon.SHGSI_LARGEICON); //less than fitting but hey
            options[3].Image = pasteIcon.icon.ToBitmap();
        }

        private string getReadeableSize(long bytes)
        {
            if (bytes / 1000000000 > 0)
            {
                return (bytes / 1000000000).ToString() + " GB";
            }
            else if (bytes / 1000000 > 0)
            {
                return (bytes / 1000000).ToString() + " MB";
            }
            else if (bytes / 1000 > 0)
            {
                return (bytes / 1000).ToString() + " KB";
            }
            else
            {
                return bytes.ToString() + " B";
            }
        }

        //called when user changed either the combo box for drive or changed the directory textBox
        public string displayDirectory(string path, ListView display)
        {
            if (File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
            }
            else if (!Directory.Exists(path))
            {
                MessageBox.Show("Directory not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null; //error
            }
            //first handle subdirectories
            string[] dirEntries;
            try
            {
                dirEntries = Directory.GetDirectories(path);
            }
            catch (System.UnauthorizedAccessException) //If we are unauthorised stop the display process
            {
                MessageBox.Show("Access Denied", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            display.Items.Clear(); //reset items
            if (Directory.GetParent(path) != null) //add the move to parent directory item
            {
                ListViewItem upDir = new ListViewItem();
                upDir.SubItems.AddRange(new string[] { "<DIR>", "" });
                upDir.Text = upDirName;
                upDir.ImageIndex = 2;
                display.Items.Add(upDir);
            }
            foreach (string dir in dirEntries) //add the directories
            {
                DirectoryInfo info = new DirectoryInfo(dir);
                ListViewItem item = new ListViewItem();
                item.SubItems.AddRange(new string[] { "<DIR>", info.LastWriteTime.ToString() });
                item.Text = info.Name;
                item.ImageIndex = 0;
                display.Items.Add(item);
            }
            //then files
            string[] fileEntries = Directory.GetFiles(path);
            foreach (string file in fileEntries) //add the files
            {
                FileInfo info = new FileInfo(file);
                ListViewItem item = new ListViewItem();
                item.SubItems.AddRange(new string[] { getReadeableSize(info.Length), info.LastWriteTime.ToString() });
                item.Text = info.Name;
                item.ImageIndex = 1;
                display.Items.Add(item);
            }
            return path;
        }
        //Handle changed disk
        private void leftDrive_SelectedIndexChanged(object sender, EventArgs e)
        {
            leftPath.Text = "";
            leftP = displayDirectory(leftDrive.Text + leftPath.Text, leftBox);
        }

        private void rightDrive_SelectedIndexChanged(object sender, EventArgs e)
        {
            rightPath.Text = "";
            rightP = displayDirectory(rightDrive.Text + rightPath.Text, rightBox);
        }

        private string getBasePath(string path)
        {
            return path.Substring(Path.GetPathRoot(path).Length);
        }

        //Handle navigable folders

        //import function that explorer.exe uses for getting the executable 
        [DllImport("shell32.dll")]
        static extern int FindExecutable(string lpFile, string lpDirectory, [Out] StringBuilder lpResult);

        //called when user double clicks a ListView
        public string itemDoubleClicked(ListView display, ListViewItem item, TextBox input, string currentPath)
        {
            string path = currentPath + "\\" + item.Text;
            if (item.SubItems[1].Text == "<DIR>")
            {
                if (item.Text == upDirName)
                {
                    DirectoryInfo dir = Directory.GetParent(currentPath);
                    input.Text = getBasePath(dir.FullName);
                    return displayDirectory(dir.FullName, display);
                }
                else
                {
                    string res = displayDirectory(path, display);
                    if (res != null)
                    {
                        input.Text = getBasePath(res);
                        return res;
                    }
                    return currentPath;

                }
            }
            else if(item.SubItems[1].Text != "<NEW>")//handle double clicking files
            {
                //first we need to query the registry to get the app 
                StringBuilder lpresult = new StringBuilder();
                int result = FindExecutable(item.Text, currentPath, lpresult);
                if (result > 32) //weird why did they not use the standard 0=success?
                {
                    string name = lpresult.ToString();
                    if (name.Contains(item.Text))
                    {
                        path = "";
                    }
                    if (name.Contains("Explorer.exe") || name.Contains("explorer.exe"))
                    {
                        path = path.Replace("\\\\", "\\");
                        path += "\\";
                    }
                    Process.Start(name, path);
                }
                else if (result == 31) //no association=try opening with notepad
                {
                    Process.Start("notepad.exe", path);
                }
                else //unexpected error
                {
                    string message = "Enexpected error has occured.";
                    if (result == 5)
                    {
                        message = "Access to file denied";
                    }
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return currentPath;
                }
            }
            return currentPath;
        }

        private void leftBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = leftBox.HitTest(e.X, e.Y);
            leftP = itemDoubleClicked(leftBox, info.Item, leftPath, leftP);
        }

        private void rightBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = rightBox.HitTest(e.X, e.Y);
            rightP = itemDoubleClicked(rightBox, info.Item, rightPath, rightP);
        }
        //Handle clickable columns
        public void boxSort(ListView box, ListViewColumnSorter sorter, int columnId)
        {
            //based on https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/sort-listview-by-column
            if (columnId == sorter.SortColumn)
            {
                if (sorter.Order == SortOrder.Ascending)
                {
                    sorter.Order = SortOrder.Descending;
                }
                else
                {
                    sorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                sorter.SortColumn = columnId;
                sorter.Order = SortOrder.Ascending;
            }
            //create the move to parent dir item
            //ListViewItem upDir = box.Items[0];
            //box.Items.Remove(upDir);
            box.Sort();
            //box.Items.Insert(0, upDir);
        }

        private void leftBox_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            boxSort(leftBox, leftColumnSorter, e.Column);
        }

        private void rightBox_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            boxSort(rightBox, rightColumnSorter, e.Column);
        }

        //Drag and drop tutorial: https://www.codeproject.com/Questions/140546/C-drag-and-drop-from-into-listView-between-two-ins

        //Handle left box drag and drop

        //Called when user hovers mouse with drag items over the leftBox
        private void leftBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)))
            {
                e.Effect = e.AllowedEffect;
            }
        }

        //Taken from https://stackoverflow.com/a/3822913
        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

        public void handleDrop(ListView box, string curPath, ListView.SelectedListViewItemCollection items)
        {
            foreach (ListViewItem current in items)
            {
                if (current.Text != upDirName)
                {
                    box.Items.Add((ListViewItem)current.Clone());
                    string path;
                    if (current.ListView == leftBox)
                    {
                        path = leftP;
                    }
                    else
                    {
                        path = rightP;
                    }
                    if (current.SubItems[1].Text == "<DIR>")
                    {
                        try
                        {
                            CopyFilesRecursively(path + "\\" + current.Text, curPath + "\\" + current.Text);
                        }
                        catch (System.IO.IOException)
                        {
                            MessageBox.Show(current.Text + " already exists in the destination directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        catch (System.UnauthorizedAccessException)
                        {
                            MessageBox.Show("Unauthorized access to file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        catch
                        {
                            MessageBox.Show("Unexpected error has occured during file copying", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }

                    }
                    else
                    {
                        try
                        {
                            File.Copy(path + "\\" + current.Text, curPath + "\\" + current.Text);
                        }
                        catch (System.IO.IOException)
                        {
                            MessageBox.Show("File " + current.Text + " already exists in the destination directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        catch (System.UnauthorizedAccessException)
                        {
                            MessageBox.Show("Unauthorized access to file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        catch
                        {
                            MessageBox.Show("Unexpected error has occured during file copying", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }
                }
            }
            displayDirectory(curPath, box);
        }

        //Called when user lets go of dragged items
        private void leftBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)))
            {
                handleDrop(leftBox, leftP, (ListView.SelectedListViewItemCollection)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection)));
            }
        }

        //Called when user begins dragging
        private void leftBox_ItemDrag(object sender, ItemDragEventArgs e)
        {
            leftBox.DoDragDrop(leftBox.SelectedItems, DragDropEffects.Copy);
        }
        //Handle right box drag and drop
        private void rightBox_ItemDrag(object sender, ItemDragEventArgs e)
        {
            rightBox.DoDragDrop(rightBox.SelectedItems, DragDropEffects.Copy);
        }

        private void rightBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)))
            {
                e.Effect = e.AllowedEffect;
            }
        }

        private void rightBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)))
            {
                handleDrop(rightBox, rightP, (ListView.SelectedListViewItemCollection)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection)));
            }
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            displayDirectory(leftP, leftBox);
            displayDirectory(rightP, rightBox);
        }

        public void handleF8(ListView box, string curPath, ListView.SelectedListViewItemCollection items)
        {
            if (MessageBox.Show("Are you sure you want to delete these files?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
            {
                return;
            }
            foreach (ListViewItem item in items)
            {
                if (item.Text != upDirName)
                {
                    if (item.SubItems[1].Text == "<DIR>")
                    {
                        Directory.Delete(curPath + "\\" + item.Text, true);
                    }
                    else
                    {
                        File.Delete(curPath + "\\" + item.Text);
                    }
                }
            }
            displayDirectory(curPath, box);
        }

        private void leftBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F8)
            {
                handleF8(leftBox, leftP, leftBox.SelectedItems);
            }
            else if (e.KeyCode == Keys.F7)
            {
                handleF7(leftBox, leftP);
            }
        }

        private void rightBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F8)
            {
                handleF8(rightBox, rightP, rightBox.SelectedItems);
            }
            else if (e.KeyCode == Keys.F7)
            {
                handleF7(rightBox, rightP);
            }
        }

        private void leftPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string res = displayDirectory(leftDrive.Text + leftPath.Text, leftBox);
                if (res == null)
                {
                    leftPath.Text = getBasePath(leftP);
                }
                else
                {
                    leftP = res;
                }
            }
        }

        private void rightPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string res = displayDirectory(rightDrive.Text + rightPath.Text, rightBox);
                if (res == null)
                {
                    rightPath.Text = getBasePath(rightP);
                }
                else
                {
                    rightP = res;
                }
            }
        }

        private void alterContext(ListView box)
        {
            boxMenu.Items[0].Enabled = box.SelectedItems.Count != 0;
            boxMenu.Items[3].Enabled = copy != null;
        }

        private void boxMenu_Opening(object sender, CancelEventArgs e)
        {
            alterContext((ListView)boxMenu.SourceControl);
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            ListView box = (ListView)boxMenu.SourceControl;
            if (box.Name == "leftBox")
            {
                handleF8(box, leftP, box.SelectedItems);
            }
            else
            {
                handleF8(box, rightP, box.SelectedItems);
            }

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copy = ((ListView)boxMenu.SourceControl).SelectedItems;
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView box = (ListView)boxMenu.SourceControl;
            if (box.Name == "leftBox")
            {
                handleDrop(box, leftP, copy);
            }
            else
            {
                handleDrop(box, rightP, copy);
            }
            copy = null;
        }

        public void handleF7(ListView box, string curPath)
        {
            if (tBox != null)
            {
                tBox = tBox.destroy();
            }
            tBox = new temporaryTextBox(box, syncer, curPath);
        }

        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView box = (ListView)boxMenu.SourceControl;
            if (box.Name == "leftBox")
            {
                handleF7(box, leftP);
            }
            else
            {
                handleF7(box, rightP);
            }

        }

        private void syncer_Tick(object sender, EventArgs e)
        {
            tBox.syncPos();
        }
    }
}
