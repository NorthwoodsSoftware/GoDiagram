using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Extensions;

namespace WinFormsExtensionControls.DrawCommandHandlerSample {
  [ToolboxItem(false)]
  public partial class DrawCommandHandlerSampleControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    
    public DrawCommandHandlerSampleControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      btnLeft.Click += (e, obj) => LeftAlign();
      btnRight.Click += (e, obj) => RightAlign();
      btnTops.Click += (e, obj) => TopAlign();
      btnBottoms.Click += (e, obj) => BottomAlign();
      btnCenterX.Click += (e, obj) => CenterXAlign();
      btnCenterY.Click += (e, obj) => CenterYAlign();
      btnRow.Click += (e, obj) => RowAlign();
      btnColumn.Click += (e, obj) => ColumnAlign();

      btn45.Click += (e, obj) => Rotate45();
      btnNeg45.Click += (e, obj) => RotateM45();
      btn90.Click += (e, obj) => Rotate90();
      btnNeg90.Click += (e, obj) => RotateM90();
      btn180.Click += (e, obj) => Rotate180();

      btnFront.Click += (e, obj) => PullToFront();
      btnBack.Click += (e, obj) => PushToBack();

      radBtnMove.CheckedChanged += (e, obj) => ArrowMode();
      radBtnScroll.CheckedChanged += (e, obj) => ArrowMode();
      radBtnSelect.CheckedChanged += (e, obj) => ArrowMode();
      radBtnTree.CheckedChanged += (e, obj) => ArrowMode();

      goWebBrowser1.Html = @"
  <p>
    This example demonstrates a custom <a>CommandHandler</a>.
    It allows the user to position selected Parts in a diagram relative to each other,
    overrides <a>CommandHandler.doKeyDown</a> to allow handling the arrow keys in additional manners,
    and uses a ""paste offset"" so that pasting objects will cascade them rather than place them on top of one another.
    It is defined in its own file, as <a href = ""DrawCommandHandler.js"">DrawCommandHandler.js</a>.
  </p>
 
   <p>
     The above buttons can be used to align Parts, rotate Parts, or change the behavior of the arrow keys.
  </p>
  <p>
    Usage can also be seen in the <a href = ""../projects/bpmn/BPMN.html"">BPMN Editor</a> sample.
  </p>
";

    }


    private void ArrowMode() {
      if (radBtnMove.Checked)
        (myDiagram.CommandHandler as DrawCommandHandler).ArrowKeyBehavior = ArrowBehavior.Move;
      else if (radBtnScroll.Checked)
        (myDiagram.CommandHandler as DrawCommandHandler).ArrowKeyBehavior = ArrowBehavior.Scroll;
      else if (radBtnSelect.Checked)
        (myDiagram.CommandHandler as DrawCommandHandler).ArrowKeyBehavior = ArrowBehavior.Select;
      else if (radBtnTree.Checked)
        (myDiagram.CommandHandler as DrawCommandHandler).ArrowKeyBehavior = ArrowBehavior.Tree;

    }
    
    

    // begin Blazor callbacks
    private void LeftAlign() {
      (diagramControl1.Diagram.CommandHandler as DrawCommandHandler).AlignLeft();
    }

    private void RightAlign() {
      (diagramControl1.Diagram.CommandHandler as DrawCommandHandler).AlignRight();
    }

    private void TopAlign() {
      (diagramControl1.Diagram.CommandHandler as DrawCommandHandler).AlignTop();
    }

    private void BottomAlign() {
      (diagramControl1.Diagram.CommandHandler as DrawCommandHandler).AlignBottom();
    }

    private void CenterXAlign() {
      (diagramControl1.Diagram.CommandHandler as DrawCommandHandler).AlignCenterX();
    }

    private void CenterYAlign() {
      (diagramControl1.Diagram.CommandHandler as DrawCommandHandler).AlignCenterY();
    }

    private void RowAlign() {
      (diagramControl1.Diagram.CommandHandler as DrawCommandHandler).AlignRow(10);
    }

    private void ColumnAlign() {
      (diagramControl1.Diagram.CommandHandler as DrawCommandHandler).AlignColumn(10);
    }

    private void Rotate45() {
      (diagramControl1.Diagram.CommandHandler as DrawCommandHandler).Rotate(45);
    }

    private void RotateM45() {
      (diagramControl1.Diagram.CommandHandler as DrawCommandHandler).Rotate(-45);
    }

    private void Rotate90() {
      (diagramControl1.Diagram.CommandHandler as DrawCommandHandler).Rotate(90);
    }

    private void RotateM90() {
      (diagramControl1.Diagram.CommandHandler as DrawCommandHandler).Rotate(-90);
    }

    private void Rotate180() {
      (diagramControl1.Diagram.CommandHandler as DrawCommandHandler).Rotate(180);
    }

    private void PullToFront() {
      (diagramControl1.Diagram.CommandHandler as DrawCommandHandler).PullToFront();
    }

    private void PushToBack() {
      (diagramControl1.Diagram.CommandHandler as DrawCommandHandler).PushToBack();
    }
    // end Blazor callbacks

    private void Setup() {

      myDiagram = diagramControl1.Diagram;

      myDiagram.CommandHandler = new DrawCommandHandler {
        ArrowKeyBehavior = ArrowBehavior.Move
      };
      myDiagram.UndoManager.IsEnabled = true;

      myDiagram.NodeTemplate = new Node(PanelLayoutAuto.Instance) {
        LocationSpot = Spot.Center
      }.Add(
        new Shape {
          Figure = "RoundedRectangle",
          StrokeWidth = 0
        }.Bind("Fill", "Color"),
        new TextBlock {
          Margin = 8
        }.Bind("Text", "Key")
      );

      var model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alpha", Color = "lightblue" },
          new NodeData { Key = "Beta", Color = "Orange" },
          new NodeData { Key = "Gamma", Color = "lightgreen" },
          new NodeData { Key = "Delta", Color = "pink" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "Alpha", To = "Beta" },
          new LinkData { From = "Alpha", To = "Gamma" },
          new LinkData { From = "Gamma", To = "Delta" }
        }
      };
      myDiagram.Model = model;
    }

  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }

  public class NodeData : Model.NodeData {
    public Brush Color { get; set; }
  }

  public class LinkData : Model.LinkData { }


}
