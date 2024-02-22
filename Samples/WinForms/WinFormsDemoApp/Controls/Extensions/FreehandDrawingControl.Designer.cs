/* Copyright 1998-2024 by Northwoods Software Corporation. */

namespace Demo.Extensions.FreehandDrawing {
  partial class FreehandDrawing {
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.drawBtn = new System.Windows.Forms.Button();
            this.selectBtn = new System.Windows.Forms.Button();
            this.resizingCb = new System.Windows.Forms.CheckBox();
            this.reshapingCb = new System.Windows.Forms.CheckBox();
            this.rotatingCb = new System.Windows.Forms.CheckBox();
            this.desc1 = new WinFormsDemoApp.GoWebBrowser();
            this.modelJson1 = new WinFormsDemoApp.ModelJson();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1342, 1326);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(1336, 687);
            this.diagramControl1.TabIndex = 0;
            this.diagramControl1.Text = "diagramControl1";
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.drawBtn, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.selectBtn, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.resizingCb, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.reshapingCb, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.rotatingCb, 4, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 696);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(807, 31);
            this.tableLayoutPanel2.TabIndex = 1;
            //
            // drawBtn
            //
            this.drawBtn.AutoSize = true;
            this.drawBtn.Location = new System.Drawing.Point(128, 3);
            this.drawBtn.Name = "drawBtn";
            this.drawBtn.Size = new System.Drawing.Size(81, 25);
            this.drawBtn.TabIndex = 1;
            this.drawBtn.Text = "Draw Mode";
            this.drawBtn.UseVisualStyleBackColor = true;
            //
            // selectBtn
            //
            this.selectBtn.AutoSize = true;
            this.selectBtn.Location = new System.Drawing.Point(3, 3);
            this.selectBtn.Name = "selectBtn";
            this.selectBtn.Size = new System.Drawing.Size(48, 25);
            this.selectBtn.TabIndex = 0;
            this.selectBtn.Text = "Select";
            this.selectBtn.UseVisualStyleBackColor = true;
            //
            // resizingCb
            //
            this.resizingCb.AutoSize = true;
            this.resizingCb.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.resizingCb.Checked = true;
            this.resizingCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.resizingCb.Location = new System.Drawing.Point(253, 3);
            this.resizingCb.Name = "resizingCb";
            this.resizingCb.Size = new System.Drawing.Size(102, 19);
            this.resizingCb.TabIndex = 4;
            this.resizingCb.Text = "Allow Resizing";
            this.resizingCb.UseVisualStyleBackColor = true;
            //
            // reshapingCb
            //
            this.reshapingCb.AutoSize = true;
            this.reshapingCb.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.reshapingCb.Checked = true;
            this.reshapingCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.reshapingCb.Location = new System.Drawing.Point(428, 3);
            this.reshapingCb.Name = "reshapingCb";
            this.reshapingCb.Size = new System.Drawing.Size(114, 19);
            this.reshapingCb.TabIndex = 5;
            this.reshapingCb.Text = "Allow Reshaping";
            this.reshapingCb.UseVisualStyleBackColor = true;
            //
            // rotatingCb
            //
            this.rotatingCb.AutoSize = true;
            this.rotatingCb.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rotatingCb.Checked = true;
            this.rotatingCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rotatingCb.Location = new System.Drawing.Point(528, 3);
            this.rotatingCb.Name = "rotatingCb";
            this.rotatingCb.Size = new System.Drawing.Size(104, 19);
            this.rotatingCb.TabIndex = 6;
            this.rotatingCb.Text = "Allow Rotating";
            this.rotatingCb.UseVisualStyleBackColor = true;
            //
            // desc1
            //
            this.desc1.CreationProperties = null;
            this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.desc1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.desc1.Location = new System.Drawing.Point(3, 744);
            this.desc1.Name = "desc1";
            this.desc1.Size = new System.Drawing.Size(1336, 159);
            this.desc1.TabIndex = 2;
            this.desc1.ZoomFactor = 1D;
            //
            // modelJson1
            //
            this.modelJson1.AutoSize = true;
            this.modelJson1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelJson1.Location = new System.Drawing.Point(3, 909);
            this.modelJson1.Name = "modelJson1";
            this.modelJson1.Size = new System.Drawing.Size(1336, 414);
            this.modelJson1.TabIndex = 3;
            //
            // FreehandDrawingControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FreehandDrawingControl";
            this.Size = new System.Drawing.Size(1342, 1326);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Button drawBtn;
    private System.Windows.Forms.Button selectBtn;
    private WinFormsDemoApp.GoWebBrowser desc1;
    private System.Windows.Forms.CheckBox resizingCb;
    private System.Windows.Forms.CheckBox reshapingCb;
    private System.Windows.Forms.CheckBox rotatingCb;
    private WinFormsDemoApp.ModelJson modelJson1;
  }
}
