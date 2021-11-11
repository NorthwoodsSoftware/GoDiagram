
namespace WinFormsSampleControls.OrgChartEditor {
  partial class OrgChartEditorControl {
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.zoomFitBtn = new System.Windows.Forms.Button();
            this.centerRootBtn = new System.Windows.Forms.Button();
            this.saveLoadModel1 = new WinFormsSharedControls.SaveLoadModel();
            this.goWebBrowser1 = new WinFormsSharedControls.GoWebBrowser();
            this.inspectorControl1 = new Northwoods.Go.Extensions.InspectorControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.saveLoadModel1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.inspectorControl1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1093, 1303);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // diagramControl1
            // 
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(60)))));
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(1087, 570);
            this.diagramControl1.TabIndex = 0;
            this.diagramControl1.Text = "diagramControl1";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.zoomFitBtn);
            this.flowLayoutPanel1.Controls.Add(this.centerRootBtn);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 579);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1087, 29);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // zoomFitBtn
            // 
            this.zoomFitBtn.Location = new System.Drawing.Point(3, 3);
            this.zoomFitBtn.Name = "zoomFitBtn";
            this.zoomFitBtn.Size = new System.Drawing.Size(104, 23);
            this.zoomFitBtn.TabIndex = 0;
            this.zoomFitBtn.Text = "Zoom to Fit";
            this.zoomFitBtn.UseVisualStyleBackColor = true;
            // 
            // centerRootBtn
            // 
            this.centerRootBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.centerRootBtn.Location = new System.Drawing.Point(113, 3);
            this.centerRootBtn.Name = "centerRootBtn";
            this.centerRootBtn.Size = new System.Drawing.Size(116, 23);
            this.centerRootBtn.TabIndex = 1;
            this.centerRootBtn.Text = "Center on root";
            this.centerRootBtn.UseVisualStyleBackColor = true;
            // 
            // saveLoadModel1
            // 
            this.saveLoadModel1.AutoSize = true;
            this.saveLoadModel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.saveLoadModel1.Location = new System.Drawing.Point(3, 885);
            this.saveLoadModel1.Name = "saveLoadModel1";
            this.saveLoadModel1.Size = new System.Drawing.Size(1087, 343);
            this.saveLoadModel1.TabIndex = 4;
            // 
            // goWebBrowser1
            // 
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 641);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(1087, 238);
            this.goWebBrowser1.TabIndex = 3;
            this.goWebBrowser1.ZoomFactor = 1D;
            // 
            // inspectorControl1
            // 
            this.inspectorControl1.AutoSize = true;
            this.inspectorControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.inspectorControl1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.inspectorControl1.Location = new System.Drawing.Point(3, 614);
            this.inspectorControl1.Name = "inspectorControl1";
            this.inspectorControl1.Size = new System.Drawing.Size(92, 21);
            this.inspectorControl1.TabIndex = 5;
            // 
            // OrgChartEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "OrgChartEditorControl";
            this.Size = new System.Drawing.Size(1093, 1303);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.Button zoomFitBtn;
    private System.Windows.Forms.Button centerRootBtn;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser1;
    private WinFormsSharedControls.SaveLoadModel saveLoadModel1;
    private Northwoods.Go.Extensions.InspectorControl inspectorControl1;
  }
}
