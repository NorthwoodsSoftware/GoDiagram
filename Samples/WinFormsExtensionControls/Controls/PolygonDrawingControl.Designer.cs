/* Copyright 1998-2022 by Northwoods Software Corporation. */

namespace WinFormsExtensionControls.PolygonDrawing {
  partial class PolygonDrawingControl {
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
            this.drawPolygon = new System.Windows.Forms.Button();
            this.undoLastPoint = new System.Windows.Forms.Button();
            this.cancelDrawing = new System.Windows.Forms.Button();
            this.finishDrawing = new System.Windows.Forms.Button();
            this.drawPolyline = new System.Windows.Forms.Button();
            this.select = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.allowRotating = new System.Windows.Forms.CheckBox();
            this.allowReshaping = new System.Windows.Forms.CheckBox();
            this.allowResizing = new System.Windows.Forms.CheckBox();
            this.allowResegmenting = new System.Windows.Forms.CheckBox();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.goWebBrowser1 = new WinFormsSharedControls.GoWebBrowser();
            this.modelJson1 = new WinFormsSharedControls.ModelJson();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
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
            this.tableLayoutPanel2.Controls.Add(this.drawPolygon, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.undoLastPoint, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.cancelDrawing, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.finishDrawing, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.drawPolyline, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.select, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 403);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(560, 31);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // drawPolygon
            // 
            this.drawPolygon.AutoSize = true;
            this.drawPolygon.Location = new System.Drawing.Point(57, 3);
            this.drawPolygon.Name = "drawPolygon";
            this.drawPolygon.Size = new System.Drawing.Size(91, 25);
            this.drawPolygon.TabIndex = 5;
            this.drawPolygon.Text = "Draw Polygon";
            this.drawPolygon.UseVisualStyleBackColor = true;
            // 
            // undoLastPoint
            // 
            this.undoLastPoint.AutoSize = true;
            this.undoLastPoint.Location = new System.Drawing.Point(456, 3);
            this.undoLastPoint.Name = "undoLastPoint";
            this.undoLastPoint.Size = new System.Drawing.Size(101, 25);
            this.undoLastPoint.TabIndex = 4;
            this.undoLastPoint.Text = "Undo Last Point";
            this.undoLastPoint.UseVisualStyleBackColor = true;
            // 
            // cancelDrawing
            // 
            this.cancelDrawing.AutoSize = true;
            this.cancelDrawing.Location = new System.Drawing.Point(350, 3);
            this.cancelDrawing.Name = "cancelDrawing";
            this.cancelDrawing.Size = new System.Drawing.Size(100, 25);
            this.cancelDrawing.TabIndex = 3;
            this.cancelDrawing.Text = "Cancel Drawing";
            this.cancelDrawing.UseVisualStyleBackColor = true;
            // 
            // finishDrawing
            // 
            this.finishDrawing.AutoSize = true;
            this.finishDrawing.Location = new System.Drawing.Point(249, 3);
            this.finishDrawing.Name = "finishDrawing";
            this.finishDrawing.Size = new System.Drawing.Size(95, 25);
            this.finishDrawing.TabIndex = 2;
            this.finishDrawing.Text = "Finish Drawing";
            this.finishDrawing.UseVisualStyleBackColor = true;
            // 
            // drawPolyline
            // 
            this.drawPolyline.AutoSize = true;
            this.drawPolyline.Location = new System.Drawing.Point(154, 3);
            this.drawPolyline.Name = "drawPolyline";
            this.drawPolyline.Size = new System.Drawing.Size(89, 25);
            this.drawPolyline.TabIndex = 1;
            this.drawPolyline.Text = "Draw Polyline";
            this.drawPolyline.UseVisualStyleBackColor = true;
            // 
            // select
            // 
            this.select.AutoSize = true;
            this.select.Location = new System.Drawing.Point(3, 3);
            this.select.Name = "select";
            this.select.Size = new System.Drawing.Size(48, 25);
            this.select.TabIndex = 0;
            this.select.Text = "Select";
            this.select.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.allowRotating, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.allowReshaping, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.allowResizing, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.allowResegmenting, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 440);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(479, 25);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // allowRotating
            // 
            this.allowRotating.AutoSize = true;
            this.allowRotating.Checked = true;
            this.allowRotating.CheckState = System.Windows.Forms.CheckState.Checked;
            this.allowRotating.Location = new System.Drawing.Point(372, 3);
            this.allowRotating.Name = "allowRotating";
            this.allowRotating.Size = new System.Drawing.Size(104, 19);
            this.allowRotating.TabIndex = 2;
            this.allowRotating.Text = "Allow Rotating";
            this.allowRotating.UseVisualStyleBackColor = true;
            this.allowRotating.CheckedChanged += new System.EventHandler(this._ToggleRotating);
            // 
            // allowReshaping
            // 
            this.allowReshaping.AutoSize = true;
            this.allowReshaping.Checked = true;
            this.allowReshaping.CheckState = System.Windows.Forms.CheckState.Checked;
            this.allowReshaping.Location = new System.Drawing.Point(111, 3);
            this.allowReshaping.Name = "allowReshaping";
            this.allowReshaping.Size = new System.Drawing.Size(114, 19);
            this.allowReshaping.TabIndex = 1;
            this.allowReshaping.Text = "Allow Reshaping";
            this.allowReshaping.UseVisualStyleBackColor = true;
            this.allowReshaping.CheckedChanged += new System.EventHandler(this._ToggleReshaping);
            // 
            // allowResizing
            // 
            this.allowResizing.AutoSize = true;
            this.allowResizing.Checked = true;
            this.allowResizing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.allowResizing.Location = new System.Drawing.Point(3, 3);
            this.allowResizing.Name = "allowResizing";
            this.allowResizing.Size = new System.Drawing.Size(102, 19);
            this.allowResizing.TabIndex = 0;
            this.allowResizing.Text = "Allow Resizing";
            this.allowResizing.UseVisualStyleBackColor = true;
            this.allowResizing.CheckedChanged += new System.EventHandler(this._ToggleResizing);
            // 
            // allowResegmenting
            // 
            this.allowResegmenting.AutoSize = true;
            this.allowResegmenting.Checked = true;
            this.allowResegmenting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.allowResegmenting.Location = new System.Drawing.Point(231, 3);
            this.allowResegmenting.Name = "allowResegmenting";
            this.allowResegmenting.Size = new System.Drawing.Size(135, 19);
            this.allowResegmenting.TabIndex = 3;
            this.allowResegmenting.Text = "Allow Resegmenting";
            this.allowResegmenting.UseVisualStyleBackColor = true;
            this.allowResegmenting.CheckedChanged += new System.EventHandler(this._ToggleResegmenting);
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
            // goWebBrowser1
            // 
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 471);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(994, 224);
            this.goWebBrowser1.TabIndex = 3;
            this.goWebBrowser1.ZoomFactor = 1D;
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
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Button undoLastPoint;
    private System.Windows.Forms.Button cancelDrawing;
    private System.Windows.Forms.Button finishDrawing;
    private System.Windows.Forms.Button drawPolyline;
    private System.Windows.Forms.Button select;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.CheckBox allowRotating;
    private System.Windows.Forms.CheckBox allowReshaping;
    private System.Windows.Forms.CheckBox allowResizing;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private System.Windows.Forms.Button drawPolygon;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser1;
    private WinFormsSharedControls.ModelJson modelJson1;
    private System.Windows.Forms.CheckBox allowResegmenting;
  }
}
