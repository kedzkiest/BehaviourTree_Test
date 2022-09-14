using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum Status
    {
        SUCCESS,
        RUNNING,
        FAILURE
    };

    public Status status;
    public List<Node> children = new List<Node>();
    public int currentChild = 0;
    public string name;

    public static string currentProcess;

    public Node() { }

    public Node(string n)
    {
        name = n;
    }

    // Description of the method Process()
    //
    // This method receives the result of one of its child nodes execution
    // currently pointed to by the "currentChild" index as a Status.
    //
    // Normally, the root node of the behaviour tree executes this method first, and this
    // method propagates to its children.
    //
    // Status is returned without further propagation when each Leaf node
    // finishes its action.

    public virtual Status Process()
    {
        return children[currentChild].Process();
    }

    public void AddChild(Node n)
    {
        children.Add(n);
    }
}
