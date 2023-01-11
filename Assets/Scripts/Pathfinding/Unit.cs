using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    const float updatePathDistance = 0.3f;
    //public Transform target;
    int targetIndex;
    private Vector3 target;
    [SerializeField] float speed = 1.0f;
    public bool flying;
    private bool followingPath;
    public static Unit instance;

    void Start() {
        instance = this;
    }

    public void StartPath(Vector3 _target) {
        target = _target;
        StopCoroutine("UpdatePath");
        StartCoroutine(UpdatePathPosition(target));
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound, flying);
    }

    public void StartPath(Transform target) {
        StopCoroutine("UpdatePath");
        StartCoroutine(UpdatePathTransform(target));
    }

    public void StopPathPosition() {
        StopCoroutine("UpdatePathPosition");
        followingPath = false;
    }

    public void StopPathTransform() {
        StopCoroutine("UpdatePathTransform");
        followingPath = false;
    }

    public void OnPathFound(Vector3[] newPath, bool successfull) {
        Debug.Log("fuck");
        if (successfull) {
            Debug.Log("hello");
            Vector3[] path = newPath;
            foreach (Vector3 node in path) {
                //Debug.Log(node);
            }
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine(FollowPath(path));
        }
    }

    IEnumerator UpdatePathPosition(Vector3 target) {
        // if (Time.timeSinceLevelLoad < 0.3f) {
        //     yield return new WaitForSeconds(1.0f);
        // }

        PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound, flying));

        float squaredUpdateDistance = updatePathDistance * updatePathDistance;
        Vector3 targetOldPosition = target;

        while (true) {
            yield return new WaitForSeconds(1.0f);
            if ((target - targetOldPosition).sqrMagnitude > squaredUpdateDistance) {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound, flying));
                targetOldPosition = target;
            }
        }
    }

    IEnumerator UpdatePathTransform(Transform target) {
        // if (Time.timeSinceLevelLoad < 0.3f) {
        //     yield return new WaitForSeconds(1.0f);
        // }

        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound, flying));

        float squaredUpdateDistance = updatePathDistance * updatePathDistance;
        Vector3 targetOldPosition = target.position;

        while (true) {
            yield return new WaitForSeconds(1.0f);
            if ((target.position - targetOldPosition).sqrMagnitude > squaredUpdateDistance) {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound, flying));
                targetOldPosition = target.position;
            }
        }
    }

    IEnumerator FollowPath(Vector3[] path) {
        followingPath = true;
        Vector3 currentWaypoint = path[0];
        Vector3 actualWaypoint = new Vector3(currentWaypoint.x, transform.position.y, currentWaypoint.z);
        while (followingPath) {
            if ((transform.position == currentWaypoint && flying) || (transform.position == actualWaypoint && !flying)) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    // targetIndex = 0;
                    // path = new Vector3[0];
                    followingPath = false;
                    yield break;
                }
                currentWaypoint = path[targetIndex];
                actualWaypoint = new Vector3(currentWaypoint.x, transform.position.y, currentWaypoint.z);
            }
            //Debug.Log(currentWaypoint);
            if (flying) {
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            } else {
                //Debug.Log(actualWaypoint);
                transform.position = Vector3.MoveTowards(transform.position, actualWaypoint, speed * Time.deltaTime);
            }

            // traansform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            
            yield return null;
        }
    }

    public bool GetFollowingPath() {
        return followingPath;
    }   
}


