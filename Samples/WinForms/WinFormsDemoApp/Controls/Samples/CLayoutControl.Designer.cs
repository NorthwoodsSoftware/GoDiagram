/* Copyright 1998-2024 by Northwoods Software Corporation. */

namespace Demo.Samples.CLayout {
  partial class CLayout {
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
            this.label3 = new System.Windows.Forms.Label();
            this.randSizes = new System.Windows.Forms.CheckBox();
            this.circ = new System.Windows.Forms.CheckBox();
            this.cyclic = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.generateBtn = new System.Windows.Forms.Button();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.radius = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.spacing = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.arrangement = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.diamFormula = new System.Windows.Forms.FlowLayoutPanel();
            this.pythagorean = new System.Windows.Forms.RadioButton();
            this.circular = new System.Windows.Forms.RadioButton();
            this.label19 = new System.Windows.Forms.Label();
            this.direction = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.sorting = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.numNodes = new System.Windows.Forms.TextBox();
            this.width = new System.Windows.Forms.TextBox();
            this.height = new System.Windows.Forms.TextBox();
            this.minLinks = new System.Windows.Forms.TextBox();
            this.maxLinks = new System.Windows.Forms.TextBox();
            this.startAngle = new System.Windows.Forms.TextBox();
            this.sweepAngle = new System.Windows.Forms.TextBox();
            this.aspectRatio = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.diamFormula.SuspendLayout();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoSize = true;
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 833);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.BackColor = System.Drawing.Color.White;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 313);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(994, 429);
            this.diagramControl1.TabIndex = 2;
            this.diagramControl1.Text = "diagramControl1";
            //
            // desc1
            //
            this.desc1.CreationProperties = null;
            this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.desc1.Dock = System.Windows.Forms.DockStyle.Top;
            this.desc1.Location = new System.Drawing.Point(3, 748);
            this.desc1.Name = "desc1";
            this.desc1.Size = new System.Drawing.Size(994, 41);
            this.desc1.TabIndex = 3;
            this.desc1.ZoomFactor = 1D;
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
            this.flowLayoutPanel2.Controls.Add(this.numNodes);
            this.flowLayoutPanel2.Controls.Add(this.label3);
            this.flowLayoutPanel2.Controls.Add(this.width);
            this.flowLayoutPanel2.Controls.Add(this.height);
            this.flowLayoutPanel2.Controls.Add(this.randSizes);
            this.flowLayoutPanel2.Controls.Add(this.circ);
            this.flowLayoutPanel2.Controls.Add(this.cyclic);
            this.flowLayoutPanel2.Controls.Add(this.label4);
            this.flowLayoutPanel2.Controls.Add(this.minLinks);
            this.flowLayoutPanel2.Controls.Add(this.label5);
            this.flowLayoutPanel2.Controls.Add(this.maxLinks);
            this.flowLayoutPanel2.Controls.Add(this.generateBtn);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(291, 255);
            this.flowLayoutPanel2.TabIndex = 0;
            //
            // label1
            //
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
            this.label2.Size = new System.Drawing.Size(75, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "# of nodes:";
            //
            // label3
            //
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Node Size:";
            //
            // randSizes
            //
            this.randSizes.AutoSize = true;
            this.randSizes.Checked = true;
            this.flowLayoutPanel2.SetFlowBreak(this.randSizes, true);
            this.randSizes.Location = new System.Drawing.Point(3, 82);
            this.randSizes.Name = "randSizes";
            this.randSizes.Size = new System.Drawing.Size(109, 21);
            this.randSizes.TabIndex = 7;
            this.randSizes.Text = "Random Sizes";
            this.randSizes.UseVisualStyleBackColor = true;
            //
            // circ
            //
            this.circ.AutoSize = true;
            this.flowLayoutPanel2.SetFlowBreak(this.circ, true);
            this.circ.Location = new System.Drawing.Point(3, 109);
            this.circ.Name = "circ";
            this.circ.Size = new System.Drawing.Size(114, 21);
            this.circ.TabIndex = 8;
            this.circ.Text = "Circular Nodes";
            this.circ.UseVisualStyleBackColor = true;
            //
            // cyclic
            //
            this.cyclic.AutoSize = true;
            this.flowLayoutPanel2.SetFlowBreak(this.cyclic, true);
            this.cyclic.Location = new System.Drawing.Point(3, 136);
            this.cyclic.Name = "cyclic";
            this.cyclic.Size = new System.Drawing.Size(145, 21);
            this.cyclic.TabIndex = 9;
            this.cyclic.Text = "Graph is simple ring";
            this.cyclic.UseVisualStyleBackColor = true;
            //
            // label4
            //
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "Min Links from Node:";
            //
            // label5
            //
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 198);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "Max Links from Node:";
            //
            // generateBtn
            //
            this.generateBtn.AutoSize = true;
            this.generateBtn.Location = new System.Drawing.Point(3, 225);
            this.generateBtn.Name = "generateBtn";
            this.generateBtn.Size = new System.Drawing.Size(107, 27);
            this.generateBtn.TabIndex = 14;
            this.generateBtn.Text = "Generate Circle";
            this.generateBtn.UseVisualStyleBackColor = true;
            //
            // flowLayoutPanel3
            //
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.Controls.Add(this.label6);
            this.flowLayoutPanel3.Controls.Add(this.label7);
            this.flowLayoutPanel3.Controls.Add(this.radius);
            this.flowLayoutPanel3.Controls.Add(this.label8);
            this.flowLayoutPanel3.Controls.Add(this.label9);
            this.flowLayoutPanel3.Controls.Add(this.aspectRatio);
            this.flowLayoutPanel3.Controls.Add(this.label10);
            this.flowLayoutPanel3.Controls.Add(this.label11);
            this.flowLayoutPanel3.Controls.Add(this.startAngle);
            this.flowLayoutPanel3.Controls.Add(this.label12);
            this.flowLayoutPanel3.Controls.Add(this.label13);
            this.flowLayoutPanel3.Controls.Add(this.sweepAngle);
            this.flowLayoutPanel3.Controls.Add(this.label14);
            this.flowLayoutPanel3.Controls.Add(this.label15);
            this.flowLayoutPanel3.Controls.Add(this.spacing);
            this.flowLayoutPanel3.Controls.Add(this.label16);
            this.flowLayoutPanel3.Controls.Add(this.label17);
            this.flowLayoutPanel3.Controls.Add(this.arrangement);
            this.flowLayoutPanel3.Controls.Add(this.label18);
            this.flowLayoutPanel3.Controls.Add(this.diamFormula);
            this.flowLayoutPanel3.Controls.Add(this.label19);
            this.flowLayoutPanel3.Controls.Add(this.direction);
            this.flowLayoutPanel3.Controls.Add(this.label20);
            this.flowLayoutPanel3.Controls.Add(this.sorting);
            this.flowLayoutPanel3.Controls.Add(this.label21);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(300, 3);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(531, 298);
            this.flowLayoutPanel3.TabIndex = 1;
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label6, true);
            this.label6.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(164, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "CircularLayout Properties";
            //
            // label7
            //
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 17);
            this.label7.TabIndex = 1;
            this.label7.Text = "Radius:";
            //
            // radius
            //
            this.radius.Location = new System.Drawing.Point(59, 20);
            this.radius.Name = "radius";
            this.radius.Size = new System.Drawing.Size(100, 25);
            this.radius.TabIndex = 2;
            this.radius.Text = "NaN";
            //
            // label8
            //
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label8, true);
            this.label8.Location = new System.Drawing.Point(165, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(162, 17);
            this.label8.TabIndex = 3;
            this.label8.Text = "(along X axis; NaN or > 0)";
            //
            // label9
            //
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 17);
            this.label9.TabIndex = 4;
            this.label9.Text = "Aspect Ratio:";
            //
            // label10
            //
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label10, true);
            this.label10.Location = new System.Drawing.Point(183, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(109, 17);
            this.label10.TabIndex = 6;
            this.label10.Text = "(1 is circular; > 0)";
            //
            // label11
            //
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 86);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 17);
            this.label11.TabIndex = 7;
            this.label11.Text = "Start Angle:";
            //
            // label12
            //
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label12, true);
            this.label12.Location = new System.Drawing.Point(174, 86);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(139, 17);
            this.label12.TabIndex = 9;
            this.label12.Text = "(angle at first element)";
            //
            // label13
            //
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 117);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 17);
            this.label13.TabIndex = 10;
            this.label13.Text = "Sweep Angle:";
            //
            // label14
            //
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label14.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label14, true);
            this.label14.Location = new System.Drawing.Point(185, 117);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(207, 17);
            this.label14.TabIndex = 12;
            this.label14.Text = "(degrees occupied; >= 1, <= 360)";
            //
            // label15
            //
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 148);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(57, 17);
            this.label15.TabIndex = 13;
            this.label15.Text = "Spacing:";
            //
            // spacing
            //
            this.spacing.Location = new System.Drawing.Point(66, 144);
            this.spacing.Name = "spacing";
            this.spacing.Size = new System.Drawing.Size(100, 25);
            this.spacing.TabIndex = 14;
            this.spacing.Text = "6";
            //
            // label16
            //
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label16.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label16, true);
            this.label16.Location = new System.Drawing.Point(172, 148);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(241, 17);
            this.label16.TabIndex = 15;
            this.label16.Text = "(actual spacing also depends on radius)";
            //
            // label17
            //
            this.label17.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 179);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(87, 17);
            this.label17.TabIndex = 16;
            this.label17.Text = "Arrangement:";
            //
            // arrangement
            //
            this.arrangement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.flowLayoutPanel3.SetFlowBreak(this.arrangement, true);
            this.arrangement.FormattingEnabled = true;
            this.arrangement.Location = new System.Drawing.Point(96, 175);
            this.arrangement.Name = "arrangement";
            this.arrangement.Size = new System.Drawing.Size(140, 25);
            this.arrangement.TabIndex = 17;
            //
            // label18
            //
            this.label18.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(3, 211);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(101, 17);
            this.label18.TabIndex = 18;
            this.label18.Text = "Node Diameter:";
            //
            // diamFormula
            //
            this.diamFormula.AutoSize = true;
            this.diamFormula.Controls.Add(this.pythagorean);
            this.diamFormula.Controls.Add(this.circular);
            this.flowLayoutPanel3.SetFlowBreak(this.diamFormula, true);
            this.diamFormula.Location = new System.Drawing.Point(110, 206);
            this.diamFormula.Name = "diamFormula";
            this.diamFormula.Size = new System.Drawing.Size(181, 27);
            this.diamFormula.TabIndex = 19;
            //
            // pythagorean
            //
            this.pythagorean.AutoSize = true;
            this.pythagorean.Checked = true;
            this.pythagorean.Location = new System.Drawing.Point(3, 3);
            this.pythagorean.Name = "pythagorean";
            this.pythagorean.Size = new System.Drawing.Size(99, 21);
            this.pythagorean.TabIndex = 0;
            this.pythagorean.TabStop = true;
            this.pythagorean.Text = "Pythagorean";
            this.pythagorean.UseVisualStyleBackColor = true;
            //
            // circular
            //
            this.circular.AutoSize = true;
            this.circular.Location = new System.Drawing.Point(108, 3);
            this.circular.Name = "circular";
            this.circular.Size = new System.Drawing.Size(70, 21);
            this.circular.TabIndex = 1;
            this.circular.Text = "Circular";
            this.circular.UseVisualStyleBackColor = true;
            //
            // label19
            //
            this.label19.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(3, 243);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(63, 17);
            this.label19.TabIndex = 20;
            this.label19.Text = "Direction:";
            //
            // direction
            //
            this.direction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.flowLayoutPanel3.SetFlowBreak(this.direction, true);
            this.direction.FormattingEnabled = true;
            this.direction.Location = new System.Drawing.Point(72, 239);
            this.direction.Name = "direction";
            this.direction.Size = new System.Drawing.Size(140, 25);
            this.direction.TabIndex = 21;
            //
            // label20
            //
            this.label20.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(3, 274);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 17);
            this.label20.TabIndex = 22;
            this.label20.Text = "Sorting:";
            //
            // sorting
            //
            this.sorting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sorting.FormattingEnabled = true;
            this.sorting.Location = new System.Drawing.Point(62, 270);
            this.sorting.Name = "sorting";
            this.sorting.Size = new System.Drawing.Size(121, 25);
            this.sorting.TabIndex = 23;
            //
            // label21
            //
            this.label21.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(189, 274);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(339, 17);
            this.label21.TabIndex = 24;
            this.label21.Text = "(use \"Optimized\" to reduce the number of link crossings)";
            //
            // numNodes
            //
            this.flowLayoutPanel2.SetFlowBreak(this.numNodes, true);
            this.numNodes.Location = new System.Drawing.Point(84, 20);
            this.numNodes.Name = "numNodes";
            this.numNodes.Size = new System.Drawing.Size(100, 25);
            this.numNodes.TabIndex = 15;
            this.numNodes.Text = "16";
            //
            // width
            //
            this.width.Location = new System.Drawing.Point(80, 51);
            this.width.Name = "width";
            this.width.Size = new System.Drawing.Size(100, 25);
            this.width.TabIndex = 16;
            this.width.Text = "25";
            //
            // height
            //
            this.flowLayoutPanel2.SetFlowBreak(this.height, true);
            this.height.Location = new System.Drawing.Point(170, 51);
            this.height.Name = "height";
            this.height.Size = new System.Drawing.Size(100, 25);
            this.height.TabIndex = 17;
            this.height.Text = "25";
            //
            // minLinks
            //
            this.flowLayoutPanel2.SetFlowBreak(this.minLinks, true);
            this.minLinks.Location = new System.Drawing.Point(143, 163);
            this.minLinks.Name = "minLinks";
            this.minLinks.Size = new System.Drawing.Size(100, 25);
            this.minLinks.TabIndex = 18;
            this.minLinks.Text = "1";
            //
            // maxLinks
            //
            this.flowLayoutPanel2.SetFlowBreak(this.maxLinks, true);
            this.maxLinks.Location = new System.Drawing.Point(146, 194);
            this.maxLinks.Name = "maxLinks";
            this.maxLinks.Size = new System.Drawing.Size(100, 25);
            this.maxLinks.TabIndex = 19;
            this.maxLinks.Text = "2";
            //
            // startAngle
            //
            this.startAngle.Location = new System.Drawing.Point(84, 82);
            this.startAngle.Name = "startAngle";
            this.startAngle.Size = new System.Drawing.Size(100, 25);
            this.startAngle.TabIndex = 25;
            this.startAngle.Text = "0";
            this.startAngle.Size = new System.Drawing.Size(84, 25);
            this.startAngle.TabIndex = 25;
            //
            // sweepAngle
            //
            this.sweepAngle.Location = new System.Drawing.Point(95, 113);
            this.sweepAngle.Name = "sweepAngle";
            this.sweepAngle.Size = new System.Drawing.Size(100, 25);
            this.sweepAngle.TabIndex = 26;
            this.sweepAngle.Text = "360";
            //
            // aspectRatio
            //
            this.aspectRatio.Location = new System.Drawing.Point(93, 51);
            this.aspectRatio.Name = "aspectRatio";
            this.aspectRatio.Size = new System.Drawing.Size(100, 25);
            this.aspectRatio.TabIndex = 27;
            this.aspectRatio.Text = "1";
            //
            // CLayoutControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CLayoutControl";
            this.Size = new System.Drawing.Size(1000, 833);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.diamFormula.ResumeLayout(false);
            this.diamFormula.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsDemoApp.GoWebBrowser desc1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.CheckBox randSizes;
    private System.Windows.Forms.CheckBox circ;
    private System.Windows.Forms.CheckBox cyclic;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Button generateBtn;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox radius;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.TextBox spacing;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.ComboBox arrangement;
    private System.Windows.Forms.Label label18;
    private System.Windows.Forms.FlowLayoutPanel diamFormula;
    private System.Windows.Forms.RadioButton pythagorean;
    private System.Windows.Forms.RadioButton circular;
    private System.Windows.Forms.Label label19;
    private System.Windows.Forms.ComboBox direction;
    private System.Windows.Forms.Label label20;
    private System.Windows.Forms.ComboBox sorting;
    private System.Windows.Forms.Label label21;
    private System.Windows.Forms.TextBox numNodes;
    private System.Windows.Forms.TextBox width;
    private System.Windows.Forms.TextBox height;
    private System.Windows.Forms.TextBox minLinks;
    private System.Windows.Forms.TextBox maxLinks;
    private System.Windows.Forms.TextBox startAngle;
    private System.Windows.Forms.TextBox sweepAngle;
    private System.Windows.Forms.TextBox aspectRatio;
  }
}
