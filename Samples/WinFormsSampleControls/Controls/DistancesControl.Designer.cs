
namespace WinFormsSampleControls.Distances {
  partial class DistancesControl {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.goWebBrowser1 = new WinFormsSharedControls.GoWebBrowser();
            this.goWebBrowser2 = new WinFormsSharedControls.GoWebBrowser();
            this.newRandomNodesBtn = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser2)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.newRandomNodesBtn, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.listBox1, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // diagramControl1
            // 
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(994, 594);
            this.diagramControl1.TabIndex = 0;
            this.diagramControl1.Text = "diagramControl1";
            // 
            // goWebBrowser1
            // 
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 603);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(994, 54);
            this.goWebBrowser1.TabIndex = 1;
            this.goWebBrowser1.ZoomFactor = 1D;
            // 
            // goWebBrowser2
            // 
            this.goWebBrowser2.CreationProperties = null;
            this.goWebBrowser2.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser2.Dock = System.Windows.Forms.DockStyle.Top;
            this.goWebBrowser2.Location = new System.Drawing.Point(3, 703);
            this.goWebBrowser2.Name = "goWebBrowser2";
            this.goWebBrowser2.Size = new System.Drawing.Size(994, 54);
            this.goWebBrowser2.TabIndex = 2;
            this.goWebBrowser2.ZoomFactor = 1D;
            // 
            // newRandomNodesBtn
            // 
            this.newRandomNodesBtn.Location = new System.Drawing.Point(3, 663);
            this.newRandomNodesBtn.Name = "newRandomNodesBtn";
            this.newRandomNodesBtn.Size = new System.Drawing.Size(233, 34);
            this.newRandomNodesBtn.TabIndex = 3;
            this.newRandomNodesBtn.Text = "Choose another two nodes at random";
            this.newRandomNodesBtn.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(3, 763);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(994, 434);
            this.listBox1.TabIndex = 4;
            // 
            // DistancesAndPathsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DistancesAndPathsControl";
            this.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser2)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser1;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser2;
    private System.Windows.Forms.Button newRandomNodesBtn;
    private System.Windows.Forms.ListBox listBox1;
  }
}
