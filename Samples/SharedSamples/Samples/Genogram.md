A *genogram* or *pedigree chart* is an extended family tree diagram that displays information about each person or each relationship.

Note that the term "marriage" here does not refer to a legal or cultural kind of relationship,
but simply one representing the female and male genetic sources for any children.

There are functions that convert an attribute value into a brush color or Shape geometry,
to be added to the Node representing the person.

A custom [LayeredDigraphLayout] does the layout, assuming there is a central person whose mother and father
each have their own ancestors.  In this case we focus on "Bill", but any of the children of "Alice" and "Aaron" would work.
The `_Add` method allows husband/wife pairs to be represented by a single [LayeredDigraphVertex].

For a simpler family tree, see the [family tree sample](demo/FamilyTree).

The node data representing the people, processed by the `_SetupDiagram` method is below. The properties are:
  + **Key**, the unique ID of the person
  + **s**, the person's name
  + **s**, the person's sex
  + **m**, the person's mother's key
  + **f**, the person's father's key
  + **ux**, the person's wife
  + **vir**, the person's husband
  + **a**, a list of the attributes or markers that the person has