/* Copyright 1998-2024 by Northwoods Software Corporation. */

namespace Demo.Extensions.ZoomSlider {
  partial class ZoomSlider {
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
            this.desc1 = new WinFormsDemoApp.GoWebBrowser();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.minusBtn = new System.Windows.Forms.Button();
            this.slider = new System.Windows.Forms.TrackBar();
            this.plusBtn = new System.Windows.Forms.Button();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slider)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.desc1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 730);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // desc1
            //
            this.desc1.CreationProperties = null;
            this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.desc1.Dock = System.Windows.Forms.DockStyle.Top;
            this.desc1.Location = new System.Drawing.Point(3, 483);
            this.desc1.Name = "desc1";
            this.desc1.Size = new System.Drawing.Size(994, 234);
            this.desc1.TabIndex = 0;
            this.desc1.ZoomFactor = 1D;
            //
            // panel1
            //
            this.panel1.Controls.Add(this.tableLayoutPanel3);
            this.panel1.Controls.Add(this.diagramControl1);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(532, 474);
            this.panel1.TabIndex = 1;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.minusBtn, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.slider, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.plusBtn, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(489, 239);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(31, 220);
            this.tableLayoutPanel3.TabIndex = 2;
            //
            // minusBtn
            //
            this.minusBtn.AutoSize = true;
            this.minusBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.minusBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.minusBtn.Location = new System.Drawing.Point(3, 183);
            this.minusBtn.Name = "minusBtn";
            this.minusBtn.Size = new System.Drawing.Size(26, 31);
            this.minusBtn.TabIndex = 7;
            this.minusBtn.Text = "-";
            this.minusBtn.UseVisualStyleBackColor = true;
            //
            // slider
            //
            this.slider.Location = new System.Drawing.Point(3, 40);
            this.slider.Maximum = 100;
            this.slider.Minimum = -50;
            this.slider.Name = "slider";
            this.slider.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.slider.Size = new System.Drawing.Size(45, 137);
            this.slider.TabIndex = 4;
            this.slider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.slider.Value = 0;
            //
            // plusBtn
            //
            this.plusBtn.AutoSize = true;
            this.plusBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.plusBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.plusBtn.Location = new System.Drawing.Point(3, 3);
            this.plusBtn.Name = "plusBtn";
            this.plusBtn.Size = new System.Drawing.Size(31, 31);
            this.plusBtn.TabIndex = 6;
            this.plusBtn.Text = "+";
            this.plusBtn.UseVisualStyleBackColor = true;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.diagramControl1.Location = new System.Drawing.Point(0, 0);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(529, 474);
            this.diagramControl1.TabIndex = 0;
            this.diagramControl1.Text = "diagramControl1";
            //
            // ZoomSliderSampleControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ZoomSliderSampleControl";
            this.Size = new System.Drawing.Size(1000, 730);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slider)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private WinFormsDemoApp.GoWebBrowser desc1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private System.Windows.Forms.TrackBar slider;
    private System.Windows.Forms.Button minusBtn;
    private System.Windows.Forms.Button plusBtn;
  }
}
