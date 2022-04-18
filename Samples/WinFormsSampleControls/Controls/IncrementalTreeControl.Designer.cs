/* Copyright 1998-2022 by Northwoods Software Corporation. */

namespace WinFormsSampleControls.IncrementalTree {
  partial class IncrementalTreeControl {
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
      this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
      this.btnExpandRandomNode = new System.Windows.Forms.Button();
      this.btnZoomToFit = new System.Windows.Forms.Button();
      this.tableLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
      this.tableLayoutPanel2.SuspendLayout();
      this.SuspendLayout();
      //
      // tableLayoutPanel1
      //
      this.tableLayoutPanel1.AutoScroll = true;
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 2);
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
      // goWebBrowser1
      //
      this.goWebBrowser1.CreationProperties = null;
      this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
      this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
      this.goWebBrowser1.Location = new System.Drawing.Point(3, 546);
      this.goWebBrowser1.Name = "goWebBrowser1";
      this.goWebBrowser1.Size = new System.Drawing.Size(994, 627);
      this.goWebBrowser1.TabIndex = 3;
      this.goWebBrowser1.ZoomFactor = 1D;
      //
      // tableLayoutPanel2
      //
      this.tableLayoutPanel2.AutoSize = true;
      this.tableLayoutPanel2.ColumnCount = 2;
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 190F));
      this.tableLayoutPanel2.Controls.Add(this.btnExpandRandomNode, 1, 0);
      this.tableLayoutPanel2.Controls.Add(this.btnZoomToFit, 0, 0);
      this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 503);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 1;
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.Size = new System.Drawing.Size(330, 37);
      this.tableLayoutPanel2.TabIndex = 4;
      //
      // btnExpandRandomNode
      //
      this.btnExpandRandomNode.AutoSize = true;
      this.btnExpandRandomNode.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.btnExpandRandomNode.Location = new System.Drawing.Point(143, 3);
      this.btnExpandRandomNode.Name = "btnExpandRandomNode";
      this.btnExpandRandomNode.Size = new System.Drawing.Size(184, 31);
      this.btnExpandRandomNode.TabIndex = 3;
      this.btnExpandRandomNode.Text = "Expand Random Node";
      this.btnExpandRandomNode.UseVisualStyleBackColor = true;
      //
      // btnZoomToFit
      //
      this.btnZoomToFit.AutoSize = true;
      this.btnZoomToFit.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.btnZoomToFit.Location = new System.Drawing.Point(3, 3);
      this.btnZoomToFit.Name = "btnZoomToFit";
      this.btnZoomToFit.Size = new System.Drawing.Size(134, 31);
      this.btnZoomToFit.TabIndex = 0;
      this.btnZoomToFit.Text = "Zoom To Fit";
      this.btnZoomToFit.UseVisualStyleBackColor = true;
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
      ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Button btnExpandRandomNode;
    private System.Windows.Forms.Button btnZoomToFit;
  }
}
