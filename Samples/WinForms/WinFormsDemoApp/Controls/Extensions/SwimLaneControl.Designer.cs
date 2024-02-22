/* Copyright 1998-2024 by Northwoods Software Corporation. */

namespace Demo.Extensions.SwimLane {
  partial class SwimLane {
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
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.conferencesRb = new System.Windows.Forms.RadioButton();
            this.divisionsRb = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.desc1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(799, 791);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // desc1
            //
            this.desc1.CreationProperties = null;
            this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.desc1.Dock = System.Windows.Forms.DockStyle.Top;
            this.desc1.Location = new System.Drawing.Point(3, 3);
            this.desc1.Name = "desc1";
            this.desc1.Size = new System.Drawing.Size(793, 29);
            this.desc1.TabIndex = 0;
            this.desc1.ZoomFactor = 1D;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 38);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(793, 694);
            this.diagramControl1.TabIndex = 1;
            this.diagramControl1.Text = "diagramControl1";
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.conferencesRb, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.divisionsRb, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 738);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(97, 50);
            this.tableLayoutPanel2.TabIndex = 2;
            //
            // conferencesRb
            //
            this.conferencesRb.AutoSize = true;
            this.conferencesRb.Location = new System.Drawing.Point(3, 3);
            this.conferencesRb.Name = "conferencesRb";
            this.conferencesRb.Size = new System.Drawing.Size(91, 19);
            this.conferencesRb.TabIndex = 0;
            this.conferencesRb.Text = "Conferences";
            this.conferencesRb.UseVisualStyleBackColor = true;
            //
            // divisionsRb
            //
            this.divisionsRb.AutoSize = true;
            this.divisionsRb.Checked = true;
            this.divisionsRb.Location = new System.Drawing.Point(3, 28);
            this.divisionsRb.Name = "divisionsRb";
            this.divisionsRb.Size = new System.Drawing.Size(72, 19);
            this.divisionsRb.TabIndex = 1;
            this.divisionsRb.TabStop = true;
            this.divisionsRb.Text = "Divisions";
            this.divisionsRb.UseVisualStyleBackColor = true;
            //
            // SwimLaneControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SwimLaneControl";
            this.Size = new System.Drawing.Size(799, 791);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private WinFormsDemoApp.GoWebBrowser desc1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.RadioButton conferencesRb;
    private System.Windows.Forms.RadioButton divisionsRb;
  }
}
