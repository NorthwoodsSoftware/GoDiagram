/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Extensions;

namespace Demo.Extensions.DrawCommandHandler {
  public partial class DrawCommandHandler : DemoControl {
    private Diagram _Diagram;

    public DrawCommandHandler() {
      InitializeComponent();

      Setup();

      leftsBtn.Click += (e, obj) => LeftAlign();
      rightsBtn.Click += (e, obj) => RightAlign();
      topsBtn.Click += (e, obj) => TopAlign();
      bottomsBtn.Click += (e, obj) => BottomAlign();
      centerXBtn.Click += (e, obj) => CenterXAlign();
      centerYBtn.Click += (e, obj) => CenterYAlign();
      rowBtn.Click += (e, obj) => RowAlign();
      columnBtn.Click += (e, obj) => ColumnAlign();

      fortyFiveBtn.Click += (e, obj) => Rotate45();
      negFortyFiveBtn.Click += (e, obj) => RotateN45();
      ninetyBtn.Click += (e, obj) => Rotate90();
      negNinetyBtn.Click += (e, obj) => RotateN90();
      oneEightyBtn.Click += (e, obj) => Rotate180();

      frontBtn.Click += (e, obj) => PullToFront();
      backBtn.Click += (e, obj) => PushToBack();

      _InitRadioButtons();

      desc1.MdText = DescriptionReader.Read("Extensions.DrawCommandHandler.md");
    }

    private void Setup() {

      _Diagram = diagramControl1.Diagram;

      _Diagram.CommandHandler = new Northwoods.Go.Extensions.DrawCommandHandler {
        ArrowKeyBehavior = ArrowBehavior.Move
      };
      _Diagram.UndoManager.IsEnabled = true;

      _Diagram.NodeTemplate = new Node(PanelType.Auto) {
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
      _Diagram.Model = model;
    }

    private void _SetArrowMode(ArrowBehavior mode) {
      (_Diagram.CommandHandler as Northwoods.Go.Extensions.DrawCommandHandler).ArrowKeyBehavior = mode;
    }

    private void LeftAlign() {
      (diagramControl1.Diagram.CommandHandler as Northwoods.Go.Extensions.DrawCommandHandler).AlignLeft();
    }

    private void RightAlign() {
      (diagramControl1.Diagram.CommandHandler as Northwoods.Go.Extensions.DrawCommandHandler).AlignRight();
    }

    private void TopAlign() {
      (diagramControl1.Diagram.CommandHandler as Northwoods.Go.Extensions.DrawCommandHandler).AlignTop();
    }

    private void BottomAlign() {
      (diagramControl1.Diagram.CommandHandler as Northwoods.Go.Extensions.DrawCommandHandler).AlignBottom();
    }

    private void CenterXAlign() {
      (diagramControl1.Diagram.CommandHandler as Northwoods.Go.Extensions.DrawCommandHandler).AlignCenterX();
    }

    private void CenterYAlign() {
      (diagramControl1.Diagram.CommandHandler as Northwoods.Go.Extensions.DrawCommandHandler).AlignCenterY();
    }

    private void RowAlign() {
      (diagramControl1.Diagram.CommandHandler as Northwoods.Go.Extensions.DrawCommandHandler).AlignRow(10);
    }

    private void ColumnAlign() {
      (diagramControl1.Diagram.CommandHandler as Northwoods.Go.Extensions.DrawCommandHandler).AlignColumn(10);
    }

    private void Rotate45() {
      (diagramControl1.Diagram.CommandHandler as Northwoods.Go.Extensions.DrawCommandHandler).Rotate(45);
    }

    private void RotateN45() {
      (diagramControl1.Diagram.CommandHandler as Northwoods.Go.Extensions.DrawCommandHandler).Rotate(-45);
    }

    private void Rotate90() {
      (diagramControl1.Diagram.CommandHandler as Northwoods.Go.Extensions.DrawCommandHandler).Rotate(90);
    }

    private void RotateN90() {
      (diagramControl1.Diagram.CommandHandler as Northwoods.Go.Extensions.DrawCommandHandler).Rotate(-90);
    }

    private void Rotate180() {
      (diagramControl1.Diagram.CommandHandler as Northwoods.Go.Extensions.DrawCommandHandler).Rotate(180);
    }

    private void PullToFront() {
      (diagramControl1.Diagram.CommandHandler as Northwoods.Go.Extensions.DrawCommandHandler).PullToFront();
    }

    private void PushToBack() {
      (diagramControl1.Diagram.CommandHandler as Northwoods.Go.Extensions.DrawCommandHandler).PushToBack();
    }
  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public Brush Color { get; set; }
  }
  public class LinkData : Model.LinkData { }
}
