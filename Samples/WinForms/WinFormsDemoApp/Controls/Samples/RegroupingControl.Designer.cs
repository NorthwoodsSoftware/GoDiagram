/* Copyright 1998-2022 by Northwoods Software Corporation. */

namespace Demo.Samples.Regrouping {
  partial class Regrouping {
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
            this.paletteControl1 = new Northwoods.Go.WinForms.PaletteControl();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.desc1 = new WinFormsDemoApp.GoWebBrowser();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.levelSlider = new System.Windows.Forms.TrackBar();
            this.modelJson1 = new WinFormsDemoApp.ModelJson();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.levelSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.paletteControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.desc1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.modelJson1, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.TabIndex = 1;
            //
            // paletteControl1
            //
            this.paletteControl1.AllowDrop = true;
            this.paletteControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(60)))));
            this.paletteControl1.Location = new System.Drawing.Point(4, 3);
            this.paletteControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.paletteControl1.Name = "paletteControl1";
            this.paletteControl1.Size = new System.Drawing.Size(132, 594);
            this.paletteControl1.TabIndex = 0;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(60)))));
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(144, 3);
            this.diagramControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(852, 594);
            this.diagramControl1.TabIndex = 1;
            //
            // desc1
            //
            this.tableLayoutPanel1.SetColumnSpan(this.desc1, 2);
            this.desc1.CreationProperties = null;
            this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.desc1.Dock = System.Windows.Forms.DockStyle.Top;
            this.desc1.Location = new System.Drawing.Point(3, 603);
            this.desc1.Name = "desc1";
            this.desc1.Size = new System.Drawing.Size(994, 160);
            this.desc1.TabIndex = 2;
            this.desc1.ZoomFactor = 1D;
            //
            // flowLayoutPanel1
            //
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.levelSlider);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 338);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(422, 37);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Semantic zoom level:";
            //
            // levelSlider
            //
            this.levelSlider.AutoSize = true;
            this.levelSlider.Location = new System.Drawing.Point(3, 828);
            this.levelSlider.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.levelSlider.Maximum = 5;
            this.levelSlider.Name = "levelSlider";
            this.levelSlider.Size = new System.Drawing.Size(222, 25);
            this.levelSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.levelSlider.TabIndex = 5;
            this.levelSlider.Value = 3;
            //
            // modelJson1
            //
            this.modelJson1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.modelJson1, 2);
            this.modelJson1.Dock = System.Windows.Forms.DockStyle.Top;
            this.modelJson1.Location = new System.Drawing.Point(3, 879);
            this.modelJson1.Name = "modelJson1";
            this.modelJson1.Size = new System.Drawing.Size(994, 335);
            this.modelJson1.TabIndex = 6;
            //
            // RegroupingControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "RegroupingControl";
            this.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.levelSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.PaletteControl paletteControl1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsDemoApp.GoWebBrowser desc1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TrackBar levelSlider;
    private WinFormsDemoApp.ModelJson modelJson1;
  }
}
