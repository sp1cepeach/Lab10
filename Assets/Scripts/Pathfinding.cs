using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Pathfinding : MonoBehaviour
{
    private List<Vector2Int> path = new List<Vector2Int>();
    [SerializeField] private Vector2Int start;
    [SerializeField] private Vector2Int goal;
    [SerializeField] private int gridSize;
    [SerializeField] private float obstacleProbability;
    [SerializeField] private bool regenerateGrid;
    [SerializeField] private bool addObstacle;
    private Vector2Int next;
    private Vector2Int current;

    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

    private int[,] grid;

    void GenerateRandomGrid(int width, int height, float obstacleProbability)
    {
        grid = new int[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[y, x] = (Random.value <= obstacleProbability) ? 1 : 0;
            }
        }

        grid[start.y, start.x] = 0;
        grid[goal.y, goal.x] = 0;
    }

    void AddObstacle(Vector2Int position)
    {
        grid[position.y, position.x] = 1;
    }

    private void Start()
    {
        GenerateRandomGrid(gridSize, gridSize, obstacleProbability);
        FindPath(start, goal);
    }

    private void OnValidate()
    {
        if (regenerateGrid == true)
        {
            GenerateRandomGrid(gridSize, gridSize, obstacleProbability);
            regenerateGrid = false;
        }

        if (addObstacle == true)
        {
            int x = Random.Range(0, gridSize);
            int y = Random.Range(0, gridSize);
            while (grid[y, x] == 1)
            {
                x = Random.Range(0, gridSize);
                y = Random.Range(0, gridSize);
            }
            AddObstacle(new Vector2Int(x, y));
            addObstacle = false;
        }
        FindPath(start, goal);
    }

    private void OnDrawGizmos()
    {
        float cellSize = 1f;

        if (grid != null)
        {
            // Draw grid cells
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    Vector3 cellPosition = new Vector3(x * cellSize, 0, y * cellSize);
                    Gizmos.color = grid[y, x] == 1 ? Color.black : Color.white;
                    Gizmos.DrawCube(cellPosition, new Vector3(cellSize, 0.1f, cellSize));
                }
            }

            // Draw path
            foreach (var step in path)
            {
                Vector3 cellPosition = new Vector3(step.x * cellSize, 0, step.y * cellSize);
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(cellPosition, new Vector3(cellSize, 0.1f, cellSize));
            }
        }

        // Draw start and goal
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector3(start.x * cellSize, 0, start.y * cellSize), new Vector3(cellSize, 0.1f, cellSize));

        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(goal.x * cellSize, 0, goal.y * cellSize), new Vector3(cellSize, 0.1f, cellSize));
    }

    private bool IsInBounds(Vector2Int point)
    {
        return point.x >= 0 && point.x < grid.GetLength(1) && point.y >= 0 && point.y < grid.GetLength(0);
    }

    private void FindPath(Vector2Int start, Vector2Int goal)
    {
        if (grid == null) return;

        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        frontier.Enqueue(start);

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        cameFrom[start] = start;

        while (frontier.Count > 0)
        {
            current = frontier.Dequeue();

            if (current == goal)
            {
                break;
            }

            foreach (Vector2Int direction in directions)
            {
                next = current + direction;

                if (IsInBounds(next) && grid[next.y, next.x] == 0 && !cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }

        path.Clear();

        if (!cameFrom.ContainsKey(goal))
        {
            Debug.Log("Path not found.");
            return;
        }

        // Trace path from goal to start
        Vector2Int step = goal;
        while (step != start)
        {
            path.Add(step);
            step = cameFrom[step];
        }
        path.Add(start);
        path.Reverse();
    }
}
