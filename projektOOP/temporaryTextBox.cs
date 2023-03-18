using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace projektOOP
{
    internal class temporaryTextBox : TextBox
    {
        public ListView parent;

        public ListViewItem item;

        public Timer syncTimer;

        public string curPath;

        public temporaryTextBox(ListView parent, Timer syncer, string path)
        {
            this.parent = parent;
            curPath = path;
            //create a new item
            item = new ListViewItem();
            item.SubItems.Add("<NEW>");
            item = parent.Items.Add(item);
            //set properties of this new input
            this.Width = parent.Columns[0].Width;
            this.Multiline = true;
            this.Height = parent.GetItemRect(parent.Items.Count - 1).Height;
            this.PlaceholderText = "New Folder";
            //set events
            this.Leave += onLeave;
            this.KeyDown += onKey;
            this.Disposed += onDisposed;
            //synchronise position with the new item
            syncPos();
            //render this
            parent.Controls.Add(this);
            //Scroll shenanigans
            parent.EnsureVisible(parent.Items.Count - 1);
            //there is no event for scroll, only this. My attempts to create custom listview with such an event failed
            parent.MouseWheel += onScroll; //couldnt find a way to refresh more effectively so resorted to timer
            //start the timer
            syncer.Start();
            syncTimer = syncer;
            
        }

        private void onScroll(object sender, EventArgs e)
        {
            syncPos();
        }

        public void syncPos()
        {
            Point pos = item.Position;
            //pos.Y = pos.Y - this.Height;
            this.Location = pos;
        }

        private void onLeave(object sender, EventArgs e)
        {
            destroy();
        }

        private void onKey(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                Directory.CreateDirectory(curPath + "\\" + this.Text);
                ((Commander)this.Parent.Parent).displayDirectory(curPath, parent);
                destroy();
            }
        }

        private void onDisposed(object sender, EventArgs e)
        {
            destroy();
        }

        public temporaryTextBox destroy()
        {
            syncTimer.Stop();
            parent.MouseMove += null;
            parent.Scrollable = true;
            parent.Items.Remove(item);
            parent.Controls.Remove(this);
            this.Dispose();
            return null;
        }

    }
}
