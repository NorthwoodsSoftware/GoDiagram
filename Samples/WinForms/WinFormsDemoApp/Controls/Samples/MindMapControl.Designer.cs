/* Copyright 1998-2024 by Northwoods Software Corporation. */

namespace Demo.Samples.MindMap {
  partial class MindMap {
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
      this.layoutBtn = new System.Windows.Forms.Button();
      this.desc1 = new WinFormsDemoApp.GoWebBrowser();
      this.modelJson1 = new WinFormsDemoApp.ModelJson();
      this.tableLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
      this.SuspendLayout();
      //
      // tableLayoutPanel1
      //
      this.tableLayoutPanel1.AutoScroll = true;
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.layoutBtn, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.desc1, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.modelJson1, 0, 3);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 4;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 846);
      this.tableLayoutPanel1.TabIndex = 0;
      //
      // diagramControl1
      //
      this.diagramControl1.AllowDrop = true;
      this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
      this.diagramControl1.Location = new System.Drawing.Point(3, 3);
      this.diagramControl1.Name = "diagramControl1";
      this.diagramControl1.Size = new System.Drawing.Size(994, 244);
      this.diagramControl1.TabIndex = 0;
      this.diagramControl1.Text = "diagramControl1";
      //
      // layoutBtn
      //
      this.layoutBtn.Location = new System.Drawing.Point(3, 253);
      this.layoutBtn.Name = "layoutBtn";
      this.layoutBtn.Size = new System.Drawing.Size(117, 34);
      this.layoutBtn.TabIndex = 1;
      this.layoutBtn.Text = "Layout Diagram";
      this.layoutBtn.UseVisualStyleBackColor = true;
      //
      // desc1
      //
      this.desc1.CreationProperties = null;
      this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
      this.desc1.Dock = System.Windows.Forms.DockStyle.Top;
      this.desc1.Location = new System.Drawing.Point(3, 293);
      this.desc1.Name = "desc1";
      this.desc1.Size = new System.Drawing.Size(994, 194);
      this.desc1.TabIndex = 2;
      this.desc1.ZoomFactor = 1D;
      //
      // modelJson1
      //
      this.modelJson1.AutoSize = true;
      this.modelJson1.Dock = System.Windows.Forms.DockStyle.Top;
      this.modelJson1.Location = new System.Drawing.Point(3, 493);
      this.modelJson1.Name = "modelJson1";
      this.modelJson1.Size = new System.Drawing.Size(994, 343);
      this.modelJson1.TabIndex = 3;
      //
      // MindMapControl
      //
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tableLayoutPanel1);
      this.Name = "MindMapControl";
      this.Size = new System.Drawing.Size(1000, 846);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private System.Windows.Forms.Button layoutBtn;
    private WinFormsDemoApp.GoWebBrowser desc1;
    private WinFormsDemoApp.ModelJson modelJson1;
  }
}
