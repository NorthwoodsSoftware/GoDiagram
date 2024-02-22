/* Copyright 1998-2024 by Northwoods Software Corporation. */

namespace Demo.Samples.AddToPalette {
  partial class AddToPaletteControl {
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.paletteControl1 = new Northwoods.Go.WinForms.PaletteControl();
            this.overviewControl1 = new Northwoods.Go.WinForms.OverviewControl();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.inspectorControl1 = new Northwoods.Go.Extensions.InspectorControl();
            this.goWebBrowser1 = new WinFormsDemoApp.GoWebBrowser();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.modelJson1 = new WinFormsDemoApp.ModelJson();
            this.modelJson2 = new WinFormsDemoApp.ModelJson();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.modelJson1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.modelJson2, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1145, 1437);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 146F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 506F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 206F));
            this.tableLayoutPanel2.Controls.Add(this.paletteControl1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.overviewControl1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.diagramControl1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.inspectorControl1, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 406F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1139, 512);
            this.tableLayoutPanel2.TabIndex = 6;
            //
            // paletteControl1
            //
            this.paletteControl1.AllowDrop = true;
            this.paletteControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paletteControl1.Location = new System.Drawing.Point(3, 3);
            this.paletteControl1.Name = "paletteControl1";
            this.paletteControl1.Size = new System.Drawing.Size(140, 400);
            this.paletteControl1.TabIndex = 0;
            this.paletteControl1.Text = "paletteControl1";
            //
            // overviewControl1
            //
            this.overviewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overviewControl1.Location = new System.Drawing.Point(3, 409);
            this.overviewControl1.Name = "overviewControl1";
            this.overviewControl1.Size = new System.Drawing.Size(140, 100);
            this.overviewControl1.TabIndex = 1;
            this.overviewControl1.Text = "overviewControl1";
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramControl1.Location = new System.Drawing.Point(149, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.tableLayoutPanel2.SetRowSpan(this.diagramControl1, 2);
            this.diagramControl1.Size = new System.Drawing.Size(500, 506);
            this.diagramControl1.TabIndex = 7;
            this.diagramControl1.Text = "diagramControl1";
            //
            // inspectorControl1
            //
            this.inspectorControl1.AutoSize = true;
            this.inspectorControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.inspectorControl1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.inspectorControl1.Location = new System.Drawing.Point(655, 3);
            this.inspectorControl1.Name = "inspectorControl1";
            this.tableLayoutPanel2.SetRowSpan(this.inspectorControl1, 2);
            this.inspectorControl1.Size = new System.Drawing.Size(92, 21);
            this.inspectorControl1.TabIndex = 7;
            //
            // goWebBrowser1
            //
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 521);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(1139, 114);
            this.goWebBrowser1.TabIndex = 1;
            this.goWebBrowser1.ZoomFactor = 1D;
            //
            // flowLayoutPanel1
            //
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.btnAdd);
            this.flowLayoutPanel1.Controls.Add(this.btnDelete);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 641);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(314, 31);
            this.flowLayoutPanel1.TabIndex = 7;
            //
            // btnAdd
            //
            this.btnAdd.AutoSize = true;
            this.btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAdd.Location = new System.Drawing.Point(3, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(93, 25);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add To Palette";
            this.btnAdd.UseVisualStyleBackColor = true;
            //
            // btnDelete
            //
            this.btnDelete.AutoSize = true;
            this.btnDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDelete.Location = new System.Drawing.Point(102, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 25);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete From Palette";
            this.btnDelete.UseVisualStyleBackColor = true;
            //
            // modelJson1
            //
            this.modelJson1.CanSaveLoad = false;
            this.modelJson1.Text = "Palette Model in JSON format:";
            this.modelJson1.AutoSize = true;
            this.modelJson1.Dock = System.Windows.Forms.DockStyle.Top;
            this.modelJson1.Location = new System.Drawing.Point(3, 678);
            this.modelJson1.Name = "modelJson1";
            this.modelJson1.Size = new System.Drawing.Size(1139, 200);
            this.modelJson1.TabIndex = 4;
            //
            // modelJson2
            //
            this.modelJson2.AutoSize = true;
            this.modelJson2.Dock = System.Windows.Forms.DockStyle.Top;
            this.modelJson2.Location = new System.Drawing.Point(3, 884);
            this.modelJson2.Name = "modelJson2";
            this.modelJson2.Size = new System.Drawing.Size(1139, 335);
            this.modelJson2.TabIndex = 5;
            //
            // AddToPaletteControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AddToPaletteControl";
            this.Size = new System.Drawing.Size(1145, 1437);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private WinFormsDemoApp.GoWebBrowser goWebBrowser1;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Button btnDelete;
    private WinFormsDemoApp.ModelJson modelJson1;
    private WinFormsDemoApp.ModelJson modelJson2;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private Northwoods.Go.WinForms.PaletteControl paletteControl1;
    private Northwoods.Go.WinForms.OverviewControl overviewControl1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private Northwoods.Go.Extensions.InspectorControl inspectorControl1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
  }
}
