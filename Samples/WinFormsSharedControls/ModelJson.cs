/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace WinFormsSharedControls {
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
    /// Sets the action for clicking the Save button.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public event EventHandler SaveClick {
      add {
        if (saveBtn != null) saveBtn.Click += value;
      }
      remove {
        if (saveBtn != null) saveBtn.Click -= value;
      }
    }

    /// <summary>
    /// Sets the action for clicking the Load button.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public event EventHandler LoadClick {
      add {
        if (loadBtn != null) loadBtn.Click += value;
      }
      remove {
        if (loadBtn != null) loadBtn.Click -= value;
      }
    }

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

    public override string Text {
      get { return label1.Text; }
      set {
        if (value != label1.Text) {
          label1.Text = value;
        }
      }
    }

    private void textBox1_TextChanged(object sender, EventArgs e) {
      if (_JsonText != textBox1.Text) _JsonText = textBox1.Text;
    }
  }
}
