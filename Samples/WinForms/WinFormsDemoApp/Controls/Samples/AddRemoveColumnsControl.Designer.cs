/* Copyright 1998-2023 by Northwoods Software Corporation. */

namespace Demo.Samples.AddRemoveColumns {
  partial class AddRemoveColumnsControl {
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
            this.btnSwapColumns = new System.Windows.Forms.Button();
            this.btnRemoveColumn = new System.Windows.Forms.Button();
            this.btnAddColumn = new System.Windows.Forms.Button();
            this.btnRemoveFromArray = new System.Windows.Forms.Button();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.goWebBrowser1 = new WinFormsDemoApp.GoWebBrowser();
            this.btnInsertIntoArray = new System.Windows.Forms.Button();
            this.goWebBrowser2 = new WinFormsDemoApp.GoWebBrowser();
            this.goWebBrowser3 = new WinFormsDemoApp.GoWebBrowser();
            this.goWebBrowser4 = new WinFormsDemoApp.GoWebBrowser();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser4)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.btnSwapColumns, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnRemoveColumn, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnAddColumn, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnRemoveFromArray, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnInsertIntoArray, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser3, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser4, 0, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 1000);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // btnSwapColumns
            //
            this.btnSwapColumns.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSwapColumns.Location = new System.Drawing.Point(3, 830);
            this.btnSwapColumns.Name = "btnSwapColumns";
            this.btnSwapColumns.Size = new System.Drawing.Size(94, 31);
            this.btnSwapColumns.TabIndex = 10;
            this.btnSwapColumns.Text = "Swap Two Columns";
            this.btnSwapColumns.UseVisualStyleBackColor = true;
            //
            // btnRemoveColumn
            //
            this.btnRemoveColumn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRemoveColumn.Location = new System.Drawing.Point(103, 742);
            this.btnRemoveColumn.Name = "btnRemoveColumn";
            this.btnRemoveColumn.Size = new System.Drawing.Size(173, 32);
            this.btnRemoveColumn.TabIndex = 7;
            this.btnRemoveColumn.Text = "Remove Column";
            this.btnRemoveColumn.UseVisualStyleBackColor = true;
            //
            // btnAddColumn
            //
            this.btnAddColumn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAddColumn.Location = new System.Drawing.Point(3, 742);
            this.btnAddColumn.Name = "btnAddColumn";
            this.btnAddColumn.Size = new System.Drawing.Size(94, 32);
            this.btnAddColumn.TabIndex = 6;
            this.btnAddColumn.Text = "Add Column";
            this.btnAddColumn.UseVisualStyleBackColor = true;
            //
            // btnRemoveFromArray
            //
            this.btnRemoveFromArray.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRemoveFromArray.Location = new System.Drawing.Point(103, 653);
            this.btnRemoveFromArray.Name = "btnRemoveFromArray";
            this.btnRemoveFromArray.Size = new System.Drawing.Size(173, 33);
            this.btnRemoveFromArray.TabIndex = 3;
            this.btnRemoveFromArray.Text = "Remove From Array";
            this.btnRemoveFromArray.UseVisualStyleBackColor = true;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.tableLayoutPanel1.SetColumnSpan(this.diagramControl1, 2);
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramControl1.Location = new System.Drawing.Point(3, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(994, 594);
            this.diagramControl1.TabIndex = 0;
            this.diagramControl1.Text = "diagramControl1";
            //
            // goWebBrowser1
            //
            this.tableLayoutPanel1.SetColumnSpan(this.goWebBrowser1, 2);
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 603);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(994, 44);
            this.goWebBrowser1.TabIndex = 1;
            this.goWebBrowser1.ZoomFactor = 1D;
            //
            // btnInsertIntoArray
            //
            this.btnInsertIntoArray.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnInsertIntoArray.Location = new System.Drawing.Point(3, 653);
            this.btnInsertIntoArray.Name = "btnInsertIntoArray";
            this.btnInsertIntoArray.Size = new System.Drawing.Size(94, 33);
            this.btnInsertIntoArray.TabIndex = 2;
            this.btnInsertIntoArray.Text = "Insert Into Array";
            this.btnInsertIntoArray.UseVisualStyleBackColor = true;
            //
            // goWebBrowser2
            //
            this.tableLayoutPanel1.SetColumnSpan(this.goWebBrowser2, 2);
            this.goWebBrowser2.CreationProperties = null;
            this.goWebBrowser2.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goWebBrowser2.Location = new System.Drawing.Point(3, 692);
            this.goWebBrowser2.Name = "goWebBrowser2";
            this.goWebBrowser2.Size = new System.Drawing.Size(994, 44);
            this.goWebBrowser2.TabIndex = 11;
            this.goWebBrowser2.ZoomFactor = 1D;
            //
            // goWebBrowser3
            //
            this.tableLayoutPanel1.SetColumnSpan(this.goWebBrowser3, 2);
            this.goWebBrowser3.CreationProperties = null;
            this.goWebBrowser3.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goWebBrowser3.Location = new System.Drawing.Point(3, 780);
            this.goWebBrowser3.Name = "goWebBrowser3";
            this.goWebBrowser3.Size = new System.Drawing.Size(994, 44);
            this.goWebBrowser3.TabIndex = 12;
            this.goWebBrowser3.ZoomFactor = 1D;
            //
            // goWebBrowser4
            //
            this.tableLayoutPanel1.SetColumnSpan(this.goWebBrowser4, 2);
            this.goWebBrowser4.CreationProperties = null;
            this.goWebBrowser4.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goWebBrowser4.Location = new System.Drawing.Point(3, 867);
            this.goWebBrowser4.Name = "goWebBrowser4";
            this.goWebBrowser4.Size = new System.Drawing.Size(994, 130);
            this.goWebBrowser4.TabIndex = 14;
            this.goWebBrowser4.ZoomFactor = 1D;
            //
            // AddRemoveColumnsControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AddRemoveColumnsControl";
            this.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser4)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Button btnSwapColumns;
    private System.Windows.Forms.Button btnRemoveColumn;
    private System.Windows.Forms.Button btnAddColumn;
    private System.Windows.Forms.Button btnRemoveFromArray;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsDemoApp.GoWebBrowser goWebBrowser1;
    private System.Windows.Forms.Button btnInsertIntoArray;
    private WinFormsDemoApp.GoWebBrowser goWebBrowser2;
    private WinFormsDemoApp.GoWebBrowser goWebBrowser3;
    private WinFormsDemoApp.GoWebBrowser goWebBrowser4;
  }
}
