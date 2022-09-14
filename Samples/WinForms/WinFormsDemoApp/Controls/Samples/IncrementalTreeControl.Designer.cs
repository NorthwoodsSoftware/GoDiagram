/* Copyright 1998-2022 by Northwoods Software Corporation. */

namespace Demo.Samples.IncrementalTree {
  partial class IncrementalTree {
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
      this.desc1 = new WinFormsDemoApp.GoWebBrowser();
      this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
      this.expandBtn = new System.Windows.Forms.Button();
      this.zoomFitBtn = new System.Windows.Forms.Button();
      this.tableLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
      this.tableLayoutPanel2.SuspendLayout();
      this.SuspendLayout();
      //
      // tableLayoutPanel1
      //
      this.tableLayoutPanel1.AutoScroll = true;
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.desc1, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
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
      // diagramControl1
      //
      this.diagramControl1.AllowDrop = true;
      this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
      this.diagramControl1.Location = new System.Drawing.Point(3, 3);
      this.diagramControl1.Name = "diagramControl1";
      this.diagramControl1.Size = new System.Drawing.Size(994, 494);
      this.diagramControl1.TabIndex = 0;
      this.diagramControl1.Text = "diagramControl1";
      //
      // desc1
      //
      this.desc1.CreationProperties = null;
      this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
      this.desc1.Dock = System.Windows.Forms.DockStyle.Top;
      this.desc1.Location = new System.Drawing.Point(3, 546);
      this.desc1.Name = "desc1";
      this.desc1.Size = new System.Drawing.Size(994, 627);
      this.desc1.TabIndex = 3;
      this.desc1.ZoomFactor = 1D;
      //
      // tableLayoutPanel2
      //
      this.tableLayoutPanel2.AutoSize = true;
      this.tableLayoutPanel2.ColumnCount = 2;
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 190F));
      this.tableLayoutPanel2.Controls.Add(this.expandBtn, 1, 0);
      this.tableLayoutPanel2.Controls.Add(this.zoomFitBtn, 0, 0);
      this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 503);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 1;
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.Size = new System.Drawing.Size(330, 37);
      this.tableLayoutPanel2.TabIndex = 4;
      //
      // expandBtn
      //
      this.expandBtn.AutoSize = true;
      this.expandBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.expandBtn.Location = new System.Drawing.Point(143, 3);
      this.expandBtn.Name = "expandBtn";
      this.expandBtn.Size = new System.Drawing.Size(184, 31);
      this.expandBtn.TabIndex = 3;
      this.expandBtn.Text = "Expand Random Node";
      this.expandBtn.UseVisualStyleBackColor = true;
      //
      // zoomFitBtn
      //
      this.zoomFitBtn.AutoSize = true;
      this.zoomFitBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.zoomFitBtn.Location = new System.Drawing.Point(3, 3);
      this.zoomFitBtn.Name = "zoomFitBtn";
      this.zoomFitBtn.Size = new System.Drawing.Size(134, 31);
      this.zoomFitBtn.TabIndex = 0;
      this.zoomFitBtn.Text = "Zoom To Fit";
      this.zoomFitBtn.UseVisualStyleBackColor = true;
      //
      // IncrementalTreeControl
      //
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tableLayoutPanel1);
      this.Name = "IncrementalTreeControl";
      this.Size = new System.Drawing.Size(1000, 1200);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsDemoApp.GoWebBrowser desc1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Button expandBtn;
    private System.Windows.Forms.Button zoomFitBtn;
  }
}
