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
            int tmp = (int)Random.Range(0, 6);

            switch(tmp)
            {
                case 0:
                    tmp = 5;
                    break;

                case 1:
                    tmp = 10;
                    break;

                case 2:
                    tmp = 20;
                    break;

                case 3:
                    tmp = 30;
                    break;

                case 4:
                    tmp = 45;
                    break;

                case 5:
                    tmp = 65;
                    break;

                case 6:
                    tmp = 80;
                    break;
            }
            randomData[i].Init(tmp, Random.Range(1, 3));

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
