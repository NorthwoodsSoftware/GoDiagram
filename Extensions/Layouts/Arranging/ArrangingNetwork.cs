using System;
using System.Collections.Generic;
using System.Text;

namespace Northwoods.Go.Layouts.Extensions {
  /// <summary>
  /// This class represents an abstract graph of <see cref="ArrangingVertex"/>es and <see cref="ArrangingEdge"/>s
  /// that can be constructed based on the <see cref="Node"/>s and <see cref="Link"/>s of a <see cref="Diagram"/>
  /// so that the <see cref="ArrangingLayout"/> can operate independently of the diagram until it
  /// is time to commit any node positioning or link routing.
  /// </summary>
  /// <remarks>
  /// This class inherits from <see cref="GenericNetwork{V, E, Y}"/>.
  /// </remarks>
  public class ArrangingNetwork : GenericNetwork<ArrangingVertex, ArrangingEdge, ArrangingLayout> {
    /// <inheritdoc cref="GenericNetwork{V, E, Y}.GenericNetwork()"/>
    public ArrangingNetwork() : base() { }

    /// <inheritdoc cref="GenericNetwork{V, E, Y}.GenericNetwork(Y)"/>
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
  /// This class inherits from <see cref="GenericNetwork{V, E, Y}.Vertex"/>.
  /// </remarks>
  public class ArrangingVertex : ArrangingNetwork.Vertex {
    /// <inheritdoc cref="GenericNetwork{V, E, Y}.Vertex.Vertex()"/>
    public ArrangingVertex() : base() { }

    /// <inheritdoc cref="GenericNetwork{V, E, Y}.Vertex.Vertex(GenericNetwork{V, E, Y})"/>
    public ArrangingVertex(ArrangingNetwork network) : base(network) { }
  }

  /// <summary>
  /// This holds <see cref="ArrangingLayout"/>-specific information about <see cref="Link"/>s.
  /// </summary>
  /// <remarks>
  /// This class inherits from <see cref="GenericNetwork{V, E, Y}.Edge"/>.
  /// </remarks>
  public class ArrangingEdge : ArrangingNetwork.Edge {
    /// <inheritdoc cref="GenericNetwork{V, E, Y}.Edge.Edge()"/>
    public ArrangingEdge() : base() { }

    /// <inheritdoc cref="GenericNetwork{V, E, Y}.Edge.Edge(GenericNetwork{V, E, Y})"/>
    public ArrangingEdge(ArrangingNetwork network) : base(network) { }
  }
}
