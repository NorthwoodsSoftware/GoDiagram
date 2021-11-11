
namespace WinFormsSampleControls.ScrollModes {
  partial class ScrollModesControl {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScrollModesControl));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.checkBxScroll = new System.Windows.Forms.CheckBox();
            this.txtScroll = new System.Windows.Forms.TextBox();
            this.checkBxPosComp = new System.Windows.Forms.CheckBox();
            this.txtPosComp = new System.Windows.Forms.TextBox();
            this.txtScaleComp = new System.Windows.Forms.TextBox();
            this.goWebBrowser1 = new WinFormsSharedControls.GoWebBrowser();
            this.checkBxScaleComp = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBxScroll, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtScroll, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.checkBxPosComp, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtPosComp, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtScaleComp, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.checkBxScaleComp, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // diagramControl1
            // 
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(994, 494);
            this.diagramControl1.TabIndex = 0;
            this.diagramControl1.Text = "diagramControl1";
            // 
            // checkBxScroll
            // 
            this.checkBxScroll.AutoSize = true;
            this.checkBxScroll.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBxScroll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBxScroll.Location = new System.Drawing.Point(3, 503);
            this.checkBxScroll.Name = "checkBxScroll";
            this.checkBxScroll.Size = new System.Drawing.Size(994, 19);
            this.checkBxScroll.TabIndex = 1;
            this.checkBxScroll.Text = "Enable Infinite Scrolling, setting Diagram.scrollMode";
            this.checkBxScroll.UseVisualStyleBackColor = true;
            // 
            // txtScroll
            // 
            this.txtScroll.BackColor = System.Drawing.Color.Black;
            this.txtScroll.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtScroll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtScroll.ForeColor = System.Drawing.Color.White;
            this.txtScroll.Location = new System.Drawing.Point(3, 528);
            this.txtScroll.Multiline = true;
            this.txtScroll.Name = "txtScroll";
            this.txtScroll.Size = new System.Drawing.Size(994, 64);
            this.txtScroll.TabIndex = 2;
            this.txtScroll.Text = "\r\n myDiagram.scrollMode = checked ? go.Diagram.InfiniteScroll : go.Diagram.Docume" +
    "ntScroll;";
            // 
            // checkBxPosComp
            // 
            this.checkBxPosComp.AutoSize = true;
            this.checkBxPosComp.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBxPosComp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBxPosComp.Location = new System.Drawing.Point(3, 598);
            this.checkBxPosComp.Name = "checkBxPosComp";
            this.checkBxPosComp.Size = new System.Drawing.Size(994, 19);
            this.checkBxPosComp.TabIndex = 3;
            this.checkBxPosComp.Text = "Enable Diagram.positionComputation function";
            this.checkBxPosComp.UseVisualStyleBackColor = true;
            // 
            // txtPosComp
            // 
            this.txtPosComp.BackColor = System.Drawing.Color.Black;
            this.txtPosComp.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtPosComp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtPosComp.ForeColor = System.Drawing.Color.White;
            this.txtPosComp.Location = new System.Drawing.Point(3, 623);
            this.txtPosComp.Multiline = true;
            this.txtPosComp.Name = "txtPosComp";
            this.txtPosComp.Size = new System.Drawing.Size(994, 164);
            this.txtPosComp.TabIndex = 4;
            this.txtPosComp.Text = resources.GetString("txtPosComp.Text");
            // 
            // txtScaleComp
            // 
            this.txtScaleComp.BackColor = System.Drawing.Color.Black;
            this.txtScaleComp.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtScaleComp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtScaleComp.ForeColor = System.Drawing.Color.White;
            this.txtScaleComp.Location = new System.Drawing.Point(3, 818);
            this.txtScaleComp.Multiline = true;
            this.txtScaleComp.Name = "txtScaleComp";
            this.txtScaleComp.Size = new System.Drawing.Size(994, 264);
            this.txtScaleComp.TabIndex = 6;
            this.txtScaleComp.Text = resources.GetString("txtScaleComp.Text");
            // 
            // goWebBrowser1
            // 
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 1088);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(994, 94);
            this.goWebBrowser1.TabIndex = 7;
            this.goWebBrowser1.ZoomFactor = 1D;
            // 
            // checkBxScaleComp
            // 
            this.checkBxScaleComp.AutoSize = true;
            this.checkBxScaleComp.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBxScaleComp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBxScaleComp.Location = new System.Drawing.Point(3, 793);
            this.checkBxScaleComp.Name = "checkBxScaleComp";
            this.checkBxScaleComp.Size = new System.Drawing.Size(994, 19);
            this.checkBxScaleComp.TabIndex = 8;
            this.checkBxScaleComp.Text = "Enable Diagram.scaleComputation function";
            this.checkBxScaleComp.UseVisualStyleBackColor = true;
            // 
            // ScrollModesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ScrollModesControl";
            this.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private System.Windows.Forms.CheckBox checkBxScroll;
    private System.Windows.Forms.TextBox txtScroll;
    private System.Windows.Forms.CheckBox checkBxPosComp;
    private System.Windows.Forms.TextBox txtPosComp;
    private System.Windows.Forms.TextBox txtScaleComp;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser1;
    private System.Windows.Forms.CheckBox checkBxScaleComp;
  }
}
