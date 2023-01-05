/* Copyright 1998-2023 by Northwoods Software Corporation. */

namespace Demo.Samples.LDLayout {
  partial class LDLayout {
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
            this.minNodes = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.maxNodes = new System.Windows.Forms.TextBox();
            this.generateBtn = new System.Windows.Forms.Button();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.direction = new System.Windows.Forms.FlowLayoutPanel();
            this.right = new System.Windows.Forms.RadioButton();
            this.down = new System.Windows.Forms.RadioButton();
            this.left = new System.Windows.Forms.RadioButton();
            this.up = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.layerSpacing = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.columnSpacing = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cycleRemove = new System.Windows.Forms.FlowLayoutPanel();
            this.depthFirst = new System.Windows.Forms.RadioButton();
            this.greedy = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.layering = new System.Windows.Forms.FlowLayoutPanel();
            this.optimalLinkLength = new System.Windows.Forms.RadioButton();
            this.longestPathSource = new System.Windows.Forms.RadioButton();
            this.longestPathSink = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.initialize = new System.Windows.Forms.FlowLayoutPanel();
            this.depthFirstOut = new System.Windows.Forms.RadioButton();
            this.depthFirstIn = new System.Windows.Forms.RadioButton();
            this.naive = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.aggressive = new System.Windows.Forms.FlowLayoutPanel();
            this.none = new System.Windows.Forms.RadioButton();
            this.less = new System.Windows.Forms.RadioButton();
            this.more = new System.Windows.Forms.RadioButton();
            this.label12 = new System.Windows.Forms.Label();
            this.median = new System.Windows.Forms.CheckBox();
            this.straighten = new System.Windows.Forms.CheckBox();
            this.expand = new System.Windows.Forms.CheckBox();
            this.setsPortSpots = new System.Windows.Forms.CheckBox();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.desc1 = new WinFormsDemoApp.GoWebBrowser();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.direction.SuspendLayout();
            this.cycleRemove.SuspendLayout();
            this.layering.SuspendLayout();
            this.initialize.SuspendLayout();
            this.aggressive.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.desc1, 0, 2);
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
            this.flowLayoutPanel2.Controls.Add(this.minNodes);
            this.flowLayoutPanel2.Controls.Add(this.label3);
            this.flowLayoutPanel2.Controls.Add(this.maxNodes);
            this.flowLayoutPanel2.Controls.Add(this.generateBtn);
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
            // minNodes
            //
            this.flowLayoutPanel2.SetFlowBreak(this.minNodes, true);
            this.minNodes.Location = new System.Drawing.Point(85, 20);
            this.minNodes.Name = "minNodes";
            this.minNodes.Size = new System.Drawing.Size(100, 25);
            this.minNodes.TabIndex = 2;
            this.minNodes.Text = "20";
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
            // maxNodes
            //
            this.flowLayoutPanel2.SetFlowBreak(this.maxNodes, true);
            this.maxNodes.Location = new System.Drawing.Point(88, 51);
            this.maxNodes.Name = "maxNodes";
            this.maxNodes.Size = new System.Drawing.Size(100, 25);
            this.maxNodes.TabIndex = 4;
            this.maxNodes.Text = "100";
            //
            // generateBtn
            //
            this.generateBtn.AutoSize = true;
            this.generateBtn.Location = new System.Drawing.Point(3, 82);
            this.generateBtn.Name = "generateBtn";
            this.generateBtn.Size = new System.Drawing.Size(122, 27);
            this.generateBtn.TabIndex = 5;
            this.generateBtn.Text = "Generate Digraph";
            this.generateBtn.UseVisualStyleBackColor = true;
            //
            // flowLayoutPanel3
            //
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.Controls.Add(this.label4);
            this.flowLayoutPanel3.Controls.Add(this.label5);
            this.flowLayoutPanel3.Controls.Add(this.direction);
            this.flowLayoutPanel3.Controls.Add(this.label6);
            this.flowLayoutPanel3.Controls.Add(this.layerSpacing);
            this.flowLayoutPanel3.Controls.Add(this.label7);
            this.flowLayoutPanel3.Controls.Add(this.columnSpacing);
            this.flowLayoutPanel3.Controls.Add(this.label8);
            this.flowLayoutPanel3.Controls.Add(this.cycleRemove);
            this.flowLayoutPanel3.Controls.Add(this.label9);
            this.flowLayoutPanel3.Controls.Add(this.layering);
            this.flowLayoutPanel3.Controls.Add(this.label10);
            this.flowLayoutPanel3.Controls.Add(this.initialize);
            this.flowLayoutPanel3.Controls.Add(this.label11);
            this.flowLayoutPanel3.Controls.Add(this.aggressive);
            this.flowLayoutPanel3.Controls.Add(this.label12);
            this.flowLayoutPanel3.Controls.Add(this.median);
            this.flowLayoutPanel3.Controls.Add(this.straighten);
            this.flowLayoutPanel3.Controls.Add(this.expand);
            this.flowLayoutPanel3.Controls.Add(this.setsPortSpots);
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
            this.direction.Controls.Add(this.right);
            this.direction.Controls.Add(this.down);
            this.direction.Controls.Add(this.left);
            this.direction.Controls.Add(this.up);
            this.flowLayoutPanel3.SetFlowBreak(this.direction, true);
            this.direction.Location = new System.Drawing.Point(72, 20);
            this.direction.Name = "direction";
            this.direction.Size = new System.Drawing.Size(340, 27);
            this.direction.TabIndex = 2;
            //
            // right
            //
            this.right.AutoSize = true;
            this.right.Checked = true;
            this.right.Location = new System.Drawing.Point(3, 3);
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(75, 21);
            this.right.TabIndex = 0;
            this.right.TabStop = true;
            this.right.Text = "Right (0)";
            this.right.UseVisualStyleBackColor = true;
            //
            // down
            //
            this.down.AutoSize = true;
            this.down.Location = new System.Drawing.Point(84, 3);
            this.down.Name = "down";
            this.down.Size = new System.Drawing.Size(85, 21);
            this.down.TabIndex = 1;
            this.down.Text = "Down (90)";
            this.down.UseVisualStyleBackColor = true;
            //
            // left
            //
            this.left.AutoSize = true;
            this.left.Location = new System.Drawing.Point(175, 3);
            this.left.Name = "left";
            this.left.Size = new System.Drawing.Size(80, 21);
            this.left.TabIndex = 2;
            this.left.Text = "Left (180)";
            this.left.UseVisualStyleBackColor = true;
            //
            // up
            //
            this.up.AutoSize = true;
            this.up.Location = new System.Drawing.Point(261, 3);
            this.up.Name = "up";
            this.up.Size = new System.Drawing.Size(76, 21);
            this.up.TabIndex = 3;
            this.up.Text = "Up (270)";
            this.up.UseVisualStyleBackColor = true;
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
            // layerSpacing
            //
            this.flowLayoutPanel3.SetFlowBreak(this.layerSpacing, true);
            this.layerSpacing.Location = new System.Drawing.Point(97, 53);
            this.layerSpacing.Name = "layerSpacing";
            this.layerSpacing.Size = new System.Drawing.Size(100, 25);
            this.layerSpacing.TabIndex = 4;
            this.layerSpacing.Text = "25";
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
            // columnSpacing
            //
            this.flowLayoutPanel3.SetFlowBreak(this.columnSpacing, true);
            this.columnSpacing.Location = new System.Drawing.Point(110, 84);
            this.columnSpacing.Name = "columnSpacing";
            this.columnSpacing.Size = new System.Drawing.Size(100, 25);
            this.columnSpacing.TabIndex = 6;
            this.columnSpacing.Text = "25";
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
            this.cycleRemove.Controls.Add(this.depthFirst);
            this.cycleRemove.Controls.Add(this.greedy);
            this.flowLayoutPanel3.SetFlowBreak(this.cycleRemove, true);
            this.cycleRemove.Location = new System.Drawing.Point(97, 115);
            this.cycleRemove.Name = "cycleRemove";
            this.cycleRemove.Size = new System.Drawing.Size(165, 27);
            this.cycleRemove.TabIndex = 8;
            //
            // depthFirst
            //
            this.depthFirst.AutoSize = true;
            this.depthFirst.Checked = true;
            this.depthFirst.Location = new System.Drawing.Point(3, 3);
            this.depthFirst.Name = "depthFirst";
            this.depthFirst.Size = new System.Drawing.Size(85, 21);
            this.depthFirst.TabIndex = 0;
            this.depthFirst.TabStop = true;
            this.depthFirst.Text = "DepthFirst";
            this.depthFirst.UseVisualStyleBackColor = true;
            //
            // greedy
            //
            this.greedy.AutoSize = true;
            this.greedy.Location = new System.Drawing.Point(94, 3);
            this.greedy.Name = "greedy";
            this.greedy.Size = new System.Drawing.Size(68, 21);
            this.greedy.TabIndex = 1;
            this.greedy.Text = "Greedy";
            this.greedy.UseVisualStyleBackColor = true;
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
            this.layering.Controls.Add(this.optimalLinkLength);
            this.layering.Controls.Add(this.longestPathSource);
            this.layering.Controls.Add(this.longestPathSink);
            this.flowLayoutPanel3.SetFlowBreak(this.layering, true);
            this.layering.Location = new System.Drawing.Point(69, 148);
            this.layering.Name = "layering";
            this.layering.Size = new System.Drawing.Size(408, 27);
            this.layering.TabIndex = 10;
            //
            // optimalLinkLength
            //
            this.optimalLinkLength.AutoSize = true;
            this.optimalLinkLength.Checked = true;
            this.optimalLinkLength.Location = new System.Drawing.Point(3, 3);
            this.optimalLinkLength.Name = "optimalLinkLength";
            this.optimalLinkLength.Size = new System.Drawing.Size(133, 21);
            this.optimalLinkLength.TabIndex = 0;
            this.optimalLinkLength.TabStop = true;
            this.optimalLinkLength.Text = "OptimalLinkLength";
            this.optimalLinkLength.UseVisualStyleBackColor = true;
            //
            // longestPathSource
            //
            this.longestPathSource.AutoSize = true;
            this.longestPathSource.Location = new System.Drawing.Point(142, 3);
            this.longestPathSource.Name = "longestPathSource";
            this.longestPathSource.Size = new System.Drawing.Size(137, 21);
            this.longestPathSource.TabIndex = 1;
            this.longestPathSource.Text = "LongestPathSource";
            this.longestPathSource.UseVisualStyleBackColor = true;
            //
            // longestPathSink
            //
            this.longestPathSink.AutoSize = true;
            this.longestPathSink.Location = new System.Drawing.Point(285, 3);
            this.longestPathSink.Name = "longestPathSink";
            this.longestPathSink.Size = new System.Drawing.Size(120, 21);
            this.longestPathSink.TabIndex = 2;
            this.longestPathSink.Text = "LongestPathSink";
            this.longestPathSink.UseVisualStyleBackColor = true;
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
            this.initialize.Controls.Add(this.depthFirstOut);
            this.initialize.Controls.Add(this.depthFirstIn);
            this.initialize.Controls.Add(this.naive);
            this.flowLayoutPanel3.SetFlowBreak(this.initialize, true);
            this.initialize.Location = new System.Drawing.Point(66, 181);
            this.initialize.Name = "initialize";
            this.initialize.Size = new System.Drawing.Size(278, 27);
            this.initialize.TabIndex = 12;
            //
            // depthFirstOut
            //
            this.depthFirstOut.AutoSize = true;
            this.depthFirstOut.Checked = true;
            this.depthFirstOut.Location = new System.Drawing.Point(3, 3);
            this.depthFirstOut.Name = "depthFirstOut";
            this.depthFirstOut.Size = new System.Drawing.Size(106, 21);
            this.depthFirstOut.TabIndex = 0;
            this.depthFirstOut.TabStop = true;
            this.depthFirstOut.Text = "DepthFirstOut";
            this.depthFirstOut.UseVisualStyleBackColor = true;
            //
            // depthFirstIn
            //
            this.depthFirstIn.AutoSize = true;
            this.depthFirstIn.Location = new System.Drawing.Point(115, 3);
            this.depthFirstIn.Name = "depthFirstIn";
            this.depthFirstIn.Size = new System.Drawing.Size(95, 21);
            this.depthFirstIn.TabIndex = 1;
            this.depthFirstIn.Text = "DepthFirstIn";
            this.depthFirstIn.UseVisualStyleBackColor = true;
            //
            // naive
            //
            this.naive.AutoSize = true;
            this.naive.Location = new System.Drawing.Point(216, 3);
            this.naive.Name = "naive";
            this.naive.Size = new System.Drawing.Size(59, 21);
            this.naive.TabIndex = 2;
            this.naive.Text = "Naive";
            this.naive.UseVisualStyleBackColor = true;
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
            this.aggressive.Controls.Add(this.none);
            this.aggressive.Controls.Add(this.less);
            this.aggressive.Controls.Add(this.more);
            this.flowLayoutPanel3.SetFlowBreak(this.aggressive, true);
            this.aggressive.Location = new System.Drawing.Point(84, 214);
            this.aggressive.Name = "aggressive";
            this.aggressive.Size = new System.Drawing.Size(185, 27);
            this.aggressive.TabIndex = 14;
            //
            // none
            //
            this.none.AutoSize = true;
            this.none.Location = new System.Drawing.Point(3, 3);
            this.none.Name = "none";
            this.none.Size = new System.Drawing.Size(58, 21);
            this.none.TabIndex = 0;
            this.none.Text = "None";
            this.none.UseVisualStyleBackColor = true;
            //
            // less
            //
            this.less.AutoSize = true;
            this.less.Checked = true;
            this.less.Location = new System.Drawing.Point(67, 3);
            this.less.Name = "less";
            this.less.Size = new System.Drawing.Size(51, 21);
            this.less.TabIndex = 1;
            this.less.TabStop = true;
            this.less.Text = "Less";
            this.less.UseVisualStyleBackColor = true;
            //
            // more
            //
            this.more.AutoSize = true;
            this.more.Location = new System.Drawing.Point(124, 3);
            this.more.Name = "more";
            this.more.Size = new System.Drawing.Size(58, 21);
            this.more.TabIndex = 2;
            this.more.Text = "More";
            this.more.UseVisualStyleBackColor = true;
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
            // median
            //
            this.median.AutoSize = true;
            this.median.Checked = true;
            this.median.CheckState = System.Windows.Forms.CheckState.Checked;
            this.median.Location = new System.Drawing.Point(46, 247);
            this.median.Name = "median";
            this.median.Size = new System.Drawing.Size(71, 21);
            this.median.TabIndex = 16;
            this.median.Text = "Median";
            this.median.UseVisualStyleBackColor = true;
            //
            // straighten
            //
            this.straighten.AutoSize = true;
            this.straighten.Checked = true;
            this.straighten.CheckState = System.Windows.Forms.CheckState.Checked;
            this.straighten.Location = new System.Drawing.Point(123, 247);
            this.straighten.Name = "straighten";
            this.straighten.Size = new System.Drawing.Size(86, 21);
            this.straighten.TabIndex = 17;
            this.straighten.Text = "Straighten";
            this.straighten.UseVisualStyleBackColor = true;
            //
            // expand
            //
            this.expand.AutoSize = true;
            this.expand.Checked = true;
            this.expand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.flowLayoutPanel3.SetFlowBreak(this.expand, true);
            this.expand.Location = new System.Drawing.Point(215, 247);
            this.expand.Name = "expand";
            this.expand.Size = new System.Drawing.Size(70, 21);
            this.expand.TabIndex = 18;
            this.expand.Text = "Expand";
            this.expand.UseVisualStyleBackColor = true;
            //
            // setsPortSpots
            //
            this.setsPortSpots.AutoSize = true;
            this.setsPortSpots.Checked = true;
            this.setsPortSpots.CheckState = System.Windows.Forms.CheckState.Checked;
            this.setsPortSpots.Location = new System.Drawing.Point(3, 274);
            this.setsPortSpots.Name = "setsPortSpots";
            this.setsPortSpots.Size = new System.Drawing.Size(108, 21);
            this.setsPortSpots.TabIndex = 19;
            this.setsPortSpots.Text = "SetsPortSpots";
            this.setsPortSpots.UseVisualStyleBackColor = true;
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
            // desc1
            //
            this.desc1.CreationProperties = null;
            this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.desc1.Dock = System.Windows.Forms.DockStyle.Top;
            this.desc1.Location = new System.Drawing.Point(3, 813);
            this.desc1.Name = "desc1";
            this.desc1.Size = new System.Drawing.Size(994, 60);
            this.desc1.TabIndex = 3;
            this.desc1.ZoomFactor = 1D;
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
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsDemoApp.GoWebBrowser desc1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox minNodes;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox maxNodes;
    private System.Windows.Forms.Button generateBtn;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.FlowLayoutPanel direction;
    private System.Windows.Forms.RadioButton right;
    private System.Windows.Forms.RadioButton down;
    private System.Windows.Forms.RadioButton left;
    private System.Windows.Forms.RadioButton up;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox layerSpacing;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox columnSpacing;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.FlowLayoutPanel cycleRemove;
    private System.Windows.Forms.RadioButton depthFirst;
    private System.Windows.Forms.RadioButton greedy;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.FlowLayoutPanel layering;
    private System.Windows.Forms.RadioButton optimalLinkLength;
    private System.Windows.Forms.RadioButton longestPathSource;
    private System.Windows.Forms.RadioButton longestPathSink;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.FlowLayoutPanel initialize;
    private System.Windows.Forms.RadioButton depthFirstOut;
    private System.Windows.Forms.RadioButton depthFirstIn;
    private System.Windows.Forms.RadioButton naive;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.FlowLayoutPanel aggressive;
    private System.Windows.Forms.RadioButton none;
    private System.Windows.Forms.RadioButton less;
    private System.Windows.Forms.RadioButton more;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.CheckBox median;
    private System.Windows.Forms.CheckBox straighten;
    private System.Windows.Forms.CheckBox expand;
    private System.Windows.Forms.CheckBox setsPortSpots;
  }
}
