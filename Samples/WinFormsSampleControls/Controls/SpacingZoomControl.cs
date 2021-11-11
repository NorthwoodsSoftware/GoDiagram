using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.SpacingZoom {
  [ToolboxItem(false)]
  public partial class SpacingZoomControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    
    public SpacingZoomControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      checkBxYAxis.CheckedChanged += (e, obj) => OnIsYSpacedToggled();

      goWebBrowser1.Html = @"
        <p>
      Click in the diagram and then try zooming in and out of the diagram by using control-mouse-wheel.
      If you don't want to hold down the control key, click the mouse wheel button in the diagram to
      toggle between mouse wheel events zooming instead of scrolling.
        </p>
        <p>
      This diagram uses a custom <a>CommandHandler</a> to replace the standard zooming behavior.
      The <a>CommandHandler.decreaseZoom</a>, <a>CommandHandler.increaseZoom</a>, and
      <a>CommandHandler.resetZoom</a> commands no longer change the <a>Diagram.scale</a>.
      Instead they change the effective <a>Part.location</a> for all of the <a>Node</a>s by changing
      the value of the conversion function that is getting the location from the ""loc"" property on the node data.
        </p>
        <p>
      As the value of SpacingCommandHandler.space changes, all of the Bindings on ""loc"" are re-evaluated,
      causing the nodes to get new locations.The value of the ""loc"" data property remains unchanged by the Binding.
      However the TwoWay Binding does cause the ""loc"" data property to be modified when the user drags a node.
        </p>
        <p>
      The conversion functions also depend on the SpacingCommandHandler.isYSpaced property, which can be toggled by the checkbox.
      When false the conversion functions do not space along the Y axis, but only along the X axis.
        </p>
        <p>
      Because the conversion functions are dependent on the particular Diagram,
      and because the node template depends on the conversion functions,
      the template cannot be used by other Diagrams.
        </p>
        <p>
      The SpacingCommandHandler.space property is duplicated on the<a> Model.modelData </a> object, both so that the information
      is saved in the model as well as to support undo/ redo.
        </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagarm properties
      myDiagram.CommandHandler = new SpacingZoomCommandHandler();
      myDiagram.ModelChanged += (_, e) => {
        if (e.IsTransactionFinished) {
          (myDiagram.CommandHandler as SpacingZoomCommandHandler).Space = (myDiagram.Model.SharedData as SharedData).Space;
          // Update spacing factor on label
          lblSpacingFactor.Text = "Scale Factor: " + (myDiagram.CommandHandler as SpacingZoomCommandHandler).Space;
        }
      };
      myDiagram.UndoManager.IsEnabled = true; // enable undo and redo

      // Define a simple Node template that cannot be shared with other Diagrams,
      // because of the use of the Node.Location Binding conversion functions.
      // The SpacingCommandHandler also assumes the Node.Location is bound to the data property named "loc".
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance).Bind(  // the Shape will go around the TextBlock
          new Binding("Location", "Loc", SpacedLocationParse).MakeTwoWay(SpacedLocationStringify)
        ).Add(
          new Shape {
            Figure = "RoundedRectangle",
            StrokeWidth = 0
          }.Bind(
            // Shape.Fill is bound to Node.Data.Color
            new Binding("Fill", "Color")
          ),
          new TextBlock {
            Margin = 8
          }.Bind( // some room around the text
                  // TextBlock.Text is bound to Node.Data.Key
            new Binding("Text", "Key")
          )
        );

      // but use the default Link template, by not setting Diagram.LinkTemplate

      // create the model data that will be represented by Nodes and Links
      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alpha", Color = "lightblue", Loc = "0 0" },
          new NodeData { Key = "Beta", Color = "orange", Loc = "100 0" },
          new NodeData { Key = "Gamma", Color = "lightgreen", Loc = "0 100" },
          new NodeData { Key = "Delta", Color = "pink", Loc = "100 100" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "Alpha", To = "Beta" },
          new LinkData { From = "Alpha", To = "Gamma" },
          new LinkData { From = "Beta", To = "Beta" },
          new LinkData { From = "Gamma", To = "Delta" },
          new LinkData { From = "Delta", To = "Alpha" }
        }
      };
      myDiagram.Model.SharedData = new SharedData {
        Space = 1.0
      };

      // conversion functions, these only work with myDiagram, assuming it uses a SpacingCommandHandler

      object SpacedLocationParse(object str, object _) {
        var cmd = myDiagram.CommandHandler as SpacingZoomCommandHandler;
        if (cmd == null) throw new Exception("not using SpacingCommandHandler");
        var pt = Point.Parse(str as string);
        pt.X = (pt.X - cmd.SpaceCenter.X) * cmd.Space + cmd.SpaceCenter.X;
        if (cmd.IsYSpaced) {
          pt.Y = (pt.Y - cmd.SpaceCenter.Y) * cmd.Space + cmd.SpaceCenter.Y;
        }
        return pt;
      }

      string SpacedLocationStringify(object ptAsObj, object dataAsObj, IModel _) {
        var cmd = myDiagram.CommandHandler as SpacingZoomCommandHandler;
        if (!cmd.IsUpdating) {
          var pt = (Point)ptAsObj;
          pt.X = (pt.X - cmd.SpaceCenter.X) / cmd.Space + cmd.SpaceCenter.X;
          if (cmd.IsYSpaced) {
            pt.Y = (pt.Y - cmd.SpaceCenter.Y) / cmd.Space + cmd.SpaceCenter.Y;
          }
          return Point.Stringify(pt);
        } else {
          return (dataAsObj as NodeData).Loc;
        }
      }

    }

    private void OnIsYSpacedToggled() {
      var cHandler = myDiagram.CommandHandler as SpacingZoomCommandHandler;
      cHandler.IsYSpaced = !cHandler.IsYSpaced;
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, SharedData, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
    public string Loc { get; set; }
  }

  public class LinkData : Model.LinkData { }

  public class SharedData {
    public double Space { get; set; }
  }

  // extend CommandHandler
  public class SpacingZoomCommandHandler : CommandHandler {

    private double _Space; // replaces Diagram.Scale, also copied to/from Model.SharedData.Space
    private Point _SpaceCenter; // not currently used -- should this be saved on SharedData too?
    private bool _IsYSpaced; // scale Y along with X?  This option is just for demonstration purposes.

    public bool IsUpdating { get; private set; }

    public double Space {
      get {
        return _Space;
      }
      set {
        if (value != _Space) {
          _Space = value;
          var diagram = Diagram;
          if (diagram != null) {  // store in model too, and support undo
            diagram.Model.Set(diagram.Model.SharedData, "Space", value);
          }
          UpdateAllLocations();
          // update the page showing the current value
        }
      }
    }
    public Point SpaceCenter {
      get {
        return _SpaceCenter;
      }
      set {
        if (!value.Equals(_SpaceCenter)) {
          _SpaceCenter = value;
        }
      }
    }
    public bool IsYSpaced {
      get {
        return _IsYSpaced;
      }
      set {
        if (value != _IsYSpaced) {
          _IsYSpaced = value;
          UpdateAllLocations();
        }
      }
    }

    // If the spacing or isYSpaced properties change value,
    // we need to update the effective locations of all nodes.
    // Assume Node.Location is data bound to "Loc" property.
    private void UpdateAllLocations() {
      var diagram = Diagram;
      if (diagram == null) return;
      IsUpdating = true;
      diagram.SkipsUndoManager = true;
      diagram.StartTransaction("respace nodes");
      foreach (var p in diagram.Parts) {
        p.UpdateTargetBindings("Loc");
      }
      foreach (var n in diagram.Nodes) {
        n.UpdateTargetBindings("Loc");
      }
      diagram.CommitTransaction("respace nodes");
      diagram.SkipsUndoManager = false;
      IsUpdating = false;
    }

    public SpacingZoomCommandHandler() : base() {
      _Space = 1.0;
      _SpaceCenter = new Point(0, 0);
      _IsYSpaced = true;
      IsUpdating = false;
    }

    public override void DecreaseZoom(double factor = -1) {
      if (factor <= 0) factor = 1.0 / ZoomFactor;
      SetSpace(Space * factor);
    }
    public override bool CanDecreaseZoom(double factor = -1) {
      if (factor <= 0) factor = 1.0 / ZoomFactor;
      return CheckSpace(Space * factor);
    }
    public override void IncreaseZoom(double factor = -1) {
      if (factor <= 0) factor = 1.0 / ZoomFactor;
      SetSpace(Space / factor);
    }
    public override bool CanIncreaseZoom(double factor = -1) {
      if (factor <= 0) factor = 1.0 / ZoomFactor;
      return CheckSpace(Space / factor);
    }
    public override void ResetZoom(double newspace = 1.0) {
      SetSpace(newspace);
    }
    public override bool CanResetZoom(double newspace = -1) {
      return base.CanResetZoom(newspace);
    }

    // helpers

    // actually set a new value for _Space
    private void SetSpace(double s) {
      Space = Math.Max(0.1, Math.Min(10.0, s));
    }

    // validity check for _Space
    private bool CheckSpace(double s) {
      return 0.1 <= s && s <= 10.0;
    }
  }

}
