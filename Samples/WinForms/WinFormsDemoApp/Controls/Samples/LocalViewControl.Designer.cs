/* Copyright 1998-2023 by Northwoods Software Corporation. */

namespace Demo.Samples.LocalView {
  partial class LocalView {
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
            this.fullDiagram = new Northwoods.Go.WinForms.DiagramControl();
            this.localDiagram = new Northwoods.Go.WinForms.DiagramControl();
            this.newTreeBtn = new System.Windows.Forms.Button();
            this.desc1 = new WinFormsDemoApp.GoWebBrowser();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.fullDiagram, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.localDiagram, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.newTreeBtn, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.desc1, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // fullDiagram
            //
            this.fullDiagram.AllowDrop = true;
            this.fullDiagram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fullDiagram.Location = new System.Drawing.Point(3, 3);
            this.fullDiagram.Name = "fullDiagram";
            this.fullDiagram.Size = new System.Drawing.Size(994, 244);
            this.fullDiagram.TabIndex = 0;
            this.fullDiagram.Text = "fullDiagram";
            //
            // localDiagram
            //
            this.localDiagram.AllowDrop = true;
            this.localDiagram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.localDiagram.Location = new System.Drawing.Point(3, 253);
            this.localDiagram.Name = "localDiagram";
            this.localDiagram.Size = new System.Drawing.Size(994, 294);
            this.localDiagram.TabIndex = 1;
            this.localDiagram.Text = "localDiagram";
            //
            // newTreeBtn
            //
            this.newTreeBtn.Location = new System.Drawing.Point(3, 553);
            this.newTreeBtn.Name = "newTreeBtn";
            this.newTreeBtn.Size = new System.Drawing.Size(116, 33);
            this.newTreeBtn.TabIndex = 2;
            this.newTreeBtn.Text = "Create New Tree";
            this.newTreeBtn.UseVisualStyleBackColor = true;
            //
            // desc1
            //
            this.desc1.CreationProperties = null;
            this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.desc1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.desc1.Location = new System.Drawing.Point(3, 592);
            this.desc1.Name = "desc1";
            this.desc1.Size = new System.Drawing.Size(994, 605);
            this.desc1.TabIndex = 3;
            this.desc1.ZoomFactor = 1D;
            //
            // LocalViewControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LocalViewControl";
            this.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl fullDiagram;
    private Northwoods.Go.WinForms.DiagramControl localDiagram;
    private System.Windows.Forms.Button newTreeBtn;
    private WinFormsDemoApp.GoWebBrowser desc1;
  }
}
