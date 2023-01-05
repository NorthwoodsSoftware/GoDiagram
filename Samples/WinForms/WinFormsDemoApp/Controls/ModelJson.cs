/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace WinFormsDemoApp {
  [ToolboxItem(true)]
  public partial class ModelJson : UserControl {
    private bool _CanSaveLoad = true;
    private string _JsonText;

    public ModelJson() {
      InitializeComponent();

      _JsonText = textBox1.Text;
    }

    [
      Description("Whether user can save/load JSON text")
    ]
    public bool CanSaveLoad {
      get { return _CanSaveLoad; }
      set {
        _CanSaveLoad = value;
        saveBtn.Visible = _CanSaveLoad;
        loadBtn.Visible = _CanSaveLoad;
        textBox1.ReadOnly = !_CanSaveLoad;
        textBox1.BackColor = _CanSaveLoad ? System.Drawing.SystemColors.ControlLightLight : System.Drawing.SystemColors.Control;
        tableLayoutPanel1.SetColumn(label1, _CanSaveLoad ? 2 : 0);
        Invalidate();
      }
    }

    /// <summary>
    /// Gets or sets the function to run upon clicking the save button.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Action SaveClick { get; set; }

    /// <summary>
    /// Gets or sets the function to run upon clicking the load button.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Action LoadClick { get; set; }

    /// <summary>
    /// Gets or sets the model JSON.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string JsonText {
      get { return _JsonText; }
      set {
        if (value != _JsonText) {
          _JsonText = value;
          textBox1.Text = _JsonText;
        }
      }
    }

    private void saveBtn_Click(object sender, EventArgs e) {
      SaveClick?.Invoke();
    }

    private void loadBtn_Click(object sender, EventArgs e) {
      LoadClick?.Invoke();
    }

    private void textBox1_TextChanged(object sender, EventArgs e) {
      if (_JsonText != textBox1.Text) _JsonText = textBox1.Text;
    }
  }
}
