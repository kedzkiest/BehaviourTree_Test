using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishManager : MonoBehaviour
{
    private int _FishNum;

    private List<GameObject> _Fish = new List<GameObject>();

    public TextMeshProUGUI FishNumText;

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
        FishNumText.text = "Number of fish: " + _FishNum;
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
