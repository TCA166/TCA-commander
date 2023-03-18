
namespace projektOOP
{
    partial class Commander
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            leftPath = new System.Windows.Forms.TextBox();
            rightPath = new System.Windows.Forms.TextBox();
            leftDrive = new System.Windows.Forms.ComboBox();
            rightDrive = new System.Windows.Forms.ComboBox();
            leftBox = new System.Windows.Forms.ListView();
            nameColumn = new System.Windows.Forms.ColumnHeader();
            sizeColumn = new System.Windows.Forms.ColumnHeader();
            dateColumn = new System.Windows.Forms.ColumnHeader();
            boxMenu = new System.Windows.Forms.ContextMenuStrip(components);
            deleteBtn = new System.Windows.Forms.ToolStripMenuItem();
            newFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            rightBox = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            refreshBtn = new System.Windows.Forms.Button();
            syncer = new System.Windows.Forms.Timer(components);
            iconList = new System.Windows.Forms.ImageList(components);
            boxMenu.SuspendLayout();
            SuspendLayout();
            // 
            // leftPath
            // 
            leftPath.Location = new System.Drawing.Point(59, 12);
            leftPath.Name = "leftPath";
            leftPath.Size = new System.Drawing.Size(313, 23);
            leftPath.TabIndex = 2;
            leftPath.KeyDown += leftPath_KeyDown;
            // 
            // rightPath
            // 
            rightPath.Location = new System.Drawing.Point(463, 12);
            rightPath.Name = "rightPath";
            rightPath.Size = new System.Drawing.Size(313, 23);
            rightPath.TabIndex = 3;
            rightPath.KeyDown += rightPath_KeyDown;
            // 
            // leftDrive
            // 
            leftDrive.FormattingEnabled = true;
            leftDrive.Location = new System.Drawing.Point(12, 12);
            leftDrive.Name = "leftDrive";
            leftDrive.Size = new System.Drawing.Size(41, 23);
            leftDrive.TabIndex = 4;
            leftDrive.SelectedIndexChanged += leftDrive_SelectedIndexChanged;
            // 
            // rightDrive
            // 
            rightDrive.FormattingEnabled = true;
            rightDrive.Location = new System.Drawing.Point(416, 12);
            rightDrive.Name = "rightDrive";
            rightDrive.Size = new System.Drawing.Size(41, 23);
            rightDrive.TabIndex = 5;
            rightDrive.SelectedIndexChanged += rightDrive_SelectedIndexChanged;
            // 
            // leftBox
            // 
            leftBox.AllowDrop = true;
            leftBox.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { nameColumn, sizeColumn, dateColumn });
            leftBox.ContextMenuStrip = boxMenu;
            leftBox.HideSelection = false;
            leftBox.Location = new System.Drawing.Point(12, 41);
            leftBox.Name = "leftBox";
            leftBox.Size = new System.Drawing.Size(360, 512);
            leftBox.TabIndex = 6;
            leftBox.UseCompatibleStateImageBehavior = false;
            leftBox.View = System.Windows.Forms.View.Details;
            leftBox.ColumnClick += leftBox_ColumnClick;
            leftBox.ItemDrag += leftBox_ItemDrag;
            leftBox.DragDrop += leftBox_DragDrop;
            leftBox.DragOver += leftBox_DragOver;
            leftBox.KeyDown += leftBox_KeyDown;
            leftBox.MouseDoubleClick += leftBox_MouseDoubleClick;
            // 
            // nameColumn
            // 
            nameColumn.Text = "Name";
            nameColumn.Width = 155;
            // 
            // sizeColumn
            // 
            sizeColumn.Text = "Size";
            // 
            // dateColumn
            // 
            dateColumn.Text = "Modified";
            dateColumn.Width = 120;
            // 
            // boxMenu
            // 
            boxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { deleteBtn, newFolderToolStripMenuItem, copyToolStripMenuItem, pasteToolStripMenuItem });
            boxMenu.Name = "boxMenu";
            boxMenu.Size = new System.Drawing.Size(158, 92);
            boxMenu.Opening += boxMenu_Opening;
            // 
            // deleteBtn
            // 
            deleteBtn.Name = "deleteBtn";
            deleteBtn.Size = new System.Drawing.Size(157, 22);
            deleteBtn.Text = "Delete (F8)";
            deleteBtn.Click += deleteBtn_Click;
            // 
            // newFolderToolStripMenuItem
            // 
            newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
            newFolderToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            newFolderToolStripMenuItem.Text = "New Folder (F7)";
            newFolderToolStripMenuItem.Click += newFolderToolStripMenuItem_Click;
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            pasteToolStripMenuItem.Text = "Paste";
            pasteToolStripMenuItem.Click += pasteToolStripMenuItem_Click;
            // 
            // rightBox
            // 
            rightBox.AllowDrop = true;
            rightBox.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            rightBox.ContextMenuStrip = boxMenu;
            rightBox.HideSelection = false;
            rightBox.Location = new System.Drawing.Point(416, 41);
            rightBox.Name = "rightBox";
            rightBox.Size = new System.Drawing.Size(360, 512);
            rightBox.TabIndex = 7;
            rightBox.UseCompatibleStateImageBehavior = false;
            rightBox.View = System.Windows.Forms.View.Details;
            rightBox.ColumnClick += rightBox_ColumnClick;
            rightBox.ItemDrag += rightBox_ItemDrag;
            rightBox.DragDrop += rightBox_DragDrop;
            rightBox.DragOver += rightBox_DragOver;
            rightBox.KeyDown += rightBox_KeyDown;
            rightBox.MouseDoubleClick += rightBox_MouseDoubleClick;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Name";
            columnHeader1.Width = 155;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Size";
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Modified";
            columnHeader3.Width = 120;
            // 
            // refreshBtn
            // 
            refreshBtn.Location = new System.Drawing.Point(378, 11);
            refreshBtn.Name = "refreshBtn";
            refreshBtn.Size = new System.Drawing.Size(32, 24);
            refreshBtn.TabIndex = 8;
            refreshBtn.Text = "↻";
            refreshBtn.UseVisualStyleBackColor = true;
            refreshBtn.Click += refreshBtn_Click;
            // 
            // syncer
            // 
            syncer.Interval = 20;
            syncer.Tick += syncer_Tick;
            // 
            // iconList
            // 
            iconList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            iconList.ImageSize = new System.Drawing.Size(16, 16);
            iconList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // TCACommander
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(792, 575);
            Controls.Add(refreshBtn);
            Controls.Add(rightBox);
            Controls.Add(leftBox);
            Controls.Add(rightDrive);
            Controls.Add(leftDrive);
            Controls.Add(rightPath);
            Controls.Add(leftPath);
            MaximizeBox = false;
            Name = "TCACommander";
            Text = "TCA Commander";
            Load += Form1_Load;
            boxMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.TextBox leftPath;
        private System.Windows.Forms.TextBox rightPath;
        private System.Windows.Forms.ComboBox leftDrive;
        private System.Windows.Forms.ComboBox rightDrive;
        private System.Windows.Forms.ListView leftBox;
        private System.Windows.Forms.ColumnHeader sizeColumn;
        private System.Windows.Forms.ColumnHeader dateColumn;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.ListView rightBox;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.ContextMenuStrip boxMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteBtn;
        private System.Windows.Forms.ToolStripMenuItem newFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.Timer syncer;
        private System.Windows.Forms.ImageList iconList;
    }
}

