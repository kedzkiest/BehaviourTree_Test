using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FisherBehaviour : MonoBehaviour
{
    public GameObject StartPoint;
    public GameObject BoardPoint;
    public GameObject FishPoint;
    public GameObject StorePoint;

    public float WaitTimeOnStart;
    public float WaitTimeOnBoarding;
    public float WaitTimeOnFishing;
    public float WaitTimeOnStoring;

    [Range(0f, 1f)]
    public float SuccessProbabilityOnStart;
    [Range (0f, 1f)]
    public float SuccessProbabilityOnBoarding;

    public int EnoughFishNum;

    public GameObject Tuna;
    public GameObject Salmon;

    [Range(0f, 1f)]
    public float SuccessProbabilityCatchTuna;
    [Range(0f, 1f)]
    public float SuccessProbabilityCatchSalmon;
    [Range(0f, 1f)]
    public float SuccessProbabilityStoreFish;

    private NavMeshAgent _Agent;
    private BehaviourTree _Tree;
    
    public enum ActionState
    {
        IDLE,
        WORKING
    };
    private ActionState _State = ActionState.IDLE;

    private Node.Status _TreeStatus = Node.Status.RUNNING;

    public SEPlayer SEPlayer;

    public FishManager FishManager;

    // Start is called before the first frame update
    void Start()
    {
        _Agent = GetComponent<NavMeshAgent>();
        transform.position = StartPoint.transform.position;

        _Tree = new BehaviourTree();
        Sequence Wander = new Sequence("Wander");

        // condition node
        Leaf hasGotFish = new Leaf("Has Got Fish", HasFish);

        Leaf goToStartPoint = new Leaf("Go To Start Point", GoToStartPoint);
        Leaf goToBoardPoint = new Leaf("Go To Board Point", GoToBoardPoint);
        Leaf goToFishPoint = new Leaf("Go To Fish Point", GoToFishPoint);
        Leaf goToStorePoint = new Leaf("Go To Store Point", GoToStorePoint);

        Selector catchFish = new Selector("Catch A Fish");
        Leaf catchTuna = new Leaf("Catch A Tuna", CatchTuna);
        Leaf catchSalmon = new Leaf("Catch A Salmon", CatchSalmon);
        catchFish.AddChild(catchTuna);
        catchFish.AddChild(catchSalmon);

        Leaf waitOnStart = new Leaf("Wait On Start", WaitOnStart);
        Leaf waitOnBoarding = new Leaf("Wait On Boarding", WaitOnBoarding);
        Leaf waitOnStoring = new Leaf("Wait On Storing", WaitOnStoring);

        Leaf storeFish = new Leaf("Store A Fish", StoreFish);

        Wander.AddChild(goToStartPoint);
        Wander.AddChild(waitOnStart);

        Wander.AddChild(hasGotFish);

        Wander.AddChild(goToBoardPoint);
        Wander.AddChild(waitOnBoarding);

        Wander.AddChild(goToFishPoint);
        // catchFish has a wait in it (doen not have a separate wait method)
        Wander.AddChild(catchFish);

        Wander.AddChild(goToStorePoint);
        Wander.AddChild(waitOnStoring);
        Wander.AddChild(storeFish);

        _Tree.AddChild(Wander);

        _Tree.PrintTree();
    }
    
    public Node.Status HasFish()
    {
        if(FishManager.FishNum < EnoughFishNum)
        {
            SEPlayer.GetComponent<AudioSource>().volume = 1;
            return Node.Status.SUCCESS;
        }

        SEPlayer.GetComponent<AudioSource>().volume = 0;
        return Node.Status.FAILURE;
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

    public Node.Status GoToStorePoint()
    {
        return GoToWaypoint(StorePoint);
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
            float rand = Random.Range(0.0f, 100.0f);
            string s = Node.currentProcess;
            float successProbability = 1;

            switch (s)
            {
                case "Wait On Start":
                    successProbability = SuccessProbabilityOnStart;
                    break;
                case "Wait On Boarding":
                    successProbability = SuccessProbabilityOnBoarding;
                    break;
                case "Wait On Storing":
                    successProbability = SuccessProbabilityStoreFish;
                    break;
            }

            if(rand <= successProbability * 100)
            {
                //Debug.Log("Success");
                if(SEPlayer.gameObject.activeSelf) SEPlayer.PlaySuccessSound();
                return Node.Status.SUCCESS;
            }

            //Debug.Log("Failure");
            if (SEPlayer.gameObject.activeSelf) SEPlayer.PlayFailureSound();
            return Node.Status.FAILURE;

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

    public Node.Status WaitOnStoring()
    {
        Node.Status s =  Wait(WaitTimeOnStoring);

        if(s == Node.Status.FAILURE)
        {
            FishManager.FishNum--;
            // remove last element of fish list
            GameObject go = FishManager.Fish[FishManager.Fish.Count - 1];
            FishManager.Fish.Remove(go);
            Destroy(go);
            
            return Node.Status.FAILURE;
        }

        return s;
    }

    public Node.Status CatchFish()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime <= WaitTimeOnFishing)
        {
            return Node.Status.RUNNING;
        }
        else
        {
            elapsedTime = 0;
            float rand = Random.Range(0.0f, 100.0f);
            string s = Node.currentProcess;
            float successProbability = 1;

            switch (s)
            {
                case "Catch A Tuna":
                    successProbability = SuccessProbabilityCatchTuna;
                    break;
                case "Catch A Salmon":
                    successProbability = SuccessProbabilityCatchSalmon;
                    break;
            }

            if (rand <= successProbability * 100)
            {
                //Debug.Log("Success");
                if (SEPlayer.gameObject.activeSelf) SEPlayer.PlaySuccessSound();
                return Node.Status.SUCCESS;
            }

            //Debug.Log("Failure");
            if (SEPlayer.gameObject.activeSelf) SEPlayer.PlayFailureSound();
            return Node.Status.FAILURE;

        }
    }

    public Node.Status CatchTuna()
    {
        Node.Status s = CatchFish();

        if (s == Node.Status.SUCCESS)
        {
            FishManager.FishNum++;
            GameObject go = Instantiate(Tuna);
            go.transform.position = transform.position + new Vector3(0, 1.0f, 0);
            go.transform.SetParent(transform);
            FishManager.Fish.Add(go);
        }

        return s;
    }

    public Node.Status CatchSalmon()
    {
        Node.Status s = CatchFish();

        if (s == Node.Status.SUCCESS)
        {
            FishManager.FishNum++;
            GameObject go = Instantiate(Salmon);
            go.transform.position = transform.position + new Vector3(-1.3f, 1.0f, -0.4f); ;
            go.transform.SetParent(transform);
            FishManager.Fish.Add(go);
        }

        return s;
    }

    public Node.Status StoreFish()
    {
        GameObject go = transform.GetChild(1).gameObject;
        go.transform.SetParent(null);
        go.transform.position = new Vector3(2 + FishManager.FishNum, 1.36f, 4.4f);

        return Node.Status.SUCCESS;
    }

    // Update is called once per frame
    void Update()
    {
        /* use this for single action flow
        if(_TreeStatus != Node.Status.SUCCESS)
        {
            _TreeStatus = _Tree.Process();
        }
        */

        // use this for resetting tree progress when action failed
        if(_TreeStatus == Node.Status.FAILURE)
        {
            _Tree.ResetTreeProgress();
        }

        // use this for looping action
        _TreeStatus = _Tree.Process();
    }
}
