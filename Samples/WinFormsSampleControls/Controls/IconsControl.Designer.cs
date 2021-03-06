/* Copyright 1998-2022 by Northwoods Software Corporation. */

namespace WinFormsSampleControls.Icons {
  partial class IconsControl {
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
            this.diagramControl2 = new Northwoods.Go.WinForms.DiagramControl();
            this.goWebBrowser2 = new WinFormsSharedControls.GoWebBrowser();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser2)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.diagramControl2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser2, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(941, 1062);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Location = new System.Drawing.Point(3, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(498, 310);
            this.diagramControl1.TabIndex = 0;
            this.diagramControl1.Text = "diagramControl1";
            //
            // goWebBrowser1
            //
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 319);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(935, 144);
            this.goWebBrowser1.TabIndex = 1;
            this.goWebBrowser1.ZoomFactor = 1D;
            //
            // diagramControl2
            //
            this.diagramControl2.AllowDrop = true;
            this.diagramControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramControl2.Location = new System.Drawing.Point(3, 469);
            this.diagramControl2.Name = "diagramControl2";
            this.diagramControl2.Size = new System.Drawing.Size(935, 394);
            this.diagramControl2.TabIndex = 2;
            this.diagramControl2.Text = "diagramControl2";
            //
            // goWebBrowser2
            //
            this.goWebBrowser2.CreationProperties = null;
            this.goWebBrowser2.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goWebBrowser2.Location = new System.Drawing.Point(3, 869);
            this.goWebBrowser2.Name = "goWebBrowser2";
            this.goWebBrowser2.Size = new System.Drawing.Size(935, 190);
            this.goWebBrowser2.TabIndex = 3;
            this.goWebBrowser2.ZoomFactor = 1D;
            //
            // IconsControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "IconsControl";
            this.Size = new System.Drawing.Size(941, 1062);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser2)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl2;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser2;
  }
}
