/* Copyright 1998-2022 by Northwoods Software Corporation. */

namespace Demo.Samples.SpacingZoom {
  partial class SpacingZoomControl {
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
            this.checkBxYAxis = new System.Windows.Forms.CheckBox();
            this.goWebBrowser1 = new WinFormsDemoApp.GoWebBrowser();
            this.lblSpacingFactor = new System.Windows.Forms.Label();
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
            this.tableLayoutPanel1.Controls.Add(this.checkBxYAxis, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblSpacingFactor, 0, 1);
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
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Location = new System.Drawing.Point(3, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(600, 594);
            this.diagramControl1.TabIndex = 0;
            this.diagramControl1.Text = "diagramControl1";
            //
            // checkBxYAxis
            //
            this.checkBxYAxis.AutoSize = true;
            this.checkBxYAxis.Checked = true;
            this.checkBxYAxis.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBxYAxis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBxYAxis.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBxYAxis.Location = new System.Drawing.Point(3, 618);
            this.checkBxYAxis.Name = "checkBxYAxis";
            this.checkBxYAxis.Size = new System.Drawing.Size(994, 19);
            this.checkBxYAxis.TabIndex = 2;
            this.checkBxYAxis.Text = "Is Y Axis Spaced?";
            this.checkBxYAxis.UseVisualStyleBackColor = true;
            //
            // goWebBrowser1
            //
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 643);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(994, 554);
            this.goWebBrowser1.TabIndex = 3;
            this.goWebBrowser1.ZoomFactor = 1D;
            //
            // lblSpacingFactor
            //
            this.lblSpacingFactor.AutoSize = true;
            this.lblSpacingFactor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSpacingFactor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSpacingFactor.Location = new System.Drawing.Point(3, 600);
            this.lblSpacingFactor.Name = "lblSpacingFactor";
            this.lblSpacingFactor.Size = new System.Drawing.Size(994, 15);
            this.lblSpacingFactor.TabIndex = 4;
            this.lblSpacingFactor.Text = "Spacing factor: 1";
            this.lblSpacingFactor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // SpacingZoomControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SpacingZoomControl";
            this.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private System.Windows.Forms.CheckBox checkBxYAxis;
    private WinFormsDemoApp.GoWebBrowser goWebBrowser1;
    private System.Windows.Forms.Label lblSpacingFactor;
  }
}
