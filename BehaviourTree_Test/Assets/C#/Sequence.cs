using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(string n)
    {
        name = n;
    }

    // Description of the method Process() overrided by Sequence
    //
    // The sequence node visits each of its child nodes in turn, and if
    // any of them return FAILURE, the sequence node immediately returns
    // FAILURE to its parent.
    //
    // The sequence node returns SUCCESS to its parent only when
    // all child nodes visited return SUCCESS.

    public override Status Process()
    {
        Status childStatus = children[currentChild].Process();
        if (childStatus == Status.RUNNING) return Status.RUNNING;
        if (childStatus == Status.FAILURE) return Status.FAILURE;

        currentChild++;
        if(currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }

        return Status.RUNNING;
    }
}
