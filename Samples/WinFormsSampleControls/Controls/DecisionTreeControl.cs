using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.DecisionTree {
  [ToolboxItem(false)]
  public partial class DecisionTreeControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public DecisionTreeControl() 
    {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      goWebBrowser1.Html = @"
        <p>
          This sample allows a user to make progressive decisions about personality types.
        </p>
        <p>
          There are two kinds of nodes, so there are two node templates (""decision"" and ""personality"")
          that determine the appearance and behavior of each <a>Node</a>.
        </p>
        <p>
         The ""decision"" template displays the abbreviated personality type and two choice buttons, all surrounded by a figure.
          Clicking a button will either expand the choice or will collapse all nodes leading from that choice.
        </p>
        <p>
          The ""personality"" template displays the personality descriptions, as the ""leaf"" nodes for the tree.
        </p>
      ";
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.InitialContentAlignment = Spot.Left;
      myDiagram.AllowSelect = false; // the user cannot select any part
      myDiagram.Layout = new TreeLayout(); // tree layout for decision tree

      // custom behavior for expanding/collapsing half of the subtree from a node
      void ButtonExpandCollapse(InputEvent e, GraphObject port) {
        var node = port.Part as Node;
        node.Diagram.StartTransaction("expand/collapse");
        var portid = port.PortId;
        foreach (var l in node.FindLinksOutOf(portid)) {
          if (l.Visible) {
            // collapse whole subtree recursively
            CollapseTree(node, portid);
          } else {
            // only expands immediate children and their links
            l.Visible = true;
            var n = l.GetOtherNode(node);
            if (n != null) {
              n.Location = node.GetDocumentPoint(Spot.TopRight);
              n.Visible = true;
            }
          }
        }
        myDiagram.ToolManager.HideToolTip();
        node.Diagram.CommitTransaction("expand/collapse");
      }

      // delegate for ButtonExpandCollapse
      Action<InputEvent, GraphObject> ButtonExpandCollapseDelegate = ButtonExpandCollapse;

      // recursive function for collapsing complete subtree
      void CollapseTree(Node node, string portid) {
        foreach (var l in node.FindLinksOutOf(portid)) {
          l.Visible = false;
          var n = l.GetOtherNode(node);
          if (n != null) {
            n.Visible = false;
            CollapseTree(n, null);  // null means all links, not just for a particular portId
          }
        }
      }

      // get the text for the tooltip from the data on the object being hovered over
      string TooltipTextConverter(object dIn, object _) {
        var data = dIn as NodeData;
        var str = "";
        var e = myDiagram.LastInput;
        var currobj = e.TargetElement;
        if (currobj != null && (currobj.Name == "ButtonA" ||
          (currobj.Panel != null && currobj.Panel.Name == "ButtonA"))) {
          str = data.AToolTip;
        } else {
          str = data.BToolTip;
        }
        return str;
      }

      // define tooltips for buttons
      var tooltipTemplate =
        Builder.Make<Adornment>("ToolTip").Add(
          new TextBlock {
            Font = new Font("Segoe UI", 8),
            Wrap = Wrap.Fit,
            DesiredSize = new Size(200, double.NaN),
            Alignment = Spot.Center,
            Margin = 6
          }.Bind(
            new Binding("Text", "", TooltipTextConverter)
          )
        );
      tooltipTemplate.FindElement("Border").Set(
        new {
          Fill = "whitesmoke",
          Stroke = "lightgray"
        }
      );

      // define the Node template for non-leaf nodes
      myDiagram.NodeTemplateMap.Add("decision",
        new Node(PanelLayoutAuto.Instance).Bind(
          new Binding("Text", "Key")
        ).Add(
          // define the node's outer shape, which will surround the Horizontal Panel
          new Shape("Rectangle") { Fill = "whitesmoke", Stroke = "lightgray" },
          // define a horizontal Panel to place the node's text alongside the buttons
          new Panel(PanelLayoutHorizontal.Instance).Add(
            new TextBlock {
              Font = new Font("Segoe UI", 30, FontWeight.Bold),
              Margin = 5
            }.Bind(
              new Binding("Text", "Key")
            ),
            // define a vertical panel to place the node's two buttons one above the other
            new Panel(PanelLayoutVertical.Instance) {
              DefaultStretch = Stretch.Fill,
              Margin = 3
            }.Add(
              Builder.Make<Panel>("Button").Set( // button A
                new {
                  Name = "ButtonA",
                  Click = ButtonExpandCollapseDelegate,
                  ToolTip = tooltipTemplate
                }
              ).Bind(
                new Binding("PortId", "A")
              ).Add(
                new TextBlock {
                  Font = new Font("Segoe UI", 16, FontWeight.Bold)
                }.Bind(
                  new Binding("Text", "AText")
                )
              ),  // end button A
              Builder.Make<Panel>("Button").Set(  // button B
                new {
                  Name = "ButtonB",
                  Click = ButtonExpandCollapseDelegate,
                  ToolTip = tooltipTemplate
                }
              ).Bind(
                new Binding("PortId", "B")
              ).Add(
                new TextBlock {
                  Font = new Font("Segoe UI", 16, FontWeight.Bold)
                }.Bind(
                  new Binding("Text", "BText")
                )
              )  // end button B
            )  // end Vertical Panel
          )  // end Horizontal Panel
        )  // end node
      );  // end call to add

      // define the Node template for leaf nodes
      myDiagram.NodeTemplateMap.Add("personality",
        new Node(PanelLayoutAuto.Instance).Bind(
          new Binding("Text", "Key")
        ).Add(
          new Shape("Rectangle") { Fill = "whitesmoke", Stroke = "lightgray" },
          new TextBlock {
            Font = new Font("Segoe UI", 13, FontWeight.Bold),
            Wrap = Wrap.Fit,
            DesiredSize = new Size(200, double.NaN),
            Margin = 5
          }.Bind(
            new Binding("Text", "Text")
          )
        )
      );

      // define the only Link template
      myDiagram.LinkTemplate =
        new Link { // the whole link panel
          Routing = LinkRouting.Orthogonal,
          FromPortId = ""
        }.Bind(
          new Binding("FromPortId", "Fromport")
        ).Add(
          new Shape { // the link shape
            Stroke = "lightblue",
            StrokeWidth = 2
          }
        );

      // model data
      var model = new Model {
        LinkFromPortIdProperty = "Fromport"
      };
      // set up the model with the node and link data
      MakeNodes(model);
      MakeLinks(model);
      myDiagram.Model = model;

      // make all but the start node invisible
      foreach (var n in myDiagram.Nodes) {
        if (n.Text != "Start") n.Visible = false;
      }
      foreach (var l in myDiagram.Links) {
        l.Visible = false;
      }


      // define the node relationships
      void MakeNodes(Model m) {
        var nodeDataSource = new List<NodeData> {
          new NodeData { Key = "Start" },  // the root node

          // intermediate Nodes = decisions on personality characteristics
          new NodeData { Key = "I" },
          new NodeData { Key = "E" },

          new NodeData { Key = "IN" },
          new NodeData { Key = "IS" },
          new NodeData { Key = "EN" },
          new NodeData { Key = "ES" },

          new NodeData { Key = "INT" },
          new NodeData { Key = "INF" },
          new NodeData { Key = "IST" },
          new NodeData { Key = "ISF" },
          new NodeData { Key = "ENT" },
          new NodeData { Key = "ENF" },
          new NodeData { Key = "EST" },
          new NodeData { Key = "ESF" },

          // terminal Nodes = the personality descriptions
          new NodeData {
            Key = "INTJ",
            Text = "INTJ = Scientist\nThe most self-confident of all types.  They focus on possibilities and use empirical logic to think about the future.  They prefer that events and people serve some positive use.  1% of population."
          },
          new NodeData {
            Key = "INTP",
            Text = "INTP = Architect\nAn architect of ideas, number systems, computer languages, and many other concepts.  They exhibit great precision in thought and language.  1% of the population."
          },
          new NodeData {
            Key = "INFJ",
            Text = "INFJ = Author\nFocus on possibilities.  Place emphasis on values and come to decisions easily.  They have a strong drive to contribute to the welfare of others.  1% of population."
          },
          new NodeData {
            Key = "INFP",
            Text = "INFP = Questor\nPresent a calm and pleasant face to the world.  Although they seem reserved, they are actually very idealistic and care passionately about a few special people or a cause.  1% of the population."
          },
          new NodeData {
            Key = "ISTJ",
            Text = "ISTJ = Trustee\nISTJs like organized lives. They are dependable and trustworthy, as they dislike chaos and work on a task until completion. They prefer to deal with facts rather than emotions. 6% of the population."
          },
          new NodeData {
            Key = "ISTP",
            Text = "ISTP = Artisan\nISTPs are quiet people who are very capable at analyzing how things work. Though quiet, they can be influential, with their seclusion making them all the more skilled. 17% of the population."
          },
          new NodeData {
            Key = "ISFJ",
            Text = "ISFJ = Conservator\nISFJs are not particularly social and tend to be most concerned with maintaining order in their lives. They are dutiful, respectful towards, and interested in others, though they are often shy. They are, therefore, trustworthy, but not bossy. 6% of the population."
          },
          new NodeData {
            Key = "ISFP",
            Text = "ISFP = Author\nFocus on possibilities.  Place emphasis on values and come to decisions easily.  They have a strong drive to contribute to the welfare of others.  1% of population."
          },
          new NodeData {
            Key = "ENTJ",
            Text = "ENTJ = Fieldmarshal\nThe driving force of this personality is to lead.  They like to impose structure and harness people to work towards distant goals.  They reject inefficiency.  5% of the population."
          },
          new NodeData {
            Key = "ENTP",
            Text = "ENTP = Inventor\nExercise their ingenuity by dealing with social, physical, and mechanical relationships.  They are always sensitive to future possibilities.  5% of the population."
          },
          new NodeData {
            Key = "ENFJ",
            Text = "ENFJ = Pedagogue\nExcellent leaders; they are charismatic and never doubt that others will follow them and do as they ask.   They place a high value on cooperation.  5% of the population."
          },
          new NodeData {
            Key = "ENFP",
            Text = "ENFP = Journalist\nPlace significance in everyday occurrences.  They have great ability to understand the motives of others.  They see life as a great drama.  They have a great impact on others.  5% of the population."
          },
          new NodeData {
            Key = "ESTJ",
            Text = "ESTJ = Administrator\nESTJs are pragmatic, and thus well-suited for business or administrative roles. They are traditionalists and conservatives, believing in the status quo. 13% of the population."
          },
          new NodeData {
            Key = "ESTP",
            Text = "ESTP = Promoter\nESTPs tend to manipulate others in order to attain access to the finer aspects of life. However, they enjoy heading to such places with others. They are social and outgoing and are well-connected. 13% of the population."
          },
          new NodeData {
            Key = "ESFJ",
            Text = "ESFJ = Seller\nESFJs tend to be social and concerned for others. They follow tradition and enjoy a structured community environment. Always magnanimous towards others, they expect the same respect and appreciation themselves. 13% of the population."
          },
          new NodeData {
            Key = "ESFP",
            Text = "ESFP = Entertainer\nThe mantra of the ESFP would be \"Carpe Diem.\" They enjoy life to the fullest. They do not, thus, like routines and long-term goals. In general, they are very concerned with others and tend to always try to help others, often perceiving well their needs. 13% of the population."
          }
        };

        // Provide the same choice information for all of the nodes on each level.
        // The level is implicit in the number of characters in the Key, except for the root node.
        // In a different application, there might be different choices for each node, so the initialization would be above, where the Info's are created.
        // But for this application, it makes sense to share the initialization code based on tree level.
        for (var i = 0; i < nodeDataSource.Count; i++) {
          var d = nodeDataSource[i];
          if (d.Key == "Start") {
            d.Category = "decision";
            d.A = "I";
            d.AText = "Introversion";
            d.AToolTip = "The Introvert is “territorial” and desires space and solitude to recover energy.  Introverts enjoy solitary activities such as reading and meditating.  25% of the population.";
            d.B = "E";
            d.BText = "Extraversion";
            d.BToolTip = "The Extravert is “sociable” and is energized by the presence of other people.  Extraverts experience loneliness when not in contact with others.  75% of the population.";
          } else {
            switch (d.Key.Length) {
              case 1:
                d.Category = "decision";
                d.A = "N";
                d.AText = "Intuition";
                d.AToolTip = "The “intuitive” person bases their lives on predictions and ingenuity.  They consider the future and enjoy planning ahead.  25% of the population.";
                d.B = "S";
                d.BText = "Sensing";
                d.BToolTip = "The “sensing” person bases their life on facts, thinking primarily of their present situation.  They are realistic and practical.  75% of the population.";
                break;
              case 2:
                d.Category = "decision";
                d.A = "T";
                d.AText = "Thinking";
                d.AToolTip = "The “thinking” person bases their decisions on facts and without personal bias.  They are more comfortable with making impersonal judgments.  50% of the population.";
                d.B = "F";
                d.BText = "Feeling";
                d.BToolTip = "The “feeling” person bases their decisions on personal experience and emotion.  They make their emotions very visible.  50% of the population.";
                break;
              case 3:
                d.Category = "decision";
                d.A = "J";
                d.AText = "Judgment";
                d.AToolTip = "The “judging” person enjoys closure.  They establish deadlines and take them seriously.  They despise being late.  50% of the population.";
                d.B = "P";
                d.BText = "Perception";
                d.BToolTip = "The “perceiving” person likes to keep options open and fluid.  They have little regard for deadlines.  Dislikes making decisions unless they are completely sure they are right.  50% of the population.";
                break;
              default:
                d.Category = "personality";
                break;
            }
          }
        }

        m.NodeDataSource = nodeDataSource;
      }

      // The key strings implicitly hold the relationship information, based on their spellings.
      // Other than the root node ("Start"), each node's key string minus its last letter is the
      // key to the "parent" node.
      void MakeLinks(Model m) {
        var linkDataSource = new List<LinkData>();
        var nda = m.NodeDataSource as List<NodeData>;
        for (var i = 0; i < nda.Count; i++) {
          var key = nda[i].Key;
          if (key == "Start" || key.Length == 0) continue;
          // e.G., if key=="INTJ", we Want = prefix="INT" and letter="J"
          var prefix = key.Substring(0, key.Length - 1);
          var letter = key.Substring(key.Length - 1, 1);
          if (prefix.Length == 0) prefix = "Start";
          var obj = new LinkData {
            From = prefix,
            Fromport = letter,
            To = key
          };
          linkDataSource.Add(obj);
        }
        model.LinkDataSource = linkDataSource;
      }
    }


  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string A { get; set; }
    public string AText { get; set; }
    public string AToolTip { get; set; }
    public string B { get; set; }
    public string BText { get; set; }
    public string BToolTip { get; set; }
  }

  public class LinkData : Model.LinkData {
    public string Fromport { get; set; }
  }

}
