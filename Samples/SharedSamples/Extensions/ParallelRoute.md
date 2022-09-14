A [ParallelRouteLink] is a custom [Link] that overrides [Link.ComputePoints]
in order to produce a middle segment that is parallel to the routes of other [ParallelRouteLink]s
connecting the same two ports.