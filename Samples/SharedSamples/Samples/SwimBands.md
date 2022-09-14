Unlike swim lane diagrams where the nodes are supposed to stay in their lanes,
layer bands run perpendicular to the growth direction of the layout.
  
This sample uses a custom [TreeLayout] that overrides the [TreeLayout.CommitLayers] method
in order to specify the position and size of each "band" that surrounds a layer of the tree.
The "bands" are held in a single Part that is bound to a particular node data object whose key is "_BANDS".
The headers, and potentially any other information that you might want to display in the headers,
are stored in this "_BANDS" object in a List.
  
This sample can be adapted to use a [GraphLinksModel] instead of a [TreeModel]
and a [LayeredDigraphLayout] instead of a [TreeLayout].