
namespace WinFormsDemoApp {
  partial class App {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.sampleTab = new System.Windows.Forms.TabPage();
            this.sampleList = new System.Windows.Forms.ListBox();
            this.extensionTab = new System.Windows.Forms.TabPage();
            this.extensionList = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.sampleTab.SuspendLayout();
            this.extensionTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 192F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1246, 637);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.sampleTab);
            this.tabControl1.Controls.Add(this.extensionTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControl1.Location = new System.Drawing.Point(2, 2);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.MinimumSize = new System.Drawing.Size(190, 100);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(190, 633);
            this.tabControl1.TabIndex = 1;
            // 
            // sampleTab
            // 
            this.sampleTab.Controls.Add(this.sampleList);
            this.sampleTab.Location = new System.Drawing.Point(4, 24);
            this.sampleTab.Name = "sampleTab";
            this.sampleTab.Padding = new System.Windows.Forms.Padding(3);
            this.sampleTab.Size = new System.Drawing.Size(182, 605);
            this.sampleTab.TabIndex = 0;
            this.sampleTab.Text = "Samples";
            this.sampleTab.UseVisualStyleBackColor = true;
            // 
            // sampleList
            // 
            this.sampleList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sampleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sampleList.IntegralHeight = false;
            this.sampleList.ItemHeight = 19;
            this.sampleList.Location = new System.Drawing.Point(3, 3);
            this.sampleList.Margin = new System.Windows.Forms.Padding(2);
            this.sampleList.Name = "sampleList";
            this.sampleList.Size = new System.Drawing.Size(176, 599);
            this.sampleList.TabIndex = 0;
            // 
            // extensionTab
            // 
            this.extensionTab.Controls.Add(this.extensionList);
            this.extensionTab.Location = new System.Drawing.Point(4, 24);
            this.extensionTab.Name = "extensionTab";
            this.extensionTab.Padding = new System.Windows.Forms.Padding(3);
            this.extensionTab.Size = new System.Drawing.Size(182, 605);
            this.extensionTab.TabIndex = 1;
            this.extensionTab.Text = "Extensions";
            this.extensionTab.UseVisualStyleBackColor = true;
            // 
            // extensionList
            // 
            this.extensionList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.extensionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extensionList.IntegralHeight = false;
            this.extensionList.ItemHeight = 19;
            this.extensionList.Location = new System.Drawing.Point(3, 3);
            this.extensionList.Margin = new System.Windows.Forms.Padding(2);
            this.extensionList.Name = "extensionList";
            this.extensionList.Size = new System.Drawing.Size(176, 599);
            this.extensionList.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoScrollMinSize = new System.Drawing.Size(250, 250);
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(194, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panel1.Size = new System.Drawing.Size(1050, 633);
            this.panel1.TabIndex = 1;
            // 
            // App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(625, 350);
            this.Name = "App";
            this.Text = "GoWinForms Demo";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.sampleTab.ResumeLayout(false);
            this.extensionTab.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage sampleTab;
    private System.Windows.Forms.TabPage extensionTab;
    private System.Windows.Forms.ListBox sampleList;
    private System.Windows.Forms.ListBox extensionList;
    private System.Windows.Forms.Panel panel1;
  }
}

