using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Text_Editor
{
    public partial class Back : Form
    {
        private int TabCount = 0;
        private string save = "";

        public Back()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AddTab();
            GetFontCollection();
            PopulateFontSize();            
        }

        #region Tabs
        private void AddTab()
        {
            RichTextBox Body = new RichTextBox();
            Body.Name = "Body";
            Body.Dock = DockStyle.Fill;
            Body.ContextMenuStrip = contextMenuStrip1;

            TabPage NewPage = new TabPage();
            TabCount += 1;

            string DocumentText = "Document " + TabCount;
            NewPage.Name = DocumentText;
            NewPage.Text = DocumentText;
            NewPage.Controls.Add(Body);

            tabControl1.TabPages.Add(NewPage);
            tabControl1.ContextMenuStrip = contextMenuStrip1;
        }

        private void RemoveTab()
        {
            if (tabControl1.TabPages.Count != 1)
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
            else
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                AddTab();
            }
        }

        private void RemoveAllTabs()
        {
            foreach (TabPage Page in tabControl1.TabPages)
            {
                tabControl1.TabPages.Remove(Page);
            }
            TabCount = 0;
            AddTab();
        }

        private void RemoveAllTabsButThisOne()
        {
            foreach (TabPage Page in tabControl1.TabPages)
            {
                if (Page.Name != tabControl1.SelectedTab.Name)
                {
                    tabControl1.TabPages.Remove(Page);
                }
            }
        }
        #endregion
        #region SaveAndOpen
        private void Save()
        {
            saveFileDialog1.FileName = tabControl1.SelectedTab.Text;
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog1.Filter = "RTF|*.rtf";
            saveFileDialog1.Title = "Save";
            if (save == "")
            {
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (saveFileDialog1.FileName.Length > 0)
                    {
                        GetCurrentDocument.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
                        save = saveFileDialog1.FileName;
                        int j = saveFileDialog1.FileName.Length;
                        bool start = false;
                        int cs = 0;
                        int ce = 0;
                        for (int i = j - 1; i > -1; i--)
                        {
                            if (start == false)
                            {
                                if (saveFileDialog1.FileName.Substring(i, 1) != ".")
                                {
                                    cs++;
                                }
                                else
                                {
                                    cs++;
                                    start = true;
                                    ce = cs;
                                }
                            }
                            else
                            {
                                if (saveFileDialog1.FileName.Substring(i, 1) != "\\")
                                {
                                    ce++;
                                } else
                                {
                                    i = 0;
                                }
                            }
                        }
                        int diff = ce - cs;
                        string name = saveFileDialog1.FileName.Substring(j - ce, diff);
                        if (name.Length <= 6)
                        {
                            tabControl1.SelectedTab.Text = name;
                        }
                        else
                        {
                            tabControl1.SelectedTab.Text = name.Substring(0, 5) + ".";
                        }
                    }
                }
            }
            else
            {
                GetCurrentDocument.SaveFile(save, RichTextBoxStreamType.RichText);
            }
        }
        private void SaveAs()
        {
            saveFileDialog1.FileName = tabControl1.SelectedTab.Name;
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog1.Filter = "RTF|*.rtf|Text Files|*.txt|VB Files|*.vb|C# Files|*.cs|All Files|*.*";
            saveFileDialog1.Title = "Save As";

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (saveFileDialog1.FileName.Length > 0)
                {
                    GetCurrentDocument.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
                    save = saveFileDialog1.FileName;
                }
            }
        }
        private void Open()
        {
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog1.Filter = "RTF|*.rtf|Text Files|*.txt|VB Files|*.vb|C# Files|*.cs|All Files|*.*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GetCurrentDocument.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.RichText);
                int j = openFileDialog1.FileName.Length;
                bool start = false;
                int cs = 0;
                int ce = 0;
                for (int i = j - 1; i > -1; i--)
                {
                    if (start == false)
                    {
                        if (openFileDialog1.FileName.Substring(i, 1) != ".")
                        {
                            cs++;
                        }
                        else
                        {
                            cs++;
                            start = true;
                            ce = cs;
                        }
                    }
                    else
                    {
                        if (openFileDialog1.FileName.Substring(i, 1) != "\\")
                        {
                            ce++;
                        }
                        else
                        {
                            i = 0;
                        }
                    }
                }
                int diff = ce - cs;
                string name = openFileDialog1.FileName.Substring(j - ce, diff);
                if (name.Length <= 6)
                {
                    tabControl1.SelectedTab.Text = name;
                }
                else
                {
                    tabControl1.SelectedTab.Text = name.Substring(0, 5) + ".";
                }
            }
        }
        #endregion
        #region Function
        private void Undo()
        {
            GetCurrentDocument.Undo();
        }
        private void Redo()
        {
            GetCurrentDocument.Redo();
        }
        private void Cut()
        {
            GetCurrentDocument.Cut();
        }
        private void Copy()
        {
            GetCurrentDocument.Copy();
        }
        private void Paste()
        {
            GetCurrentDocument.Paste();
        }
        #endregion
        #region Properties

        private RichTextBox GetCurrentDocument
        {
            get { return (RichTextBox)tabControl1.SelectedTab.Controls["Body"]; }
        }
        private void SelectAll()
        {
            GetCurrentDocument.SelectAll();
        }
        #endregion
        #region General
        private void GetFontCollection()
        {
            InstalledFontCollection InsFonts = new InstalledFontCollection();

            foreach (FontFamily item in InsFonts.Families)
            {
                toolStripComboBox1.Items.Add(item.Name);
            }
            toolStripComboBox1.SelectedIndex = 0;
        }
        private void PopulateFontSize()
        {
            for (int i = 1; i<=75; i++)
            {
                toolStripComboBox2.Items.Add(i);
            }
            toolStripComboBox2.SelectedIndex = 7;
        }

        private void Style(FontStyle input)
        {
            bool b = GetCurrentDocument.SelectionFont.Bold;
            bool u = GetCurrentDocument.SelectionFont.Underline;
            bool i = GetCurrentDocument.SelectionFont.Italic;
            bool s = GetCurrentDocument.SelectionFont.Strikeout;
            switch (input)
            {
                case FontStyle.Bold:
                    b = b ? false : true; 
                    break;
                case FontStyle.Underline:
                    u = u ? false : true;
                    break;
                case FontStyle.Italic:
                    i = i ? false : true;
                    break;
                case FontStyle.Strikeout:
                    s = s ? false : true;
                    break;
            }
            FontStyle Style = FontStyle.Regular;
            if (b)
            {
                Style = FontStyle.Bold;
            }
            if (u)
            {
                Style = Style | FontStyle.Underline;
            }
            if (i)
            {
                Style = Style | FontStyle.Italic;
            }
            if (s)
            {
                Style = Style | FontStyle.Strikeout;
            }
            GetCurrentDocument.SelectionFont = new Font(GetCurrentDocument.SelectionFont, Style);
        }
        #endregion
        #region Buttons
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void newToolStripButton1_Click(object sender, EventArgs e)
        {
            AddTab();
        }

        private void openToolStripButton1_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void saveToolStripButton1_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void cutToolStripButton1_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void copyToolStripButton1_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void pasteToolStripButton1_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void RemoveTabToolStripButton_Click(object sender, EventArgs e)
        {
            RemoveTab();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTab();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void undoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void redoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveTab();
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveAllTabs();
        }

        private void closeAllButThisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveAllTabsButThisOne();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if(GetCurrentDocument.SelectionLength != 0)
            {
                Style(FontStyle.Bold);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (GetCurrentDocument.SelectionLength != 0)
            {
                Style(FontStyle.Italic);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (GetCurrentDocument.SelectionLength != 0)
            {
                Style(FontStyle.Underline);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

            if (GetCurrentDocument.SelectionLength != 0)
            {
                Style(FontStyle.Strikeout);
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectedText = GetCurrentDocument.SelectedText.ToUpper();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectedText = GetCurrentDocument.SelectedText.ToLower();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            if (GetCurrentDocument.SelectedText.Length > 0)
            {
                float FontSize = GetCurrentDocument.SelectionFont.SizeInPoints ;
                GetCurrentDocument.SelectionFont = new Font(GetCurrentDocument.SelectionFont.Name, FontSize+1, GetCurrentDocument.SelectionFont.Style);
                toolStripComboBox2.SelectedIndex = Convert.ToInt32(FontSize);
            }
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            float FontSize = GetCurrentDocument.SelectionFont.SizeInPoints;
            GetCurrentDocument.SelectionFont = new Font(GetCurrentDocument.SelectionFont.Name, FontSize - 1, GetCurrentDocument.SelectionFont.Style);
            toolStripComboBox2.SelectedIndex = Convert.ToInt32(FontSize-2);
        }

        #endregion
        #region Unused
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox3_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox2_Click(object sender, EventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        #endregion
    }
}
