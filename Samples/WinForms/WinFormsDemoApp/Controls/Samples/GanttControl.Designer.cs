/* Copyright 1998-2023 by Northwoods Software Corporation. */

namespace Demo.Samples.Gantt {
  partial class Gantt {
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
            this.tasksControl = new Northwoods.Go.WinForms.DiagramControl();
            this.ganttControl = new Northwoods.Go.WinForms.DiagramControl();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.widthSlider = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.desc1 = new WinFormsDemoApp.GoWebBrowser();
            this.modelJson1 = new WinFormsDemoApp.ModelJson();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.widthSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tasksControl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ganttControl, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.desc1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.modelJson1, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 500));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 690);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // tasksControl
            //
            this.tasksControl.AllowDrop = true;
            this.tasksControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tasksControl.Location = new System.Drawing.Point(3, 3);
            this.tasksControl.Name = "tasksControl";
            this.tasksControl.Size = new System.Drawing.Size(150, 494);
            this.tasksControl.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tasksControl.TabIndex = 0;
            this.tasksControl.Text = "tasksControl";
            //
            // ganttControl
            //
            this.ganttControl.AllowDrop = true;
            this.ganttControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.ganttControl.Location = new System.Drawing.Point(3, 3);
            this.ganttControl.Name = "ganttControl";
            this.ganttControl.Size = new System.Drawing.Size(994, 494);
            this.ganttControl.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.ganttControl.TabIndex = 1;
            this.ganttControl.Text = "ganttControl";
            //
            // flowLayoutPanel1
            //
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.widthSlider);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 503);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 34);
            this.flowLayoutPanel1.TabIndex = 2;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Spacing:";
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            //
            // widthSlider
            //
            this.widthSlider.Location = new System.Drawing.Point(103, 3);
            this.widthSlider.Maximum = 24;
            this.widthSlider.Minimum = 8;
            this.widthSlider.Name = "widthSlider";
            this.widthSlider.Size = new System.Drawing.Size(94, 28);
            this.widthSlider.TabIndex = 1;
            this.widthSlider.Value = 12;
            //
            // desc1
            //
            this.tableLayoutPanel1.SetColumnSpan(this.desc1, 2);
            this.desc1.CreationProperties = null;
            this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.desc1.Dock = System.Windows.Forms.DockStyle.Top;
            this.desc1.Location = new System.Drawing.Point(3, 543);
            this.desc1.Name = "desc1";
            this.desc1.Size = new System.Drawing.Size(994, 100);
            this.desc1.TabIndex = 3;
            this.desc1.ZoomFactor = 1D;
            //
            // modelJson1
            //
            this.tableLayoutPanel1.SetColumnSpan(this.modelJson1, 2);
            this.modelJson1.AutoSize = true;
            this.modelJson1.Dock = System.Windows.Forms.DockStyle.Top;
            this.modelJson1.Location = new System.Drawing.Point(3, 885);
            this.modelJson1.Name = "modelJson1";
            this.modelJson1.Size = new System.Drawing.Size(1087, 343);
            this.modelJson1.TabIndex = 4;
            this.modelJson1.CanSaveLoad = false;
            //
            // GanttControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "GanttControl";
            this.Size = new System.Drawing.Size(1000, 690);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.widthSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl tasksControl;
    private Northwoods.Go.WinForms.DiagramControl ganttControl;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.TrackBar widthSlider;
    private System.Windows.Forms.Label label1;
    private WinFormsDemoApp.GoWebBrowser desc1;
    private WinFormsDemoApp.ModelJson modelJson1;
  }
}
