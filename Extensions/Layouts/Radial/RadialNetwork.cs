/*
*  Copyright (C) 1998-2022 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main Go library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

using System.Collections.Generic;

namespace Northwoods.Go.Layouts.Extensions {
  /// <summary>
  /// This class represents an abstract graph of <see cref="RadialVertex"/>es and <see cref="RadialEdge"/>s
  /// that can be constructed based on the <see cref="Node"/>s and <see cref="Link"/>s of a <see cref="Diagram"/>
  /// so that the <see cref="RadialLayout"/> can operate independently of the diagram until it
  /// is time to commit any node positioning or link routing.
  /// </summary>
  /// <remarks>
  /// This class inherits from <see cref="GenericNetwork{V, E, Y}"/>.
  /// </remarks>
  public class RadialNetwork : GenericNetwork<RadialVertex, RadialEdge, RadialLayout> {
    /// <inheritdoc cref="GenericNetwork{V, E, Y}.GenericNetwork(Y)"/>
    public RadialNetwork(RadialLayout layout) : base(layout) { }

    /// <inheritdoc/>
    public override RadialVertex CreateVertex() {
      return new RadialVertex(this);
    }

    /// <inheritdoc/>
    public override RadialEdge CreateEdge() {
      return new RadialEdge(this);
    }
  }

  /// <summary>
  /// This holds <see cref="RadialLayout"/>-specific information about <see cref="Node"/>s.
  /// </summary>
  /// <remarks>
  /// This class inherits from <see cref="GenericNetwork{V, E, Y}.Vertex"/>.
  /// </remarks>
  public class RadialVertex : RadialNetwork.Vertex {
    private int _Distance = int.MaxValue;
    private bool _Laid = false;
    private double _Angle = 0;
    private double _Sweep = 0;
    private double _Radius = 0;
    private List<RadialVertex> _Children = null;

    /// <inheritdoc cref="GenericNetwork{V, E, Y}.Vertex.Vertex(GenericNetwork{V, E, Y})"/>
    public RadialVertex(RadialNetwork network) : base(network) { }

    /// <summary>
    /// Number of layers from the root, non-negative integers.
    /// </summary>
    public int Distance {
      get {
        return _Distance;
      }
      set {
        if (_Distance != value) {
          _Distance = value;
        }
      }
    }

    /// <summary>
    /// Used internally to keep track.
    /// </summary>
    public bool Laid {
      get {
        return _Laid;
      }
      set {
        if (_Laid != value) {
          _Laid = value;
        }
      }
    }

    /// <summary>
    /// The direction at which the node is placed relative to the root node.
    /// </summary>
    public double Angle {
      get {
        return _Angle;
      }
      set {
        if (_Angle != value) {
          _Angle = value;
        }
      }
    }

    /// <summary>
    /// The angle subtended by the vertex.
    /// </summary>
    public double Sweep {
      get {
        return _Sweep;
      }
      set {
        if (_Sweep != value) {
          _Sweep = value;
        }
      }
    }

    /// <summary>
    /// The inner radius of the layer containing this vertex.
    /// </summary>
    public double Radius {
      get {
        return _Radius;
      }
      set {
        if (_Radius != value) {
          _Radius = value;
        }
      }
    }

    /// <summary>
    /// A list of the RadialVertex children of this vertex, when treating the network as a tree.
    /// </summary>
    public List<RadialVertex> Children {
      get {
        return _Children;
      }
      set {
        if (_Children != value) {
          _Children = value;
        }
      }
    }
  }

  /// <summary>
  /// This holds <see cref="RadialLayout"/>-specific information about <see cref="Link"/>s.
  /// </summary>
  /// <remarks>
  /// This class inherits from <see cref="GenericNetwork{V, E, Y}.Edge"/>.
  /// </remarks>
  public class RadialEdge : RadialNetwork.Edge {
    /// <inheritdoc cref="GenericNetwork{V, E, Y}.Edge.Edge(GenericNetwork{V, E, Y})"/>
    public RadialEdge(RadialNetwork network) : base(network) { }
  }
}
