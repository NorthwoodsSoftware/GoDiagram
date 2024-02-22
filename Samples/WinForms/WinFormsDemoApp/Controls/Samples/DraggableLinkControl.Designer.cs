/* Copyright 1998-2024 by Northwoods Software Corporation. */

namespace Demo.Samples.DraggableLink {
  partial class DraggableLink {
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.paletteControl1 = new Northwoods.Go.WinForms.PaletteControl();
            this.desc1 = new WinFormsDemoApp.GoWebBrowser();
            this.modelJson1 = new WinFormsDemoApp.ModelJson();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.desc1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.modelJson1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 850F));
            this.tableLayoutPanel2.Controls.Add(this.diagramControl1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.paletteControl1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(994, 594);
            this.tableLayoutPanel2.TabIndex = 0;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(147, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(844, 588);
            this.diagramControl1.TabIndex = 0;
            this.diagramControl1.Text = "diagramControl1";
            //
            // paletteControl1
            //
            this.paletteControl1.AllowDrop = true;
            this.paletteControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.paletteControl1.Location = new System.Drawing.Point(3, 3);
            this.paletteControl1.Name = "paletteControl1";
            this.paletteControl1.Size = new System.Drawing.Size(138, 588);
            this.paletteControl1.TabIndex = 1;
            this.paletteControl1.Text = "paletteControl1";
            //
            // desc1
            //
            this.desc1.CreationProperties = null;
            this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.desc1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.desc1.Location = new System.Drawing.Point(3, 603);
            this.desc1.Name = "desc1";
            this.desc1.Size = new System.Drawing.Size(994, 244);
            this.desc1.TabIndex = 1;
            this.desc1.ZoomFactor = 1D;
            //
            // modelJson1
            //
            this.modelJson1.AutoSize = true;
            this.modelJson1.Dock = System.Windows.Forms.DockStyle.Top;
            this.modelJson1.Location = new System.Drawing.Point(3, 853);
            this.modelJson1.Name = "modelJson1";
            this.modelJson1.Size = new System.Drawing.Size(994, 343);
            this.modelJson1.TabIndex = 2;
            //
            // DraggableLinkControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DraggableLinkControl";
            this.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private Northwoods.Go.WinForms.PaletteControl paletteControl1;
    private WinFormsDemoApp.GoWebBrowser desc1;
    private WinFormsDemoApp.ModelJson modelJson1;
  }
}
