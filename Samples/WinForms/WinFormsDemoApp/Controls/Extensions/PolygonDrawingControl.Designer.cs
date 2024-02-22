/* Copyright 1998-2024 by Northwoods Software Corporation. */

namespace Demo.Extensions.PolygonDrawing {
  partial class PolygonDrawing {
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.drawPolygonBtn = new System.Windows.Forms.Button();
            this.undoPtBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.finishBtn = new System.Windows.Forms.Button();
            this.drawPolylineBtn = new System.Windows.Forms.Button();
            this.selectBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.rotatingCb = new System.Windows.Forms.CheckBox();
            this.reshapingCb = new System.Windows.Forms.CheckBox();
            this.resizingCb = new System.Windows.Forms.CheckBox();
            this.resegmentingCb = new System.Windows.Forms.CheckBox();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.desc1 = new WinFormsDemoApp.GoWebBrowser();
            this.modelJson1 = new WinFormsDemoApp.ModelJson();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.desc1, 0, 3);
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
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.drawPolygonBtn, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.undoPtBtn, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.cancelBtn, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.finishBtn, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.drawPolylineBtn, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.selectBtn, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 403);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(560, 31);
            this.tableLayoutPanel2.TabIndex = 0;
            //
            // drawPolygonBtn
            //
            this.drawPolygonBtn.AutoSize = true;
            this.drawPolygonBtn.Location = new System.Drawing.Point(57, 3);
            this.drawPolygonBtn.Name = "drawPolygonBtn";
            this.drawPolygonBtn.Size = new System.Drawing.Size(91, 25);
            this.drawPolygonBtn.TabIndex = 5;
            this.drawPolygonBtn.Text = "Draw Polygon";
            this.drawPolygonBtn.UseVisualStyleBackColor = true;
            //
            // undoPtBtn
            //
            this.undoPtBtn.AutoSize = true;
            this.undoPtBtn.Location = new System.Drawing.Point(456, 3);
            this.undoPtBtn.Name = "undoPtBtn";
            this.undoPtBtn.Size = new System.Drawing.Size(101, 25);
            this.undoPtBtn.TabIndex = 4;
            this.undoPtBtn.Text = "Undo Last Point";
            this.undoPtBtn.UseVisualStyleBackColor = true;
            //
            // cancelBtn
            //
            this.cancelBtn.AutoSize = true;
            this.cancelBtn.Location = new System.Drawing.Point(350, 3);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(100, 25);
            this.cancelBtn.TabIndex = 3;
            this.cancelBtn.Text = "Cancel Drawing";
            this.cancelBtn.UseVisualStyleBackColor = true;
            //
            // finishBtn
            //
            this.finishBtn.AutoSize = true;
            this.finishBtn.Location = new System.Drawing.Point(249, 3);
            this.finishBtn.Name = "finishBtn";
            this.finishBtn.Size = new System.Drawing.Size(95, 25);
            this.finishBtn.TabIndex = 2;
            this.finishBtn.Text = "Finish Drawing";
            this.finishBtn.UseVisualStyleBackColor = true;
            //
            // drawPolylineBtn
            //
            this.drawPolylineBtn.AutoSize = true;
            this.drawPolylineBtn.Location = new System.Drawing.Point(154, 3);
            this.drawPolylineBtn.Name = "drawPolylineBtn";
            this.drawPolylineBtn.Size = new System.Drawing.Size(89, 25);
            this.drawPolylineBtn.TabIndex = 1;
            this.drawPolylineBtn.Text = "Draw Polyline";
            this.drawPolylineBtn.UseVisualStyleBackColor = true;
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
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.rotatingCb, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.reshapingCb, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.resizingCb, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.resegmentingCb, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 440);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(479, 25);
            this.tableLayoutPanel3.TabIndex = 1;
            //
            // rotatingCb
            //
            this.rotatingCb.AutoSize = true;
            this.rotatingCb.Checked = true;
            this.rotatingCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rotatingCb.Location = new System.Drawing.Point(372, 3);
            this.rotatingCb.Name = "rotatingCb";
            this.rotatingCb.Size = new System.Drawing.Size(104, 19);
            this.rotatingCb.TabIndex = 2;
            this.rotatingCb.Text = "Allow Rotating";
            this.rotatingCb.UseVisualStyleBackColor = true;
            //
            // reshapingCb
            //
            this.reshapingCb.AutoSize = true;
            this.reshapingCb.Checked = true;
            this.reshapingCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.reshapingCb.Location = new System.Drawing.Point(111, 3);
            this.reshapingCb.Name = "reshapingCb";
            this.reshapingCb.Size = new System.Drawing.Size(114, 19);
            this.reshapingCb.TabIndex = 1;
            this.reshapingCb.Text = "Allow Reshaping";
            this.reshapingCb.UseVisualStyleBackColor = true;
            //
            // resizingCb
            //
            this.resizingCb.AutoSize = true;
            this.resizingCb.Checked = true;
            this.resizingCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.resizingCb.Location = new System.Drawing.Point(3, 3);
            this.resizingCb.Name = "resizingCb";
            this.resizingCb.Size = new System.Drawing.Size(102, 19);
            this.resizingCb.TabIndex = 0;
            this.resizingCb.Text = "Allow Resizing";
            this.resizingCb.UseVisualStyleBackColor = true;
            //
            // resegmentingCb
            //
            this.resegmentingCb.AutoSize = true;
            this.resegmentingCb.Checked = true;
            this.resegmentingCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.resegmentingCb.Location = new System.Drawing.Point(231, 3);
            this.resegmentingCb.Name = "resegmentingCb";
            this.resegmentingCb.Size = new System.Drawing.Size(135, 19);
            this.resegmentingCb.TabIndex = 3;
            this.resegmentingCb.Text = "Allow Resegmenting";
            this.resegmentingCb.UseVisualStyleBackColor = true;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.BackColor = System.Drawing.Color.White;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramControl1.Location = new System.Drawing.Point(3, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(994, 394);
            this.diagramControl1.TabIndex = 2;
            this.diagramControl1.Text = "diagramControl1";
            //
            // desc1
            //
            this.desc1.CreationProperties = null;
            this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.desc1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.desc1.Location = new System.Drawing.Point(3, 471);
            this.desc1.Name = "desc1";
            this.desc1.Size = new System.Drawing.Size(994, 224);
            this.desc1.TabIndex = 3;
            this.desc1.ZoomFactor = 1D;
            //
            // modelJson1
            //
            this.modelJson1.AutoSize = true;
            this.modelJson1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelJson1.Location = new System.Drawing.Point(3, 701);
            this.modelJson1.Name = "modelJson1";
            this.modelJson1.Size = new System.Drawing.Size(994, 496);
            this.modelJson1.TabIndex = 4;
            //
            // PolygonDrawingControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PolygonDrawingControl";
            this.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Button undoPtBtn;
    private System.Windows.Forms.Button cancelBtn;
    private System.Windows.Forms.Button finishBtn;
    private System.Windows.Forms.Button drawPolylineBtn;
    private System.Windows.Forms.Button selectBtn;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.CheckBox rotatingCb;
    private System.Windows.Forms.CheckBox reshapingCb;
    private System.Windows.Forms.CheckBox resizingCb;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private System.Windows.Forms.Button drawPolygonBtn;
    private WinFormsDemoApp.GoWebBrowser desc1;
    private WinFormsDemoApp.ModelJson modelJson1;
    private System.Windows.Forms.CheckBox resegmentingCb;
  }
}
