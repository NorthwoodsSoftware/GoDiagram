using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace WinFormsSharedControls {
  [ToolboxItem(true)]
  public partial class SaveLoadModel : UserControl {
    private string _ModelJson;

    public SaveLoadModel() {
      InitializeComponent();

      _ModelJson = textBox1.Text;
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
    public string ModelJson {
      get { return _ModelJson; }
      set {
        if (value != _ModelJson) {
          _ModelJson = value;
          textBox1.Text = _ModelJson;
        }
      }
    }
  }
}
