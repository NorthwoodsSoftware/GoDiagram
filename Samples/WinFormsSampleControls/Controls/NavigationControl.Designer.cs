/* Copyright 1998-2022 by Northwoods Software Corporation. */

namespace WinFormsSampleControls.Navigation {
  partial class NavigationControl {
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
            this.unhighlightAll = new System.Windows.Forms.RadioButton();
            this.linksIn = new System.Windows.Forms.RadioButton();
            this.linksOut = new System.Windows.Forms.RadioButton();
            this.linksAll = new System.Windows.Forms.RadioButton();
            this.nodesIn = new System.Windows.Forms.RadioButton();
            this.nodesOut = new System.Windows.Forms.RadioButton();
            this.nodesConnect = new System.Windows.Forms.RadioButton();
            this.nodesReach = new System.Windows.Forms.RadioButton();
            this.group = new System.Windows.Forms.RadioButton();
            this.groupsAll = new System.Windows.Forms.RadioButton();
            this.nodesMember = new System.Windows.Forms.RadioButton();
            this.nodesMembersAll = new System.Windows.Forms.RadioButton();
            this.linksMember = new System.Windows.Forms.RadioButton();
            this.linksMembersAll = new System.Windows.Forms.RadioButton();
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
            this.goWebBrowser1 = new WinFormsSharedControls.GoWebBrowser();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
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
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 1);
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
            this.tableLayoutPanel3.Controls.Add(this.unhighlightAll, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.linksIn, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.linksOut, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.linksAll, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.nodesIn, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.nodesOut, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.nodesConnect, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.nodesReach, 0, 8);
            this.tableLayoutPanel3.Controls.Add(this.group, 0, 9);
            this.tableLayoutPanel3.Controls.Add(this.groupsAll, 0, 10);
            this.tableLayoutPanel3.Controls.Add(this.nodesMember, 0, 11);
            this.tableLayoutPanel3.Controls.Add(this.nodesMembersAll, 0, 12);
            this.tableLayoutPanel3.Controls.Add(this.linksMember, 0, 13);
            this.tableLayoutPanel3.Controls.Add(this.linksMembersAll, 0, 14);
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
            // unhighlightAll
            //
            this.unhighlightAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.unhighlightAll.AutoSize = true;
            this.unhighlightAll.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.unhighlightAll.Location = new System.Drawing.Point(3, 20);
            this.unhighlightAll.Name = "unhighlightAll";
            this.unhighlightAll.Size = new System.Drawing.Size(179, 21);
            this.unhighlightAll.TabIndex = 0;
            this.unhighlightAll.Text = "Unhighlight All";
            this.unhighlightAll.UseVisualStyleBackColor = true;
            this.unhighlightAll.CheckedChanged += new System.EventHandler(this._RadioChanged);
            //
            // linksIn
            //
            this.linksIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.linksIn.AutoSize = true;
            this.linksIn.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.linksIn.Location = new System.Drawing.Point(3, 47);
            this.linksIn.Name = "linksIn";
            this.linksIn.Size = new System.Drawing.Size(179, 21);
            this.linksIn.TabIndex = 1;
            this.linksIn.Text = "Links Into";
            this.linksIn.UseVisualStyleBackColor = true;
            this.linksIn.CheckedChanged += new System.EventHandler(this._RadioChanged);
            //
            // linksOut
            //
            this.linksOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.linksOut.AutoSize = true;
            this.linksOut.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.linksOut.Location = new System.Drawing.Point(3, 74);
            this.linksOut.Name = "linksOut";
            this.linksOut.Size = new System.Drawing.Size(179, 21);
            this.linksOut.TabIndex = 2;
            this.linksOut.Text = "Links Out Of";
            this.linksOut.UseVisualStyleBackColor = true;
            this.linksOut.CheckedChanged += new System.EventHandler(this._RadioChanged);
            //
            // linksAll
            //
            this.linksAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.linksAll.AutoSize = true;
            this.linksAll.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.linksAll.Location = new System.Drawing.Point(3, 101);
            this.linksAll.Name = "linksAll";
            this.linksAll.Size = new System.Drawing.Size(179, 21);
            this.linksAll.TabIndex = 3;
            this.linksAll.Text = "Links Connected";
            this.linksAll.UseVisualStyleBackColor = true;
            this.linksAll.CheckedChanged += new System.EventHandler(this._RadioChanged);
            //
            // nodesIn
            //
            this.nodesIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nodesIn.AutoSize = true;
            this.nodesIn.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nodesIn.Location = new System.Drawing.Point(3, 128);
            this.nodesIn.Name = "nodesIn";
            this.nodesIn.Size = new System.Drawing.Size(179, 21);
            this.nodesIn.TabIndex = 4;
            this.nodesIn.Text = "Nodes Into";
            this.nodesIn.UseVisualStyleBackColor = true;
            this.nodesIn.CheckedChanged += new System.EventHandler(this._RadioChanged);
            //
            // nodesOut
            //
            this.nodesOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nodesOut.AutoSize = true;
            this.nodesOut.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nodesOut.Location = new System.Drawing.Point(3, 155);
            this.nodesOut.Name = "nodesOut";
            this.nodesOut.Size = new System.Drawing.Size(179, 21);
            this.nodesOut.TabIndex = 5;
            this.nodesOut.Text = "Nodes Out Of";
            this.nodesOut.UseVisualStyleBackColor = true;
            this.nodesOut.CheckedChanged += new System.EventHandler(this._RadioChanged);
            //
            // nodesConnect
            //
            this.nodesConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nodesConnect.AutoSize = true;
            this.nodesConnect.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nodesConnect.Location = new System.Drawing.Point(3, 182);
            this.nodesConnect.Name = "nodesConnect";
            this.nodesConnect.Size = new System.Drawing.Size(179, 21);
            this.nodesConnect.TabIndex = 6;
            this.nodesConnect.Text = "Nodes Connected";
            this.nodesConnect.UseVisualStyleBackColor = true;
            this.nodesConnect.CheckedChanged += new System.EventHandler(this._RadioChanged);
            //
            // nodesReach
            //
            this.nodesReach.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nodesReach.AutoSize = true;
            this.nodesReach.Checked = true;
            this.nodesReach.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nodesReach.Location = new System.Drawing.Point(3, 209);
            this.nodesReach.Name = "nodesReach";
            this.nodesReach.Size = new System.Drawing.Size(179, 21);
            this.nodesReach.TabIndex = 7;
            this.nodesReach.TabStop = true;
            this.nodesReach.Text = "Nodes Reachable";
            this.nodesReach.UseVisualStyleBackColor = true;
            this.nodesReach.CheckedChanged += new System.EventHandler(this._RadioChanged);
            //
            // group
            //
            this.group.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.group.AutoSize = true;
            this.group.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.group.Location = new System.Drawing.Point(3, 236);
            this.group.Name = "group";
            this.group.Size = new System.Drawing.Size(179, 21);
            this.group.TabIndex = 8;
            this.group.Text = "Containing Group (Parent)";
            this.group.UseVisualStyleBackColor = true;
            this.group.CheckedChanged += new System.EventHandler(this._RadioChanged);
            //
            // groupsAll
            //
            this.groupsAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.groupsAll.AutoSize = true;
            this.groupsAll.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupsAll.Location = new System.Drawing.Point(3, 263);
            this.groupsAll.Name = "groupsAll";
            this.groupsAll.Size = new System.Drawing.Size(179, 21);
            this.groupsAll.TabIndex = 9;
            this.groupsAll.Text = "Containing Groups (All)";
            this.groupsAll.UseVisualStyleBackColor = true;
            this.groupsAll.CheckedChanged += new System.EventHandler(this._RadioChanged);
            //
            // nodesMember
            //
            this.nodesMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nodesMember.AutoSize = true;
            this.nodesMember.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nodesMember.Location = new System.Drawing.Point(3, 290);
            this.nodesMember.Name = "nodesMember";
            this.nodesMember.Size = new System.Drawing.Size(179, 21);
            this.nodesMember.TabIndex = 10;
            this.nodesMember.Text = "Member Nodes (Children)";
            this.nodesMember.UseVisualStyleBackColor = true;
            this.nodesMember.CheckedChanged += new System.EventHandler(this._RadioChanged);
            //
            // nodesMembersAll
            //
            this.nodesMembersAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nodesMembersAll.AutoSize = true;
            this.nodesMembersAll.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nodesMembersAll.Location = new System.Drawing.Point(3, 317);
            this.nodesMembersAll.Name = "nodesMembersAll";
            this.nodesMembersAll.Size = new System.Drawing.Size(179, 21);
            this.nodesMembersAll.TabIndex = 11;
            this.nodesMembersAll.Text = "Member Nodes (All)";
            this.nodesMembersAll.UseVisualStyleBackColor = true;
            this.nodesMembersAll.CheckedChanged += new System.EventHandler(this._RadioChanged);
            //
            // linksMember
            //
            this.linksMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.linksMember.AutoSize = true;
            this.linksMember.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.linksMember.Location = new System.Drawing.Point(3, 344);
            this.linksMember.Name = "linksMember";
            this.linksMember.Size = new System.Drawing.Size(179, 21);
            this.linksMember.TabIndex = 12;
            this.linksMember.Text = "Member Links (Children)";
            this.linksMember.UseVisualStyleBackColor = true;
            this.linksMember.CheckedChanged += new System.EventHandler(this._RadioChanged);
            //
            // linksMembersAll
            //
            this.linksMembersAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.linksMembersAll.AutoSize = true;
            this.linksMembersAll.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.linksMembersAll.Location = new System.Drawing.Point(3, 371);
            this.linksMembersAll.Name = "linksMembersAll";
            this.linksMembersAll.Size = new System.Drawing.Size(179, 21);
            this.linksMembersAll.TabIndex = 13;
            this.linksMembersAll.Text = "Member Links (All)";
            this.linksMembersAll.UseVisualStyleBackColor = true;
            this.linksMembersAll.CheckedChanged += new System.EventHandler(this._RadioChanged);
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
            // goWebBrowser1
            //
            this.tableLayoutPanel1.SetColumnSpan(this.goWebBrowser1, 2);
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 638);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(994, 112);
            this.goWebBrowser1.TabIndex = 3;
            this.goWebBrowser1.ZoomFactor = 1D;
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
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.RadioButton linksMembersAll;
    private System.Windows.Forms.RadioButton linksMember;
    private System.Windows.Forms.RadioButton nodesMembersAll;
    private System.Windows.Forms.RadioButton nodesMember;
    private System.Windows.Forms.RadioButton groupsAll;
    private System.Windows.Forms.RadioButton group;
    private System.Windows.Forms.RadioButton nodesReach;
    private System.Windows.Forms.RadioButton nodesConnect;
    private System.Windows.Forms.RadioButton nodesOut;
    private System.Windows.Forms.RadioButton nodesIn;
    private System.Windows.Forms.RadioButton linksAll;
    private System.Windows.Forms.RadioButton linksOut;
    private System.Windows.Forms.RadioButton linksIn;
    private System.Windows.Forms.RadioButton unhighlightAll;
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
    private WinFormsSharedControls.GoWebBrowser goWebBrowser1;
  }
}
