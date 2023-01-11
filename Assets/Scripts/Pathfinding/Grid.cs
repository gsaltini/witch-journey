using System.Text.RegularExpressions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public Transform playerTransform, enemyTransform;
    public LayerMask unwalkableMask;
    public LayerMask obstacleMask;
    public Vector2 worldSize;
    public float nodeRadius;
    public Node[,] grid;
    public List<Node> path;
    
    
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    // Start is called before the first frame update
    void Awake() {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = (int) Math.Round(worldSize.x / nodeDiameter);
        gridSizeY = (int) Math.Round(worldSize.y / nodeDiameter);
        CreateGrid();
    }

    
    void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(transform.position, new Vector3(worldSize.x, worldSize.y, 1.0f));

        if (grid != null) {
            
            Node playerNode = GetNodeFromWorldPosition(playerTransform.position);
            Node enemyNode = GetNodeFromWorldPosition(enemyTransform.position);

            foreach (Node n in grid) {
                if (n.walkable && n.flyable) {
                    Gizmos.color = Color.white;
                } else if (n.flyable) {
                    Gizmos.color = Color.magenta;
                } else {
                    Gizmos.color = Color.red;
                }
                if (playerNode == n) {
                    Gizmos.color = Color.green;
                }
                if (enemyNode == n) {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
            }
        }
    }

    void CreateGrid() {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - (Vector3.right * worldSize.x / 2) - (Vector3.up * worldSize.y / 2); 

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                Vector3 worldPosition = worldBottomLeft + Vector3.right * (nodeDiameter * x + nodeRadius) + Vector3.up * (nodeDiameter * y + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPosition, nodeRadius-0.05f, unwalkableMask));
                grid[x,y] = new Node(walkable, false, worldPosition, x, y);
            }
        }

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 1; y < gridSizeY; y++) {
                if (y < gridSizeY - 1) {
                    if (grid[x,y].walkable && !grid[x, y-1].walkable && grid[x, y+1].walkable) {
                        grid[x, y].SetFlyable(true);
                    }
                    if (grid[x,y].walkable && (grid[x, y-1].walkable || grid[x, y-1].flyable) && grid[x, y+1].walkable) {
                        grid[x,y].SetFlyable(true);
                        grid[x,y].SetWalkable(false);
                    }
                } else {
                    if (grid[x,y].walkable && !grid[x, y-1].walkable) {
                        grid[x,y].SetFlyable(true);
                    }
                    if (grid[x,y].walkable && (grid[x, y-1].walkable || grid[x, y-1].flyable)) {
                        grid[x,y].SetFlyable(true);
                        grid[x,y].SetWalkable(false);
                    }
                }
            }
        }
    }

    public Node GetNodeFromWorldPosition(Vector3 worldPosition) {
        float percentX = Math.Clamp((worldPosition.x + worldSize.x/2) / worldSize.x, 0, 1);
        float percentY = Math.Clamp((worldPosition.y + worldSize.y/2) / worldSize.y, 0, 1);
        
        int gridX = (int) Math.Round((gridSizeX-1) * percentX);
        int gridY = (int) Math.Round((gridSizeY-1) * percentY);

        return grid[gridX,gridY];
    }

    public List<Node> GetNodeNeighbors(Node node) {
        List<Node> list = new List<Node>();
        
        if (node.gridX - 1 > 0) {
            list.Add(grid[node.gridX-1, node.gridY]);
        }
        if (node.gridY - 1 > 0) {
            list.Add(grid[node.gridX, node.gridY-1]);
        }
        if (node.gridX + 1 < gridSizeX) {
            list.Add(grid[node.gridX+1, node.gridY]);
        }
        if (node.gridY + 1 < gridSizeY) {
            list.Add(grid[node.gridX, node.gridY+1]);
        }
        if (node.gridX - 1 > 0 && node.gridY - 1 > 0) {
            list.Add(grid[node.gridX-1, node.gridY-1]);
        }
        if (node.gridX - 1 > 0 && node.gridY + 1 < gridSizeY) {
            list.Add(grid[node.gridX-1, node.gridY+1]);
        }
        if (node.gridX + 1 < gridSizeX && node.gridY + 1 < gridSizeY) {
            list.Add(grid[node.gridX+1, node.gridY+1]);
        }
        if (node.gridX + 1 < gridSizeX && node.gridY - 1 > 0) {
            list.Add(grid[node.gridX+1, node.gridY-1]);
        }
        
        return list;
    }

    public int GetGridSizeX() {
        return gridSizeX;
    }

    public int GetGridSizeY() {
        return gridSizeY;
    }

}
