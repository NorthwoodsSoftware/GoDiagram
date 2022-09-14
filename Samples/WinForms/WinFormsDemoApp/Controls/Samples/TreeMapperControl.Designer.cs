/* Copyright 1998-2022 by Northwoods Software Corporation. */

namespace Demo.Samples.TreeMapper {
  partial class TreeMapperControl {
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
            this.goWebBrowser1 = new WinFormsDemoApp.GoWebBrowser();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.routing = new System.Windows.Forms.FlowLayoutPanel();
            this.normalRB = new System.Windows.Forms.RadioButton();
            this.toGroupRB = new System.Windows.Forms.RadioButton();
            this.toNodeRB = new System.Windows.Forms.RadioButton();
            this.modelJson1 = new WinFormsDemoApp.ModelJson();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.routing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.modelJson1, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 909);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(994, 594);
            this.diagramControl1.TabIndex = 0;
            this.diagramControl1.Text = "diagramControl1";
            //
            // goWebBrowser1
            //
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 603);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(994, 94);
            this.goWebBrowser1.TabIndex = 1;
            this.goWebBrowser1.ZoomFactor = 1D;
            //
            // flowLayoutPanel1
            //
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.routing);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(994, 304);
            this.flowLayoutPanel1.TabIndex = 2;
            //
            // label1
            //
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "This sample supports three different routing styles:";
            //
            // routing
            //
            this.routing.AutoSize = true;
            this.routing.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.routing.Controls.Add(this.normalRB);
            this.routing.Controls.Add(this.toGroupRB);
            this.routing.Controls.Add(this.toNodeRB);
            this.routing.Location = new System.Drawing.Point(84, 214);
            this.routing.Name = "routing";
            this.routing.Size = new System.Drawing.Size(185, 27);
            this.routing.TabIndex = 1;
            //
            // normalRB
            //
            this.normalRB.AutoSize = true;
            this.normalRB.Location = new System.Drawing.Point(3, 3);
            this.normalRB.Name = "normalRB";
            this.normalRB.Size = new System.Drawing.Size(58, 21);
            this.normalRB.TabIndex = 0;
            this.normalRB.Text = "'Normal'";
            this.normalRB.UseVisualStyleBackColor = true;
            this.normalRB.CheckedChanged += new System.EventHandler(this._ChangeStyle);
            //
            // toGroupRB
            //
            this.toGroupRB.AutoSize = true;
            this.toGroupRB.Checked = true;
            this.toGroupRB.Location = new System.Drawing.Point(67, 3);
            this.toGroupRB.Name = "toGroupRB";
            this.toGroupRB.Size = new System.Drawing.Size(51, 21);
            this.toGroupRB.TabIndex = 1;
            this.toGroupRB.TabStop = true;
            this.toGroupRB.Text = "'ToGroup', where the links stop at the border of the group";
            this.toGroupRB.UseVisualStyleBackColor = true;
            this.toGroupRB.CheckedChanged += new System.EventHandler(this._ChangeStyle);
            //
            // toNodeRB
            //
            this.toNodeRB.AutoSize = true;
            this.toNodeRB.Location = new System.Drawing.Point(124, 3);
            this.toNodeRB.Name = "toNodeRB";
            this.toNodeRB.Size = new System.Drawing.Size(58, 21);
            this.toNodeRB.TabIndex = 2;
            this.toNodeRB.Text = "'ToNode', where the links bend at the border of the group but go all the way to the node, as normal";
            this.toNodeRB.UseVisualStyleBackColor = true;
            this.toNodeRB.CheckedChanged += new System.EventHandler(this._ChangeStyle);
            //
            // modelJson1
            //
            this.modelJson1.CanSaveLoad = false;
            this.modelJson1.AutoSize = true;
            this.modelJson1.Dock = System.Windows.Forms.DockStyle.Top;
            this.modelJson1.Location = new System.Drawing.Point(3, 703);
            this.modelJson1.Name = "modelJson1";
            this.modelJson1.Size = new System.Drawing.Size(994, 244);
            this.modelJson1.TabIndex = 3;
            //
            // TreeMapperControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TreeMapperControl";
            this.Size = new System.Drawing.Size(1000, 909);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.routing.ResumeLayout(false);
            this.routing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsDemoApp.GoWebBrowser goWebBrowser1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.FlowLayoutPanel routing;
    private System.Windows.Forms.RadioButton normalRB;
    private System.Windows.Forms.RadioButton toGroupRB;
    private System.Windows.Forms.RadioButton toNodeRB;
    private WinFormsDemoApp.ModelJson modelJson1;
  }
}
