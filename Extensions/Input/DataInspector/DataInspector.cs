/*
*  Copyright (C) 1998-2022 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoDiagram library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Northwoods.Go.Models;

namespace Northwoods.Go.Extensions {
  /// <summary>
  /// An interface for an Inspector element/control.
  /// </summary>
  public interface IInspector {
    /// <summary>
    /// Iterates over the <see cref="Inspector.Rows"/> and updates input values or rebuilds the table.
    /// </summary>
    /// <param name="inspector"></param>
    /// <param name="rebuild"></param>
    public void UpdateUI(Inspector inspector, bool rebuild = false);
    /// <summary>
    /// Iterates over the inputs and updates associated data properties.
    /// </summary>
    /// <param name="inspector"></param>
    public void UpdatePropertiesFromInput(Inspector inspector);
  }

  /// <summary>
  /// This class implements an inspector for model data objects.
  /// </summary>
  /// <remarks>
  /// Example usage of Inspector:
  /// <code language="cs">
  /// var inspector = new Inspector(inspectorControl1, myDiagram, new Inspector.Options {
  ///   IncludesOwnProperties = false,
  ///   Properties = new Dictionary&lt;string, Inspector.PropertyOptions&gt; {
  ///     { "Key", new Inspector.PropertyOptions { ReadOnly = true } },
  ///     { "Comments", new Inspector.PropertyOptions { Show = Inspector.ShowIfNode } },
  ///     { "Chosen", new Inspector.PropertyOptions { Type = "checkbox" } },
  ///     { "State", new Inspector.PropertyOptions { Type = "select", Choices = new int[] { 1, 2, 3, 4 } } }
  ///   }
  /// });
  /// </code>
  /// </remarks>
  /// @category Extension
  public class Inspector {
    private readonly IInspector _Inspector;
    private Diagram _Diagram;
    private object _InspectedObject = null;
    // Inspector options defaults:
    private bool _InspectSelection = true;
    private bool _IncludesOwnProperties = true;
    private Dictionary<string, PropertyOptions> _Properties;
    private Action<string, object, Inspector> _PropertyModified;

    private readonly List<Row> _Rows = new();

    // Private variables used to keep track of internal state
    private Dictionary<string, Row> _InspectedProperties;

    /// <summary>
    /// An object describing options for this <see cref="Inspector"/>.
    /// </summary>
    public class Options {
      /// <summary>
      /// See <see cref="Inspector.InspectSelection"/>.
      /// </summary>
      public bool InspectSelection { get; set; } = true;
      /// <summary>
      /// See <see cref="Inspector.IncludesOwnProperties"/>.
      /// </summary>
      public bool IncludesOwnProperties { get; set; } = true;
      /// <summary>
      /// See <see cref="Inspector.Properties"/>.
      /// </summary>
      public Dictionary<string, PropertyOptions> Properties { get; set; }
      /// <summary>
      /// See <see cref="Inspector.PropertyModified"/>.
      /// </summary>
      public Action<string, object, Inspector> PropertyModified { get; set; }
    }

    /// <summary>
    /// An object describing options for an inspected property.
    /// </summary>
    public class PropertyOptions {
      /// <summary>
      /// The display name for the property.
      /// </summary>
      public string Name { get; set; }
      /// <summary>
      /// A predicate to conditionally show or hide the property.
      /// </summary>
      public Func<object, string, bool> Show { get; set; }
      /// <summary>
      /// Whether the property is read-only.
      /// </summary>
      public bool ReadOnly { get; set; } = false;
      /// <summary>
      /// A string describing the data type. Supported values: "string|int|double|float|bool|arrayofnumber|point|rect|size|spot|margin|select|checkbox".
      /// </summary>
      public string Type { get; set; }
      /// <summary>
      /// A default value for the property.
      /// </summary>
      public object DefaultValue { get; set; }
      /// <summary>
      /// When <see cref="Type"/> == "select", the IList or Array of choices.
      /// </summary>
      public IEnumerable Choices { get; set; }
    }

    /// <summary>
    /// An object representing a row to be displayed in the UI.
    /// </summary>
    public class Row {
      /// <summary>
      /// The property name and label to be displayed for the input.
      /// </summary>
      public string PropertyName { get; set; }
      /// <summary>
      /// The options associated with the property.
      /// </summary>
      public PropertyOptions Options { get; set; }
      /// <summary>
      /// The property value.
      /// </summary>
      public object PropertyValue { get; set; }
    }

    /// <summary>
    /// Constructs an Inspector and sets up properties based on the options provided.
    /// Also sets up change listeners on the Diagram so the Inspector stays up-to-date.
    /// </summary>
    /// <param name="inspector">a reference to the inspector element/control</param>
    /// <param name="diagram">a reference to a Diagram</param>
    /// <param name="options">an optional object describing options for the inspector</param>
    public Inspector(IInspector inspector, Diagram diagram, Options options) {
      _Inspector = inspector;
      _Diagram = diagram;
      // Set properties based on options
      if (options != null) {
        _InspectSelection = options.InspectSelection;
        _IncludesOwnProperties = options.IncludesOwnProperties;
        _Properties = options.Properties;
        _PropertyModified = options.PropertyModified;
      }
      // Prepare change listeners
      _Diagram.ModelChanged += _OnModelChanged;
      if (_InspectSelection) _Diagram.ChangedSelection += _OnChangedSelection;
    }

    // Methods used to keep the Inspector up-to-date
    private void _OnModelChanged(object obj, ChangedEvent e) {
      if (e.IsTransactionFinished) InspectObject();
    }

    private void _OnChangedSelection(object obj, DiagramEvent e) {
      InspectObject();
    }

    /// <summary>
    /// Gets or sets the <see cref="Diagram"/> associated with this Inspector.
    /// </summary>
    public Diagram Diagram {
      get {
        return _Diagram;
      }
      set {
        if (value != _Diagram) {
          // First, unassociate change listeners with current inspected diagram
          _Diagram.ModelChanged -= _OnModelChanged;
          _Diagram.ChangedSelection -= _OnChangedSelection;
          // Now set the diagram and add the necessary change listeners
          _Diagram = value;
          _Diagram.ModelChanged += _OnModelChanged;
          if (_InspectSelection) {
            _Diagram.ChangedSelection += _OnChangedSelection;
          } else {
            InspectObject(null);
          }
        }
      }
    }

    /// <summary>
    /// This read-only property returns the object currently being inspected.
    /// </summary>
    /// <remarks>
    /// To set the inspected object, call <see cref="InspectObject"/>.
    /// </remarks>
    public object InspectedObject {
      get {
        return _InspectedObject;
      }
    }

    /// <summary>
    /// Gets or sets whether the Inspector automatically inspects the associated Diagram's selection.
    /// </summary>
    /// <remarks>
    /// When set to false, the Inspector won't show anything until <see cref="InspectObject"/> is called.
    ///
    /// The default value is true.
    /// </remarks>
    public bool InspectSelection {
      get {
        return _InspectSelection;
      }
      set {
        if (value != _InspectSelection) {
          _InspectSelection = value;
          if (_InspectSelection) {
            _Diagram.ChangedSelection += _OnChangedSelection;
            InspectObject();
          } else {
            _Diagram.ChangedSelection -= _OnChangedSelection;
            InspectObject(null);
          }
        }
      }
    }

    /// <summary>
    /// Gets or sets whether the Inspector includes all properties currently on the inspected object.
    /// </summary>
    /// <remarks>
    /// The default value is true.
    /// </remarks>
    public bool IncludesOwnProperties {
      get {
        return _IncludesOwnProperties;
      }
      set {
        if (value != _IncludesOwnProperties) {
          _IncludesOwnProperties = value;
          InspectObject();
        }
      }
    }

    /// <summary>
    /// Gets or sets the properties that the Inspector will inspect, maybe setting options for those properties.
    /// </summary>
    /// <remarks>
    /// The dictionary should contain string: Object pairs representing propertyName: propertyOptions.
    /// Can be used to include or exclude additional properties.
    ///
    /// The default value is null.
    /// </remarks>
    public Dictionary<string, PropertyOptions> Properties {
      get {
        return _Properties;
      }
      set {
        if (value != _Properties) {
          _Properties = value;
          InspectObject();
        }
      }
    }

    /// <summary>
    /// Gets or sets the function to be called when a property is modified by the Inspector.
    /// </summary>
    /// <remarks>
    /// The first parameter will be the property name, the second will be the new value, and the third will be a reference to this Inspector.
    ///
    /// The default value is null, meaning nothing will be done.
    /// </remarks>
    public Action<string, object, Inspector> PropertyModified {
      get {
        return _PropertyModified;
      }
      set {
        if (value != _PropertyModified) {
          _PropertyModified = value;
        }
      }
    }

    /// <summary>
    /// Gets the rows of properties and their options to be displayed.
    /// </summary>
    public IList<Row> Rows {
      get {
        return _Rows;
      }
    }

    /// <summary>
    /// This predicate function can be used as a value for <see cref="PropertyOptions.Show"/>.
    /// When used, the property will only be shown when inspecting a <see cref="Node"/>.
    /// </summary>
    /// <param name="part">the Part being inspected</param>
    /// <param name="_">unused</param>
    public static bool ShowIfNode(object part, string _) { return part is Node; }

    /// <summary>
    /// This predicate function can be used as a value for <see cref="PropertyOptions.Show"/>.
    /// When used, the property will only be shown when inspecting a <see cref="Link"/>.
    /// </summary>
    /// <param name="part">the Part being inspected</param>
    /// <param name="_">unused</param>
    public static bool ShowIfLink(object part, string _) { return part is Link; }

    /// <summary>
    /// This predicate function can be used as a value for <see cref="PropertyOptions.Show"/>.
    /// When used, the property will only be shown when inspecting a <see cref="Group"/>.
    /// </summary>
    /// <param name="part">the Part being inspected</param>
    /// <param name="_">unused</param>
    public static bool ShowIfGroup(object part, string _) { return part is Group; }

    /// <summary>
    /// Update the UI state of this Inspector with the given object.
    /// </summary>
    /// <remarks>
    /// If passed an object, the Inspector will inspect that object.
    /// If no parameter is supplied, the <see cref="InspectedObject"/> will be set based on the value of <see cref="InspectSelection"/>.
    /// </remarks>
    public void InspectObject(object obj = null) {
      var inspectedObject = obj;
      if (obj == null) {
        if (_InspectSelection) {
          inspectedObject = _Diagram.Selection.FirstOrDefault();
        } else {  // if there is a single inspected object
          inspectedObject = _InspectedObject;
        }
      }

      // if same object, just update existing table
      if (inspectedObject != null && _InspectedObject == inspectedObject) {  
        UpdateInspectorTable(inspectedObject);
        _Inspector.UpdateUI(this);
        return;
      }

      // otherwise, rebuild the table
      _InspectedObject = inspectedObject;
      _Rows.Clear();
      if (inspectedObject == null) {
        _Inspector.UpdateUI(this, true);
        return;
      }

      // use either the Part.Data or the object itself (for SharedData)
      var data = (inspectedObject is Part p) ? p.Data : inspectedObject;
      if (data == null) {
        _Inspector.UpdateUI(this, true);
        return;
      }

      // Build table
      _InspectedProperties = new Dictionary<string, Row>();
      var declaredProperties = _Properties;

      // Go through all the properties passed in to the inspector and add them to the map, if appropriate:
      foreach (var kvp in declaredProperties) {
        var name = kvp.Key;
        var opts = kvp.Value;
        if (!CanShowProperty(name, opts, inspectedObject)) continue;
        var val = FindValue(name, opts, data);
        AddInspectorRow(name, val);
      }
      if (IncludesOwnProperties) {
        foreach (var pi in data.GetType().GetProperties()) {
          if (_InspectedProperties.ContainsKey(pi.Name)) continue;  // already exists
          if (declaredProperties.TryGetValue(pi.Name, out var opts) && !CanShowProperty(pi.Name, opts, inspectedObject)) continue;
          AddInspectorRow(pi.Name, pi.GetValue(data));
        }
      }
      _Inspector.UpdateUI(this, true);
    }

    private void UpdateInspectorTable(object inspectedObject) {
      var data = (inspectedObject is Part p) ? p.Data : inspectedObject;
      if (data != null) {  // update all of the fields
        foreach (var row in Rows) {
          var name = row.PropertyName;
          var opts = row.Options;
          row.PropertyValue = FindValue(name, opts, data);
        }
      }
    }

    /// <summary>
    /// This predicate should be false if the given property should not be shown.
    /// Normally it only checks the value of <see cref="PropertyOptions.Show"/>.
    /// </summary>
    /// <remarks>
    /// The default value is true.
    /// </remarks>
    /// <param name="propertyName">the property name</param>
    /// <param name="opts">the property options</param>
    /// <param name="inspectedObject">the data object</param>
    /// <returns>whether a particular property should be shown in this Inspector</returns>
    public bool CanShowProperty(string propertyName, PropertyOptions opts, object inspectedObject) {
      if (opts?.Show != null) return opts.Show(inspectedObject, propertyName);
      return true;
    }

    /// <summary>
    /// This predicate should be false if the given property should not be editable by the user.
    /// Normally it only checks the value of <see cref="PropertyOptions.ReadOnly"/>.
    /// </summary>
    /// <remarks>
    /// The default value is true.
    /// </remarks>
    /// <param name="propertyName">the property name</param>
    /// <param name="opts">the property options</param>
    /// <param name="inspectedObject">the data object</param>
    /// <returns>whether a particular property should be shown in this Inspector</returns>
    public bool CanEditProperty(string propertyName, PropertyOptions opts, object inspectedObject) {
      if (_Diagram.IsReadOnly || _Diagram.IsModelReadOnly) return false;
      if (inspectedObject == null) return false;
      // assume property values that are functions of Objects cannot be edited
      var data = (inspectedObject is Part p) ? p.Data : inspectedObject;
      if (_Diagram.Model is IGroupModel glm && propertyName == glm.NodeIsGroupProperty) return false; 
      if (opts != null) {
        if (opts.ReadOnly == true) return false;
      }
      return true;
    }

    private static object FindValue(string propName, PropertyOptions propDesc, object data) {
      object val = null;
      if (propDesc != null && propDesc.DefaultValue != null) val = propDesc.DefaultValue;
      var pi = data.GetType().GetProperty(propName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (pi != null && pi.CanRead) {
        try {
          val = pi.GetValue(data, null);
        } catch {
          Trace.Error($"property get error: failed to get property \"{pi.Name}\"");
        }
      }
      return val;
    }

    private void AddInspectorRow(string name, object value) {
      var row = new Row();
      var isDeclared = _Properties.TryGetValue(name, out var opts);
      if (isDeclared) {
        row.PropertyName = opts.Name ?? name;
        row.Options = opts;
      } else {
        row.PropertyName = name;
      }
      row.PropertyValue = value;
      _Rows.Add(row);
      _InspectedProperties[name] = row;
    }

    // convert to color?

    private static List<double> ConvertToArrayOfNumber(string propertyValue) {
      if (propertyValue == "null") return null;
      var split = propertyValue.Split(' ');
      var arr = new List<double>();
      for (var i = 0; i < split.Length; i++) {
        var str = split[i];
        if (str == null) continue;
        arr.Add(double.Parse(str));
      }
      return arr;
    }

    /// <summary>
    /// (undocumented)
    /// </summary>
    [Undocumented]
    public static string ConvertToString(object x) {
      if (x == null) return "null";
      if (x is Point point) return Point.Stringify(point);
      if (x is Size size) return Size.Stringify(size);
      if (x is Rect rect) return Rect.Stringify(rect);
      if (x is Spot spot) return Spot.Stringify(spot);
      if (x is Margin margin) return Margin.Stringify(margin);
      if (typeof(ICollection).IsAssignableFrom(x.GetType())) {
        var str = "";
        foreach (var v in x as ICollection) {
          str += ConvertToString(v) + " ";
        }
        return str.Trim();
      }
      return x.ToString();
    }

    /// <summary>
    /// (undocumented)
    /// </summary>
    [Undocumented]
    public static object ParseValue(PropertyOptions decProp, object value, object oldval) {
      // If it's a bool, or if its previous value was bool,
      // parse the value to be a bool and then update the input.Value to match
      var type = "";
      if (decProp != null && decProp.Type != null) {
        type = decProp.Type;
      }
      if (type == "" || type == "select") {
        if (oldval is bool) type = "bool"; // infer bool
        else if (oldval is int) type = "int";
        else if (oldval is double) type = "double";
        else if (oldval is float) type = "float";
        else if (oldval is Point) type = "point";
        else if (oldval is Size) type = "size";
        else if (oldval is Rect) type = "rect";
        else if (oldval is Spot) type = "spot";
        else if (oldval is Margin) type = "margin";
      }

      // convert to specific type, if needed
      var strVal = value.ToString();
      switch (type) {
        case "bool": value = bool.Parse(strVal); break;
        case "int": value = int.Parse(strVal); break;
        case "double": value = double.Parse(strVal); break;
        case "float": value = float.Parse(strVal); break;
        case "arrayofnumber": value = ConvertToArrayOfNumber(strVal); break;
        case "point": value = Point.Parse(strVal); break;
        case "size": value = Size.Parse(strVal); break;
        case "rect": value = Rect.Parse(strVal); break;
        case "spot": value = Spot.Parse(strVal); break;
        case "margin": value = Margin.Parse(strVal); break;
        case "checkbox": value = bool.Parse(strVal); break;
      }

      return value;
    }
  }
}
