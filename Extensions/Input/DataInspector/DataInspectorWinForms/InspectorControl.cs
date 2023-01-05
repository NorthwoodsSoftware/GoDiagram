/*
*  Copyright (C) 1998-2023 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoDiagram library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Northwoods.Go.Extensions {
  /// <summary>
  /// Implements the <see cref="IInspector"/> interface for the WinForms platform.
  /// </summary>
  public partial class InspectorControl : UserControl, IInspector {
    private readonly Dictionary<string, Control> _PropertyControls = new();

    public InspectorControl() {
      InitializeComponent();
    }

    /// <summary>
    /// Implements <see cref="IInspector.UpdateUI"/> to update the input fields or rebuild the table of inputs.
    /// </summary>
    public void UpdateUI(Inspector inspector, bool rebuild = false) {
      var table = tableLayoutPanel1;
      var rows = inspector.Rows;
      if (!rebuild) {
        foreach (var tr in rows) {
          var name = tr.PropertyName;
          var val = tr.PropertyValue;
          var opts = tr.Options;
          if (_PropertyControls.TryGetValue(name, out var input)) {
            if (input is TextBox tb) tb.Text = Inspector.ConvertToString(val);
            else if (input is CheckBox cb) cb.Checked = (bool)val;
            else if (input is ComboBox cbb) cbb.SelectedItem = val;
            // other input types?
          }
        }
      } else {
        // clear out old table
        table.Controls.Clear();
        table.SuspendLayout();
        SuspendLayout();
        table.RowCount = 0;
        table.RowStyles.Clear();
        _PropertyControls.Clear();
        if (!rows.Any()) table.Visible = false;
        // now refill it with the new rows
        for (var i = 0; i < rows.Count; i++) {
          var tr = rows[i];
          BuildPropertyRow(table, tr, inspector);
        }
        table.ResumeLayout();
        ResumeLayout();
      }
      table.Visible = rows.Any();
    }

    /// <summary>
    /// Implements <see cref="IInspector.UpdatePropertiesFromInput"/> to update data properties.
    /// </summary>
    public void UpdatePropertiesFromInput(Inspector inspector) {
      var rows = inspector.Rows;
      var data = inspector.InspectedObject is Part p ? p.Data : inspector.InspectedObject;
      var diagram = inspector.Diagram;
      if (data == null) return;  // must not try to update data when there's no data!

      diagram.StartTransaction("set all properties");
      foreach (var tr in rows) {
        var name = tr.PropertyName;
        var opts = tr.Options;

        // don't update "ReadOnly" data properties
        if (!inspector.CanEditProperty(name, opts, inspector.InspectedObject)) continue;

        if (!_PropertyControls.TryGetValue(name, out var input)) continue;

        object value = null;
        if (input is TextBox tb) value = tb.Text;
        else if (input is CheckBox cb) value = cb.Checked;
        else if (input is ComboBox cbb) value = cbb.SelectedItem;

        object oldval = null;
        var pi = data.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (pi != null && pi.CanRead) {
          try {
            oldval = pi.GetValue(data, null);
          } catch {
            Trace.Error($"property get error: failed to get property \"{pi.Name}\"");
          }
        }

        value = Inspector.ParseValue(opts, value, oldval);

        // in case parsed to be different, such as in the case of bool values,
        // the value shown should match the actual value
        if (input is TextBox tb2) tb2.Text = value.ToString();
        else if (input is CheckBox cb2) cb2.Checked = (bool)value;
        else if (input is ComboBox cbb2) cbb2.SelectedItem = value;

        // modify the data object in an undo-able fashion
        diagram.Model.Set(data, name, value);

        // notify any listener
        inspector.PropertyModified?.Invoke(name, value, inspector);
      }
      diagram.CommitTransaction("set all properties");
    }

    private void BuildPropertyRow(TableLayoutPanel table, Inspector.Row row, Inspector inspector) {
      table.RowCount++;
      table.RowStyles.Add(new RowStyle());

      // create the label
      var label = new Label {
        AutoSize = true,
        Anchor = AnchorStyles.Left,
        Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, GraphicsUnit.Point),
        ForeColor = SystemColors.ControlLightLight,
        Text = row.PropertyName,
        TextAlign = ContentAlignment.MiddleLeft
      };
      table.Controls.Add(label, 0, table.RowCount - 1);

      // create the input control
      if (row.Options?.Type == "select") {
        var comboBox = new ComboBox {
          Enabled = inspector.CanEditProperty(row.PropertyName, row.Options, inspector.InspectedObject),
          DataSource = row.Options.Choices,
          SelectedItem = row.PropertyValue,
          DropDownStyle = ComboBoxStyle.DropDownList,
          Width = 160
        };
        comboBox.SelectedIndexChanged += (e, obj) => {
          row.PropertyValue = comboBox.SelectedItem;
          UpdatePropertiesFromInput(inspector);
        };
        table.Controls.Add(comboBox, 1, table.RowCount - 1);
        _PropertyControls.Add(row.PropertyName, comboBox);
      } else if (row.Options?.Type == "checkbox") {
        var checkBox = new CheckBox {
          Enabled = inspector.CanEditProperty(row.PropertyName, row.Options, inspector.InspectedObject),
          Checked = (bool)row.PropertyValue
        };
        checkBox.CheckedChanged += (e, obj) => {
          row.PropertyValue = checkBox.Checked;
          UpdatePropertiesFromInput(inspector);
        };
        table.Controls.Add(checkBox, 1, table.RowCount - 1);
        _PropertyControls.Add(row.PropertyName, checkBox);
      } else {
        var textBox = new TextBox {
          Enabled = inspector.CanEditProperty(row.PropertyName, row.Options, inspector.InspectedObject),
          Text = Inspector.ConvertToString(row.PropertyValue),
          Width = 160
        };
        textBox.Leave += (e, obj) => {
          row.PropertyValue = textBox.Text;
          UpdatePropertiesFromInput(inspector);
        };
        table.Controls.Add(textBox, 1, table.RowCount - 1);
        _PropertyControls.Add(row.PropertyName, textBox);
      }
    }
  }
}
