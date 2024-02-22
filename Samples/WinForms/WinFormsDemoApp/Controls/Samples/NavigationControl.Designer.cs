/* Copyright 1998-2024 by Northwoods Software Corporation. */

namespace Demo.Samples.Navigation {
  partial class Navigation {
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.unhighlightAllRb = new System.Windows.Forms.RadioButton();
            this.linksIntoRb = new System.Windows.Forms.RadioButton();
            this.linksOutOfRb = new System.Windows.Forms.RadioButton();
            this.linksConnectedRb = new System.Windows.Forms.RadioButton();
            this.nodesIntoRb = new System.Windows.Forms.RadioButton();
            this.nodesOutOfRb = new System.Windows.Forms.RadioButton();
            this.nodesConnectedRb = new System.Windows.Forms.RadioButton();
            this.nodesReachableRb = new System.Windows.Forms.RadioButton();
            this.containingGroupParentRb = new System.Windows.Forms.RadioButton();
            this.containingGroupsAllRb = new System.Windows.Forms.RadioButton();
            this.memberNodesChildrenRb = new System.Windows.Forms.RadioButton();
            this.memberNodesAllRb = new System.Windows.Forms.RadioButton();
            this.memberLinksChildrenRb = new System.Windows.Forms.RadioButton();
            this.memberLinksAllRb = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.blackPanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.bluePanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.greenPanel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.orangePanel = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.redPanel = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.purplePanel = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.desc1 = new WinFormsDemoApp.GoWebBrowser();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.desc1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 850);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.BackColor = System.Drawing.Color.White;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramControl1.Location = new System.Drawing.Point(4, 3);
            this.diagramControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(795, 629);
            this.diagramControl1.TabIndex = 0;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(806, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(191, 629);
            this.tableLayoutPanel2.TabIndex = 2;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.unhighlightAllRb, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.linksIntoRb, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.linksOutOfRb, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.linksConnectedRb, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.nodesIntoRb, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.nodesOutOfRb, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.nodesConnectedRb, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.nodesReachableRb, 0, 8);
            this.tableLayoutPanel3.Controls.Add(this.containingGroupParentRb, 0, 9);
            this.tableLayoutPanel3.Controls.Add(this.containingGroupsAllRb, 0, 10);
            this.tableLayoutPanel3.Controls.Add(this.memberNodesChildrenRb, 0, 11);
            this.tableLayoutPanel3.Controls.Add(this.memberNodesAllRb, 0, 12);
            this.tableLayoutPanel3.Controls.Add(this.memberLinksChildrenRb, 0, 13);
            this.tableLayoutPanel3.Controls.Add(this.memberLinksAllRb, 0, 14);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 15;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(185, 395);
            this.tableLayoutPanel3.TabIndex = 2;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Related Parts Highlighted";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // unhighlightAllRb
            //
            this.unhighlightAllRb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.unhighlightAllRb.AutoSize = true;
            this.unhighlightAllRb.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.unhighlightAllRb.Location = new System.Drawing.Point(3, 20);
            this.unhighlightAllRb.Name = "unhighlightAllRb";
            this.unhighlightAllRb.Size = new System.Drawing.Size(179, 21);
            this.unhighlightAllRb.TabIndex = 0;
            this.unhighlightAllRb.Text = "Unhighlight All";
            this.unhighlightAllRb.UseVisualStyleBackColor = true;
            //
            // linksIntoRb
            //
            this.linksIntoRb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.linksIntoRb.AutoSize = true;
            this.linksIntoRb.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.linksIntoRb.Location = new System.Drawing.Point(3, 47);
            this.linksIntoRb.Name = "linksIntoRb";
            this.linksIntoRb.Size = new System.Drawing.Size(179, 21);
            this.linksIntoRb.TabIndex = 1;
            this.linksIntoRb.Text = "Links Into";
            this.linksIntoRb.UseVisualStyleBackColor = true;
            //
            // linksOutOfRb
            //
            this.linksOutOfRb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.linksOutOfRb.AutoSize = true;
            this.linksOutOfRb.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.linksOutOfRb.Location = new System.Drawing.Point(3, 74);
            this.linksOutOfRb.Name = "linksOutOfRb";
            this.linksOutOfRb.Size = new System.Drawing.Size(179, 21);
            this.linksOutOfRb.TabIndex = 2;
            this.linksOutOfRb.Text = "Links Out Of";
            this.linksOutOfRb.UseVisualStyleBackColor = true;
            //
            // linksConnectedRb
            //
            this.linksConnectedRb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.linksConnectedRb.AutoSize = true;
            this.linksConnectedRb.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.linksConnectedRb.Location = new System.Drawing.Point(3, 101);
            this.linksConnectedRb.Name = "linksConnectedRb";
            this.linksConnectedRb.Size = new System.Drawing.Size(179, 21);
            this.linksConnectedRb.TabIndex = 3;
            this.linksConnectedRb.Text = "Links Connected";
            this.linksConnectedRb.UseVisualStyleBackColor = true;
            //
            // nodesIntoRb
            //
            this.nodesIntoRb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nodesIntoRb.AutoSize = true;
            this.nodesIntoRb.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nodesIntoRb.Location = new System.Drawing.Point(3, 128);
            this.nodesIntoRb.Name = "nodesIntoRb";
            this.nodesIntoRb.Size = new System.Drawing.Size(179, 21);
            this.nodesIntoRb.TabIndex = 4;
            this.nodesIntoRb.Text = "Nodes Into";
            this.nodesIntoRb.UseVisualStyleBackColor = true;
            //
            // nodesOutOfRb
            //
            this.nodesOutOfRb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nodesOutOfRb.AutoSize = true;
            this.nodesOutOfRb.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nodesOutOfRb.Location = new System.Drawing.Point(3, 155);
            this.nodesOutOfRb.Name = "nodesOutOfRb";
            this.nodesOutOfRb.Size = new System.Drawing.Size(179, 21);
            this.nodesOutOfRb.TabIndex = 5;
            this.nodesOutOfRb.Text = "Nodes Out Of";
            this.nodesOutOfRb.UseVisualStyleBackColor = true;
            //
            // nodesConnectedRb
            //
            this.nodesConnectedRb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nodesConnectedRb.AutoSize = true;
            this.nodesConnectedRb.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nodesConnectedRb.Location = new System.Drawing.Point(3, 182);
            this.nodesConnectedRb.Name = "nodesConnectedRb";
            this.nodesConnectedRb.Size = new System.Drawing.Size(179, 21);
            this.nodesConnectedRb.TabIndex = 6;
            this.nodesConnectedRb.Text = "Nodes Connected";
            this.nodesConnectedRb.UseVisualStyleBackColor = true;
            //
            // nodesReachableRb
            //
            this.nodesReachableRb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nodesReachableRb.AutoSize = true;
            this.nodesReachableRb.Checked = true;
            this.nodesReachableRb.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nodesReachableRb.Location = new System.Drawing.Point(3, 209);
            this.nodesReachableRb.Name = "nodesReachableRb";
            this.nodesReachableRb.Size = new System.Drawing.Size(179, 21);
            this.nodesReachableRb.TabIndex = 7;
            this.nodesReachableRb.TabStop = true;
            this.nodesReachableRb.Text = "Nodes Reachable";
            this.nodesReachableRb.UseVisualStyleBackColor = true;
            //
            // containingGroupParentRb
            //
            this.containingGroupParentRb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.containingGroupParentRb.AutoSize = true;
            this.containingGroupParentRb.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.containingGroupParentRb.Location = new System.Drawing.Point(3, 236);
            this.containingGroupParentRb.Name = "containingGroupParentRb";
            this.containingGroupParentRb.Size = new System.Drawing.Size(179, 21);
            this.containingGroupParentRb.TabIndex = 8;
            this.containingGroupParentRb.Text = "Containing Group (Parent)";
            this.containingGroupParentRb.UseVisualStyleBackColor = true;
            //
            // containingGroupsAllRb
            //
            this.containingGroupsAllRb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.containingGroupsAllRb.AutoSize = true;
            this.containingGroupsAllRb.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.containingGroupsAllRb.Location = new System.Drawing.Point(3, 263);
            this.containingGroupsAllRb.Name = "containingGroupsAllRb";
            this.containingGroupsAllRb.Size = new System.Drawing.Size(179, 21);
            this.containingGroupsAllRb.TabIndex = 9;
            this.containingGroupsAllRb.Text = "Containing Groups (All)";
            this.containingGroupsAllRb.UseVisualStyleBackColor = true;
            //
            // memberNodesChildrenRb
            //
            this.memberNodesChildrenRb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.memberNodesChildrenRb.AutoSize = true;
            this.memberNodesChildrenRb.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.memberNodesChildrenRb.Location = new System.Drawing.Point(3, 290);
            this.memberNodesChildrenRb.Name = "memberNodesChildrenRb";
            this.memberNodesChildrenRb.Size = new System.Drawing.Size(179, 21);
            this.memberNodesChildrenRb.TabIndex = 10;
            this.memberNodesChildrenRb.Text = "Member Nodes (Children)";
            this.memberNodesChildrenRb.UseVisualStyleBackColor = true;
            //
            // memberNodesAllRb
            //
            this.memberNodesAllRb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.memberNodesAllRb.AutoSize = true;
            this.memberNodesAllRb.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.memberNodesAllRb.Location = new System.Drawing.Point(3, 317);
            this.memberNodesAllRb.Name = "memberNodesAllRb";
            this.memberNodesAllRb.Size = new System.Drawing.Size(179, 21);
            this.memberNodesAllRb.TabIndex = 11;
            this.memberNodesAllRb.Text = "Member Nodes (All)";
            this.memberNodesAllRb.UseVisualStyleBackColor = true;
            //
            // memberLinksChildrenRb
            //
            this.memberLinksChildrenRb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.memberLinksChildrenRb.AutoSize = true;
            this.memberLinksChildrenRb.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.memberLinksChildrenRb.Location = new System.Drawing.Point(3, 344);
            this.memberLinksChildrenRb.Name = "memberLinksChildrenRb";
            this.memberLinksChildrenRb.Size = new System.Drawing.Size(179, 21);
            this.memberLinksChildrenRb.TabIndex = 12;
            this.memberLinksChildrenRb.Text = "Member Links (Children)";
            this.memberLinksChildrenRb.UseVisualStyleBackColor = true;
            //
            // memberLinksAllRb
            //
            this.memberLinksAllRb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.memberLinksAllRb.AutoSize = true;
            this.memberLinksAllRb.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.memberLinksAllRb.Location = new System.Drawing.Point(3, 371);
            this.memberLinksAllRb.Name = "memberLinksAllRb";
            this.memberLinksAllRb.Size = new System.Drawing.Size(179, 21);
            this.memberLinksAllRb.TabIndex = 13;
            this.memberLinksAllRb.Text = "Member Links (All)";
            this.memberLinksAllRb.UseVisualStyleBackColor = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.blackPanel, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label3, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.bluePanel, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.label4, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.greenPanel, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.label5, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.orangePanel, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.label6, 1, 4);
            this.tableLayoutPanel4.Controls.Add(this.redPanel, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.label7, 1, 5);
            this.tableLayoutPanel4.Controls.Add(this.purplePanel, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.label8, 1, 6);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 404);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 7;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(185, 222);
            this.tableLayoutPanel4.TabIndex = 1;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.label2, 2);
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(179, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Relationship Colors";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // blackPanel
            //
            this.blackPanel.BackColor = System.Drawing.Color.Black;
            this.blackPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.blackPanel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.blackPanel.Location = new System.Drawing.Point(6, 22);
            this.blackPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.blackPanel.Name = "blackPanel";
            this.blackPanel.Size = new System.Drawing.Size(25, 24);
            this.blackPanel.TabIndex = 0;
            //
            // label3
            //
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 15);
            this.label3.TabIndex = 20;
            this.label3.Text = "Not related";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // bluePanel
            //
            this.bluePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.bluePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bluePanel.Location = new System.Drawing.Point(6, 56);
            this.bluePanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.bluePanel.Name = "bluePanel";
            this.bluePanel.Size = new System.Drawing.Size(25, 24);
            this.bluePanel.TabIndex = 2;
            //
            // label4
            //
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 15);
            this.label4.TabIndex = 21;
            this.label4.Text = "Directly related";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // greenPanel
            //
            this.greenPanel.BackColor = System.Drawing.Color.DarkGreen;
            this.greenPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.greenPanel.Location = new System.Drawing.Point(6, 90);
            this.greenPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.greenPanel.Name = "greenPanel";
            this.greenPanel.Size = new System.Drawing.Size(25, 24);
            this.greenPanel.TabIndex = 1;
            //
            // label5
            //
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 15);
            this.label5.TabIndex = 22;
            this.label5.Text = "2 relationships apart";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // orangePanel
            //
            this.orangePanel.BackColor = System.Drawing.Color.Orange;
            this.orangePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.orangePanel.Location = new System.Drawing.Point(6, 124);
            this.orangePanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.orangePanel.Name = "orangePanel";
            this.orangePanel.Size = new System.Drawing.Size(25, 24);
            this.orangePanel.TabIndex = 1;
            //
            // label6
            //
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(40, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 15);
            this.label6.TabIndex = 23;
            this.label6.Text = "3 relationships apart";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // redPanel
            //
            this.redPanel.BackColor = System.Drawing.Color.Red;
            this.redPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.redPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.redPanel.Location = new System.Drawing.Point(6, 158);
            this.redPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.redPanel.Name = "redPanel";
            this.redPanel.Size = new System.Drawing.Size(25, 24);
            this.redPanel.TabIndex = 1;
            //
            // label7
            //
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(40, 162);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 15);
            this.label7.TabIndex = 24;
            this.label7.Text = "4 relationships apart";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // purplePanel
            //
            this.purplePanel.BackColor = System.Drawing.Color.Purple;
            this.purplePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.purplePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.purplePanel.Location = new System.Drawing.Point(6, 192);
            this.purplePanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.purplePanel.Name = "purplePanel";
            this.purplePanel.Size = new System.Drawing.Size(25, 25);
            this.purplePanel.TabIndex = 1;
            //
            // label8
            //
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(40, 197);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 15);
            this.label8.TabIndex = 25;
            this.label8.Text = "Very indirectly related";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // desc1
            //
            this.tableLayoutPanel1.SetColumnSpan(this.desc1, 2);
            this.desc1.CreationProperties = null;
            this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.desc1.Dock = System.Windows.Forms.DockStyle.Top;
            this.desc1.Location = new System.Drawing.Point(3, 638);
            this.desc1.Name = "desc1";
            this.desc1.Size = new System.Drawing.Size(994, 112);
            this.desc1.TabIndex = 3;
            this.desc1.ZoomFactor = 1D;
            //
            // NavigationControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "NavigationControl";
            this.Size = new System.Drawing.Size(1000, 850);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.RadioButton memberLinksAllRb;
    private System.Windows.Forms.RadioButton memberLinksChildrenRb;
    private System.Windows.Forms.RadioButton memberNodesAllRb;
    private System.Windows.Forms.RadioButton memberNodesChildrenRb;
    private System.Windows.Forms.RadioButton containingGroupsAllRb;
    private System.Windows.Forms.RadioButton containingGroupParentRb;
    private System.Windows.Forms.RadioButton nodesReachableRb;
    private System.Windows.Forms.RadioButton nodesConnectedRb;
    private System.Windows.Forms.RadioButton nodesOutOfRb;
    private System.Windows.Forms.RadioButton nodesIntoRb;
    private System.Windows.Forms.RadioButton linksConnectedRb;
    private System.Windows.Forms.RadioButton linksOutOfRb;
    private System.Windows.Forms.RadioButton linksIntoRb;
    private System.Windows.Forms.RadioButton unhighlightAllRb;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    private System.Windows.Forms.Panel purplePanel;
    private System.Windows.Forms.Panel redPanel;
    private System.Windows.Forms.Panel orangePanel;
    private System.Windows.Forms.Panel greenPanel;
    private System.Windows.Forms.Panel blackPanel;
    private System.Windows.Forms.Panel bluePanel;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsDemoApp.GoWebBrowser desc1;
  }
}
