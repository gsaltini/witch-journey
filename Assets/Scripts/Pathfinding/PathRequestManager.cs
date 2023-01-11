using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathResult> results = new Queue<PathResult>();
    static PathRequestManager instance;
    Pathfinding pathfinding;

    void Awake () {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    void Update () {
        if (results.Count > 0) {
            int itemsInQueue = results.Count;
            lock (results) {
                PathResult pathResult = results.Dequeue();
                pathResult.callback(pathResult.path, pathResult.success);
            }
        }
    }

    public static void RequestPath(PathRequest request) { 
        ThreadStart threadStart = delegate {
            instance.pathfinding.FindPath(request, instance.FinishedProcessingPath);
        };
        threadStart.Invoke();
    } 

    public void FinishedProcessingPath(PathResult pathResult) {
        lock (results) {
            results.Enqueue(pathResult);
        }
    } 
}

public struct PathResult {
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;

    public PathResult(Vector3[] _path, bool _success, Action<Vector3[], bool> _callback) {
        path = _path;
        success = _success;
        callback = _callback;
    }
}

public struct PathRequest {
    public Vector3 startPosition;
    public Vector3 endPosition;
    public Action<Vector3[], bool> callback;
    public bool flying;

    public PathRequest(Vector3 _startPosition, Vector3 _endPosition, Action<Vector3[], bool> _callback, bool _flying) {
        startPosition = _startPosition;
        endPosition = _endPosition;
        callback = _callback;
        flying = _flying;
    }
}

