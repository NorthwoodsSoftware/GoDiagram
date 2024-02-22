/* Copyright 1998-2024 by Northwoods Software Corporation. */

namespace Demo.Samples.Virtualized {
  partial class VirtualizedControl {
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
            this.goWebBrowser1 = new WinFormsDemoApp.GoWebBrowser();
            this.lblNodes = new System.Windows.Forms.Label();
            this.lblLinks = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblNodes, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblLinks, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1058, 831);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(1052, 711);
            this.diagramControl1.TabIndex = 0;
            this.diagramControl1.Text = "diagramControl1";
            //
            // goWebBrowser1
            //
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 720);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(1052, 39);
            this.goWebBrowser1.TabIndex = 1;
            this.goWebBrowser1.ZoomFactor = 1D;
            //
            // lblNodes
            //
            this.lblNodes.AutoSize = true;
            this.lblNodes.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNodes.Location = new System.Drawing.Point(3, 762);
            this.lblNodes.Name = "lblNodes";
            this.lblNodes.Size = new System.Drawing.Size(1052, 15);
            this.lblNodes.TabIndex = 2;
            this.lblNodes.Text = "label1";
            //
            // lblLinks
            //
            this.lblLinks.AutoSize = true;
            this.lblLinks.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLinks.Location = new System.Drawing.Point(3, 777);
            this.lblLinks.Name = "lblLinks";
            this.lblLinks.Size = new System.Drawing.Size(1052, 15);
            this.lblLinks.TabIndex = 3;
            this.lblLinks.Text = "label2";
            //
            // VirtualizedControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "VirtualizedControl";
            this.Size = new System.Drawing.Size(1058, 831);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsDemoApp.GoWebBrowser goWebBrowser1;
    private System.Windows.Forms.Label lblNodes;
    private System.Windows.Forms.Label lblLinks;
  }
}
