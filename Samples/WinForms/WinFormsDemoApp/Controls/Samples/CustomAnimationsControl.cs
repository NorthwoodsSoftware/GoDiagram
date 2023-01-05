/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Samples.CustomAnimations {
  [ToolboxItem(false)]
  public partial class CustomAnimationsControl : DemoControl {
    private Diagram _Diagram;

    public CustomAnimationsControl() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      creation.DisplayMember = "Text";
      creation.DataSource = new WinFormsDemoApp.ComboItem[] {
        new WinFormsDemoApp.ComboItem { ID = "spinIn", Text = "Spin In" },
        new WinFormsDemoApp.ComboItem { ID = "fadeIn", Text = "Fade In" },
        new WinFormsDemoApp.ComboItem { ID = "floatIn", Text = "Float In" },
        new WinFormsDemoApp.ComboItem { ID = "zoomIn", Text = "Zoom In" },
        new WinFormsDemoApp.ComboItem { ID = "none", Text = "--None--" }
      };
      deletion.DisplayMember = "Text";
      deletion.DataSource = new WinFormsDemoApp.ComboItem[] {
        new WinFormsDemoApp.ComboItem { ID = "spinOut", Text = "Spin Out" },
        new WinFormsDemoApp.ComboItem { ID = "fadeOut", Text = "Fade Out" },
        new WinFormsDemoApp.ComboItem { ID = "floatOut", Text = "Float Out" },
        new WinFormsDemoApp.ComboItem { ID = "zoomOut", Text = "Zoom Out" },
        new WinFormsDemoApp.ComboItem { ID = "bounceOut", Text = "Bounce Out" },
        new WinFormsDemoApp.ComboItem { ID = "none", Text = "--None--" }
      };
      links.DisplayMember = "Text";
      links.DataSource = new WinFormsDemoApp.ComboItem[] {
        new WinFormsDemoApp.ComboItem { ID = "bezier", Text = "Bezier Curve" },
        new WinFormsDemoApp.ComboItem { ID = "orthogonal", Text = "Orthogonal Curve" },
        new WinFormsDemoApp.ComboItem { ID = "linear", Text = "Linear (no animation)" }
      };

      Setup();

      goWebBrowser1.Html = @"
        <p style=""margin-bottom: 0;"">
          This sample implements several custom animations within GoDiagram. It may be useful to copy some of them into your own project.
        </p>
        <ul>
          <li>Double click in the Diagram background to create a node, or copy and paste nodes, to view creation animations.</li>
          <li>Delete a node to view deletion animations.</li>
          <li>Draw links to see new link creation animations.</li>
          <li>Select a node and press the + button or the button below to see a link-and-node creation animation.</li>
          <li>Reload the model using the button below to see the custom load animation</li>
        </ul>";
    }

    private void Setup() {
      _Diagram.ToolManager.ClickCreatingTool.ArchetypeNodeData = new NodeData { Key = "node", Color = "palegreen" };
      _Diagram.UndoManager.IsEnabled = true;
      _Diagram.AnimationManager.IsInitial = false;  // To use custom initial animation instead
      _Diagram.InitialLayoutCompleted += animateFadeIn;  // animate with this function

      static void animateFadeIn(object sender, DiagramEvent e) {
        var diagram = e.Diagram;
        var animation = new Animation {
          IsViewportUnconstrained = true,
          Easing = Animation.EaseOutExpo,  // Looks better for a fade in animation
          Duration = 900
        };
        animation.Add(diagram, "Position", diagram.Position.Offset(0, diagram is Palette ? 200 : -200), diagram.Position);
        animation.Add(diagram, "Opacity", 0, 1);
        animation.Start();
      }

      var addNodeAdornment =
        new Adornment("Spot")
          .Add(
            new Panel("Auto")
              .Add(
                new Shape { Fill = null, Stroke = "dodgerblue", StrokeWidth = 3 },
                new Placeholder()
              ),
            // the button to create a "next" node, at the top-right corner
            Builder.Make<Panel>("Button")
              .Set(
                new {
                  Alignment = Spot.TopRight,
                  Click = new Action<InputEvent, GraphObject>(AddNode)  // this function is defined below
                }
              )
              .Add(new Shape("PlusLine") { DesiredSize = new Size(6, 6) })
          );

      _Diagram.NodeTemplate =
        new Node("Auto") {
            SelectionAdornmentTemplate = addNodeAdornment,
            LocationSpot = Spot.Center
          }
          .Bind(new Binding("Location", "Loc").MakeTwoWay())
          .Add(
            new Shape("RoundedRectangle") {
                StrokeWidth = 2,
                PortId = "",  // this Shape is the Node's port, not the whole Node
                FromLinkable = true, FromLinkableSelfNode = true, FromLinkableDuplicates = true,
                ToLinkable = true, ToLinkableSelfNode = true, ToLinkableDuplicates = true,
                Cursor = "pointer"
              }
              .Bind("Fill", "Color"),
            new TextBlock {
                Margin = 10, Font = new Font("Segoe UI", 14)
              }
              .Bind("Text", "Key")
          );

      _Diagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alpha", Loc = new Point(0, 0), Color = "lightblue" },
          new NodeData { Key = "Beta", Loc = new Point(150, 0), Color = "orange" },
          new NodeData { Key = "Gamma", Loc = new Point(0, 150), Color = "lightgreen" },
          new NodeData { Key = "Delta", Loc = new Point(150, 150), Color = "pink" }
        },
        LinkDataSource = new List<LinkData> { } // no Links to start
      };

      // This animation can be used in LinkDrawn Diagram listeners to animate
      // from a straight temporary link to a Bezier finished link
      // Custom animation for the curviness of a bezier link
      AnimationManager.DefineAnimationEffect("Curviness",
        (obj, startValue, endValue, easing, currentTime, duration, animation) => {
          var link = obj as Link;
          var start = Convert.ToDouble(startValue);
          if (double.IsNaN(start)) start = 0;
          var end = Convert.ToDouble(endValue);
          if (double.IsNaN(end)) end = 20;
          link.Curviness = easing(currentTime, start, end - start, duration);
        }
      );

      // This animation can be used in LinkDrawn Diagram listeners to animate
      // from a straight temporary link to an Orthogonal finished link
      AnimationManager.DefineAnimationEffect("OrthogonalLinkAnim",
        (obj, startValue, endValue, easing, currentTime, duration, animation) => {
          var link = obj as Link;
          var initPoints = startValue as List<Point>;
          var tempPoints = endValue as List<Point>;
          var animationState = animation.GetTemporaryState(link);
          if (!animationState.Contains("Initial")) {
            // On the first animaiton tick, save the initial points
            animationState["Initial"] = true;
            var pts = new List<Point>(link.Points);
            tempPoints.AddRange(pts);
            animationState["StartPt"] = pts.First();
            animationState["ToPt1"] = pts[2];
            animationState["ToPt2"] = pts[3];
            animationState["EndPt"] = pts.Last();
          }
          var startPt = (Point)animationState["StartPt"];
          var toPt1 = (Point)animationState["ToPt1"];
          var toPt2 = (Point)animationState["ToPt2"];
          var endPt = (Point)animationState["EndPt"];
          var newpt1 = new Point(
            easing(currentTime, startPt.X, toPt1.X - startPt.X, duration),
            easing(currentTime, startPt.Y, toPt1.Y - startPt.Y, duration));
          var newpt2 = new Point(
            easing(currentTime, endPt.X, -(endPt.X - toPt2.X), duration),
            easing(currentTime, endPt.Y, -(endPt.Y - toPt2.Y), duration));

          // Setting Link.Points will automatically call MakeGeometry which will redraw the segments of the line
          link.Points = new List<Point> {
            startPt, tempPoints[1],
            newpt1, newpt2,
            tempPoints[4], endPt
          };
        }
      );

      AnimationManager.DefineAnimationEffect("Corner",
        (obj, startValue, endValue, easing, currentTime, duration, animation) => {
          var link = obj as Link;
          var start = Convert.ToDouble(startValue);
          // If no corner set, default end to 20
          var end = Convert.ToDouble(startValue);
          if (end == 0) end = 20.0;
          link.Corner = easing(currentTime, start, end - start, duration);
        }
      );

      _Diagram.LinkDrawn += (s, e) => {
        var link = e.Subject as Link;
        var animation = new Animation();
        var linkChoice = ((WinFormsDemoApp.ComboItem)links.SelectedItem).ID;
        if (linkChoice == "bezier") {
          link.Curve = LinkCurve.Bezier;
          animation.Easing = ElasticEase;
          animation.Add(link, "Curviness", 0, link.Curviness);
          animation.Duration = 500;
        } else if (linkChoice == "orthogonal") {
          // The orthogonal animation is two animations, chained together. One to modify the points,
          // and then another to modify the link.Corner value.

          // Store the initial link.Corner value,
          // then set it to 0 so that in between animations it does not revert back to the original state
          var initCorner = link.Corner;
          link.Corner = 0;
          // Store the original points to this list
          var tempPoints = new List<Point>();
          animation.Add(link, "OrthogonalLinkAnim", link.Points, tempPoints);
          animation.Duration = 300;
          // Chain animation after first one is completed
          animation.Finished = (a) => {
            // Set points back to what they were before the animation
            _Diagram.StartTransaction("Fix Points");
            link.Points = tempPoints;
            _Diagram.CommitTransaction("Fix Points");
            // Need to make a new animation object
            var animation2 = new Animation();
            animation2.Add(link, "Corner", 0, initCorner);
            animation2.Duration = 250;
            animation2.Start();
          };
          animation.Start();

          link.Routing = LinkRouting.Orthogonal;
          link.EnsureBounds();
        }
        animation.Start();
      };

      AnimationManager.DefineAnimationEffect("BounceDelete",
        (obj, startValue, endValue, easing, currentTime, duration, animation) => {
          var part = obj as Part;
          var animationState = animation.GetTemporaryState(part);
          if (!animationState.Contains("Initial")) {
            // Set the initial positions as part of the animationState data
            animationState["YPos"] = part.Location.Y;
            animationState["XPos"] = part.Location.X;
            animationState["YVelo"] = 0.0;
            animationState["XVelo"] = 0.0;
            animationState["NewTime"] = 0.0;
            animationState["OldTime"] = 0.0;
            animationState["Initial"] = true;
          }
          part.Location = getPointBounceDelete(currentTime, part, animationState, part.Diagram);
        }
      );

      // Get the point the object should be at based upon the time and original point
      static Point getPointBounceDelete(double currentTime, Part part, Hashtable animationState, Diagram diagram) {
        var yPos = (double)animationState["YPos"];
        var xPos = (double)animationState["XPos"];
        var yVelo = (double)animationState["YVelo"];
        var xVelo = (double)animationState["XVelo"];
        var newTime = (double)animationState["NewTime"];
        var oldTime = (double)animationState["OldTime"];
        if (diagram == null) return new Point(xPos, yPos);
        var height = part.ActualBounds.Height;
        newTime = currentTime;
        // Animation uses a change in time in order to be more consistant
        var delTime = (newTime - oldTime) / 3;
        yVelo += .05 * delTime;
        // Add a slight easing to the x movement at the beginning of the animation
        if (currentTime < 200) {
          xVelo = currentTime / 300;
        }
        // Adjust positions based on the velocities and the change in time
        yPos += yVelo * delTime;
        xPos += xVelo * delTime;
        // Check to see if the Y position will be less than the bottom of the diagram, if so,
        // change the direction and scale down the velocity and set the position to the bottom of the diagram
        if (yPos > diagram.ViewportBounds.Height / 2 - height) {
          yVelo = -.75 * yVelo;
          yPos = diagram.ViewportBounds.Height / 2 - height;
        }
        // Set state for next iteration
        animationState["YPos"] = yPos;
        animationState["XPos"] = xPos;
        animationState["YVelo"] = yVelo;
        animationState["XVelo"] = xVelo;
        animationState["NewTime"] = newTime;
        animationState["OldTime"] = newTime;
        return new Point(xPos, yPos);
      }

      _Diagram.SelectionDeleting += (s, e) => {
        var deletionSelection = ((WinFormsDemoApp.ComboItem)deletion.SelectedItem).ID;
        var animation = new Animation();
        var diagram = e.Diagram;
        foreach (var p in (IEnumerable<Part>)e.Subject) {
          // Because we are deleting this part, we cannot animate it, instead we must animate a temporary copy
          // The animation handles this via addTemporaryPart, which must be passed a copy
          var part = (Part)p.Copy();
          animation.AddTemporaryPart(part, diagram);
          var initPosition = part.Position;
          part.LocationSpot = Spot.Center;
          part.Position = initPosition;
          switch (deletionSelection) {
            case "spinOut":
              animation.Add(part, "Angle", part.Angle, part.Angle + 1000);
              animation.Add(part, "Scale", part.Scale, 0.01);
              break;
            case "fadeOut":
              animation.Add(part, "Opacity", part.Opacity, 0);
              break;
            case "zoomOut":
              animation.Add(part, "Scale", part.Scale, 0.01);
              break;
            case "floatOut":
              animation.Add(part, "Opacity", part.Opacity, 0);
              animation.Add(part, "Position", part.Position, part.Position.Offset(0, -80));
              break;
            case "bounceOut":
              animation.Add(part, "BounceDelete", part.Location, null); // does't need an end value, bounceDelete determines one
              animation.Add(part, "Scale", part.Scale, 0.01);
              animation.Duration = 1500;
              animation.IsViewportUnconstrained = true;
              break;
            default:
              // nothing animates
              break;
          }
        }
        animation.Start();
      };

      _Diagram.ClipboardPasted += (s, e) => {
        var creationSelection = ((WinFormsDemoApp.ComboItem)creation.SelectedItem).ID;
        // For best performance, be sure to use only one Animation for the entire selection
        // instead of creating one animation for each object in the selection
        var animation = new Animation();
        foreach (var part in (IEnumerable<Part>)e.Subject) {
          addCreatedPart(part, animation, creationSelection);
        }
        animation.Start();
      };

      _Diagram.PartCreated += (s, e) => {  // From ClickCreatingTool
        var creationSelection = ((WinFormsDemoApp.ComboItem)creation.SelectedItem).ID;
        var animation = new Animation();
        addCreatedPart((Part)e.Subject, animation, creationSelection);
        animation.Start();
      };

      static void addCreatedPart(Part part, Animation animation, string creationSelection) {
        switch (creationSelection) {
          case "spinIn":
            animation.Add(part, "Angle", part.Angle + 1000, part.Angle);
            animation.Add(part, "Scale", 0.01, part.Scale);
            break;
          case "fadeIn":
            animation.Add(part, "Opacity", 0, part.Opacity);
            break;
          case "zoomIn":
            animation.Add(part, "Scale", 0.01, part.Scale);
            break;
          case "floatIn":
            animation.Add(part, "Opacity", 0, part.Opacity);
            animation.Add(part, "Location", part.Location.Offset(0, -80), part.Location);
            break;
          default:
            // nothing animates
            break;
        }
      }
    }

    private void AddNode(InputEvent e, GraphObject obj) {
      var diagram = _Diagram;
      diagram.StartTransaction("Add States");
      var tempNodes = new List<Node>(diagram.Nodes.Where(n => n.IsSelected));
      var animation = new Animation {
        // Set the easing to a custom easing function
        Easing = ElasticEase
      };
      // Add a new node to the right of each node
      foreach (var part in tempNodes) {
        // get the node data for which the user clicked the button
        var fromNode = part;
        var fromData = part.Data as NodeData;
        // create a new "State" data object, positioned off to the right of the adorned Node
        var toData = new NodeData { Key = "new", Color = "purple" };
        var p = fromNode.Location;
        // Place the new node randomly somewhere along a circular 200px radius
        var rand = new Random();
        var angle = rand.NextDouble() * Math.PI * 2;
        p.X += Math.Cos(angle) * 200;
        p.Y += Math.Sin(angle) * 200;
        toData.Loc = p;
        // add the new node data to the model
        var model = diagram.Model as Model;
        model.AddNodeData(toData);

        // create a link data from the old node data to the new node data
        var linkdata = new LinkData {
          From = model.GetKeyForNodeData(fromData),  // or just: fromData.key
          To = model.GetKeyForNodeData(toData),
        };
        // and add the link data to the model
        model.AddLinkData(linkdata);

        var newnode = diagram.FindNodeForData(toData);
        // Animate each newly created node
        animation.Add(newnode, "Position", part.Location, newnode.Location);
      }

      animation.Start();
      diagram.CommitTransaction("Add States");
    }

    private static double ElasticEase(double currentTime, double startValue, double byValue, int duration) {
      var ts = (currentTime /= duration) * currentTime;
      var tc = ts * currentTime;
      return startValue + byValue * (56 * tc * ts + -175 * ts * ts + 200 * tc + -100 * ts + 20 * currentTime);
    }

    private void Button1_Click(object sender, EventArgs e) => AddNode(null, null);
    private void Button2_Click(object sender, EventArgs e) {
      _Diagram.Model = Model.FromJson<Model>(_Diagram.Model.ToJson());
    }

    // define the model data
    public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
    public class NodeData : Model.NodeData {
      public string Color { get; set; }
      public Point Loc { get; set; }
    }
    public class LinkData : Model.LinkData { }
  }
}
