/* Copyright 1998-2023 by Northwoods Software Corporation. */

namespace Demo.Samples.Flowgrammer {
  partial class FlowgrammerControl {
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
            this.overviewControl1 = new Northwoods.Go.WinForms.OverviewControl();
            this.btnNewDiagram = new System.Windows.Forms.Button();
            this.goWebBrowser1 = new WinFormsDemoApp.GoWebBrowser();
            this.modelJson1 = new WinFormsDemoApp.ModelJson();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
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
            this.tableLayoutPanel1.Controls.Add(this.overviewControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnNewDiagram, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.modelJson1, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // paletteControl1
            //
            this.paletteControl1.AllowDrop = true;
            this.paletteControl1.Location = new System.Drawing.Point(3, 3);
            this.paletteControl1.Name = "paletteControl1";
            this.paletteControl1.Size = new System.Drawing.Size(144, 444);
            this.paletteControl1.TabIndex = 0;
            this.paletteControl1.Text = "paletteControl1";
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(153, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.tableLayoutPanel1.SetRowSpan(this.diagramControl1, 2);
            this.diagramControl1.Size = new System.Drawing.Size(844, 594);
            this.diagramControl1.TabIndex = 1;
            this.diagramControl1.Text = "diagramControl1";
            //
            // overviewControl1
            //
            this.overviewControl1.AllowDrop = true;
            this.overviewControl1.Location = new System.Drawing.Point(3, 453);
            this.overviewControl1.Name = "overviewControl1";
            this.overviewControl1.Size = new System.Drawing.Size(144, 144);
            this.overviewControl1.TabIndex = 2;
            this.overviewControl1.Text = "overviewControl1";
            //
            // btnNewDiagram
            //
            this.btnNewDiagram.Location = new System.Drawing.Point(3, 603);
            this.btnNewDiagram.Name = "btnNewDiagram";
            this.btnNewDiagram.Size = new System.Drawing.Size(127, 35);
            this.btnNewDiagram.TabIndex = 3;
            this.btnNewDiagram.Text = "New Diagram";
            this.btnNewDiagram.UseVisualStyleBackColor = true;
            //
            // goWebBrowser1
            //
            this.tableLayoutPanel1.SetColumnSpan(this.goWebBrowser1, 2);
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 644);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(994, 144);
            this.goWebBrowser1.TabIndex = 5;
            this.goWebBrowser1.ZoomFactor = 1D;
            //
            // modelJson1
            //
            this.modelJson1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.modelJson1, 2);
            this.modelJson1.Dock = System.Windows.Forms.DockStyle.Top;
            this.modelJson1.Location = new System.Drawing.Point(3, 794);
            this.modelJson1.Name = "modelJson1";
            this.modelJson1.Size = new System.Drawing.Size(994, 343);
            this.modelJson1.TabIndex = 6;
            //
            // FlowgrammerControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FlowgrammerControl";
            this.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.PaletteControl paletteControl1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private Northwoods.Go.WinForms.OverviewControl overviewControl1;
    private System.Windows.Forms.Button btnNewDiagram;
    private WinFormsDemoApp.GoWebBrowser goWebBrowser1;
    private WinFormsDemoApp.ModelJson modelJson1;
  }
}
