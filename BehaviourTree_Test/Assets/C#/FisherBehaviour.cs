using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FisherBehaviour : MonoBehaviour
{
    public GameObject StartPoint;
    public GameObject BoardPoint;
    public GameObject FishPoint;

    public float WaitTimeOnStart;
    public float WaitTimeOnBoarding;
    public float WaitTimeOnFishing;

    private NavMeshAgent _Agent;
    private BehaviourTree _Tree;
    
    public enum ActionState
    {
        IDLE,
        WORKING
    };
    private ActionState _State = ActionState.IDLE;

    private Node.Status _TreeStatus = Node.Status.RUNNING;

    // Start is called before the first frame update
    void Start()
    {
        _Agent = GetComponent<NavMeshAgent>();
        transform.position = StartPoint.transform.position;

        _Tree = new BehaviourTree();
        Sequence Wander = new Sequence("Wander");
        Leaf goToStartPoint = new Leaf("Go To Start Point", GoToStartPoint);
        Leaf goToBoardPoint = new Leaf("Go To Board Point", GoToBoardPoint);
        Leaf goToFishPoint = new Leaf("Go To Fish Point", GoToFishPoint);

        Leaf waitOnStart = new Leaf("Wait", WaitOnStart);
        Leaf waitOnBoarding = new Leaf("Wait On Boarding", WaitOnBoarding);
        Leaf waitOnFishing = new Leaf("Wait On Fishing", WaitOnFishing);

        Wander.AddChild(waitOnStart);

        Wander.AddChild(goToBoardPoint);
        Wander.AddChild(waitOnBoarding);

        Wander.AddChild(goToFishPoint);
        Wander.AddChild(waitOnFishing);

        Wander.AddChild(goToStartPoint);
        _Tree.AddChild(Wander);

        _Tree.PrintTree();
    }

    public Node.Status GoToBoardPoint()
    {
        return GoToWaypoint(BoardPoint);
    }

    public Node.Status GoToFishPoint()
    {
        return GoToWaypoint(FishPoint);
    }

    public Node.Status GoToStartPoint()
    {
        return GoToWaypoint(StartPoint);
    }

    public Node.Status GoToWaypoint(GameObject waypoint)
    {
        Node.Status s = GoToLocation(waypoint.transform.position);
        return s;
    }

    Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, transform.position);
        if(_State == ActionState.IDLE)
        {
            _Agent.SetDestination(destination);
            _State = ActionState.WORKING;
        }
        else if(Vector3.Distance(_Agent.pathEndPosition, destination) >= 2)
        {
            _State = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if(distanceToTarget < 0.5f)
        {
            _State = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }

        return Node.Status.RUNNING;
    }

    private float elapsedTime = 0;

    public Node.Status Wait(float waitSeconds)
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime <= waitSeconds)
        {
            return Node.Status.RUNNING;
        }
        else
        {
            elapsedTime = 0;
            return Node.Status.SUCCESS;
        }
    }

    public Node.Status WaitOnStart()
    {
        return Wait(WaitTimeOnStart);
    }

    public Node.Status WaitOnBoarding()
    {
        return Wait(WaitTimeOnBoarding);
    }

    public Node.Status WaitOnFishing()
    {
        return Wait(WaitTimeOnFishing);
    }

    // Update is called once per frame
    void Update()
    {
        if(_TreeStatus != Node.Status.SUCCESS)
        {
            _TreeStatus = _Tree.Process();
        }
        else
        {
            _TreeStatus = _Tree.Process();
        }

        Debug.Log(elapsedTime);
    }
}
