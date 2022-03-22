using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts.Extensions;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.Wordcloud {
  [ToolboxItem(false)]
  public partial class WordcloudControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public WordcloudControl() {
      InitializeComponent();

      Setup();

      btnGenerate.Click += (e, obj) => _RebuildGraph();
      checkBxRandomize.CheckedChanged += (e, obj) => _RebuildGraph();

      goWebBrowser1.Html = @"
   <p>
    This sample demonstrates a simple way to create a wordcloud visualization
    using the PackedLayout extension. The most frequent words in the text are
    shown larger, and common english words such as 'the' or 'of' are skipped.
    This example shows the text from the GoDiagram home page,
    but you can easily create your own by copying text into the box above.
  </p>
";

    }

    private string _WordString = @"
          GoDiagram is a feature-rich .NET library for implementing custom interactive diagrams and complex visualizations across modern web browsers and platforms. GoDiagram makes constructing diagrams of complex nodes, links, and groups easy with customizable templates and layouts.

          GoDiagram offers many advanced features for user interactivity such as drag-and-drop, copy-and-paste, in-place text editing, tooltips, context menus, automatic layouts, templates, data binding and models, transactional state and undo management, palettes, overviews, event handlers, commands, and an extensible tool system for custom operations.

          Build custom modeling environments and domain-specific visual languages using the powerful features of GoDiagram. Provide both a system editor and a read-only status monitor using shared code and templates. Simultaneously show alternative visualizations of the same data in different diagrams. Implement drill-down using expansion of subtrees and subgraphs or a detailed view in another diagram.

          Yet GoDiagram is remarkably simple for such a powerful and flexible system. Our thorough documentation introduces the basic concepts and demonstrate typical features that most apps want to offer. Nodes and links can be arbitrarily detailed according to the needs of the application. The API consists of only a few dozen important classes which encapsulate many useful features that interact with each other. There are many properties that permit simple customizations; some methods may be overridden for more complicated customizations.

          Explore
          Start from over 200 sample apps that demonstrate flowcharts, org charts, mind maps, UML diagrams, BPMN diagrams, graph editors, data visualization, custom tools and layouts, and much more.

          Learn
          Get started with a step-by-step description of how to build a .NET diagram in WinForms using GoDiagram and some model data.

          Download
          Get a copy of the library and all of the samples, extensions, and documentation. Search the C# code and modify the samples to start your app.

          Technical Introduction
          Read our introduction for a overview of GoDiagram concepts and features..

          Unlimited Evaluation
          Evaluate the full library without any limitations. Register with us and get free support for a month to help build your app.

          API Documentation
          Read our comprehensive documentation for an in-depth reference of the properties and methods of all of the .NET classes.


          When upgrading to a newer version, please read the Change Log.

          You can explore the newest features and samples in the GoDiagram Beta for the next version, if a next version is available. Read the beta change log for the new features. Please note that our GitHub directory does not have the beta files.
    ";


    private void Setup() {

      myDiagram = diagramControl1.Diagram;

      myDiagram.IsReadOnly = true;
      myDiagram.AutoScale = AutoScale.Uniform;

      // nodes have a template with bindings for size, shape, and fill color
      myDiagram.NodeTemplate = new Node(PanelLayoutAuto.Instance).Add(
        new TextBlock()
          .Bind("Text", "Text")
      ).Bind("Scale", "Scale");

      txtWords.Text = _WordString;

      // create a layout with the default values
      _RebuildGraph();
    }

    // returns a map (word -> frequency) of words in the text
    private Dictionary<string, int> _GetWordFrequencies() {
      // list of english words to exclude from the wordcloud
      var stopWords = new List<string>() {
        "i", "me", "my", "myself", "we", "us", "our", "ours", "ourselves", "you", "your", "yours",
        "yourself", "yourselves", "he", "him", "his", "himself", "she", "her", "hers", "herself", "it", "its", "itself",
        "they", "them", "their", "theirs", "themselves", "what", "which", "who", "whom", "whose", "this", "that", "these",
        "those", "am", "is", "are", "was", "were", "be", "been", "being", "have", "has", "had", "having", "do", "does",
        "did", "doing", "will", "would", "should", "can", "could", "ought", "i\"m", "you\"re", "he\"s", "she\"s", "it\"s",
        "we\"re", "they\"re", "i\"ve", "you\"ve", "we\"ve", "they\"ve", "i\"d", "you\"d", "he\"d", "she\"d", "we\"d",
        "they\"d", "i\"ll", "you\"ll", "he\"ll", "she\"ll", "we\"ll", "they\"ll", "isn\"t", "aren\"t", "wasn\"t",
        "weren\"t", "hasn\"t", "haven\"t", "hadn\"t", "doesn\"t", "don\"t", "didn\"t", "won\"t", "wouldn\"t", "shan\"t",
        "shouldn\"t", "can\"t", "cannot", "couldn\"t", "mustn\"t", "let\"s", "that\"s", "who\"s", "what\"s", "here\"s",
        "there\"s", "when\"s", "where\"s", "why\"s", "how\"s", "a", "an", "the", "and", "but", "if", "or", "because", "as",
        "until", "while", "of", "at", "by", "for", "with", "about", "against", "between", "into", "through", "during",
        "before", "after", "above", "below", "to", "from", "up", "upon", "down", "in", "out", "on", "off", "over", "under",
        "again", "further", "then", "once", "here", "there", "when", "where", "why", "how", "all", "any", "both", "each",
        "few", "more", "most", "other", "some", "such", "no", "nor", "not", "only", "own", "same", "so", "than", "too",
        "very", "say", "says", "said", "shall"
      };

      var words = _WordString.ToLower().Split(' ', '\n');

      var frequencies = new Dictionary<string, int>();
      foreach (var word in words) {
        string trimmedWord = word.Trim();
        if (stopWords.IndexOf(trimmedWord) > -1 || trimmedWord == "") { // skip stop words
          continue;
        }
        if (int.TryParse(trimmedWord, out var _)) {
          continue;
        }

        if (frequencies.ContainsKey(trimmedWord)) {
          frequencies[trimmedWord]++;
        } else {
          frequencies.Add(trimmedWord, 1);
        }
      }

      return frequencies;
    }

    // creates a list of nodes from the frequency map
    private List<NodeData> _GenerateNodeData(Dictionary<string, int> frequencies) {
      // convert map to array
      var freqArr = new List<FreqWord>();
      foreach (var it in frequencies) {
        freqArr.Add(new FreqWord { _Word = it.Key, _Freq = it.Value });
      }

      // sort the frequency array in descending order
      freqArr.Sort((a, b) => {
        return b._Freq - a._Freq;
      });

      // create an array of nodes, scaled appropriately by frequency
      var nodes = new List<NodeData>();
      var singleOccurrenceCount = 0;
      for (var i = 0; i < freqArr.Count; i++) {
        if (freqArr[i]._Freq == 1) {
          singleOccurrenceCount++;
        }
        // stop creating nodes if we've already added too many that only occurred once, or if we're added 150
        if ((i > 25 && singleOccurrenceCount > 0.25 * i) || i > 150) {
          break;
        }
        // scale node size logarithmically with frequency
        var scale = 2 * Math.Log(freqArr[i]._Freq) + 1;
        nodes.Add(new NodeData {
          Text = freqArr[i]._Word,
          Scale = scale
        });
      }

      // TODO randomize order if checkbox is checked
      // randomize order if checkbox is checked
      if (checkBxRandomize.CheckState == System.Windows.Forms.CheckState.Checked) {
          int i, j;
        NodeData x;
        var rand = new Random();
        for (i = nodes.Count - 1; i > 0; i--) {
          j = (int)Math.Floor((double)rand.Next(i+1));
            x = nodes[i];
            nodes[i] = nodes[j];
            nodes[j] = x;
          }
        }

        return nodes;
    }

    private void _RebuildGraph() {
      myDiagram = diagramControl1.Diagram;

      _WordString = txtWords.Text;
      var frequencies = _GetWordFrequencies();
      var nodeData = _GenerateNodeData(frequencies);
      myDiagram.Model = new Model {
        NodeDataSource = nodeData
      };

      myDiagram.StartTransaction("create wordcloud");
      myDiagram.Layout = new PackedLayout {
        Spacing = 5,
        ArrangesToOrigin = false
      };
      myDiagram.CommitTransaction("create wordcloud");
    }

  }

  public class Model : Model<NodeData, int, object> { }

  public class NodeData : Model.NodeData {
    public double Scale { get; set; }
  }

  struct FreqWord {
    public string _Word;
    public int _Freq;
  }

}
