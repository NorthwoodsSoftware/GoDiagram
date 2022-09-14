/* Copyright 1998-2022 by Northwoods Software Corporation. */

namespace Demo.Samples.VisualTree {
  partial class VisualTree {
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.diagramControl2 = new Northwoods.Go.WinForms.DiagramControl();
            this.desc1 = new WinFormsDemoApp.GoWebBrowser();
            this.drawVisualTreeBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.drawVisualTreeBtn, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.diagramControl2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.desc1, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 844);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "_Diagram, the diagram being inspected:";
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "_VisualTree, showing the Layers, Nodes and Links that are in _Diagram above:";
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            //
            // diagramControl2
            //
            this.diagramControl2.AllowDrop = true;
            this.diagramControl2.Location = new System.Drawing.Point(3, 53);
            this.diagramControl2.Name = "diagramControl2";
            this.diagramControl2.Size = new System.Drawing.Size(988, 244);
            this.diagramControl2.TabIndex = 1;
            this.diagramControl2.Text = "diagramControl2";
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Location = new System.Drawing.Point(3, 48);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(344, 244);
            this.diagramControl1.TabIndex = 0;
            this.diagramControl1.Text = "diagramControl1";
            //
            // desc1
            //
            this.desc1.CreationProperties = null;
            this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.desc1.Dock = System.Windows.Forms.DockStyle.Top;
            this.desc1.Location = new System.Drawing.Point(3, 3);
            this.desc1.Name = "desc1";
            this.desc1.Size = new System.Drawing.Size(344, 250);
            this.desc1.TabIndex = 1;
            this.desc1.ZoomFactor = 1D;
            //
            // drawVisualTreeBtn
            //
            this.drawVisualTreeBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.drawVisualTreeBtn.Location = new System.Drawing.Point(3, 303);
            this.drawVisualTreeBtn.Name = "drawVisualTreeBtn";
            this.drawVisualTreeBtn.Size = new System.Drawing.Size(156, 39);
            this.drawVisualTreeBtn.TabIndex = 2;
            this.drawVisualTreeBtn.Text = "Draw Visual Tree";
            this.drawVisualTreeBtn.UseVisualStyleBackColor = true;
            //
            // VisualTreeControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "VisualTreeControl";
            this.Size = new System.Drawing.Size(1000, 844);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Label label1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private System.Windows.Forms.Button drawVisualTreeBtn;
    private System.Windows.Forms.Label label2;
    private Northwoods.Go.WinForms.DiagramControl diagramControl2;
    private WinFormsDemoApp.GoWebBrowser desc1;
  }
}
