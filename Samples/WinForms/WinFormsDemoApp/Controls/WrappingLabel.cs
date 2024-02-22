/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsDemoApp {
  [ToolboxItem(true)]
  public partial class WrappingLabel : Label {
    public WrappingLabel() {
      AutoSize = false;
    }

    protected override void OnResize(EventArgs e) {
      base.OnResize(e);
      FitContent();
    }

    protected override void OnTextChanged(EventArgs e) {
      base.OnTextChanged(e);
      FitContent();
    }

    protected virtual void FitContent() {
      var size = GetPreferredSize(new Size(Width, 0));
      Height = size.Height;
    }

    [DefaultValue(false), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override bool AutoSize {
      get { return base.AutoSize; }
      set { base.AutoSize = value; }
    }
  }
}
