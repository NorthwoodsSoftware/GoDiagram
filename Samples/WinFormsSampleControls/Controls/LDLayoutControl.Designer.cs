/* Copyright 1998-2022 by Northwoods Software Corporation. */

namespace WinFormsSampleControls.LDLayout {
  partial class LDLayoutControl {
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.minNodesTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.maxNodesTB = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.direction = new System.Windows.Forms.FlowLayoutPanel();
            this.rightRB = new System.Windows.Forms.RadioButton();
            this.downRB = new System.Windows.Forms.RadioButton();
            this.leftRB = new System.Windows.Forms.RadioButton();
            this.upRB = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.layerSpacingTB = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.columnSpacingTB = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cycleRemove = new System.Windows.Forms.FlowLayoutPanel();
            this.depthFirstRB = new System.Windows.Forms.RadioButton();
            this.greedyRB = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.layering = new System.Windows.Forms.FlowLayoutPanel();
            this.optimalLinkLengthRB = new System.Windows.Forms.RadioButton();
            this.longestPathSourceRB = new System.Windows.Forms.RadioButton();
            this.longestPathSinkRB = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.initialize = new System.Windows.Forms.FlowLayoutPanel();
            this.depthFirstOutRB = new System.Windows.Forms.RadioButton();
            this.depthFirstInRB = new System.Windows.Forms.RadioButton();
            this.naiveRB = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.aggressive = new System.Windows.Forms.FlowLayoutPanel();
            this.noneRB = new System.Windows.Forms.RadioButton();
            this.lessRB = new System.Windows.Forms.RadioButton();
            this.moreRB = new System.Windows.Forms.RadioButton();
            this.label12 = new System.Windows.Forms.Label();
            this.medianCB = new System.Windows.Forms.CheckBox();
            this.straightenCB = new System.Windows.Forms.CheckBox();
            this.expandCB = new System.Windows.Forms.CheckBox();
            this.setsPortSpotsCB = new System.Windows.Forms.CheckBox();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.goWebBrowser1 = new WinFormsSharedControls.GoWebBrowser();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.direction.SuspendLayout();
            this.cycleRemove.SuspendLayout();
            this.layering.SuspendLayout();
            this.initialize.SuspendLayout();
            this.aggressive.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 923);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // flowLayoutPanel1
            //
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.AliceBlue;
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel2);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel3);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(994, 304);
            this.flowLayoutPanel1.TabIndex = 4;
            //
            // flowLayoutPanel2
            //
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.Controls.Add(this.label1);
            this.flowLayoutPanel2.Controls.Add(this.label2);
            this.flowLayoutPanel2.Controls.Add(this.minNodesTB);
            this.flowLayoutPanel2.Controls.Add(this.label3);
            this.flowLayoutPanel2.Controls.Add(this.maxNodesTB);
            this.flowLayoutPanel2.Controls.Add(this.button1);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(191, 112);
            this.flowLayoutPanel2.TabIndex = 0;
            //
            // label1
            //
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.flowLayoutPanel2.SetFlowBreak(this.label1, true);
            this.label1.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "New Graph";
            //
            // label2
            //
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Min Nodes:";
            //
            // minNodesTB
            //
            this.flowLayoutPanel2.SetFlowBreak(this.minNodesTB, true);
            this.minNodesTB.Location = new System.Drawing.Point(85, 20);
            this.minNodesTB.Name = "minNodesTB";
            this.minNodesTB.Size = new System.Drawing.Size(100, 25);
            this.minNodesTB.TabIndex = 2;
            this.minNodesTB.Text = "20";
            //
            // label3
            //
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Max Nodes:";
            //
            // maxNodesTB
            //
            this.flowLayoutPanel2.SetFlowBreak(this.maxNodesTB, true);
            this.maxNodesTB.Location = new System.Drawing.Point(88, 51);
            this.maxNodesTB.Name = "maxNodesTB";
            this.maxNodesTB.Size = new System.Drawing.Size(100, 25);
            this.maxNodesTB.TabIndex = 4;
            this.maxNodesTB.Text = "100";
            //
            // button1
            //
            this.button1.AutoSize = true;
            this.button1.Location = new System.Drawing.Point(3, 82);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 27);
            this.button1.TabIndex = 5;
            this.button1.Text = "Generate Digraph";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            //
            // flowLayoutPanel3
            //
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.Controls.Add(this.label4);
            this.flowLayoutPanel3.Controls.Add(this.label5);
            this.flowLayoutPanel3.Controls.Add(this.direction);
            this.flowLayoutPanel3.Controls.Add(this.label6);
            this.flowLayoutPanel3.Controls.Add(this.layerSpacingTB);
            this.flowLayoutPanel3.Controls.Add(this.label7);
            this.flowLayoutPanel3.Controls.Add(this.columnSpacingTB);
            this.flowLayoutPanel3.Controls.Add(this.label8);
            this.flowLayoutPanel3.Controls.Add(this.cycleRemove);
            this.flowLayoutPanel3.Controls.Add(this.label9);
            this.flowLayoutPanel3.Controls.Add(this.layering);
            this.flowLayoutPanel3.Controls.Add(this.label10);
            this.flowLayoutPanel3.Controls.Add(this.initialize);
            this.flowLayoutPanel3.Controls.Add(this.label11);
            this.flowLayoutPanel3.Controls.Add(this.aggressive);
            this.flowLayoutPanel3.Controls.Add(this.label12);
            this.flowLayoutPanel3.Controls.Add(this.medianCB);
            this.flowLayoutPanel3.Controls.Add(this.straightenCB);
            this.flowLayoutPanel3.Controls.Add(this.expandCB);
            this.flowLayoutPanel3.Controls.Add(this.setsPortSpotsCB);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(200, 3);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(480, 298);
            this.flowLayoutPanel3.TabIndex = 1;
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label4, true);
            this.label4.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(215, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "LayeredDigraphLayout Properties";
            //
            // label5
            //
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 17);
            this.label5.TabIndex = 1;
            this.label5.Text = "Direction:";
            //
            // direction
            //
            this.direction.AutoSize = true;
            this.direction.Controls.Add(this.rightRB);
            this.direction.Controls.Add(this.downRB);
            this.direction.Controls.Add(this.leftRB);
            this.direction.Controls.Add(this.upRB);
            this.flowLayoutPanel3.SetFlowBreak(this.direction, true);
            this.direction.Location = new System.Drawing.Point(72, 20);
            this.direction.Name = "direction";
            this.direction.Size = new System.Drawing.Size(340, 27);
            this.direction.TabIndex = 2;
            //
            // rightRB
            //
            this.rightRB.AutoSize = true;
            this.rightRB.Checked = true;
            this.rightRB.Location = new System.Drawing.Point(3, 3);
            this.rightRB.Name = "rightRB";
            this.rightRB.Size = new System.Drawing.Size(75, 21);
            this.rightRB.TabIndex = 0;
            this.rightRB.TabStop = true;
            this.rightRB.Text = "Right (0)";
            this.rightRB.UseVisualStyleBackColor = true;
            this.rightRB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // downRB
            //
            this.downRB.AutoSize = true;
            this.downRB.Location = new System.Drawing.Point(84, 3);
            this.downRB.Name = "downRB";
            this.downRB.Size = new System.Drawing.Size(85, 21);
            this.downRB.TabIndex = 1;
            this.downRB.Text = "Down (90)";
            this.downRB.UseVisualStyleBackColor = true;
            this.downRB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // leftRB
            //
            this.leftRB.AutoSize = true;
            this.leftRB.Location = new System.Drawing.Point(175, 3);
            this.leftRB.Name = "leftRB";
            this.leftRB.Size = new System.Drawing.Size(80, 21);
            this.leftRB.TabIndex = 2;
            this.leftRB.Text = "Left (180)";
            this.leftRB.UseVisualStyleBackColor = true;
            this.leftRB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // upRB
            //
            this.upRB.AutoSize = true;
            this.upRB.Location = new System.Drawing.Point(261, 3);
            this.upRB.Name = "upRB";
            this.upRB.Size = new System.Drawing.Size(76, 21);
            this.upRB.TabIndex = 3;
            this.upRB.Text = "Up (270)";
            this.upRB.UseVisualStyleBackColor = true;
            this.upRB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // label6
            //
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "LayerSpacing:";
            //
            // layerSpacingTB
            //
            this.flowLayoutPanel3.SetFlowBreak(this.layerSpacingTB, true);
            this.layerSpacingTB.Location = new System.Drawing.Point(97, 53);
            this.layerSpacingTB.Name = "layerSpacingTB";
            this.layerSpacingTB.Size = new System.Drawing.Size(100, 25);
            this.layerSpacingTB.TabIndex = 4;
            this.layerSpacingTB.Text = "25";
            this.layerSpacingTB.Leave += new System.EventHandler(this._PropertyChanged);
            //
            // label7
            //
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 17);
            this.label7.TabIndex = 5;
            this.label7.Text = "ColumnSpacing:";
            //
            // columnSpacingTB
            //
            this.flowLayoutPanel3.SetFlowBreak(this.columnSpacingTB, true);
            this.columnSpacingTB.Location = new System.Drawing.Point(110, 84);
            this.columnSpacingTB.Name = "columnSpacingTB";
            this.columnSpacingTB.Size = new System.Drawing.Size(100, 25);
            this.columnSpacingTB.TabIndex = 6;
            this.columnSpacingTB.Text = "25";
            this.columnSpacingTB.Leave += new System.EventHandler(this._PropertyChanged);
            //
            // label8
            //
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 120);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 17);
            this.label8.TabIndex = 7;
            this.label8.Text = "CycleRemove:";
            //
            // cycleRemove
            //
            this.cycleRemove.AutoSize = true;
            this.cycleRemove.Controls.Add(this.depthFirstRB);
            this.cycleRemove.Controls.Add(this.greedyRB);
            this.flowLayoutPanel3.SetFlowBreak(this.cycleRemove, true);
            this.cycleRemove.Location = new System.Drawing.Point(97, 115);
            this.cycleRemove.Name = "cycleRemove";
            this.cycleRemove.Size = new System.Drawing.Size(165, 27);
            this.cycleRemove.TabIndex = 8;
            //
            // depthFirstRB
            //
            this.depthFirstRB.AutoSize = true;
            this.depthFirstRB.Checked = true;
            this.depthFirstRB.Location = new System.Drawing.Point(3, 3);
            this.depthFirstRB.Name = "depthFirstRB";
            this.depthFirstRB.Size = new System.Drawing.Size(85, 21);
            this.depthFirstRB.TabIndex = 0;
            this.depthFirstRB.TabStop = true;
            this.depthFirstRB.Text = "DepthFirst";
            this.depthFirstRB.UseVisualStyleBackColor = true;
            this.depthFirstRB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // greedyRB
            //
            this.greedyRB.AutoSize = true;
            this.greedyRB.Location = new System.Drawing.Point(94, 3);
            this.greedyRB.Name = "greedyRB";
            this.greedyRB.Size = new System.Drawing.Size(68, 21);
            this.greedyRB.TabIndex = 1;
            this.greedyRB.Text = "Greedy";
            this.greedyRB.UseVisualStyleBackColor = true;
            this.greedyRB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // label9
            //
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 153);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 17);
            this.label9.TabIndex = 9;
            this.label9.Text = "Layering:";
            //
            // layering
            //
            this.layering.AutoSize = true;
            this.layering.Controls.Add(this.optimalLinkLengthRB);
            this.layering.Controls.Add(this.longestPathSourceRB);
            this.layering.Controls.Add(this.longestPathSinkRB);
            this.flowLayoutPanel3.SetFlowBreak(this.layering, true);
            this.layering.Location = new System.Drawing.Point(69, 148);
            this.layering.Name = "layering";
            this.layering.Size = new System.Drawing.Size(408, 27);
            this.layering.TabIndex = 10;
            //
            // optimalLinkLengthRB
            //
            this.optimalLinkLengthRB.AutoSize = true;
            this.optimalLinkLengthRB.Checked = true;
            this.optimalLinkLengthRB.Location = new System.Drawing.Point(3, 3);
            this.optimalLinkLengthRB.Name = "optimalLinkLengthRB";
            this.optimalLinkLengthRB.Size = new System.Drawing.Size(133, 21);
            this.optimalLinkLengthRB.TabIndex = 0;
            this.optimalLinkLengthRB.TabStop = true;
            this.optimalLinkLengthRB.Text = "OptimalLinkLength";
            this.optimalLinkLengthRB.UseVisualStyleBackColor = true;
            this.optimalLinkLengthRB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // longestPathSourceRB
            //
            this.longestPathSourceRB.AutoSize = true;
            this.longestPathSourceRB.Location = new System.Drawing.Point(142, 3);
            this.longestPathSourceRB.Name = "longestPathSourceRB";
            this.longestPathSourceRB.Size = new System.Drawing.Size(137, 21);
            this.longestPathSourceRB.TabIndex = 1;
            this.longestPathSourceRB.Text = "LongestPathSource";
            this.longestPathSourceRB.UseVisualStyleBackColor = true;
            this.longestPathSourceRB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // longestPathSinkRB
            //
            this.longestPathSinkRB.AutoSize = true;
            this.longestPathSinkRB.Location = new System.Drawing.Point(285, 3);
            this.longestPathSinkRB.Name = "longestPathSinkRB";
            this.longestPathSinkRB.Size = new System.Drawing.Size(120, 21);
            this.longestPathSinkRB.TabIndex = 2;
            this.longestPathSinkRB.Text = "LongestPathSink";
            this.longestPathSinkRB.UseVisualStyleBackColor = true;
            this.longestPathSinkRB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // label10
            //
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 186);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 17);
            this.label10.TabIndex = 11;
            this.label10.Text = "Initialize:";
            //
            // initialize
            //
            this.initialize.AutoSize = true;
            this.initialize.Controls.Add(this.depthFirstOutRB);
            this.initialize.Controls.Add(this.depthFirstInRB);
            this.initialize.Controls.Add(this.naiveRB);
            this.flowLayoutPanel3.SetFlowBreak(this.initialize, true);
            this.initialize.Location = new System.Drawing.Point(66, 181);
            this.initialize.Name = "initialize";
            this.initialize.Size = new System.Drawing.Size(278, 27);
            this.initialize.TabIndex = 12;
            //
            // depthFirstOutRB
            //
            this.depthFirstOutRB.AutoSize = true;
            this.depthFirstOutRB.Checked = true;
            this.depthFirstOutRB.Location = new System.Drawing.Point(3, 3);
            this.depthFirstOutRB.Name = "depthFirstOutRB";
            this.depthFirstOutRB.Size = new System.Drawing.Size(106, 21);
            this.depthFirstOutRB.TabIndex = 0;
            this.depthFirstOutRB.TabStop = true;
            this.depthFirstOutRB.Text = "DepthFirstOut";
            this.depthFirstOutRB.UseVisualStyleBackColor = true;
            this.depthFirstOutRB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // depthFirstInRB
            //
            this.depthFirstInRB.AutoSize = true;
            this.depthFirstInRB.Location = new System.Drawing.Point(115, 3);
            this.depthFirstInRB.Name = "depthFirstInRB";
            this.depthFirstInRB.Size = new System.Drawing.Size(95, 21);
            this.depthFirstInRB.TabIndex = 1;
            this.depthFirstInRB.Text = "DepthFirstIn";
            this.depthFirstInRB.UseVisualStyleBackColor = true;
            this.depthFirstInRB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // naiveRB
            //
            this.naiveRB.AutoSize = true;
            this.naiveRB.Location = new System.Drawing.Point(216, 3);
            this.naiveRB.Name = "naiveRB";
            this.naiveRB.Size = new System.Drawing.Size(59, 21);
            this.naiveRB.TabIndex = 2;
            this.naiveRB.Text = "Naive";
            this.naiveRB.UseVisualStyleBackColor = true;
            this.naiveRB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // label11
            //
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 219);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 17);
            this.label11.TabIndex = 13;
            this.label11.Text = "Aggressive:";
            //
            // aggressive
            //
            this.aggressive.AutoSize = true;
            this.aggressive.Controls.Add(this.noneRB);
            this.aggressive.Controls.Add(this.lessRB);
            this.aggressive.Controls.Add(this.moreRB);
            this.flowLayoutPanel3.SetFlowBreak(this.aggressive, true);
            this.aggressive.Location = new System.Drawing.Point(84, 214);
            this.aggressive.Name = "aggressive";
            this.aggressive.Size = new System.Drawing.Size(185, 27);
            this.aggressive.TabIndex = 14;
            //
            // noneRB
            //
            this.noneRB.AutoSize = true;
            this.noneRB.Location = new System.Drawing.Point(3, 3);
            this.noneRB.Name = "noneRB";
            this.noneRB.Size = new System.Drawing.Size(58, 21);
            this.noneRB.TabIndex = 0;
            this.noneRB.Text = "None";
            this.noneRB.UseVisualStyleBackColor = true;
            this.noneRB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // lessRB
            //
            this.lessRB.AutoSize = true;
            this.lessRB.Checked = true;
            this.lessRB.Location = new System.Drawing.Point(67, 3);
            this.lessRB.Name = "lessRB";
            this.lessRB.Size = new System.Drawing.Size(51, 21);
            this.lessRB.TabIndex = 1;
            this.lessRB.TabStop = true;
            this.lessRB.Text = "Less";
            this.lessRB.UseVisualStyleBackColor = true;
            this.lessRB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // moreRB
            //
            this.moreRB.AutoSize = true;
            this.moreRB.Location = new System.Drawing.Point(124, 3);
            this.moreRB.Name = "moreRB";
            this.moreRB.Size = new System.Drawing.Size(58, 21);
            this.moreRB.TabIndex = 2;
            this.moreRB.Text = "More";
            this.moreRB.UseVisualStyleBackColor = true;
            this.moreRB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // label12
            //
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 249);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 17);
            this.label12.TabIndex = 15;
            this.label12.Text = "Pack:";
            //
            // medianCB
            //
            this.medianCB.AutoSize = true;
            this.medianCB.Checked = true;
            this.medianCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.medianCB.Location = new System.Drawing.Point(46, 247);
            this.medianCB.Name = "medianCB";
            this.medianCB.Size = new System.Drawing.Size(71, 21);
            this.medianCB.TabIndex = 16;
            this.medianCB.Text = "Median";
            this.medianCB.UseVisualStyleBackColor = true;
            this.medianCB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // straightenCB
            //
            this.straightenCB.AutoSize = true;
            this.straightenCB.Checked = true;
            this.straightenCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.straightenCB.Location = new System.Drawing.Point(123, 247);
            this.straightenCB.Name = "straightenCB";
            this.straightenCB.Size = new System.Drawing.Size(86, 21);
            this.straightenCB.TabIndex = 17;
            this.straightenCB.Text = "Straighten";
            this.straightenCB.UseVisualStyleBackColor = true;
            this.straightenCB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // expandCB
            //
            this.expandCB.AutoSize = true;
            this.expandCB.Checked = true;
            this.expandCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.flowLayoutPanel3.SetFlowBreak(this.expandCB, true);
            this.expandCB.Location = new System.Drawing.Point(215, 247);
            this.expandCB.Name = "expandCB";
            this.expandCB.Size = new System.Drawing.Size(70, 21);
            this.expandCB.TabIndex = 18;
            this.expandCB.Text = "Expand";
            this.expandCB.UseVisualStyleBackColor = true;
            this.expandCB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // setsPortSpotsCB
            //
            this.setsPortSpotsCB.AutoSize = true;
            this.setsPortSpotsCB.Checked = true;
            this.setsPortSpotsCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.setsPortSpotsCB.Location = new System.Drawing.Point(3, 274);
            this.setsPortSpotsCB.Name = "setsPortSpotsCB";
            this.setsPortSpotsCB.Size = new System.Drawing.Size(108, 21);
            this.setsPortSpotsCB.TabIndex = 19;
            this.setsPortSpotsCB.Text = "SetsPortSpots";
            this.setsPortSpotsCB.UseVisualStyleBackColor = true;
            this.setsPortSpotsCB.CheckedChanged += new System.EventHandler(this._PropertyChanged);
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.BackColor = System.Drawing.Color.White;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(4, 313);
            this.diagramControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(992, 494);
            this.diagramControl1.TabIndex = 2;
            //
            // goWebBrowser1
            //
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 813);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(994, 60);
            this.goWebBrowser1.TabIndex = 3;
            this.goWebBrowser1.ZoomFactor = 1D;
            //
            // LayeredDigraphControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LayeredDigraphControl";
            this.Size = new System.Drawing.Size(1000, 923);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.direction.ResumeLayout(false);
            this.direction.PerformLayout();
            this.cycleRemove.ResumeLayout(false);
            this.cycleRemove.PerformLayout();
            this.layering.ResumeLayout(false);
            this.layering.PerformLayout();
            this.initialize.ResumeLayout(false);
            this.initialize.PerformLayout();
            this.aggressive.ResumeLayout(false);
            this.aggressive.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox minNodesTB;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox maxNodesTB;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.FlowLayoutPanel direction;
    private System.Windows.Forms.RadioButton rightRB;
    private System.Windows.Forms.RadioButton downRB;
    private System.Windows.Forms.RadioButton leftRB;
    private System.Windows.Forms.RadioButton upRB;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox layerSpacingTB;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox columnSpacingTB;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.FlowLayoutPanel cycleRemove;
    private System.Windows.Forms.RadioButton depthFirstRB;
    private System.Windows.Forms.RadioButton greedyRB;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.FlowLayoutPanel layering;
    private System.Windows.Forms.RadioButton optimalLinkLengthRB;
    private System.Windows.Forms.RadioButton longestPathSourceRB;
    private System.Windows.Forms.RadioButton longestPathSinkRB;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.FlowLayoutPanel initialize;
    private System.Windows.Forms.RadioButton depthFirstOutRB;
    private System.Windows.Forms.RadioButton depthFirstInRB;
    private System.Windows.Forms.RadioButton naiveRB;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.FlowLayoutPanel aggressive;
    private System.Windows.Forms.RadioButton noneRB;
    private System.Windows.Forms.RadioButton lessRB;
    private System.Windows.Forms.RadioButton moreRB;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.CheckBox medianCB;
    private System.Windows.Forms.CheckBox straightenCB;
    private System.Windows.Forms.CheckBox expandCB;
    private System.Windows.Forms.CheckBox setsPortSpotsCB;
  }
}
