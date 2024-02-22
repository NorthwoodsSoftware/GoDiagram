/* Copyright 1998-2024 by Northwoods Software Corporation. */

namespace Demo.Samples.OrgChartStatic {
  partial class OrgChartStatic {
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.searchBtn = new System.Windows.Forms.Button();
            this.desc1 = new WinFormsDemoApp.GoWebBrowser();
            this.overviewControl1 = new Northwoods.Go.WinForms.OverviewControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.desc1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1430, 1435);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(5, 6);
            this.diagramControl1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(1420, 1063);
            this.diagramControl1.TabIndex = 0;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel2.Controls.Add(this.textBox1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.searchBtn, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 1078);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(231, 48);
            this.tableLayoutPanel2.TabIndex = 1;
            //
            // textBox1
            //
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(143, 31);
            this.textBox1.TabIndex = 3;
            //
            // searchBtn
            //
            this.searchBtn.AutoSize = true;
            this.searchBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.searchBtn.Location = new System.Drawing.Point(153, 3);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(74, 35);
            this.searchBtn.TabIndex = 4;
            this.searchBtn.Text = "Search";
            this.searchBtn.UseVisualStyleBackColor = true;
            //
            // desc1
            //
            this.desc1.CreationProperties = null;
            this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.desc1.Dock = System.Windows.Forms.DockStyle.Top;
            this.desc1.Location = new System.Drawing.Point(3, 1132);
            this.desc1.Name = "desc1";
            this.desc1.Size = new System.Drawing.Size(1424, 300);
            this.desc1.TabIndex = 2;
            this.desc1.ZoomFactor = 1D;
            //
            // overviewControl1
            //
            this.overviewControl1.AllowDrop = true;
            this.overviewControl1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.overviewControl1.Location = new System.Drawing.Point(10, 10);
            this.overviewControl1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.overviewControl1.Name = "overviewControl1";
            this.overviewControl1.Size = new System.Drawing.Size(286, 167);
            this.overviewControl1.TabIndex = 3;
            //
            // OrgChartStaticControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.overviewControl1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "OrgChartStaticControl";
            this.Size = new System.Drawing.Size(1430, 1435);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Button searchBtn;
    private WinFormsDemoApp.GoWebBrowser desc1;
    private Northwoods.Go.WinForms.OverviewControl overviewControl1;
  }
}
