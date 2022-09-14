using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    public Selector(string n)
    {
        name = n;
    }

    // Description of the method Process() overrided by Selector
    //
    // The selector node visits each of its child nodes in turn, and if
    // any of them return SUCCESS, the selector node immediately returns
    // SUCCESS to its parent.
    //
    // The selector node returns FAILURE to its parent only when
    // all child nodes visited return FAILURE.

    public override Status Process()
    {
        Status childStatus = children[currentChild].Process();
        if (childStatus == Status.RUNNING) return Status.RUNNING;

        if(childStatus == Status.SUCCESS)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }

        currentChild++;
        if(currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.FAILURE;
        }

        return Status.RUNNING;
    }
}
