/* Copyright 1998-2022 by Northwoods Software Corporation. */

namespace Demo.Extensions.Packed {
  partial class Packed {
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
            this.desc1 = new WinFormsDemoApp.GoWebBrowser();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.packShape = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.packMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.aspectRatio = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.width = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.height = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.spacing = new System.Windows.Forms.NumericUpDown();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.sortOrder = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.sortMode = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.hasCircularNodes = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.numNodes = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.nodeShape = new System.Windows.Forms.FlowLayoutPanel();
            this.rectangleRb = new System.Windows.Forms.RadioButton();
            this.ellipseRb = new System.Windows.Forms.RadioButton();
            this.label15 = new System.Windows.Forms.Label();
            this.nodeMinSide = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.nodeMaxSide = new System.Windows.Forms.NumericUpDown();
            this.sameSides = new System.Windows.Forms.CheckBox();
            this.randomizeBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spacing)).BeginInit();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNodes)).BeginInit();
            this.nodeShape.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nodeMinSide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nodeMaxSide)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.desc1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1234, 1632);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // diagramControl1
            // 
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.BackColor = System.Drawing.Color.White;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 224);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(1228, 560);
            this.diagramControl1.TabIndex = 3;
            this.diagramControl1.Text = "diagramControl1";
            // 
            // desc1
            // 
            this.desc1.CreationProperties = null;
            this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.desc1.Dock = System.Windows.Forms.DockStyle.Top;
            this.desc1.Location = new System.Drawing.Point(3, 790);
            this.desc1.Name = "desc1";
            this.desc1.Size = new System.Drawing.Size(1228, 100);
            this.desc1.TabIndex = 4;
            this.desc1.ZoomFactor = 1D;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.AliceBlue;
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel2);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel3);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel4);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1228, 215);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.Controls.Add(this.label1);
            this.flowLayoutPanel2.Controls.Add(this.label2);
            this.flowLayoutPanel2.Controls.Add(this.packShape);
            this.flowLayoutPanel2.Controls.Add(this.label3);
            this.flowLayoutPanel2.Controls.Add(this.packMode);
            this.flowLayoutPanel2.Controls.Add(this.label4);
            this.flowLayoutPanel2.Controls.Add(this.aspectRatio);
            this.flowLayoutPanel2.Controls.Add(this.label5);
            this.flowLayoutPanel2.Controls.Add(this.width);
            this.flowLayoutPanel2.Controls.Add(this.label6);
            this.flowLayoutPanel2.Controls.Add(this.height);
            this.flowLayoutPanel2.Controls.Add(this.label7);
            this.flowLayoutPanel2.Controls.Add(this.spacing);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(207, 203);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.flowLayoutPanel2.SetFlowBreak(this.label1, true);
            this.label1.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "General Properties";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "PackShape:";
            // 
            // packShape
            // 
            this.packShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.flowLayoutPanel2.SetFlowBreak(this.packShape, true);
            this.packShape.FormattingEnabled = true;
            this.packShape.Location = new System.Drawing.Point(82, 20);
            this.packShape.Name = "packShape";
            this.packShape.Size = new System.Drawing.Size(121, 25);
            this.packShape.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "PackMode:";
            // 
            // packMode
            // 
            this.packMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.flowLayoutPanel2.SetFlowBreak(this.packMode, true);
            this.packMode.FormattingEnabled = true;
            this.packMode.Location = new System.Drawing.Point(81, 51);
            this.packMode.Name = "packMode";
            this.packMode.Size = new System.Drawing.Size(121, 25);
            this.packMode.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Aspect ratio:";
            // 
            // aspectRatio
            // 
            this.aspectRatio.DecimalPlaces = 2;
            this.flowLayoutPanel2.SetFlowBreak(this.aspectRatio, true);
            this.aspectRatio.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.aspectRatio.Location = new System.Drawing.Point(90, 82);
            this.aspectRatio.Name = "aspectRatio";
            this.aspectRatio.Size = new System.Drawing.Size(84, 25);
            this.aspectRatio.TabIndex = 6;
            this.aspectRatio.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Layout width:";
            // 
            // width
            // 
            this.flowLayoutPanel2.SetFlowBreak(this.width, true);
            this.width.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.width.Location = new System.Drawing.Point(93, 113);
            this.width.Maximum = new decimal(new int[] {
            1200,
            0,
            0,
            0});
            this.width.Name = "width";
            this.width.Size = new System.Drawing.Size(84, 25);
            this.width.TabIndex = 8;
            this.width.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 148);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "Layout height:";
            // 
            // height
            // 
            this.flowLayoutPanel2.SetFlowBreak(this.height, true);
            this.height.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.height.Location = new System.Drawing.Point(98, 144);
            this.height.Maximum = new decimal(new int[] {
            1200,
            0,
            0,
            0});
            this.height.Name = "height";
            this.height.Size = new System.Drawing.Size(84, 25);
            this.height.TabIndex = 10;
            this.height.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 179);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 17);
            this.label7.TabIndex = 11;
            this.label7.Text = "Spacing:";
            // 
            // spacing
            // 
            this.spacing.Location = new System.Drawing.Point(66, 175);
            this.spacing.Name = "spacing";
            this.spacing.Size = new System.Drawing.Size(84, 25);
            this.spacing.TabIndex = 12;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.Controls.Add(this.label8);
            this.flowLayoutPanel3.Controls.Add(this.label9);
            this.flowLayoutPanel3.Controls.Add(this.sortOrder);
            this.flowLayoutPanel3.Controls.Add(this.label10);
            this.flowLayoutPanel3.Controls.Add(this.sortMode);
            this.flowLayoutPanel3.Controls.Add(this.label11);
            this.flowLayoutPanel3.Controls.Add(this.hasCircularNodes);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(216, 3);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(239, 133);
            this.flowLayoutPanel3.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label8, true);
            this.label8.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(157, 17);
            this.label8.TabIndex = 0;
            this.label8.Text = "Node Sorting Properties";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 17);
            this.label9.TabIndex = 1;
            this.label9.Text = "SortOrder:";
            // 
            // sortOrder
            // 
            this.sortOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.flowLayoutPanel3.SetFlowBreak(this.sortOrder, true);
            this.sortOrder.FormattingEnabled = true;
            this.sortOrder.Location = new System.Drawing.Point(79, 20);
            this.sortOrder.Name = "sortOrder";
            this.sortOrder.Size = new System.Drawing.Size(121, 25);
            this.sortOrder.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 17);
            this.label10.TabIndex = 3;
            this.label10.Text = "SortMode:";
            // 
            // sortMode
            // 
            this.sortMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.flowLayoutPanel3.SetFlowBreak(this.sortMode, true);
            this.sortMode.FormattingEnabled = true;
            this.sortMode.Location = new System.Drawing.Point(79, 51);
            this.sortMode.Name = "sortMode";
            this.sortMode.Size = new System.Drawing.Size(121, 25);
            this.sortMode.TabIndex = 4;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label11, true);
            this.label11.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label11.Location = new System.Drawing.Point(3, 79);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 17);
            this.label11.TabIndex = 5;
            this.label11.Text = "Circle Packing";
            // 
            // hasCircularNodes
            // 
            this.hasCircularNodes.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.hasCircularNodes, true);
            this.hasCircularNodes.Location = new System.Drawing.Point(3, 109);
            this.hasCircularNodes.Name = "hasCircularNodes";
            this.hasCircularNodes.Size = new System.Drawing.Size(132, 21);
            this.hasCircularNodes.TabIndex = 6;
            this.hasCircularNodes.Text = "HasCircularNodes";
            this.hasCircularNodes.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.AutoSize = true;
            this.flowLayoutPanel4.Controls.Add(this.label12);
            this.flowLayoutPanel4.Controls.Add(this.label13);
            this.flowLayoutPanel4.Controls.Add(this.numNodes);
            this.flowLayoutPanel4.Controls.Add(this.label14);
            this.flowLayoutPanel4.Controls.Add(this.nodeShape);
            this.flowLayoutPanel4.Controls.Add(this.label15);
            this.flowLayoutPanel4.Controls.Add(this.nodeMinSide);
            this.flowLayoutPanel4.Controls.Add(this.label16);
            this.flowLayoutPanel4.Controls.Add(this.nodeMaxSide);
            this.flowLayoutPanel4.Controls.Add(this.sameSides);
            this.flowLayoutPanel4.Controls.Add(this.randomizeBtn);
            this.flowLayoutPanel4.Location = new System.Drawing.Point(461, 3);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(270, 209);
            this.flowLayoutPanel4.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.flowLayoutPanel4.SetFlowBreak(this.label12, true);
            this.label12.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label12.Location = new System.Drawing.Point(3, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(113, 17);
            this.label12.TabIndex = 0;
            this.label12.Text = "Node Generation";
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 24);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(75, 17);
            this.label13.TabIndex = 1;
            this.label13.Text = "# of nodes:";
            // 
            // numNodes
            // 
            this.flowLayoutPanel4.SetFlowBreak(this.numNodes, true);
            this.numNodes.Location = new System.Drawing.Point(84, 20);
            this.numNodes.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numNodes.Name = "numNodes";
            this.numNodes.Size = new System.Drawing.Size(84, 25);
            this.numNodes.TabIndex = 2;
            this.numNodes.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 56);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(83, 17);
            this.label14.TabIndex = 3;
            this.label14.Text = "Node shape:";
            // 
            // nodeShape
            // 
            this.nodeShape.AutoSize = true;
            this.nodeShape.Controls.Add(this.rectangleRb);
            this.nodeShape.Controls.Add(this.ellipseRb);
            this.flowLayoutPanel4.SetFlowBreak(this.nodeShape, true);
            this.nodeShape.Location = new System.Drawing.Point(92, 51);
            this.nodeShape.Name = "nodeShape";
            this.nodeShape.Size = new System.Drawing.Size(158, 27);
            this.nodeShape.TabIndex = 4;
            // 
            // rectangleRb
            // 
            this.rectangleRb.AutoSize = true;
            this.rectangleRb.Checked = true;
            this.rectangleRb.Location = new System.Drawing.Point(3, 3);
            this.rectangleRb.Name = "rectangleRb";
            this.rectangleRb.Size = new System.Drawing.Size(83, 21);
            this.rectangleRb.TabIndex = 0;
            this.rectangleRb.TabStop = true;
            this.rectangleRb.Text = "Rectangle";
            this.rectangleRb.UseVisualStyleBackColor = true;
            // 
            // ellipseRb
            // 
            this.ellipseRb.AutoSize = true;
            this.ellipseRb.Location = new System.Drawing.Point(92, 3);
            this.ellipseRb.Name = "ellipseRb";
            this.ellipseRb.Size = new System.Drawing.Size(63, 21);
            this.ellipseRb.TabIndex = 1;
            this.ellipseRb.TabStop = true;
            this.ellipseRb.Text = "Ellipse";
            this.ellipseRb.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 88);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(101, 17);
            this.label15.TabIndex = 5;
            this.label15.Text = "Min side length:";
            // 
            // nodeMinSide
            // 
            this.flowLayoutPanel4.SetFlowBreak(this.nodeMinSide, true);
            this.nodeMinSide.Location = new System.Drawing.Point(110, 84);
            this.nodeMinSide.Name = "nodeMinSide";
            this.nodeMinSide.Size = new System.Drawing.Size(84, 25);
            this.nodeMinSide.TabIndex = 6;
            this.nodeMinSide.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(3, 119);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(104, 17);
            this.label16.TabIndex = 7;
            this.label16.Text = "Max side length:";
            // 
            // nodeMaxSide
            // 
            this.flowLayoutPanel4.SetFlowBreak(this.nodeMaxSide, true);
            this.nodeMaxSide.Location = new System.Drawing.Point(113, 115);
            this.nodeMaxSide.Name = "nodeMaxSide";
            this.nodeMaxSide.Size = new System.Drawing.Size(84, 25);
            this.nodeMaxSide.TabIndex = 8;
            this.nodeMaxSide.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // sameSides
            // 
            this.sameSides.AutoSize = true;
            this.flowLayoutPanel4.SetFlowBreak(this.sameSides, true);
            this.sameSides.Location = new System.Drawing.Point(3, 146);
            this.sameSides.Name = "sameSides";
            this.sameSides.Size = new System.Drawing.Size(135, 21);
            this.sameSides.TabIndex = 9;
            this.sameSides.Text = "Same width/height";
            this.sameSides.UseVisualStyleBackColor = true;
            // 
            // randomizeBtn
            // 
            this.randomizeBtn.AutoSize = true;
            this.randomizeBtn.Location = new System.Drawing.Point(3, 179);
            this.randomizeBtn.Name = "randomizeBtn";
            this.randomizeBtn.Size = new System.Drawing.Size(123, 27);
            this.randomizeBtn.TabIndex = 10;
            this.randomizeBtn.Text = "Randomize Graph";
            this.randomizeBtn.UseVisualStyleBackColor = true;
            // 
            // PackedControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PackedControl";
            this.Size = new System.Drawing.Size(1234, 1632);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spacing)).EndInit();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNodes)).EndInit();
            this.nodeShape.ResumeLayout(false);
            this.nodeShape.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nodeMinSide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nodeMaxSide)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsDemoApp.GoWebBrowser desc1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox packShape;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox packMode;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.NumericUpDown aspectRatio;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.NumericUpDown width;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.NumericUpDown height;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.ComboBox sortOrder;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.ComboBox sortMode;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.NumericUpDown spacing;
    private System.Windows.Forms.CheckBox hasCircularNodes;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.NumericUpDown numNodes;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.FlowLayoutPanel nodeShape;
    private System.Windows.Forms.RadioButton rectangleRb;
    private System.Windows.Forms.RadioButton ellipseRb;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.NumericUpDown nodeMinSide;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.NumericUpDown nodeMaxSide;
    private System.Windows.Forms.CheckBox sameSides;
    private System.Windows.Forms.Button randomizeBtn;
  }
}
