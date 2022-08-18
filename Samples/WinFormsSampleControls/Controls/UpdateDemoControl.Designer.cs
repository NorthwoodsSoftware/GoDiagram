/* Copyright 1998-2022 by Northwoods Software Corporation. */

namespace WinFormsSampleControls.UpdateDemo {
  partial class UpdateDemoControl {
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
            this.goWebBrowser1 = new WinFormsSharedControls.GoWebBrowser();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.clearLogModelBtn = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.diagramControl2 = new Northwoods.Go.WinForms.DiagramControl();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.redoBtn = new System.Windows.Forms.Button();
            this.undoBtn = new System.Windows.Forms.Button();
            this.diagramControl3 = new Northwoods.Go.WinForms.DiagramControl();
            this.goWebBrowser2 = new WinFormsSharedControls.GoWebBrowser();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser2)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.diagramControl2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser2, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // goWebBrowser1
            //
            this.tableLayoutPanel1.SetColumnSpan(this.goWebBrowser1, 2);
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 3);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(994, 24);
            this.goWebBrowser1.TabIndex = 0;
            this.goWebBrowser1.ZoomFactor = 1D;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 33);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(494, 224);
            this.diagramControl1.TabIndex = 1;
            this.diagramControl1.Text = "diagramControl1";
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.clearLogModelBtn, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.listBox1, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(503, 33);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.91304F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.08696F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(494, 224);
            this.tableLayoutPanel2.TabIndex = 2;
            //
            // clearLogModelBtn
            //
            this.clearLogModelBtn.Location = new System.Drawing.Point(3, 3);
            this.clearLogModelBtn.Name = "clearLogModelBtn";
            this.clearLogModelBtn.Size = new System.Drawing.Size(163, 25);
            this.clearLogModelBtn.TabIndex = 0;
            this.clearLogModelBtn.Text = "Clear Model Log";
            this.clearLogModelBtn.UseVisualStyleBackColor = true;
            //
            // listBox1
            //
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(3, 34);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(488, 184);
            this.listBox1.TabIndex = 1;
            //
            // diagramControl2
            //
            this.diagramControl2.AllowDrop = true;
            this.diagramControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl2.Location = new System.Drawing.Point(3, 263);
            this.diagramControl2.Name = "diagramControl2";
            this.diagramControl2.Size = new System.Drawing.Size(494, 274);
            this.diagramControl2.TabIndex = 3;
            this.diagramControl2.Text = "diagramControl2";
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.64777F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.35223F));
            this.tableLayoutPanel3.Controls.Add(this.redoBtn, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.undoBtn, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.diagramControl3, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(503, 263);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.23022F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.76978F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(494, 274);
            this.tableLayoutPanel3.TabIndex = 4;
            //
            // redoBtn
            //
            this.redoBtn.Location = new System.Drawing.Point(104, 3);
            this.redoBtn.Name = "redoBtn";
            this.redoBtn.Size = new System.Drawing.Size(75, 26);
            this.redoBtn.TabIndex = 2;
            this.redoBtn.Text = "Redo";
            this.redoBtn.UseVisualStyleBackColor = true;
            //
            // undoBtn
            //
            this.undoBtn.Location = new System.Drawing.Point(3, 3);
            this.undoBtn.Name = "undoBtn";
            this.undoBtn.Size = new System.Drawing.Size(93, 26);
            this.undoBtn.TabIndex = 1;
            this.undoBtn.Text = "Undo";
            this.undoBtn.UseVisualStyleBackColor = true;
            //
            // diagramControl3
            //
            this.diagramControl3.AllowDrop = true;
            this.tableLayoutPanel3.SetColumnSpan(this.diagramControl3, 2);
            this.diagramControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl3.Location = new System.Drawing.Point(3, 36);
            this.diagramControl3.Name = "diagramControl3";
            this.diagramControl3.Size = new System.Drawing.Size(488, 235);
            this.diagramControl3.TabIndex = 3;
            this.diagramControl3.Text = "diagramControl3";
            //
            // goWebBrowser2
            //
            this.tableLayoutPanel1.SetColumnSpan(this.goWebBrowser2, 2);
            this.goWebBrowser2.CreationProperties = null;
            this.goWebBrowser2.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser2.Dock = System.Windows.Forms.DockStyle.Top;
            this.goWebBrowser2.Location = new System.Drawing.Point(3, 543);
            this.goWebBrowser2.Name = "goWebBrowser2";
            this.goWebBrowser2.Size = new System.Drawing.Size(994, 344);
            this.goWebBrowser2.TabIndex = 5;
            this.goWebBrowser2.ZoomFactor = 1D;
            //
            // UpdateDemoControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UpdateDemoControl";
            this.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser2)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Button clearLogModelBtn;
    private System.Windows.Forms.ListBox listBox1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl2;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.Button redoBtn;
    private System.Windows.Forms.Button undoBtn;
    private Northwoods.Go.WinForms.DiagramControl diagramControl3;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser2;
  }
}
