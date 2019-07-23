using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.IO;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentExample : MonoBehaviour
{
    // Inspector Assigned Variable
    public AIWaypointNetwork    WaypointNetwork  = null;
    public int                  CurrentIndex     = 0;
    public bool                 HasPath          = false;
    public bool                 PathPending      = false;
    public bool                 PathStale        = false;
    public NavMeshPathStatus    PathStatus       = NavMeshPathStatus.PathInvalid;
    public AnimationCurve       JumpCurve        = new AnimationCurve();

    //Private Members
    private NavMeshAgent _navAgent = null;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();

        if (WaypointNetwork == null) return;

        SetNextDestination(false);
    }

    void SetNextDestination(bool increment)
    {
        if (!WaypointNetwork) return;

        int incStep = increment ? 1 : 0;
        Transform nextWaypointTransform = null;

        int nextWayPoint = (CurrentIndex + incStep >= WaypointNetwork.Waypoints.Count) 
                ? 0 : CurrentIndex + incStep;
            nextWaypointTransform = WaypointNetwork.Waypoints[nextWayPoint];

            if (nextWaypointTransform != null)
            {
                CurrentIndex = nextWayPoint;
                _navAgent.destination = nextWaypointTransform.position;
                return;
            }
        CurrentIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        HasPath = _navAgent.hasPath;
        PathPending = _navAgent.pathPending;
        PathStale = _navAgent.isPathStale;
        PathStatus = _navAgent.pathStatus;

        if (_navAgent.isOnOffMeshLink)
        if (_navAgent.isOnOffMeshLink)
        {
            StartCoroutine(Jump(1.0f));
            return;
        }

        // if we don't have a path and one isn't pending then set the next
        // waypoint as the target, otherwise if the path is stale regenerate path
        if ((_navAgent.remainingDistance <= _navAgent.stoppingDistance && !PathPending) || PathStatus == NavMeshPathStatus.PathInvalid /*|| PathStatus == NavMeshPathStatus.PathPartial*/)
            SetNextDestination(true);
        else
        if (_navAgent.isPathStale)
            SetNextDestination(false);


        IEnumerator Jump(float duration)
        {
            OffMeshLinkData data = _navAgent.currentOffMeshLinkData;
            Vector3 startPos = _navAgent.transform.position;
            Vector3 endPos = data.endPos + (_navAgent.baseOffset * Vector3.up);
            float time = 0.0f;

            while (time < duration)
            {
                float t = time / duration;
                _navAgent.transform.position = Vector3.Lerp(startPos, endPos, t) + (JumpCurve.Evaluate(t) * Vector3.up);
                time += Time.deltaTime;
                yield return null;
            }

            _navAgent.CompleteOffMeshLink();
        }
    }
}      
