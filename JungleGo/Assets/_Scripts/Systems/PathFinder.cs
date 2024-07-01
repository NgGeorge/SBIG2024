using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Provides methods for pathfinding using the A* algorithm on a 2D grid.
/// </summary>
/// <remarks>
/// This class implements the A* algorithm to find the shortest path between two points
/// on a 2D grid represented by BoardManager.Instance.Board, where 'X' indicates impassable terrain.
/// </remarks>
public class PathFinder : MonoBehaviour
{
    
    public static PathFinder Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Persist this object across scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Performs A* pathfinding to find the shortest path from start to goal coordinates on a board.
    /// </summary>
    /// <param name="start">The starting coordinates (x, y) on the board.</param>
    /// <param name="goal">The goal coordinates (x, y) to reach on the board.</param>
    /// <returns>
    /// A list of coordinates (x, y) representing the shortest path from start to goal,
    /// or <c>null</c> if no path is found.
    /// </returns>
    public List<(int, int)> AStarPathfind((int, int) start, (int, int) goal)
    {
        var openSet = new HashSet<(int, int)>();
        var cameFrom = new Dictionary<(int, int), (int, int)>();
        var gScore = new Dictionary<(int, int), double>();
        var fScore = new Dictionary<(int, int), double>();

        openSet.Add(start);
        gScore[start] = 0;
        fScore[start] = HeuristicCostEstimate(start, goal);

        while (openSet.Count > 0)
        {
            var current = GetLowestFScoreNode(openSet, fScore);
            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);

            foreach (var neighbor in GetNeighbors(current))
            {
                if (BoardManager.Instance.Board[neighbor.Item1, neighbor.Item2].Type != TileType.Empty) continue;

                double tentativeGScore = gScore[current] + Distance(current, neighbor);

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Reconstructs the path from the start node to the given current node using the cameFrom dictionary.
    /// </summary>
    /// <param name="cameFrom">A dictionary that maps each node to its predecessor along the path.</param>
    /// <param name="current">The current node to reconstruct the path from.</param>
    /// <returns>
    /// A list of nodes representing the reconstructed path from the start node to the current node.
    /// </returns>
    private List<(int, int)> ReconstructPath(Dictionary<(int, int), (int, int)> cameFrom, (int, int) current)
    {
        var totalPath = new List<(int, int)> { current };
        
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }
        
        totalPath.Reverse();
        return totalPath;
    }

    /// <summary>
    /// Finds and returns the node with the lowest f-score from the given open set.
    /// </summary>
    /// <param name="openSet">The set of nodes to search for the lowest f-score.</param>
    /// <param name="fScore">A dictionary that maps nodes to their f-scores.</param>
    /// <returns>
    /// The node with the lowest f-score from the open set.
    /// If no node is found, (-1, -1) is returned.
    /// </returns>
    private (int, int) GetLowestFScoreNode(HashSet<(int, int)> openSet, Dictionary<(int, int), double> fScore)
    {
        (int, int) lowest = (-1, -1);
        double lowestScore = double.PositiveInfinity;
        
        foreach (var node in openSet)
        {
            double score = fScore.ContainsKey(node) ? fScore[node] : double.PositiveInfinity;
            if (score < lowestScore)
            {
                lowest = node;
                lowestScore = score;
            }
        }
        
        return lowest;
    }

    /// <summary>
    /// Computes the heuristic cost estimate between two points (a and b) using the Manhattan distance.
    /// </summary>
    /// <param name="a">start coordiates</param>
    /// <param name="b">end coordiantes</param>
    /// <returns>The heuristic cost estimate (Manhattan distance) between points a and b.</returns>
    private double HeuristicCostEstimate((int, int) a, (int, int) b)
    {
        return Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);
    }

    /// <summary>
    /// Computes the Euclidean distance between two points (a and b).
    /// </summary>
    /// <param name="a">The first point (x, y).</param>
    /// <param name="b">The second point (x, y).</param>
    /// <returns>The Euclidean distance between points a and b.</returns>
    private double Distance((int, int) a, (int, int) b)
    {
        return Math
            .Sqrt(Math.Pow(a.Item1 - b.Item1, 2) +
            Math.Pow(a.Item2 - b.Item2, 2));
    }

    /// <summary>
    /// Gets the valid neighboring nodes of a given node within bounds.
    /// </summary>
    /// <param name="node">The coordinates (x, y) of the node.</param>
    /// <returns>A list of neighboring nodes (x, y) that are within bounds.</returns>
    private List<(int, int)> GetNeighbors((int, int) node)
    {
        var neighbors =
            new List<(int, int)> {
                (node.Item1 - 1, node.Item2),
                (node.Item1 + 1, node.Item2),
                (node.Item1, node.Item2 - 1),
                (node.Item1, node.Item2 + 1)
            };

        neighbors.RemoveAll(n => !BoardManager.Instance.IsInBounds(n.Item1, n.Item2));
        return neighbors;
    }
}
