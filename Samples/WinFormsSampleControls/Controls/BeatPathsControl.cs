using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;
using System.ComponentModel;

namespace WinFormsSampleControls.BeatPaths {
  [ToolboxItem(false)]
  public partial class BeatPathsControl : System.Windows.Forms.UserControl {
    private Diagram _Diagram;
    public BeatPathsControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      goWebBrowser1.Html = @"
        <p>
          This sample demonstrates reading JSON data describing the relative rankings of NFL teams
          during the 2015 season and generating a diagram from that data.
          The ranking information came from beatgraphs.com.
        </p>
        <p>
          The JSON data is basically just a list of relationships.
          Unlike most model data, there are no elements describing the nodes --
          the node definitions are implicit in the references from the links.
          Hence the <a>Diagram.Model</a> has <a>GraphLinksModel.ArchetypeNodeData</a> set to an object.
        </p>
        <p>
          The node template uses the <b>ConvertKeyImage</b> function to convert the team name
          into a URI referring to an image on our web site.
        </p>
      ";
    }

    private void Setup() {
      _Diagram = diagramControl1.Diagram;

      // diagram properties

      // automatically scale the diagram to fit the viewport's size
      _Diagram.InitialAutoScale = AutoScaleType.Uniform;
      // disable user copying of parts
      _Diagram.AllowCopy = false;
      // position all of the nodes and route all of the links
      _Diagram.Layout =
        new LayeredDigraphLayout {
          Direction = 90,
          LayerSpacing = 10,
          ColumnSpacing = 15,
          SetsPortSpots = false
        };

      // replace the default Node template in the nodeTemplateMap
      _Diagram.NodeTemplate =
        new Node(PanelLayoutVertical.Instance).Add(  // the whole node panel
          new TextBlock().Bind(  // the text label
            new Binding("Text", "Key")),
          new Picture  // the icon showing the logo
                       // You should set the desiredSize (or width and height)
                       // whenever you know what size the Picture should be.
            { DesiredSize = new Size(75, 50) }.Bind(
            new Binding("Source", "Key", ConvertKeyImage))
        );

      // replace the default Link template in the linkTemplateMap
      _Diagram.LinkTemplate =
        new Link  // the whole link panel
          { Curve = LinkCurve.Bezier, ToShortLength = 2 }.Add(
          new Shape  // the link shape
            { StrokeWidth = 1.5 },
          new Shape  // the arrowhead
            { ToArrow = "Standard", Stroke = null }
        );

      // the array of link data Objects = the relationships between the nodes
      var linkDataSource = new List<LinkData> {
        new LinkData { From = "CAR", To = "ARI" },
        new LinkData { From = "ARI", To = "CIN" },
        new LinkData { From = "ARI", To = "GB" },
        new LinkData { From = "DEN", To = "GB" },
        new LinkData { From = "DEN", To = "CIN" },
        new LinkData { From = "DEN", To = "NE" },
        new LinkData { From = "GB", To = "WAS" },
        new LinkData { From = "WAS", To = "STL" },
        new LinkData { From = "CIN", To = "STL" },
        new LinkData { From = "STL", To = "SEA" },
        new LinkData { From = "SEA", To = "SF" },
        new LinkData { From = "SEA", To = "MIN" },
        new LinkData { From = "NE", To = "NYG" },
        new LinkData { From = "NE", To = "KC" },
        new LinkData { From = "MIN", To = "DET" },
        new LinkData { From = "MIN", To = "KC" },
        new LinkData { From = "KC", To = "HOU" },
        new LinkData { From = "KC", To = "BUF" },
        new LinkData { From = "KC", To = "BAL" },
        new LinkData { From = "KC", To = "OAK" },
        new LinkData { From = "BUF", To = "NYJ" },
        new LinkData { From = "BAL", To = "PIT" },
        new LinkData { From = "DET", To = "NO" },
        new LinkData { From = "DET", To = "PHI" },
        new LinkData { From = "DET", To = "CHI" },
        new LinkData { From = "HOU", To = "JAC" },
        new LinkData { From = "HOU", To = "TEN" },
        new LinkData { From = "PIT", To = "IND" },
        new LinkData { From = "PIT", To = "SD" },
        new LinkData { From = "OAK", To = "NYJ" },
        new LinkData { From = "OAK", To = "SD" },
        new LinkData { From = "NO", To = "ATL" },
        new LinkData { From = "NO", To = "NYG" },
        new LinkData { From = "PHI", To = "NYG" },
        new LinkData { From = "CHI", To = "TB" },
        new LinkData { From = "NYJ", To = "IND" },
        new LinkData { From = "NYJ", To = "CLE" },
        new LinkData { From = "IND", To = "TB" },
        new LinkData { From = "TB", To = "ATL" },
        new LinkData { From = "SD", To = "CLE" },
        new LinkData { From = "ATL", To = "DAL" },
        new LinkData { From = "ATL", To = "JAC" },
        new LinkData { From = "CLE", To = "TEN" },
        new LinkData { From = "DAL", To = "MIA" },
        new LinkData { From = "MIA", To = "TEN" }
      };

      // create the model and assign it to the Diagram
      _Diagram.Model = new Model {
        // automatically create node data objects for each "from" or "to" reference
        // (set this property before setting the linkDataSource)
        ArchetypeNodeData = new NodeData(),
        // process all of the link relationship data
        LinkDataSource = linkDataSource
      };

      string ConvertKeyImage(object data, object obj) {
        var key = data as string;
        if (key == null) key = "NE";
        return "https://www.nwoods.com/go/beatpaths/" + key + "_logo-75x50.png";
      }
    }
    // define the model data
    public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
    public class NodeData : Model.NodeData { }
    public class LinkData : Model.LinkData { }

    
  }
}
