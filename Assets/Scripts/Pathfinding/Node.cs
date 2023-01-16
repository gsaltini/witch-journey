using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {
    public bool walkable, flyable;
    public Vector3 worldPosition;
    public int gCost;
    public int hCost;
    public int gridX;
    public Node parent;
    public int gridY;
    int heapIndex;

    public Node(bool _walkable, bool _flyable, Vector3 _worldPosition, int _gridX, int _gridY) {
        walkable = _walkable;
        flyable = _flyable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
    }

    public void SetFlyable(bool _flyable) {
        flyable = _flyable;
    }

    public void SetWalkable(bool _walkable) {
        walkable = _walkable;
    }

    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare) {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
