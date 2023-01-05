/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using Northwoods.Go.Layouts;

namespace Demo.Samples.TLayout {
  partial class TLayout {
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
            this.style = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.layerStyle = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.minNodes = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.maxNodes = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.minChil = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.maxChil = new System.Windows.Forms.NumericUpDown();
            this.randomSizes = new System.Windows.Forms.CheckBox();
            this.generateBtn = new System.Windows.Forms.Button();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.angle = new System.Windows.Forms.FlowLayoutPanel();
            this.right = new System.Windows.Forms.RadioButton();
            this.down = new System.Windows.Forms.RadioButton();
            this.left = new System.Windows.Forms.RadioButton();
            this.up = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.align = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.nodeSpacing = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.nodeIndent = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.nodeIndentPastParent = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.layerSpacing = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.layerSpacingParentOverlap = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.sorting = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.compaction = new System.Windows.Forms.FlowLayoutPanel();
            this.block = new System.Windows.Forms.RadioButton();
            this.none = new System.Windows.Forms.RadioButton();
            this.label23 = new System.Windows.Forms.Label();
            this.breadthLimit = new System.Windows.Forms.NumericUpDown();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.rowSpacing = new System.Windows.Forms.NumericUpDown();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.rowIndent = new System.Windows.Forms.NumericUpDown();
            this.label28 = new System.Windows.Forms.Label();
            this.setsPortSpot = new System.Windows.Forms.CheckBox();
            this.setsChildPortSpot = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.altAngle = new System.Windows.Forms.FlowLayoutPanel();
            this.altRight = new System.Windows.Forms.RadioButton();
            this.altDown = new System.Windows.Forms.RadioButton();
            this.altLeft = new System.Windows.Forms.RadioButton();
            this.altUp = new System.Windows.Forms.RadioButton();
            this.label31 = new System.Windows.Forms.Label();
            this.altAlign = new System.Windows.Forms.ComboBox();
            this.label32 = new System.Windows.Forms.Label();
            this.altNodeSpacing = new System.Windows.Forms.NumericUpDown();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.altNodeIndent = new System.Windows.Forms.NumericUpDown();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.altNodeIndentPastParent = new System.Windows.Forms.NumericUpDown();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.altLayerSpacing = new System.Windows.Forms.NumericUpDown();
            this.label39 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.altLayerSpacingParentOverlap = new System.Windows.Forms.NumericUpDown();
            this.label41 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.altSorting = new System.Windows.Forms.ComboBox();
            this.label43 = new System.Windows.Forms.Label();
            this.altCompaction = new System.Windows.Forms.FlowLayoutPanel();
            this.altBlock = new System.Windows.Forms.RadioButton();
            this.altNone = new System.Windows.Forms.RadioButton();
            this.label44 = new System.Windows.Forms.Label();
            this.altBreadthLimit = new System.Windows.Forms.NumericUpDown();
            this.label45 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.altRowSpacing = new System.Windows.Forms.NumericUpDown();
            this.label47 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.altRowIndent = new System.Windows.Forms.NumericUpDown();
            this.label49 = new System.Windows.Forms.Label();
            this.altSetsPortSpot = new System.Windows.Forms.CheckBox();
            this.altSetsChildPortSpot = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minNodes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxNodes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minChil)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxChil)).BeginInit();
            this.flowLayoutPanel3.SuspendLayout();
            this.angle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nodeSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nodeIndent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nodeIndentPastParent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layerSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layerSpacingParentOverlap)).BeginInit();
            this.compaction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.breadthLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowIndent)).BeginInit();
            this.flowLayoutPanel4.SuspendLayout();
            this.altAngle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.altNodeSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.altNodeIndent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.altNodeIndentPastParent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.altLayerSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.altLayerSpacingParentOverlap)).BeginInit();
            this.altCompaction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.altBreadthLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.altRowSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.altRowIndent)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1501, 1649);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.BackColor = System.Drawing.Color.White;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 419);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(1495, 369);
            this.diagramControl1.TabIndex = 2;
            this.diagramControl1.Text = "diagramControl1";
            //
            // desc1
            //
            this.desc1.CreationProperties = null;
            this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.desc1.Dock = System.Windows.Forms.DockStyle.Top;
            this.desc1.Location = new System.Drawing.Point(3, 794);
            this.desc1.Name = "desc1";
            this.desc1.Size = new System.Drawing.Size(1495, 42);
            this.desc1.TabIndex = 3;
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
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1495, 410);
            this.flowLayoutPanel1.TabIndex = 4;
            //
            // flowLayoutPanel2
            //
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.Controls.Add(this.label1);
            this.flowLayoutPanel2.Controls.Add(this.style);
            this.flowLayoutPanel2.Controls.Add(this.label2);
            this.flowLayoutPanel2.Controls.Add(this.layerStyle);
            this.flowLayoutPanel2.Controls.Add(this.label3);
            this.flowLayoutPanel2.Controls.Add(this.label4);
            this.flowLayoutPanel2.Controls.Add(this.minNodes);
            this.flowLayoutPanel2.Controls.Add(this.label5);
            this.flowLayoutPanel2.Controls.Add(this.maxNodes);
            this.flowLayoutPanel2.Controls.Add(this.label6);
            this.flowLayoutPanel2.Controls.Add(this.minChil);
            this.flowLayoutPanel2.Controls.Add(this.label7);
            this.flowLayoutPanel2.Controls.Add(this.maxChil);
            this.flowLayoutPanel2.Controls.Add(this.randomSizes);
            this.flowLayoutPanel2.Controls.Add(this.generateBtn);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(221, 306);
            this.flowLayoutPanel2.TabIndex = 0;
            //
            // label1
            //
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.flowLayoutPanel2.SetFlowBreak(this.label1, true);
            this.label1.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tree Style";
            //
            // style
            //
            this.style.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.flowLayoutPanel2.SetFlowBreak(this.style, true);
            this.style.FormattingEnabled = true;
            this.style.Location = new System.Drawing.Point(3, 26);
            this.style.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.style.Name = "style";
            this.style.Size = new System.Drawing.Size(121, 25);
            this.style.TabIndex = 1;
            //
            // label2
            //
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.flowLayoutPanel2.SetFlowBreak(this.label2, true);
            this.label2.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(3, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Layer Style";
            //
            // layerStyle
            //
            this.layerStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.flowLayoutPanel2.SetFlowBreak(this.layerStyle, true);
            this.layerStyle.FormattingEnabled = true;
            this.layerStyle.Location = new System.Drawing.Point(3, 78);
            this.layerStyle.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.layerStyle.Name = "layerStyle";
            this.layerStyle.Size = new System.Drawing.Size(121, 25);
            this.layerStyle.TabIndex = 3;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.flowLayoutPanel2.SetFlowBreak(this.label3, true);
            this.label3.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(3, 115);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "New Tree";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "MinNodes:";
            //
            // minNodes
            //
            this.flowLayoutPanel2.SetFlowBreak(this.minNodes, true);
            this.minNodes.Location = new System.Drawing.Point(79, 133);
            this.minNodes.Margin = new System.Windows.Forms.Padding(1);
            this.minNodes.Name = "minNodes";
            this.minNodes.Size = new System.Drawing.Size(84, 25);
            this.minNodes.TabIndex = 6;
            this.minNodes.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "MaxNodes:";
            //
            // maxNodes
            //
            this.flowLayoutPanel2.SetFlowBreak(this.maxNodes, true);
            this.maxNodes.Location = new System.Drawing.Point(82, 160);
            this.maxNodes.Margin = new System.Windows.Forms.Padding(1);
            this.maxNodes.Name = "maxNodes";
            this.maxNodes.Size = new System.Drawing.Size(59, 25);
            this.maxNodes.TabIndex = 8;
            this.maxNodes.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 186);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "MinChildren:";
            //
            // minChil
            //
            this.flowLayoutPanel2.SetFlowBreak(this.minChil, true);
            this.minChil.Location = new System.Drawing.Point(88, 187);
            this.minChil.Margin = new System.Windows.Forms.Padding(1);
            this.minChil.Name = "minChil";
            this.minChil.Size = new System.Drawing.Size(84, 25);
            this.minChil.TabIndex = 10;
            this.minChil.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            //
            // label7
            //
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 213);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 17);
            this.label7.TabIndex = 11;
            this.label7.Text = "MaxChildren:";
            //
            // maxChil
            //
            this.flowLayoutPanel2.SetFlowBreak(this.maxChil, true);
            this.maxChil.Location = new System.Drawing.Point(91, 214);
            this.maxChil.Margin = new System.Windows.Forms.Padding(1);
            this.maxChil.Name = "maxChil";
            this.maxChil.Size = new System.Drawing.Size(84, 25);
            this.maxChil.TabIndex = 12;
            this.maxChil.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            //
            // randomSizes
            //
            this.randomSizes.AutoSize = true;
            this.flowLayoutPanel2.SetFlowBreak(this.randomSizes, true);
            this.randomSizes.Location = new System.Drawing.Point(3, 243);
            this.randomSizes.Name = "randomSizes";
            this.randomSizes.Size = new System.Drawing.Size(109, 21);
            this.randomSizes.TabIndex = 15;
            this.randomSizes.Text = "Random Sizes";
            this.randomSizes.UseVisualStyleBackColor = true;
            //
            // generateBtn
            //
            this.generateBtn.AutoSize = true;
            this.generateBtn.Location = new System.Drawing.Point(3, 276);
            this.generateBtn.Name = "generateBtn";
            this.generateBtn.Size = new System.Drawing.Size(100, 27);
            this.generateBtn.TabIndex = 13;
            this.generateBtn.Text = "Generate Tree";
            this.generateBtn.UseVisualStyleBackColor = true;
            //
            // flowLayoutPanel3
            //
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.Controls.Add(this.label8);
            this.flowLayoutPanel3.Controls.Add(this.label9);
            this.flowLayoutPanel3.Controls.Add(this.angle);
            this.flowLayoutPanel3.Controls.Add(this.label10);
            this.flowLayoutPanel3.Controls.Add(this.align);
            this.flowLayoutPanel3.Controls.Add(this.label11);
            this.flowLayoutPanel3.Controls.Add(this.nodeSpacing);
            this.flowLayoutPanel3.Controls.Add(this.label12);
            this.flowLayoutPanel3.Controls.Add(this.label13);
            this.flowLayoutPanel3.Controls.Add(this.nodeIndent);
            this.flowLayoutPanel3.Controls.Add(this.label14);
            this.flowLayoutPanel3.Controls.Add(this.label15);
            this.flowLayoutPanel3.Controls.Add(this.nodeIndentPastParent);
            this.flowLayoutPanel3.Controls.Add(this.label16);
            this.flowLayoutPanel3.Controls.Add(this.label17);
            this.flowLayoutPanel3.Controls.Add(this.layerSpacing);
            this.flowLayoutPanel3.Controls.Add(this.label18);
            this.flowLayoutPanel3.Controls.Add(this.label19);
            this.flowLayoutPanel3.Controls.Add(this.layerSpacingParentOverlap);
            this.flowLayoutPanel3.Controls.Add(this.label20);
            this.flowLayoutPanel3.Controls.Add(this.label21);
            this.flowLayoutPanel3.Controls.Add(this.sorting);
            this.flowLayoutPanel3.Controls.Add(this.label22);
            this.flowLayoutPanel3.Controls.Add(this.compaction);
            this.flowLayoutPanel3.Controls.Add(this.label23);
            this.flowLayoutPanel3.Controls.Add(this.breadthLimit);
            this.flowLayoutPanel3.Controls.Add(this.label24);
            this.flowLayoutPanel3.Controls.Add(this.label25);
            this.flowLayoutPanel3.Controls.Add(this.rowSpacing);
            this.flowLayoutPanel3.Controls.Add(this.label26);
            this.flowLayoutPanel3.Controls.Add(this.label27);
            this.flowLayoutPanel3.Controls.Add(this.rowIndent);
            this.flowLayoutPanel3.Controls.Add(this.label28);
            this.flowLayoutPanel3.Controls.Add(this.setsPortSpot);
            this.flowLayoutPanel3.Controls.Add(this.setsChildPortSpot);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(230, 3);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(393, 404);
            this.flowLayoutPanel3.TabIndex = 1;
            //
            // label8
            //
            this.label8.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label8, true);
            this.label8.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(121, 17);
            this.label8.TabIndex = 0;
            this.label8.Text = "Default Properties";
            //
            // label9
            //
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 17);
            this.label9.TabIndex = 1;
            this.label9.Text = "Angle:";
            //
            // angle
            //
            this.angle.AutoSize = true;
            this.angle.Controls.Add(this.right);
            this.angle.Controls.Add(this.down);
            this.angle.Controls.Add(this.left);
            this.angle.Controls.Add(this.up);
            this.flowLayoutPanel3.SetFlowBreak(this.angle, true);
            this.angle.Location = new System.Drawing.Point(53, 20);
            this.angle.Name = "angle";
            this.angle.Size = new System.Drawing.Size(229, 27);
            this.angle.TabIndex = 2;
            //
            // right
            //
            this.right.AutoSize = true;
            this.right.Checked = true;
            this.right.Location = new System.Drawing.Point(3, 3);
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(56, 21);
            this.right.TabIndex = 0;
            this.right.TabStop = true;
            this.right.Text = "Right";
            this.right.UseVisualStyleBackColor = true;
            //
            // down
            //
            this.down.AutoSize = true;
            this.down.Location = new System.Drawing.Point(65, 3);
            this.down.Name = "down";
            this.down.Size = new System.Drawing.Size(59, 21);
            this.down.TabIndex = 1;
            this.down.Text = "Down";
            this.down.UseVisualStyleBackColor = true;
            //
            // left
            //
            this.left.AutoSize = true;
            this.left.Location = new System.Drawing.Point(130, 3);
            this.left.Name = "left";
            this.left.Size = new System.Drawing.Size(47, 21);
            this.left.TabIndex = 2;
            this.left.Text = "Left";
            this.left.UseVisualStyleBackColor = true;
            //
            // up
            //
            this.up.AutoSize = true;
            this.up.Location = new System.Drawing.Point(183, 3);
            this.up.Name = "up";
            this.up.Size = new System.Drawing.Size(43, 21);
            this.up.TabIndex = 3;
            this.up.Text = "Up";
            this.up.UseVisualStyleBackColor = true;
            //
            // label10
            //
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 57);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 17);
            this.label10.TabIndex = 3;
            this.label10.Text = "Alignment:";
            //
            // align
            //
            this.align.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.flowLayoutPanel3.SetFlowBreak(this.align, true);
            this.align.FormattingEnabled = true;
            this.align.Location = new System.Drawing.Point(78, 53);
            this.align.Name = "align";
            this.align.Size = new System.Drawing.Size(121, 25);
            this.align.TabIndex = 4;
            //
            // label11
            //
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 87);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(90, 17);
            this.label11.TabIndex = 5;
            this.label11.Text = "NodeSpacing:";
            //
            // nodeSpacing
            //
            this.nodeSpacing.Location = new System.Drawing.Point(98, 83);
            this.nodeSpacing.Margin = new System.Windows.Forms.Padding(2);
            this.nodeSpacing.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nodeSpacing.Name = "nodeSpacing";
            this.nodeSpacing.Size = new System.Drawing.Size(120, 25);
            this.nodeSpacing.TabIndex = 6;
            this.nodeSpacing.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            //
            // label12
            //
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label12, true);
            this.label12.Location = new System.Drawing.Point(223, 87);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(162, 17);
            this.label12.TabIndex = 7;
            this.label12.Text = "(negative causes overlaps)";
            //
            // label13
            //
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 116);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 17);
            this.label13.TabIndex = 8;
            this.label13.Text = "NodeIndent:";
            //
            // nodeIndent
            //
            this.nodeIndent.Location = new System.Drawing.Point(88, 112);
            this.nodeIndent.Margin = new System.Windows.Forms.Padding(2);
            this.nodeIndent.Name = "nodeIndent";
            this.nodeIndent.Size = new System.Drawing.Size(120, 25);
            this.nodeIndent.TabIndex = 9;
            //
            // label14
            //
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label14.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label14, true);
            this.label14.Location = new System.Drawing.Point(213, 116);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(156, 17);
            this.label14.TabIndex = 10;
            this.label14.Text = "(when Start or End; >= 0)";
            //
            // label15
            //
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 145);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(141, 17);
            this.label15.TabIndex = 11;
            this.label15.Text = "NodeIndentPastParent:";
            //
            // nodeIndentPastParent
            //
            this.nodeIndentPastParent.DecimalPlaces = 2;
            this.nodeIndentPastParent.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nodeIndentPastParent.Location = new System.Drawing.Point(149, 141);
            this.nodeIndentPastParent.Margin = new System.Windows.Forms.Padding(2);
            this.nodeIndentPastParent.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nodeIndentPastParent.Name = "nodeIndentPastParent";
            this.nodeIndentPastParent.Size = new System.Drawing.Size(120, 25);
            this.nodeIndentPastParent.TabIndex = 12;
            //
            // label16
            //
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label16.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label16, true);
            this.label16.Location = new System.Drawing.Point(274, 145);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(86, 17);
            this.label16.TabIndex = 13;
            this.label16.Text = "(fraction; 0-1)";
            //
            // label17
            //
            this.label17.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 174);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(88, 17);
            this.label17.TabIndex = 14;
            this.label17.Text = "LayerSpacing:";
            //
            // layerSpacing
            //
            this.layerSpacing.Location = new System.Drawing.Point(96, 170);
            this.layerSpacing.Margin = new System.Windows.Forms.Padding(2);
            this.layerSpacing.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.layerSpacing.Name = "layerSpacing";
            this.layerSpacing.Size = new System.Drawing.Size(120, 25);
            this.layerSpacing.TabIndex = 15;
            this.layerSpacing.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            //
            // label18
            //
            this.label18.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label18.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label18, true);
            this.label18.Location = new System.Drawing.Point(221, 174);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(162, 17);
            this.label18.TabIndex = 16;
            this.label18.Text = "(negative causes overlaps)";
            //
            // label19
            //
            this.label19.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(3, 203);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(171, 17);
            this.label19.TabIndex = 17;
            this.label19.Text = "LayerSpacingParentOverlap:";
            //
            // layerSpacingParentOverlap
            //
            this.layerSpacingParentOverlap.DecimalPlaces = 2;
            this.layerSpacingParentOverlap.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.layerSpacingParentOverlap.Location = new System.Drawing.Point(179, 199);
            this.layerSpacingParentOverlap.Margin = new System.Windows.Forms.Padding(2);
            this.layerSpacingParentOverlap.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.layerSpacingParentOverlap.Name = "layerSpacingParentOverlap";
            this.layerSpacingParentOverlap.Size = new System.Drawing.Size(120, 25);
            this.layerSpacingParentOverlap.TabIndex = 18;
            //
            // label20
            //
            this.label20.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label20.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label20, true);
            this.label20.Location = new System.Drawing.Point(304, 203);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(86, 17);
            this.label20.TabIndex = 19;
            this.label20.Text = "(fraction; 0-1)";
            //
            // label21
            //
            this.label21.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(3, 233);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(53, 17);
            this.label21.TabIndex = 20;
            this.label21.Text = "Sorting:";
            //
            // sorting
            //
            this.sorting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.flowLayoutPanel3.SetFlowBreak(this.sorting, true);
            this.sorting.FormattingEnabled = true;
            this.sorting.Location = new System.Drawing.Point(62, 229);
            this.sorting.Name = "sorting";
            this.sorting.Size = new System.Drawing.Size(121, 25);
            this.sorting.TabIndex = 21;
            //
            // label22
            //
            this.label22.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(3, 265);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(81, 17);
            this.label22.TabIndex = 22;
            this.label22.Text = "Compaction:";
            //
            // compaction
            //
            this.compaction.AutoSize = true;
            this.compaction.Controls.Add(this.block);
            this.compaction.Controls.Add(this.none);
            this.flowLayoutPanel3.SetFlowBreak(this.compaction, true);
            this.compaction.Location = new System.Drawing.Point(90, 260);
            this.compaction.Name = "compaction";
            this.compaction.Size = new System.Drawing.Size(126, 27);
            this.compaction.TabIndex = 23;
            //
            // block
            //
            this.block.AutoSize = true;
            this.block.Checked = true;
            this.block.Location = new System.Drawing.Point(3, 3);
            this.block.Name = "block";
            this.block.Size = new System.Drawing.Size(56, 21);
            this.block.TabIndex = 0;
            this.block.TabStop = true;
            this.block.Text = "Block";
            this.block.UseVisualStyleBackColor = true;
            //
            // none
            //
            this.none.AutoSize = true;
            this.none.Location = new System.Drawing.Point(65, 3);
            this.none.Name = "none";
            this.none.Size = new System.Drawing.Size(58, 21);
            this.none.TabIndex = 1;
            this.none.Text = "None";
            this.none.UseVisualStyleBackColor = true;
            //
            // label23
            //
            this.label23.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(3, 296);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(83, 17);
            this.label23.TabIndex = 24;
            this.label23.Text = "BreadthLimit:";
            //
            // breadthLimit
            //
            this.breadthLimit.Location = new System.Drawing.Point(91, 292);
            this.breadthLimit.Margin = new System.Windows.Forms.Padding(2);
            this.breadthLimit.Name = "breadthLimit";
            this.breadthLimit.Size = new System.Drawing.Size(120, 25);
            this.breadthLimit.TabIndex = 25;
            //
            // label24
            //
            this.label24.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label24.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label24, true);
            this.label24.Location = new System.Drawing.Point(216, 296);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(112, 17);
            this.label24.TabIndex = 26;
            this.label24.Text = "(0 means no limit)";
            //
            // label25
            //
            this.label25.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(3, 325);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(82, 17);
            this.label25.TabIndex = 27;
            this.label25.Text = "RowSpacing:";
            //
            // rowSpacing
            //
            this.rowSpacing.Location = new System.Drawing.Point(90, 321);
            this.rowSpacing.Margin = new System.Windows.Forms.Padding(2);
            this.rowSpacing.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.rowSpacing.Name = "rowSpacing";
            this.rowSpacing.Size = new System.Drawing.Size(120, 25);
            this.rowSpacing.TabIndex = 28;
            this.rowSpacing.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            //
            // label26
            //
            this.label26.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label26.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label26, true);
            this.label26.Location = new System.Drawing.Point(215, 325);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(162, 17);
            this.label26.TabIndex = 29;
            this.label26.Text = "(negative causes overlaps)";
            //
            // label27
            //
            this.label27.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(3, 354);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(72, 17);
            this.label27.TabIndex = 30;
            this.label27.Text = "RowIndent:";
            //
            // rowIndent
            //
            this.rowIndent.Location = new System.Drawing.Point(80, 350);
            this.rowIndent.Margin = new System.Windows.Forms.Padding(2);
            this.rowIndent.Name = "rowIndent";
            this.rowIndent.Size = new System.Drawing.Size(120, 25);
            this.rowIndent.TabIndex = 31;
            this.rowIndent.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            //
            // label28
            //
            this.label28.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label28.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label28, true);
            this.label28.Location = new System.Drawing.Point(205, 354);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(45, 17);
            this.label28.TabIndex = 32;
            this.label28.Text = "(>= 0)";
            //
            // setsPortSpot
            //
            this.setsPortSpot.AutoSize = true;
            this.setsPortSpot.Checked = true;
            this.setsPortSpot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.setsPortSpot.Location = new System.Drawing.Point(3, 380);
            this.setsPortSpot.Name = "setsPortSpot";
            this.setsPortSpot.Size = new System.Drawing.Size(102, 21);
            this.setsPortSpot.TabIndex = 33;
            this.setsPortSpot.Text = "SetsPortSpot";
            this.setsPortSpot.UseVisualStyleBackColor = true;
            //
            // setsChildPortSpot
            //
            this.setsChildPortSpot.AutoSize = true;
            this.setsChildPortSpot.Checked = true;
            this.setsChildPortSpot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.setsChildPortSpot.Location = new System.Drawing.Point(111, 380);
            this.setsChildPortSpot.Name = "setsChildPortSpot";
            this.setsChildPortSpot.Size = new System.Drawing.Size(131, 21);
            this.setsChildPortSpot.TabIndex = 34;
            this.setsChildPortSpot.Text = "SetsChildPortSpot";
            this.setsChildPortSpot.UseVisualStyleBackColor = true;
            //
            // flowLayoutPanel4
            //
            this.flowLayoutPanel4.AutoSize = true;
            this.flowLayoutPanel4.Controls.Add(this.label29);
            this.flowLayoutPanel4.Controls.Add(this.label30);
            this.flowLayoutPanel4.Controls.Add(this.altAngle);
            this.flowLayoutPanel4.Controls.Add(this.label31);
            this.flowLayoutPanel4.Controls.Add(this.altAlign);
            this.flowLayoutPanel4.Controls.Add(this.label32);
            this.flowLayoutPanel4.Controls.Add(this.altNodeSpacing);
            this.flowLayoutPanel4.Controls.Add(this.label33);
            this.flowLayoutPanel4.Controls.Add(this.label34);
            this.flowLayoutPanel4.Controls.Add(this.altNodeIndent);
            this.flowLayoutPanel4.Controls.Add(this.label35);
            this.flowLayoutPanel4.Controls.Add(this.label36);
            this.flowLayoutPanel4.Controls.Add(this.altNodeIndentPastParent);
            this.flowLayoutPanel4.Controls.Add(this.label37);
            this.flowLayoutPanel4.Controls.Add(this.label38);
            this.flowLayoutPanel4.Controls.Add(this.altLayerSpacing);
            this.flowLayoutPanel4.Controls.Add(this.label39);
            this.flowLayoutPanel4.Controls.Add(this.label40);
            this.flowLayoutPanel4.Controls.Add(this.altLayerSpacingParentOverlap);
            this.flowLayoutPanel4.Controls.Add(this.label41);
            this.flowLayoutPanel4.Controls.Add(this.label42);
            this.flowLayoutPanel4.Controls.Add(this.altSorting);
            this.flowLayoutPanel4.Controls.Add(this.label43);
            this.flowLayoutPanel4.Controls.Add(this.altCompaction);
            this.flowLayoutPanel4.Controls.Add(this.label44);
            this.flowLayoutPanel4.Controls.Add(this.altBreadthLimit);
            this.flowLayoutPanel4.Controls.Add(this.label45);
            this.flowLayoutPanel4.Controls.Add(this.label46);
            this.flowLayoutPanel4.Controls.Add(this.altRowSpacing);
            this.flowLayoutPanel4.Controls.Add(this.label47);
            this.flowLayoutPanel4.Controls.Add(this.label48);
            this.flowLayoutPanel4.Controls.Add(this.altRowIndent);
            this.flowLayoutPanel4.Controls.Add(this.label49);
            this.flowLayoutPanel4.Controls.Add(this.altSetsPortSpot);
            this.flowLayoutPanel4.Controls.Add(this.altSetsChildPortSpot);
            this.flowLayoutPanel4.Location = new System.Drawing.Point(629, 3);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(393, 404);
            this.flowLayoutPanel4.TabIndex = 2;
            //
            // label29
            //
            this.label29.AutoSize = true;
            this.flowLayoutPanel4.SetFlowBreak(this.label29, true);
            this.label29.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label29.Location = new System.Drawing.Point(3, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(71, 17);
            this.label29.TabIndex = 0;
            this.label29.Text = "Alternates";
            //
            // label30
            //
            this.label30.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(3, 25);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(44, 17);
            this.label30.TabIndex = 1;
            this.label30.Text = "Angle:";
            //
            // altAngle
            //
            this.altAngle.AutoSize = true;
            this.altAngle.Controls.Add(this.altRight);
            this.altAngle.Controls.Add(this.altDown);
            this.altAngle.Controls.Add(this.altLeft);
            this.altAngle.Controls.Add(this.altUp);
            this.flowLayoutPanel4.SetFlowBreak(this.altAngle, true);
            this.altAngle.Location = new System.Drawing.Point(53, 20);
            this.altAngle.Name = "altAngle";
            this.altAngle.Size = new System.Drawing.Size(229, 27);
            this.altAngle.TabIndex = 2;
            //
            // altRight
            //
            this.altRight.AutoSize = true;
            this.altRight.Checked = true;
            this.altRight.Location = new System.Drawing.Point(3, 3);
            this.altRight.Name = "altRight";
            this.altRight.Size = new System.Drawing.Size(56, 21);
            this.altRight.TabIndex = 0;
            this.altRight.TabStop = true;
            this.altRight.Text = "Right";
            this.altRight.UseVisualStyleBackColor = true;
            //
            // altDown
            //
            this.altDown.AutoSize = true;
            this.altDown.Location = new System.Drawing.Point(65, 3);
            this.altDown.Name = "altDown";
            this.altDown.Size = new System.Drawing.Size(59, 21);
            this.altDown.TabIndex = 1;
            this.altDown.Text = "Down";
            this.altDown.UseVisualStyleBackColor = true;
            //
            // altLeft
            //
            this.altLeft.AutoSize = true;
            this.altLeft.Location = new System.Drawing.Point(130, 3);
            this.altLeft.Name = "altLeft";
            this.altLeft.Size = new System.Drawing.Size(47, 21);
            this.altLeft.TabIndex = 2;
            this.altLeft.Text = "Left";
            this.altLeft.UseVisualStyleBackColor = true;
            //
            // altUp
            //
            this.altUp.AutoSize = true;
            this.altUp.Location = new System.Drawing.Point(183, 3);
            this.altUp.Name = "altUp";
            this.altUp.Size = new System.Drawing.Size(43, 21);
            this.altUp.TabIndex = 3;
            this.altUp.Text = "Up";
            this.altUp.UseVisualStyleBackColor = true;
            //
            // label31
            //
            this.label31.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(3, 57);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(69, 17);
            this.label31.TabIndex = 3;
            this.label31.Text = "Alignment:";
            //
            // altAlign
            //
            this.altAlign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.flowLayoutPanel4.SetFlowBreak(this.altAlign, true);
            this.altAlign.FormattingEnabled = true;
            this.altAlign.Location = new System.Drawing.Point(78, 53);
            this.altAlign.Name = "altAlign";
            this.altAlign.Size = new System.Drawing.Size(121, 25);
            this.altAlign.TabIndex = 4;
            //
            // label32
            //
            this.label32.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(3, 87);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(90, 17);
            this.label32.TabIndex = 5;
            this.label32.Text = "NodeSpacing:";
            //
            // altNodeSpacing
            //
            this.altNodeSpacing.Location = new System.Drawing.Point(98, 83);
            this.altNodeSpacing.Margin = new System.Windows.Forms.Padding(2);
            this.altNodeSpacing.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.altNodeSpacing.Name = "altNodeSpacing";
            this.altNodeSpacing.Size = new System.Drawing.Size(120, 25);
            this.altNodeSpacing.TabIndex = 6;
            this.altNodeSpacing.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            //
            // label33
            //
            this.label33.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label33.AutoSize = true;
            this.flowLayoutPanel4.SetFlowBreak(this.label33, true);
            this.label33.Location = new System.Drawing.Point(223, 87);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(162, 17);
            this.label33.TabIndex = 7;
            this.label33.Text = "(negative causes overlaps)";
            //
            // label34
            //
            this.label34.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(3, 116);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(80, 17);
            this.label34.TabIndex = 8;
            this.label34.Text = "NodeIndent:";
            //
            // altNodeIndent
            //
            this.altNodeIndent.Location = new System.Drawing.Point(88, 112);
            this.altNodeIndent.Margin = new System.Windows.Forms.Padding(2);
            this.altNodeIndent.Name = "altNodeIndent";
            this.altNodeIndent.Size = new System.Drawing.Size(120, 25);
            this.altNodeIndent.TabIndex = 9;
            //
            // label35
            //
            this.label35.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label35.AutoSize = true;
            this.flowLayoutPanel4.SetFlowBreak(this.label35, true);
            this.label35.Location = new System.Drawing.Point(213, 116);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(156, 17);
            this.label35.TabIndex = 10;
            this.label35.Text = "(when Start or End; >= 0)";
            //
            // label36
            //
            this.label36.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(3, 145);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(141, 17);
            this.label36.TabIndex = 11;
            this.label36.Text = "NodeIndentPastParent:";
            //
            // altNodeIndentPastParent
            //
            this.altNodeIndentPastParent.DecimalPlaces = 2;
            this.altNodeIndentPastParent.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.altNodeIndentPastParent.Location = new System.Drawing.Point(149, 141);
            this.altNodeIndentPastParent.Margin = new System.Windows.Forms.Padding(2);
            this.altNodeIndentPastParent.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.altNodeIndentPastParent.Name = "altNodeIndentPastParent";
            this.altNodeIndentPastParent.Size = new System.Drawing.Size(120, 25);
            this.altNodeIndentPastParent.TabIndex = 12;
            //
            // label37
            //
            this.label37.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label37.AutoSize = true;
            this.flowLayoutPanel4.SetFlowBreak(this.label37, true);
            this.label37.Location = new System.Drawing.Point(274, 145);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(86, 17);
            this.label37.TabIndex = 13;
            this.label37.Text = "(fraction; 0-1)";
            //
            // label38
            //
            this.label38.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(3, 174);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(88, 17);
            this.label38.TabIndex = 14;
            this.label38.Text = "LayerSpacing:";
            //
            // altLayerSpacing
            //
            this.altLayerSpacing.Location = new System.Drawing.Point(96, 170);
            this.altLayerSpacing.Margin = new System.Windows.Forms.Padding(2);
            this.altLayerSpacing.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.altLayerSpacing.Name = "altLayerSpacing";
            this.altLayerSpacing.Size = new System.Drawing.Size(120, 25);
            this.altLayerSpacing.TabIndex = 15;
            this.altLayerSpacing.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            //
            // label39
            //
            this.label39.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label39.AutoSize = true;
            this.flowLayoutPanel4.SetFlowBreak(this.label39, true);
            this.label39.Location = new System.Drawing.Point(221, 174);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(162, 17);
            this.label39.TabIndex = 16;
            this.label39.Text = "(negative causes overlaps)";
            //
            // label40
            //
            this.label40.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(3, 203);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(171, 17);
            this.label40.TabIndex = 17;
            this.label40.Text = "LayerSpacingParentOverlap:";
            //
            // altLayerSpacingParentOverlap
            //
            this.altLayerSpacingParentOverlap.DecimalPlaces = 2;
            this.altLayerSpacingParentOverlap.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.altLayerSpacingParentOverlap.Location = new System.Drawing.Point(179, 199);
            this.altLayerSpacingParentOverlap.Margin = new System.Windows.Forms.Padding(2);
            this.altLayerSpacingParentOverlap.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.altLayerSpacingParentOverlap.Name = "altLayerSpacingParentOverlap";
            this.altLayerSpacingParentOverlap.Size = new System.Drawing.Size(120, 25);
            this.altLayerSpacingParentOverlap.TabIndex = 18;
            //
            // label41
            //
            this.label41.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label41.AutoSize = true;
            this.flowLayoutPanel4.SetFlowBreak(this.label41, true);
            this.label41.Location = new System.Drawing.Point(304, 203);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(86, 17);
            this.label41.TabIndex = 19;
            this.label41.Text = "(fraction; 0-1)";
            //
            // label42
            //
            this.label42.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(3, 233);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(53, 17);
            this.label42.TabIndex = 20;
            this.label42.Text = "Sorting:";
            //
            // altSorting
            //
            this.altSorting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.flowLayoutPanel4.SetFlowBreak(this.altSorting, true);
            this.altSorting.FormattingEnabled = true;
            this.altSorting.Location = new System.Drawing.Point(62, 229);
            this.altSorting.Name = "altSorting";
            this.altSorting.Size = new System.Drawing.Size(121, 25);
            this.altSorting.TabIndex = 21;
            //
            // label43
            //
            this.label43.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(3, 265);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(81, 17);
            this.label43.TabIndex = 22;
            this.label43.Text = "Compaction:";
            //
            // altCompaction
            //
            this.altCompaction.AutoSize = true;
            this.altCompaction.Controls.Add(this.altBlock);
            this.altCompaction.Controls.Add(this.altNone);
            this.flowLayoutPanel4.SetFlowBreak(this.altCompaction, true);
            this.altCompaction.Location = new System.Drawing.Point(90, 260);
            this.altCompaction.Name = "altCompaction";
            this.altCompaction.Size = new System.Drawing.Size(126, 27);
            this.altCompaction.TabIndex = 23;
            //
            // altBlock
            //
            this.altBlock.AutoSize = true;
            this.altBlock.Checked = true;
            this.altBlock.Location = new System.Drawing.Point(3, 3);
            this.altBlock.Name = "altBlock";
            this.altBlock.Size = new System.Drawing.Size(56, 21);
            this.altBlock.TabIndex = 0;
            this.altBlock.TabStop = true;
            this.altBlock.Text = "Block";
            this.altBlock.UseVisualStyleBackColor = true;
            //
            // altNone
            //
            this.altNone.AutoSize = true;
            this.altNone.Location = new System.Drawing.Point(65, 3);
            this.altNone.Name = "altNone";
            this.altNone.Size = new System.Drawing.Size(58, 21);
            this.altNone.TabIndex = 1;
            this.altNone.Text = "None";
            this.altNone.UseVisualStyleBackColor = true;
            //
            // label44
            //
            this.label44.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(3, 296);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(83, 17);
            this.label44.TabIndex = 24;
            this.label44.Text = "BreadthLimit:";
            //
            // altBreadthLimit
            //
            this.altBreadthLimit.Location = new System.Drawing.Point(91, 292);
            this.altBreadthLimit.Margin = new System.Windows.Forms.Padding(2);
            this.altBreadthLimit.Name = "altBreadthLimit";
            this.altBreadthLimit.Size = new System.Drawing.Size(120, 25);
            this.altBreadthLimit.TabIndex = 25;
            //
            // label45
            //
            this.label45.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label45.AutoSize = true;
            this.flowLayoutPanel4.SetFlowBreak(this.label45, true);
            this.label45.Location = new System.Drawing.Point(216, 296);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(112, 17);
            this.label45.TabIndex = 26;
            this.label45.Text = "(0 means no limit)";
            //
            // label46
            //
            this.label46.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(3, 325);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(82, 17);
            this.label46.TabIndex = 27;
            this.label46.Text = "RowSpacing:";
            //
            // altRowSpacing
            //
            this.altRowSpacing.Location = new System.Drawing.Point(90, 321);
            this.altRowSpacing.Margin = new System.Windows.Forms.Padding(2);
            this.altRowSpacing.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.altRowSpacing.Name = "altRowSpacing";
            this.altRowSpacing.Size = new System.Drawing.Size(120, 25);
            this.altRowSpacing.TabIndex = 28;
            this.altRowSpacing.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            //
            // label47
            //
            this.label47.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label47.AutoSize = true;
            this.flowLayoutPanel4.SetFlowBreak(this.label47, true);
            this.label47.Location = new System.Drawing.Point(215, 325);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(162, 17);
            this.label47.TabIndex = 29;
            this.label47.Text = "(negative causes overlaps)";
            //
            // label48
            //
            this.label48.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(3, 354);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(72, 17);
            this.label48.TabIndex = 30;
            this.label48.Text = "RowIndent:";
            //
            // altRowIndent
            //
            this.altRowIndent.Location = new System.Drawing.Point(80, 350);
            this.altRowIndent.Margin = new System.Windows.Forms.Padding(2);
            this.altRowIndent.Name = "altRowIndent";
            this.altRowIndent.Size = new System.Drawing.Size(120, 25);
            this.altRowIndent.TabIndex = 31;
            this.altRowIndent.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            //
            // label49
            //
            this.label49.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label49.AutoSize = true;
            this.flowLayoutPanel4.SetFlowBreak(this.label49, true);
            this.label49.Location = new System.Drawing.Point(205, 354);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(45, 17);
            this.label49.TabIndex = 32;
            this.label49.Text = "(>= 0)";
            //
            // altSetsPortSpot
            //
            this.altSetsPortSpot.AutoSize = true;
            this.altSetsPortSpot.Checked = true;
            this.altSetsPortSpot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.altSetsPortSpot.Location = new System.Drawing.Point(3, 380);
            this.altSetsPortSpot.Name = "altSetsPortSpot";
            this.altSetsPortSpot.Size = new System.Drawing.Size(102, 21);
            this.altSetsPortSpot.TabIndex = 33;
            this.altSetsPortSpot.Text = "SetsPortSpot";
            this.altSetsPortSpot.UseVisualStyleBackColor = true;
            //
            // altSetsChildPortSpot
            //
            this.altSetsChildPortSpot.AutoSize = true;
            this.altSetsChildPortSpot.Checked = true;
            this.altSetsChildPortSpot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.altSetsChildPortSpot.Location = new System.Drawing.Point(111, 380);
            this.altSetsChildPortSpot.Name = "altSetsChildPortSpot";
            this.altSetsChildPortSpot.Size = new System.Drawing.Size(131, 21);
            this.altSetsChildPortSpot.TabIndex = 34;
            this.altSetsChildPortSpot.Text = "SetsChildPortSpot";
            this.altSetsChildPortSpot.UseVisualStyleBackColor = true;
            //
            // TLayoutControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TLayoutControl";
            this.Size = new System.Drawing.Size(1501, 1649);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minNodes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxNodes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minChil)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxChil)).EndInit();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.angle.ResumeLayout(false);
            this.angle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nodeSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nodeIndent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nodeIndentPastParent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layerSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layerSpacingParentOverlap)).EndInit();
            this.compaction.ResumeLayout(false);
            this.compaction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.breadthLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowIndent)).EndInit();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            this.altAngle.ResumeLayout(false);
            this.altAngle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.altNodeSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.altNodeIndent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.altNodeIndentPastParent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.altLayerSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.altLayerSpacingParentOverlap)).EndInit();
            this.altCompaction.ResumeLayout(false);
            this.altCompaction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.altBreadthLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.altRowSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.altRowIndent)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsDemoApp.GoWebBrowser desc1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox style;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox layerStyle;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.NumericUpDown minNodes;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.NumericUpDown maxNodes;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.NumericUpDown minChil;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.NumericUpDown maxChil;
    private System.Windows.Forms.CheckBox randomSizes;
    private System.Windows.Forms.Button generateBtn;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.FlowLayoutPanel angle;
    private System.Windows.Forms.RadioButton right;
    private System.Windows.Forms.RadioButton down;
    private System.Windows.Forms.RadioButton left;
    private System.Windows.Forms.RadioButton up;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.ComboBox align;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.NumericUpDown nodeSpacing;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.NumericUpDown nodeIndent;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.NumericUpDown nodeIndentPastParent;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.NumericUpDown layerSpacing;
    private System.Windows.Forms.Label label18;
    private System.Windows.Forms.Label label19;
    private System.Windows.Forms.NumericUpDown layerSpacingParentOverlap;
    private System.Windows.Forms.Label label20;
    private System.Windows.Forms.Label label21;
    private System.Windows.Forms.ComboBox sorting;
    private System.Windows.Forms.Label label22;
    private System.Windows.Forms.FlowLayoutPanel compaction;
    private System.Windows.Forms.RadioButton block;
    private System.Windows.Forms.RadioButton none;
    private System.Windows.Forms.Label label23;
    private System.Windows.Forms.NumericUpDown breadthLimit;
    private System.Windows.Forms.Label label24;
    private System.Windows.Forms.Label label25;
    private System.Windows.Forms.NumericUpDown rowSpacing;
    private System.Windows.Forms.Label label26;
    private System.Windows.Forms.Label label27;
    private System.Windows.Forms.NumericUpDown rowIndent;
    private System.Windows.Forms.Label label28;
    private System.Windows.Forms.CheckBox setsPortSpot;
    private System.Windows.Forms.CheckBox setsChildPortSpot;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
    private System.Windows.Forms.Label label29;
    private System.Windows.Forms.Label label30;
    private System.Windows.Forms.FlowLayoutPanel altAngle;
    private System.Windows.Forms.RadioButton altRight;
    private System.Windows.Forms.RadioButton altDown;
    private System.Windows.Forms.RadioButton altLeft;
    private System.Windows.Forms.RadioButton altUp;
    private System.Windows.Forms.Label label31;
    private System.Windows.Forms.ComboBox altAlign;
    private System.Windows.Forms.Label label32;
    private System.Windows.Forms.NumericUpDown altNodeSpacing;
    private System.Windows.Forms.Label label33;
    private System.Windows.Forms.Label label34;
    private System.Windows.Forms.NumericUpDown altNodeIndent;
    private System.Windows.Forms.Label label35;
    private System.Windows.Forms.Label label36;
    private System.Windows.Forms.NumericUpDown altNodeIndentPastParent;
    private System.Windows.Forms.Label label37;
    private System.Windows.Forms.Label label38;
    private System.Windows.Forms.NumericUpDown altLayerSpacing;
    private System.Windows.Forms.Label label39;
    private System.Windows.Forms.Label label40;
    private System.Windows.Forms.NumericUpDown altLayerSpacingParentOverlap;
    private System.Windows.Forms.Label label41;
    private System.Windows.Forms.Label label42;
    private System.Windows.Forms.ComboBox altSorting;
    private System.Windows.Forms.Label label43;
    private System.Windows.Forms.FlowLayoutPanel altCompaction;
    private System.Windows.Forms.RadioButton altBlock;
    private System.Windows.Forms.RadioButton altNone;
    private System.Windows.Forms.Label label44;
    private System.Windows.Forms.NumericUpDown altBreadthLimit;
    private System.Windows.Forms.Label label45;
    private System.Windows.Forms.Label label46;
    private System.Windows.Forms.NumericUpDown altRowSpacing;
    private System.Windows.Forms.Label label47;
    private System.Windows.Forms.Label label48;
    private System.Windows.Forms.NumericUpDown altRowIndent;
    private System.Windows.Forms.Label label49;
    private System.Windows.Forms.CheckBox altSetsPortSpot;
    private System.Windows.Forms.CheckBox altSetsChildPortSpot;
  }
}
