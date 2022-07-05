/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.ProductionProcess {
  [ToolboxItem(false)]
  public partial class ProductionProcessControl : System.Windows.Forms.UserControl {
    private static HttpClient _HttpClient = new();

    private Diagram myDiagram;

    static ProductionProcessControl() {
      _HttpClient.DefaultRequestHeaders.Add("User-Agent", "Production Process Image Getter");
    }

    public ProductionProcessControl() {
      InitializeComponent();

      Setup();
      txtJSON.Text = myModelData;

      goWebBrowser1.Html = @"
        <p>
      This process flow diagram partially simulates the production process for gas and oil byproducts into their end products.
        </p>
        <p>
      This diagram cannot be modified by the user, but its model was created in the corresponding editor:
      <a href=""ProductionEditor"">Production Process Editor</a>, which shares the same templates
      and has additional tools for editing diagrams.
        </p>
        <p>
      This viewer also has some animation of the pipes (links) that the editor does not show, to avoid distractions.
      A real application would also show additional information about the nodes and the links.
        </p>
";

    }

    private string myModelData = @"{
  ""NodeDataSource"": [
    {""Key"":1, ""Pos"":""-170 -48"", ""Icon"":""Natgas"", ""Color"":""blue"", ""Text"":""Gas Companies"", ""Description"":""Provides natural gas liquids (NGLs)."", ""Caption"":""Gas Drilling Well"", ""Imgsrc"":""https://upload.wikimedia.org/wikipedia/commons/thumb/5/5d/BarnettShaleDrilling-9323.jpg/256px-BarnettShaleDrilling-9323.jpg""},
    {""Key"":2, ""Pos"":""-170 96"", ""Icon"":""Oil"", ""Color"":""blue"", ""Text"":""Oil Companies"", ""Description"":""Provides associated petroleum gas (APG). This type of gas used to be released as waste from oil drilling, but is now commonly captured for processing."", ""Caption"":""Offshore oil well"", ""Imgsrc"":""https://upload.wikimedia.org/wikipedia/commons/thumb/a/ab/Oil_platform_P-51_%28Brazil%29.jpg/512px-Oil_platform_P-51_%28Brazil%29.jpg""},
    {""Key"":3, ""Pos"":""-70 90"", ""Icon"":""Gasprocessing"", ""Color"":""blue"", ""Text"":""Gas Processing"", ""Description"":""APG processing turns associated petrolium gas into natural gas liquids (NGLs) and stable natural gas (SGN)."", ""Caption"":""Natural gas plant"", ""Imgsrc"":""https://upload.wikimedia.org/wikipedia/commons/thumb/6/68/Solohiv_natural_gas_plant_-_fragment.jpg/256px-Solohiv_natural_gas_plant_-_fragment.jpg""},
    {""Key"":4, ""Pos"":""40 -50"", ""Icon"":""Fractionation"", ""Color"":""blue"", ""Text"":""Gas Fractionation"", ""Description"":""Natural gas liquids are separated into individual hydrocarbons like propane and butanes, hydrocarbon mixtures (naphtha) and liquefied petroleum gases (LPGs)."", ""Caption"":""Gas Plant"", ""Imgsrc"":""https://upload.wikimedia.org/wikipedia/commons/thumb/2/20/Gasblok.jpg/256px-Gasblok.jpg""},
    {""Key"":5, ""Pos"":""140 -50"", ""Icon"":""Pyrolysis"", ""Color"":""orange"", ""Text"":""Pyrolysis (Cracking)"", ""Description"":""Liquefied petroleum gases (LPGs) are transformed into Ethylene, propylene, benzene, and other by-products."", ""Caption"":""Pyrolysis plant"", ""Imgsrc"":""https://upload.wikimedia.org/wikipedia/commons/6/6c/Guelph.jpg""},
    {""Key"":6, ""Pos"":""340 -130"", ""Icon"":""Polymerization"", ""Color"":""red"", ""Text"":""Basic Polymers"", ""Description"":""Ethylene and propylene (monomers) are processed into end products using various methods (polymerization)."", ""Caption"":""Plastics engineering-Polymer products"", ""Imgsrc"":""https://upload.wikimedia.org/wikipedia/commons/thumb/4/4c/Plastics_engineering-Polymer_products.jpg/256px-Plastics_engineering-Polymer_products.jpg""},
    {""Key"":7, ""Pos"":""340 -40"", ""Icon"":""Polymerization"", ""Color"":""green"", ""Text"":""Plastics"", ""Description"":""Polymerization produces PET, glycols, alcohols, expandable polystyrene, acrylates, BOPP-films and geosynthetics."", ""Caption"":""Lego Color Bricks"", ""Imgsrc"":""https://upload.wikimedia.org/wikipedia/commons/thumb/3/32/Lego_Color_Bricks.jpg/256px-Lego_Color_Bricks.jpg""},
    {""Key"":8, ""Pos"":""340 50"", ""Icon"":""Polymerization"", ""Color"":""lightgreen"", ""Text"":""Synthetic Rubbers"", ""Description"":""Polymerization produces commodity and specialty rubbers and thermoplastic elastomers."", ""Caption"":""Sheet of synthetic rubber coming off the rolling mill at the plant of Goodrich"", ""Imgsrc"":""https://upload.wikimedia.org/wikipedia/commons/thumb/2/23/Sheet_of_synthetic_rubber_coming_off_the_rolling_mill_at_the_plant_of_Goodrich.jpg/512px-Sheet_of_synthetic_rubber_coming_off_the_rolling_mill_at_the_plant_of_Goodrich.jpg""},
    {""Key"":9, ""Pos"":""340 120"", ""Color"":""orange"", ""Text"":""Intermediates"", ""Description"":""Produced Ethylene, Propylene, Butenes, Benzene, and other by-products."", ""Caption"":""Propylene Containers"", ""Imgsrc"":""https://upload.wikimedia.org/wikipedia/commons/thumb/2/2e/XVJ-12_Propylene_%288662385917%29.jpg/256px-XVJ-12_Propylene_%288662385917%29.jpg""},
    {""Key"":10, ""Pos"":""340 210"", ""Icon"":""Finishedgas"", ""Color"":""blue"", ""Text"":""LPG, Naphtha,\nMTBE"", ""Description"":""Propane, butane, and other general purpose fuels and byproducts."", ""Caption"":""Liquid Petroleum Gas Truck"", ""Imgsrc"":""https://upload.wikimedia.org/wikipedia/commons/thumb/5/52/LPG_Truck.jpg/256px-LPG_Truck.jpg""},
    {""Key"":11, ""Pos"":""340 300"", ""Icon"":""Finishedgas"", ""Color"":""blue"", ""Text"":""Natural Gas, NGLs"", ""Description"":""Used for heating, cooking, and electricity generation"", ""Caption"":""Natural Gas Flame"", ""Imgsrc"":""https://upload.wikimedia.org/wikipedia/commons/thumb/0/03/%22LPG_flame%22.JPG/512px-%22LPG_flame%22.JPG""}
  ],
  ""LinkDataSource"": [
    {""From"":1, ""To"":4, ""Points"":[""-150 -41"",""-140 -41"",""-69 -41"",""-69 -41.33333333333333"",""2 -41.33333333333333"",""20 -41.33333333333333""]},
    {""From"":2, ""To"":3, ""Points"":[""-150 103"",""-140 103"",""-120 103"",""-120 103"",""-100 103"",""-90 103""]},
    {""From"":3, ""To"":4, ""Points"":[""-50 98.66666666666667"",""-40 98.66666666666667"",""-15 98.66666666666667"",""-15 -32.666666666666664"",""10 -32.666666666666664"",""20 -32.666666666666664""]},
    {""From"":3, ""To"":5, ""ToSpot"":""BottomSide"", ""Points"":[""-50 107.33333333333333"",""-32 107.33333333333333"",""140 107.33333333333333"",""140 46.666666666666664"",""140 -14"",""140 -24""]},
    {""From"":4, ""To"":5, ""Points"":[""60 -37"",""70 -37"",""90 -37"",""90 -37"",""110 -37"",""120 -37""]},
    {""From"":3, ""To"":11, ""FromSpot"":""BottomSide"", ""Points"":[""-70 116"",""-70 126"",""-70 307"",""120 307"",""310 307"",""320 307""]},
    {""From"":4, ""To"":10, ""FromSpot"":""BottomSide"", ""Points"":[""40 -12"",""40 -2"",""40 217"",""175 217"",""310 217"",""320 217""]},
    {""From"":5, ""To"":6, ""FromSpot"":""Right"", ""Points"":[""160 -37"",""170 -37"",""240 -37"",""240 -123"",""310 -123"",""320 -123""]},
    {""From"":5, ""To"":7, ""FromSpot"":""Right"", ""Points"":[""160 -37"",""170 -37"",""240 -37"",""240 -33"",""310 -33"",""320 -33""]},
    {""From"":5, ""To"":8, ""FromSpot"":""Right"", ""Points"":[""160 -37"",""170 -37"",""240 -37"",""240 57"",""310 57"",""320 57""]},
    {""From"":5, ""To"":9, ""FromSpot"":""Right"", ""Points"":[""160 -37"",""170 -37"",""240 -37"",""240 127"",""310 127"",""320 127""]}
  ]
}";

    // consts
    private const string defaultDescription = "Select a node to see more information.";

    // vars for blazor bindings
    private string myImageSrc;
    private string myImageAlt;
    private string myTitle;
    private string myDescription = defaultDescription;

    // vars for animations
    double opacity = 1;
    bool down = true;

    // dictionaries for bindings
    // colors
    Dictionary<string, string> colors = new Dictionary<string, string> {
       { "red", "#be4b15" },
       { "green", "#52ce60" },
       { "blue", "#6ea5f8" },
       { "lightred", "#fd8852" },
       { "lightblue", "#afd4fe" },
       { "lightgreen", "#b9e986" },
       { "pink", "#faadc1" },
       { "purple", "#d689ff" },
       { "orange", "#f08c00" }
      };
    // geometries
    Dictionary<string, Geometry> icons = new Dictionary<string, Geometry> {
        { "Natgas", Geometry.Parse("F M244.414,133.231 L180.857,133.231 178.509,156.191 250.527,192.94z M179.027,276.244 262.328,308.179 253.451,221.477z M267.717,360.866 264.845,332.807 220.179,360.866z M167.184,266.775 247.705,207.524 176.95,171.421z M157.551,360.866 192.975,360.866 256.447,320.996 165.218,286.021z M141.262,374.366 141.262,397.935 161.396,397.935 161.396,425.268 179.197,425.268 179.197,397.935 246.07,397.935 246.07,425.268 263.872,425.268 263.872,397.935 284.006,397.935 284.006,374.366z", true) },
        { "Oil", Geometry.Parse("F M190.761,109.999c-3.576-9.132-8.076-22.535,7.609-37.755c0.646,13.375,14.067,13.99,11.351,36.794 c6.231-2.137,6.231-2.137,9.188-3.781c17.285-9.612,20.39-25.205,7.64-42.896c-7.316-10.153-11.945-20.58-10.927-33.23 c-4.207,4.269-5.394,9.444-6.744,17.129c-5.116-3.688,3.067-41.28-22.595-46.26c5.362,13.836,7.564,25.758-2.607,40.076 c-0.667-5.422-3.255-12.263-8.834-17.183c-0.945,16.386,0.97,23.368-9.507,44.682c-2.945,8.902-5.02,17.635,0.533,26.418 C171.354,102.673,180.555,108.205,190.761,109.999z M330.738,371.614h-15.835v-61.829l-74.409-78.541v-21.516c0-6.073-4.477-11.087-10.309-11.957v-82.156h-63.632v82.156 c-5.831,0.869-10.308,5.883-10.308,11.957v21.516l-74.409,78.541v61.829H66l-25.124,25.123h314.984L330.738,371.614z M166.554,371.614h-61.717v-29.782h61.717V371.614z M166.554,319.956h-61.717v-1.007l51.471-54.329 c0.555,5.513,4.813,9.919,10.246,10.729V319.956L166.554,319.956z M291.903,371.614h-61.718v-29.782h61.718V371.614z M291.903,319.956h-61.718V275.35c5.435-0.811,9.691-5.217,10.246-10.729l51.472,54.329V319.956z", true) },
        { "Pyrolysis", Geometry.Parse("F M226.46,198.625v-75.5h-87.936v-19.391h-14.304V92.319h-5.079l-3.724-82.777H91.766l-3.724,82.777h-6.18v11.415H68.535 V92.319h-5.079L59.731,9.542H36.08l-3.724,82.777h-6.18v11.415H11.872v94.891H0v35.167h243.333v-35.167H226.46z M61.355,191.792h-28 v-69.333h28V191.792z M117.041,191.792h-28v-69.333h28V191.792z M168.46,198.625h-29.936v-17.5h29.936V198.625z M206.46,198.625h-18 v-37.5h-49.936v-18h67.936V198.625z", true) },
        { "Fractionation", Geometry.Parse("F M224.609,218.045l-5.24-173.376h9.172V18.297h-9.969L218.019,0h-32.956l-0.553,18.297h-9.969v26.372h9.171l-2.475,81.878 h-39.196l-1.833-52.987h8.998V47.188h-9.91l-0.633-18.297h-32.913l-0.633,18.297h-9.911V73.56h8.999l-1.833,52.987H62.081 l-0.974-24.097h8.767V76.079h-9.833l-0.74-18.298H26.446l-0.739,18.298h-9.832v26.371h8.766L19.97,218.045H3.041v26.371h238.333 v-26.371z  M144.536,198.667h34.522l-0.586,19.378h-33.267L144.536,198.667z M143.624,172.296l-0.67-19.378h37.487 l-0.586,19.378H143.624z M100.792,172.296H63.93l-0.783-19.378h38.315L100.792,172.296z M99.88,198.667l-0.67,19.378h-33.43 l-0.783-19.378H99.88z", true) },
        { "Gasprocessing", Geometry.Parse("F M242.179,212.635V58.634h-80.936v40.877h-13.465l-1.351-33.828h5.284V45.247h-6.1l-0.415-10.382h6.515V14.431h-46.927 v20.435h6.515l-0.415,10.382h-6.1v20.436h5.284l-2.8,70.125H96.186V95.007H10.642v117.628H0v25.755h252.82v-25.755H242.179z M73.501,135.808H51.714v76.827H33.327v-94.942h40.174V135.808z M137.797,213.516h-19.099v-88h19.099V213.516z M219.494,212.635 h-18.316v-51.411h18.316V212.635z M219.494,138.539h-18.316V99.511h-17.25V81.319h35.566V138.539z", true) },
        { "Polymerization", Geometry.Parse("F M399.748,237.029 L363.965,174.401 345.094,174.401 343.113,155.463 326.566,155.463 322.797,29.385 290.486,29.385 286.715,155.463 270.17,155.463 261.634,237.029 242.029,237.029 242.029,190.314 192.029,190.314 192.029,230.587 109.84,187.329 109.84,230.486 27.84,187.329 27.84,237.029 0,237.029 0,394.674 424.059,394.674 424.059,237.029z", true) },
        { "Finishedgas", Geometry.Parse("F M422.504,346.229v-68.306h-16.678v-24.856c0-21.863-16.199-39.935-37.254-42.882v-0.798 c0-26.702-21.723-48.426-48.426-48.426h-1.609c-26.699,0-48.426,21.724-48.426,48.426v87.633h-23.641v-93.169 c0-6.083-3.248-11.394-8.096-14.333c5.662-1.667,9.799-6.896,9.799-13.098c0-7.544-6.117-13.661-13.662-13.661h-10.981v-12.727h-17 v12.727h-10.984c-7.545,0-13.66,6.116-13.66,13.661c0,6.202,4.137,11.431,9.799,13.098c-4.848,2.94-8.098,8.25-8.098,14.333v93.169 h-23v-85.596c0-4.458-3.613-8.071-8.07-8.071h-16.412v-87.591c0-16.03-13.041-29.071-29.07-29.071v-1.267 c0-23.608-19.139-42.748-42.748-42.748S21.54,61.817,21.54,85.425v260.805H0v55.139h444.045v-55.139H422.504z M286.256,209.387 c0-17.801,14.48-32.284,32.281-32.284h1.609c17.803,0,32.285,14.483,32.285,32.284v1.559 c-19.059,4.545-33.232,21.673-33.232,42.124v24.855h-16.676v19.098h-16.27v-87.635H286.256z M302.525,313.162v33.067h-16.27 v-33.067H302.525z M270.113,313.162v33.067h-23.641v-33.067H270.113z M144.447,219.496v85.596c0,4.458,3.613,8.071,8.07,8.071 h31.07v33.068h-47.482V219.496H144.447z M107.035,102.834c7.129,0,12.93,5.8,12.93,12.929v87.591h-12.93V102.834z M107.035,219.496 h12.93v126.733h-12.93V219.496z", true) }
      };

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      string ColorFunc(object colornameAsObj, object _) {
        var c = colors[colornameAsObj as string];
        return c ?? "gray";
      }

      // A data binding conversion function. Given a name, return the Geometry.
      Geometry GeoFunc(object geonameAsObj, object _) {
        return icons[geonameAsObj as string];
      }

      // diagram properties
      myDiagram.InitialAutoScale = AutoScale.None; // scale to show all of the contents
      myDiagram.ChangedSelection += OnSelectionChanged; // view additional information
      myDiagram.MaxSelectionCount = 1; // don't allow users to select more than one thing at a time
      myDiagram.IsReadOnly = true;

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutSpot.Instance) {
          LocationElementName = "PORT",
          LocationSpot = Spot.Top,  // location point is the middle top of the PORT
          LinkConnected = UpdatePortHeight,
          ToolTip =
            Builder.Make<Adornment>("ToolTip").Add(
              new TextBlock {
                Margin = 4,
                Width = 140
              }.Bind(
                new Binding("Text", "", (data, _) => {
                  return (data as NodeData).Text + ":\n\n" + (data as NodeData).Description;
                })
              )
            )
        }.Bind(
          new Binding("Location", "Pos", Point.Parse).MakeTwoWay(Point.Stringify)
        ).Add(
          // The main element of the Spot panel is a vertical panel housing an optional icon,
          // plus a rectangle that acts as the port
          new Panel(PanelLayoutVertical.Instance).Add(
            new Shape {
              Width = 40,
              Height = 0,
              Stroke = (Brush)null,
              StrokeWidth = 0,
              Fill = "gray"
            }.Bind(
              new Binding("Height", "Icon", (_, __) => { return 40; }),
              new Binding("Fill", "Color", ColorFunc),
              new Binding("Geometry", "Icon", GeoFunc)),
            new Shape {
              Name = "PORT",
              Width = 40,
              Height = 24,
              Margin = new Margin(-1, 0, 0, 0),
              Stroke = (Brush)null,
              StrokeWidth = 0,
              Fill = "gray",
              PortId = "",
              FromLinkable = true,
              ToLinkable = true
            }.Bind(
              new Binding("Fill", "Color", ColorFunc)
            ),
            new TextBlock {
              Font = new Font("Segoe UI", 14, FontWeight.Bold),
              TextAlign = TextAlign.Center,
              Margin = 3,
              MaxSize = new Size(105, double.NaN),
              Alignment = Spot.Top,
              AlignmentFocus = Spot.Bottom,
              Editable = true
            }.Bind(
              new Binding("Text").MakeTwoWay()
            )
          )
        );

      void UpdatePortHeight(Node node, Link link, GraphObject port, bool Connected) {
        var sideinputs = 0;
        var sideoutputs = 0;
        foreach (var l in node.FindLinksConnected()) {
          if (l.ToNode == node && l.ToSpot == Spot.LeftSide) sideinputs++;
          if (l.FromNode == node && l.FromSpot == Spot.RightSide) sideoutputs++;
        }
        var tot = Math.Max(sideinputs, sideoutputs);
        tot = Math.Max(1, Math.Min(tot, 2));
        port.Height = tot * (10 + 2) + 2;  // where 10 is the link path's strokeWidth
      }

      // link template
      myDiagram.LinkTemplate =
        new Link {
          LayerName = "Background",
          Routing = LinkRouting.Orthogonal,
          Corner = 15,
          Reshapable = true,
          Resegmentable = true,
          FromSpot = Spot.RightSide,
          ToSpot = Spot.LeftSide
        }.Bind(
          // make sure links come in from the proper direction and go out appropriately
          new Binding("FromSpot", "FromSpot", Spot.Parse),
          new Binding("ToSpot", "ToSpot", Spot.Parse),
          new Binding("Points", "Points", PointArrConverter, PointArrBackConverter)
        ).Add(
          // mark each Shape to get the link geometry with IsPanelMain = true
          new Shape {
            IsPanelMain = true,
            Stroke = "gray",
            StrokeWidth = 10
          }.Bind(
            // get the default stroke color from the fromNode
            new Binding("Stroke", "FromNode", (n, _) => {
              if (n == null) return "gray";
              var color = colors[((n as Node).Data as NodeData).Color];
              if (color == null) return "gray";
              return Brush.Lighten(color);
            }).OfElement(),
            // but use the link's data.Color if it is set
            new Binding("Stroke", "Color", ColorFunc)
          ),
          new Shape {
            IsPanelMain = true,
            Stroke = "white",
            StrokeWidth = 3,
            Name = "PIPE",
            StrokeDashArray = new float[] { 20, 40 },
            StrokeDashOffset = 3
          }
        );

      myDiagram.Model = Model.FromJson<Model>(myModelData);

      Loop();
    }

    private static List<Point> PointArrConverter(object pts) {
      var ptList = pts as List<string>;
      if (ptList.Count % 2 != 0) throw new ArgumentException();
      var points = new List<Point>();
      for (var i = 0; i < ptList.Count; i++) {
        points.Add(Point.Parse(ptList[i]));
      }
      return points;
    }

    private static List<string> PointArrBackConverter(object pts) {
      var ptList = pts as List<Point>;
      var points = new List<string>();
      for (var i = 0; i < ptList.Count; i++) {
        points.Add(Point.Stringify(ptList[i]));
      }
      return points;
    }

    private void Loop() {
      var diagram = myDiagram;
      Task.Delay(60).ContinueWith((t) => {
        var oldskips = diagram.SkipsUndoManager;
        diagram.SkipsUndoManager = true;
        foreach (var link in diagram.Links) {
          var shape = link.FindElement("PIPE") as Shape;
          var off = shape.StrokeDashOffset - 3;
          // animate (move) the stroke dash
          shape.StrokeDashOffset = (off <= 0) ? 60 : off;
          // animte (strobe) the opacity:
          if (down) opacity = opacity - 0.01;
          else opacity = opacity + 0.003;
          if (opacity <= 0) { down = !down; opacity = 0; }
          if (opacity > 1) { down = !down; opacity = 1; }
          shape.Opacity = opacity;
        }
        diagram.SkipsUndoManager = oldskips;
        Loop();
      });
    }

    void OnSelectionChanged(object _, DiagramEvent e) {
      var sel = myDiagram.Selection;
      if (sel.Count >= 1 && sel.First() is Node node) { // a node is selected
        UpdateInformation(node);
      } else { // no selection
        myImageSrc = "";
        myImageAlt = "";
        myTitle = "";
        myDescription = defaultDescription;
      }
    }

    async void UpdateInformation(Node node) {
      var data = node.Data as NodeData;
      if (data.Imgsrc != null) {
        myImageAlt = data.Caption;
        myImageSrc = data.Imgsrc;
      } else {
        myImageSrc = "";
        myImageAlt = "";
      }
      myTitle = data.Text;
      myDescription = data.Description;
      pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

      using var response = await _HttpClient.GetAsync(myImageSrc);
      if (response.IsSuccessStatusCode) {
        using var stream = await response.Content.ReadAsStreamAsync();
        pictureBox1.Image = System.Drawing.Image.FromStream(stream);
      }
      txtCaption.Text = myTitle;
      txtDescription.Text = myDescription;

    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }

  public class NodeData : Model.NodeData {
    public string Imgsrc { get; set; }
    public string Caption { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public string Pos { get; set; }
    public string Icon { get; set; }
  }

  public class LinkData : Model.LinkData {
    public List<string> Points { get; set; }
    public string FromSpot { get; set; }
  }

}
