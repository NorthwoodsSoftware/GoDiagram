This sample demonstrates reading JSON data describing the relative rankings of NFL teams
during the 2015 season and generating a diagram from that data.
The ranking information came from beatgraphs.com.

Unlike most model data, there are no elements describing the nodes --
the node definitions are implicit in the references from the links.
Hence the [Diagram.Model] has [GraphLinksModel.ArchetypeNodeData] set to an object.

The node template uses the **ConvertKeyImage** function to convert the team name
into a URI referring to an image on our web site.