/* Copyright 1998-2024 by Northwoods Software Corporation. */

namespace Demo.Samples.ProductionEditor {
  partial class ProductionEditorControl {
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.paletteControl1 = new Northwoods.Go.WinForms.PaletteControl();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.title = new System.Windows.Forms.Label();
            this.description = new WinFormsDemoApp.WrappingLabel();
            this.goWebBrowser1 = new WinFormsDemoApp.GoWebBrowser();
            this.modelJson1 = new WinFormsDemoApp.ModelJson();
            this.caption = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.paletteControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1200, 800);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // paletteControl1
            //
            this.paletteControl1.AllowDrop = true;
            this.paletteControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.paletteControl1.Location = new System.Drawing.Point(3, 3);
            this.paletteControl1.Name = "paletteControl1";
            this.paletteControl1.Size = new System.Drawing.Size(1194, 120);
            this.paletteControl1.TabIndex = 0;
            this.paletteControl1.Text = "paletteControl1";
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 129);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(1194, 550);
            this.diagramControl1.TabIndex = 1;
            this.diagramControl1.Text = "diagramControl1";
            //
            // flowLayoutPanel1
            //
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel2);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 685);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(268, 69);
            this.flowLayoutPanel1.TabIndex = 5;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.title, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.description, 0, 2);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(262, 63);
            this.tableLayoutPanel2.TabIndex = 0;
            //
            // pictureBox1
            //
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(256, 15);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            //
            // title
            //
            this.title.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.title.ForeColor = System.Drawing.Color.White;
            this.title.Location = new System.Drawing.Point(3, 21);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(256, 21);
            this.title.TabIndex = 1;
            this.title.Text = "Title";
            this.title.Visible = false;
            //
            // description
            //
            this.description.ForeColor = System.Drawing.Color.White;
            this.description.Location = new System.Drawing.Point(3, 45);
            this.description.Margin = new System.Windows.Forms.Padding(3);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(256, 15);
            this.description.TabIndex = 2;
            this.description.Text = "Select a node to see more information.";
            //
            // goWebBrowser1
            //
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 760);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(1194, 142);
            this.goWebBrowser1.TabIndex = 3;
            this.goWebBrowser1.ZoomFactor = 1D;
            //
            // modelJson1
            //
            this.modelJson1.CanSaveLoad = false;
            this.modelJson1.AutoSize = true;
            this.modelJson1.Dock = System.Windows.Forms.DockStyle.Top;
            this.modelJson1.Location = new System.Drawing.Point(3, 908);
            this.modelJson1.Name = "modelJson1";
            this.modelJson1.Size = new System.Drawing.Size(1194, 400);
            this.modelJson1.TabIndex = 4;
            //
            // ProductionEditorControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ProductionEditorControl";
            this.Size = new System.Drawing.Size(1200, 800);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.PaletteControl paletteControl1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsDemoApp.GoWebBrowser goWebBrowser1;
    private WinFormsDemoApp.ModelJson modelJson1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Label title;
    private WinFormsDemoApp.WrappingLabel description;
    private System.Windows.Forms.ToolTip caption;
  }
}
