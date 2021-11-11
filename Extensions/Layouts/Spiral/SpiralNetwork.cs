namespace Northwoods.Go.Layouts.Extensions {
  /// <summary>
  /// This class represents an abstract graph of <see cref="SpiralVertex"/>es and <see cref="SpiralEdge"/>s
  /// that can be constructed based on the <see cref="Node"/>s and <see cref="Link"/>s of a <see cref="Diagram"/>
  /// so that the <see cref="SpiralLayout"/> can operate independently of the diagram until it
  /// is time to commit any node positioning or link routing.
  /// </summary>
  /// <remarks>
  /// This class inherits from <see cref="GenericNetwork{V, E, Y}"/>.
  /// </remarks>
  public class SpiralNetwork : GenericNetwork<SpiralVertex, SpiralEdge, SpiralLayout> {
    /// <inheritdoc cref="GenericNetwork{V, E, Y}.GenericNetwork()"/>
    public SpiralNetwork() : base() { }

    /// <inheritdoc cref="GenericNetwork{V, E, Y}.GenericNetwork(Y)"/>
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
  /// This class inherits from <see cref="GenericNetwork{V, E, Y}.Vertex"/>.
  /// </remarks>
  public class SpiralVertex : SpiralNetwork.Vertex {
    /// <inheritdoc cref="GenericNetwork{V, E, Y}.Vertex.Vertex()"/>
    public SpiralVertex() : base() { }

    /// <inheritdoc cref="GenericNetwork{V, E, Y}.Vertex.Vertex(GenericNetwork{V, E, Y})"/>
    public SpiralVertex(SpiralNetwork network) : base(network) { }
  }

  /// <summary>
  /// This holds <see cref="SpiralLayout"/>-specific information about <see cref="Link"/>s.
  /// </summary>
  /// <remarks>
  /// This class inherits from <see cref="GenericNetwork{V, E, Y}.Edge"/>.
  /// </remarks>
  public class SpiralEdge : SpiralNetwork.Edge {
    /// <inheritdoc cref="GenericNetwork{V, E, Y}.Edge.Edge()"/>
    public SpiralEdge() : base() { }

    /// <inheritdoc cref="GenericNetwork{V, E, Y}.Edge.Edge(GenericNetwork{V, E, Y})"/>
    public SpiralEdge(SpiralNetwork network) : base(network) { }
  }
}
