/*
*  Copyright (C) 1998-2023 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main Go library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

namespace Northwoods.Go.Layouts.Extensions {
  /// <summary>
  /// This class represents an abstract graph of <see cref="SpiralVertex"/>es and <see cref="SpiralEdge"/>s
  /// that can be constructed based on the <see cref="Node"/>s and <see cref="Link"/>s of a <see cref="Diagram"/>
  /// so that the <see cref="SpiralLayout"/> can operate independently of the diagram until it
  /// is time to commit any node positioning or link routing.
  /// </summary>
  /// <remarks>
  /// This class inherits from <see cref="Network{V, E, Y}"/>.
  /// </remarks>
  public class SpiralNetwork : Network<SpiralVertex, SpiralEdge, SpiralLayout> {
    /// <inheritdoc cref="Network{V, E, Y}.Network(Y)"/>
    public SpiralNetwork(SpiralLayout layout) : base(layout) { }

    /// <inheritdoc/>
    public override SpiralVertex CreateVertex() {
      return new SpiralVertex(this);
    }

    /// <inheritdoc/>
    public override SpiralEdge CreateEdge() {
      return new SpiralEdge(this);
    }
  }

  /// <summary>
  /// This holds <see cref="SpiralLayout"/>-specific information about <see cref="Node"/>s.
  /// </summary>
  /// <remarks>
  /// This class inherits from <see cref="Network{V, E, Y}.Vertex"/>.
  /// </remarks>
  public class SpiralVertex : SpiralNetwork.Vertex {
    /// <inheritdoc cref="Network{V, E, Y}.Vertex.Vertex(Network{V, E, Y})"/>
    public SpiralVertex(SpiralNetwork network) : base(network) { }
  }

  /// <summary>
  /// This holds <see cref="SpiralLayout"/>-specific information about <see cref="Link"/>s.
  /// </summary>
  /// <remarks>
  /// This class inherits from <see cref="Network{V, E, Y}.Edge"/>.
  /// </remarks>
  public class SpiralEdge : SpiralNetwork.Edge {
    /// <inheritdoc cref="Network{V, E, Y}.Edge.Edge(Network{V, E, Y})"/>
    public SpiralEdge(SpiralNetwork network) : base(network) { }
  }
}
