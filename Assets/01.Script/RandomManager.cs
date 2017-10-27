using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomManager : MonoBehaviour {

    GameObject[] random = new GameObject[3];
    RandomData[] randomData = new RandomData[3];

    Text[] randomPercent = new Text[3];
    Text[] randomSkill = new Text[3];

	// Use this for initialization
	void Start () {
        Init();
        RandomMix();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Init()
    {
        for (int i = 0; i < 3; i++)
        {
            random[i] = GameObject.Find("Random" + i);
            randomData[i] = new RandomData();

            randomPercent[i] = random[i].transform.Find("Percent").transform.Find("TextPercent").GetComponent<Text>();
        }
    }

    void RandomMix()
    {
        for (int i = 0; i < 3; i++)
        {
            randomData[i].Init(Random.Range(1, 100), Random.Range(1, 3));

            randomPercent[i].text = randomData[i].GetPercent().ToString() + "%";
        }
    }
}

class RandomData
{
    int percent = -1;
    int type = -1;

    public void Init(int percent, int type)
    {
        this.percent = percent;
        this.type = type;
    }

    public int GetPercent()
    {
        return percent;
    }

    public int GetType()
    {
        return type;
    }
}
