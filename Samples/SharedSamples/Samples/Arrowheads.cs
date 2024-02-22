/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.Arrowheads {
  public partial class Arrowheads : DemoControl {
    private Diagram _Diagram;

    public Arrowheads() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Samples.Arrowheads.md");
    }

    private void Setup() {
      _Diagram.IsReadOnly = true; // don't allow move or delete
      _Diagram.Layout = new CircularLayout {
        Radius = 100,  // minimum radius
        Spacing = 0,   // circular nodes will touch each other
        NodeDiameterFormula = CircularNodeDiameterFormula.Circular,  // assume nodes are circular
        StartAngle = 270  // first node will be at top
      };
      _Diagram.LayoutCompleted += (e, _) => {
        // now that the CircularLayout has finished, we know where its center is
        var cntr = _Diagram.FindNodeForKey("Center");
        cntr.Location = (_Diagram.Layout as CircularLayout).ActualCenter;
      };

      // construct a shared radial gradient brush
      var radBrush = new Brush(new RadialGradientPaint(new Dictionary<float, string> {
          { 0, "#550266" },
          { 1, "#80418C" }
        }
      ));

      // node template
      // these are the nodes that are in a circle
      _Diagram.NodeTemplate =
        new Node {
          LocationSpot = Spot.Center,
          Click = ShowArrowInfo,  // defined below
          ToolTip =  // define a tooltip for each link that displays its information
            Builder.Make<Adornment>("ToolTip").Add(
              new TextBlock { Margin = 4 }.Bind(
                new Binding("Text", "", InfoString).OfElement())
            )
        }.Add(
          new Shape {
            Figure = "Circle",
            DesiredSize = new Northwoods.Go.Size(28, 28),
            // TEMPORARY
            //Fill = radBrush,
            // END TEMPORARY
            Fill = "purple",
            StrokeWidth = 0,
            Stroke = (Brush)null
          } // no outline
        );

      // use a special template for the center node
      _Diagram.NodeTemplateMap.Add("Center",
        new Node(PanelType.Spot) {
          Selectable = false,
          IsLayoutPositioned = false,  // the Diagram.Layout will not position this node
          LocationSpot = Spot.Center
        }.Add(
          new Shape {
            Figure = "Circle",
            // TEMPORARY
            //Fill = radBrush,
            // END TEMPORARY
            Fill = "purple",
            StrokeWidth = 0,
            Stroke = (Brush)null,
            DesiredSize = new Northwoods.Go.Size(200, 200)
          }, // no outline
          new TextBlock {
            Text = "Arrowheads",
            Margin = 1,
            Stroke = "white",
            Font = new Font("Segoe UI", 14, Northwoods.Go.FontWeight.Bold)
          }
        )
      );

      // all Links have both "toArrow" and "fromArrow" Shapes,
      // where both arrow properties are data bound
      _Diagram.LinkTemplate =
        new Link { // the whole link panel
          Routing = LinkRouting.Normal,
          Click = ShowArrowInfo,
          ToolTip =  // define a tooltip for each link that displays its information
            Builder.Make<Adornment>("ToolTip").Add(
              new TextBlock { Margin = 4 }.Bind(
                new Binding("Text", "", InfoString).OfElement()
              )
            ) as Adornment
        }.Add(
          new Shape { // the link shape
            // the first element is assumed to be main Element = as if isPanelMain were true
            Stroke = "gray",
            StrokeWidth = 2
          },
          new Shape { // the "from" arrowhead
            Scale = 2,
            Fill = "#D4B52C"
          }.Bind(
            new Binding("FromArrow", "FromArrow")
          ),
          new Shape { // the "to" arrowhead
            Scale = 2,
            Fill = "#D4B52C"
          }.Bind(
            new Binding("ToArrow", "ToArrow")
          )
        );

      // collect all of the predefined arrowhead names
      var arrowheads = Shape.GetArrowheadGeometries().Keys.ToList();
      if (arrowheads.Count % 2 == 1) arrowheads.Add("");  // make sure there's an even number

      // create all of the link data, two arrowheads per link
      var linkdata = new List<LinkData>();
      var i = 0;
      for (var j = 0; j < arrowheads.Count; j += 2) {
        linkdata.Add(
          new LinkData {
            From = "Center",
            To = (i++).ToString(),
            ToArrow = arrowheads[j],
            FromArrow = arrowheads[j + 1]
          }
        );
      }

      _Diagram.Model = new Model {
        // this gets copied automatically when there's a link data reference to a new node key
        // and is then added to the nodeDataSource
        ArchetypeNodeData = new NodeData(),
        // the node array starts with just the special Center node
        NodeDataSource = new List<NodeData> {
          new NodeData { Category = "Center", Key = "Center" }
        },
        // the link array was created above
        LinkDataSource = linkdata
      };

      // functions
      // a conversion function used to get arrowhead information for a Part
      string InfoString(object obj, object _) {
        var objAsGraphObject = obj as GraphObject;
        var part = objAsGraphObject.Part;
        if (part is Adornment p) {
          part = p.AdornedPart;
        }
        var msg = "";
        if (part is Link) {
          var link = part;
          var data = link.Data as LinkData;
          msg = "toArrow: " + data.ToArrow + "\nfromArrow: " + data.FromArrow;
        } else if (part is Node) {
          var node = part as Node;
          var link = node.LinksConnected.First();
          var data = link.Data as LinkData;
          msg = "toArrow: " + data.ToArrow + "\nfromArrow: " + data.FromArrow;
        }
        return msg;
      }

      // a GraphObject.Click event handler to show arrowhead information
      void ShowArrowInfo(InputEvent e, GraphObject obj) {
        var msg = InfoString(obj, null);
        if (msg != null) {
          arrowTb.Text = msg;
        }
      }
    }
  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData { }
  public class LinkData : Model.LinkData {
    public string ToArrow { get; set; }
    public string FromArrow { get; set; }
  }
}
