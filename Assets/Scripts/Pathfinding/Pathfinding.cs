using System.Runtime.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    Grid grid;

    void Awake() {
        grid = GetComponent<Grid>();
    }

    // void Update() {
    //     FindPath(player.position, target.position);
    // }

    public void FindPath(PathRequest pathRequest, Action<PathResult> callback) {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.GetNodeFromWorldPosition(pathRequest.startPosition);
        startNode.gCost = 0;
        startNode.parent = null;
        Node endNode = grid.GetNodeFromWorldPosition(pathRequest.endPosition);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0) {
            Node current = openSet.RemoveFirst();

            closedSet.Add(current);

            if (current == endNode) {
                sw.Stop();
                print("Path found " + sw.ElapsedMilliseconds + " ms");
                pathSuccess = true;
                break;
            }

            List<Node> neighbors = grid.GetNodeNeighbors(current);

            foreach (Node node in neighbors) {
                if ((!node.walkable && !node.flyable) || (node.flyable && !node.walkable && !pathRequest.flying) || closedSet.Contains(node)) {
                    continue;
                }

                int newMovementCostToNeighbor = current.gCost + GetDistance(current, node);
                if (newMovementCostToNeighbor < node.gCost || !openSet.Contains(node)) {
                    node.gCost = newMovementCostToNeighbor;
                    node.hCost = GetDistance(node, endNode);
                    node.parent = current;

                    if (!openSet.Contains(node)) {
                        openSet.Add(node);
                    }
                }
            }
        }
        if (pathSuccess) {
            waypoints = RetracePath(startNode, endNode);
            pathSuccess = waypoints.Length > 0;
        }
        callback(new PathResult(waypoints, pathSuccess, pathRequest.callback));
    }

    int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY) {
            return 14 * dstY + 10 * (dstX - dstY);
        } else {
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }

    // void RetracePath(Node startNode, Node endNode) {
    //     //List<Vector3> path = new List<Vector3>();
    //     List<Node> path = new List<Node>();
    //     Node current = endNode;
    //     //Debug.Log("Hello");

    //     while(current != startNode) {
    //         path.Add(current);
    //         //nodePath.Add(current);
    //         current = current.parent;
    //     }
    //     path.Reverse();

    //     grid.path = path;
    //     // Vector3[] waypoints = path.ToArray();
    //     // Array.Reverse(waypoints);
    //     //return waypoints;
    // }

    Vector3[] RetracePath(Node startNode, Node endNode) {
        List<Vector3> path = new List<Vector3>();
        Node current = endNode;

        while (current != startNode) {
            path.Add(current.worldPosition);
            current = current.parent;
        }

        Vector3[] waypoints = path.ToArray();
        Array.Reverse(waypoints);
        return waypoints;

    }

    Node FindLowestF(List<Node> list) {
        Node current = list[0];

        foreach (Node n in list) {
            if (n.gCost < current.gCost) {
                current = n;
            }
        }

        return current;
    }

}
