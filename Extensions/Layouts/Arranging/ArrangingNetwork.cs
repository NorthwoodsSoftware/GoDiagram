/*
*  Copyright (C) 1998-2024 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoDiagram library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

namespace Northwoods.Go.Layouts.Extensions {
  /// <summary>
  /// This class represents an abstract graph of <see cref="ArrangingVertex"/>es and <see cref="ArrangingEdge"/>s
  /// that can be constructed based on the <see cref="Node"/>s and <see cref="Link"/>s of a <see cref="Diagram"/>
  /// so that the <see cref="ArrangingLayout"/> can operate independently of the diagram until it
  /// is time to commit any node positioning or link routing.
  /// </summary>
  /// <remarks>
  /// This class inherits from <see cref="Network{V, E, Y}"/>.
  /// </remarks>
  public class ArrangingNetwork : Network<ArrangingVertex, ArrangingEdge, ArrangingLayout> {
    /// <inheritdoc cref="Network{V, E, Y}.Network(Y)"/>
    public ArrangingNetwork(ArrangingLayout layout) : base(layout) { }

    /// <inheritdoc/>
    public override ArrangingVertex CreateVertex() {
      return new ArrangingVertex(this);
    }

    /// <inheritdoc/>
    public override ArrangingEdge CreateEdge() {
      return new ArrangingEdge(this);
    }
  }

  /// <summary>
  /// This holds <see cref="ArrangingLayout"/>-specific information about <see cref="Node"/>s.
  /// </summary>
  /// <remarks>
  /// This class inherits from <see cref="Network{V, E, Y}.Vertex"/>.
  /// </remarks>
  public class ArrangingVertex : ArrangingNetwork.Vertex {
    /// <inheritdoc cref="Network{V, E, Y}.Vertex.Vertex(Network{V, E, Y})"/>
    public ArrangingVertex(ArrangingNetwork network) : base(network) { }
  }

  /// <summary>
  /// This holds <see cref="ArrangingLayout"/>-specific information about <see cref="Link"/>s.
  /// </summary>
  /// <remarks>
  /// This class inherits from <see cref="Network{V, E, Y}.Edge"/>.
  /// </remarks>
  public class ArrangingEdge : ArrangingNetwork.Edge {
    /// <inheritdoc cref="Network{V, E, Y}.Edge.Edge(Network{V, E, Y})"/>
    public ArrangingEdge(ArrangingNetwork network) : base(network) { }
  }
}
