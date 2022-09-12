using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    [SerializeField]
    private int _FishNum;

    private List<GameObject> _Fish = new List<GameObject>();

    public List<GameObject> Fish
    {
        get { return _Fish; }
        set { _Fish = value; }
    }

    public int FishNum
    {
        get { return _FishNum; }
        set { _FishNum = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ClearFish();
        }
    }

    public void ClearFish()
    {
        _FishNum = 0;
        foreach(GameObject go in _Fish)
        {
            Destroy(go);
        }
        _Fish.Clear();
    }
}
