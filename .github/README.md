# Lab10

https://github.com/user-attachments/assets/0ec911ca-cd5d-4c83-9b9c-8705f8343358

**How does A\* pathfinding calculate and prioritize paths?**

A* calculates path cost for each node and draws a path based on the lowest estimated cost. The cost is estimated using the measured cost up to the node plus the heuristic estimate to the goal. This heuristic can be based on manhattan distance, euclidean distance, and/or diagonal distance.

**What challenges arise when dynamically updating obstacles in real-time?**

While dynamically updating obstacles, it's possible for there the grid to end up with no path possible from start to goal. Even if there is a path, new obstacles may require recalculate the pathfinding all over again, unless an advanced algorithm is used like Dynamic Astar, Local Repair Astar, or Hierarchical Pathfinding.

**How could you adapt this code for larger grids or open-world settings?**

For larger grids, you could convert this Breadth-First Search pathfinding into A* so that fewer nodes need to be searched. For even more flexibility, like for open-world settings, you could adapt this code to use Unity's NavMeshes which come with a lot of features out-of-the-box.

**What would your approach be if you were to add weighted cells (e.g., "difficult terrain" areas)?**

My approach would be to modify A*'s cost and heuristic cost functions to take into account each cell's "weight". Cells with higher weights ("difficult terrain") would result in a higher path cost, so the A* algorithm would prefer paths with less difficult terrain.
