using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour {
    const float updatePathDistance = 0.3f;
    //public Transform target;
    int targetIndex;
    private Vector3 target;
    private Vector3 dest;
    [SerializeField] float speed = 1.0f;
    public bool flying;
    private bool followingPath;
    public static Unit instance;

    void Start() {
        instance = this;
    }

    public void StartPath(Vector3 _target) {
        target = _target;
        StopCoroutine("UpdatePathPosition");
        StartCoroutine(UpdatePathPosition(target));
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound, flying);
    }

    public void StartPath(Transform target) {
        StopCoroutine("UpdatePathTransform");
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
        if (successfull) {
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
        Debug.Log("hello9");
        // if (Time.timeSinceLevelLoad < 0.3f) {
        //     yield return new WaitForSeconds(1.0f);
        // }

        PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound, flying));

        float squaredUpdateDistance = updatePathDistance * updatePathDistance;
        Vector3 targetOldPosition = target;

        while (true) {
            yield return new WaitForSeconds(1.0f);
            Debug.Log((target - targetOldPosition).sqrMagnitude);
            if ((target - targetOldPosition).sqrMagnitude > squaredUpdateDistance) {
                Debug.Log("update path distance");
                PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound, flying));
                targetOldPosition = target;
            }
        }
    }

    IEnumerator UpdatePathTransform(Transform target) {
        // if (Time.timeSinceLevelLoad < 0.3f) {
        //     yield return new WaitForSeconds(1.0f);
        // }
        dest = new Vector3(target.position.x, transform.position.y, transform.position.z);
        PathRequestManager.RequestPath(new PathRequest(transform.position, dest, OnPathFound, flying));

        float squaredUpdateDistance = updatePathDistance * updatePathDistance;
        Vector3 targetOldPosition = dest;

        //yield return null;

        while (true) {
            yield return new WaitForSeconds(2.0f);
            if ((target.position - targetOldPosition).sqrMagnitude > squaredUpdateDistance) {
                dest = new Vector3(target.position.x, transform.position.y, transform.position.z);
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound, flying));
                targetOldPosition = target.position;
            }
        }
    }

    IEnumerator FollowPath(Vector3[] path) {

        followingPath = true;
        Debug.Log(path[path.Length - 1]);
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


