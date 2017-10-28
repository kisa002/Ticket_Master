using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomManager : MonoBehaviour {

    GameObject[] random = new GameObject[3];
    public RandomData[] randomData = new RandomData[3];

    Text[] randomPercent = new Text[3];
    Text[] randomSkill = new Text[3];

    public GameManager _gameManager;
    public GameObject[] mask = new GameObject[3];
    public Sprite[] skillSprite;
    Image[] skillImage = new Image[3];

    GameObject[] scratchSprite = new GameObject[3];

    public ScratchManager[] _scratchManager;
    // Use this for initialization

    private void Awake()
    {
        //Init();
        //RandomMix();
    }

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
            skillImage[i] = random[i].transform.Find("Skill").GetComponent<Image>();
            mask[i] = random[i].transform.Find("Mask").gameObject;

            scratchSprite[i] = random[i].transform.Find("Scratch").gameObject;
        }

        //skillSprite[0] = Resources.Load<Sprite>("ConfirmImage/icon/skillicon.atk.01");
        //skillSprite[1] = Resources.Load<Sprite>("ConfirmImage/icon/skillicon.");
        //skillSprite[2] = Resources.Load<Sprite>("ConfirmImage/icon/skillicon.");
        //skillSprite[3] = Resources.Load<Sprite>("ConfirmImage/icon/skillicon.");
        //skillSprite[4] = Resources.Load<Sprite>("ConfirmImage/icon/skillicon.");
        //skillSprite[5] = Resources.Load<Sprite>("ConfirmImage/icon/skillicon.");
        //skillSprite[6] = Resources.Load<Sprite>("ConfirmImage/icon/skillicon.");
        //skillSprite[7] = Resources.Load<Sprite>("ConfirmImage/icon/skillicon.");
        //skillSprite[8] = Resources.Load<Sprite>("ConfirmImage/icon/skillicon.");
        //skillSprite[9] = Resources.Load<Sprite>("ConfirmImage/icon/skillicon.");
        //skillSprite[10] = Resources.Load<Sprite>("ConfirmImage/icon/skillicon.");
        //skillSprite[11] = Resources.Load<Sprite>("ConfirmImage/icon/skillicon.");
    }

    public void RandomMix()
    {
        for (int i = 0; i < _scratchManager.Length; i++)
        {
            for (int j = 0; j < _scratchManager[i].hiddenPool.Count; j++)
            {
                _scratchManager[i].hiddenPool[j].SetActive(true);
            }
        }
        for (int i = 0; i < 3; i++)
        {
            int randomNum = Random.Range(0,7);
            if(randomNum == 0)
            {
                randomData[i].Init(5, 200, Random.Range(0, 3));
            }
            else if (randomNum == 1)
            {
                randomData[i].Init(10, 140, Random.Range(0, 3));
            }
            else if (randomNum == 2)
            {
                randomData[i].Init(20, 80, Random.Range(0, 3));
            }
            else if (randomNum == 3)
            {
                randomData[i].Init(30, 70, Random.Range(0, 3));
            }
            else if (randomNum == 4)
            {
                randomData[i].Init(45, 45, Random.Range(0, 3));
            }
            else if (randomNum == 5)
            {
                randomData[i].Init(65, 30, Random.Range(0, 3));
            }
            else if (randomNum == 6)
            {
                randomData[i].Init(80, 20, Random.Range(0, 3));
            }

            if (randomData[i].GetPercent() > (int)Random.Range(1, 100))
                scratchSprite[i].GetComponent<Image>().sprite = _gameManager.scratchSprite[0, (int)Random.Range(1, 9)];
            else
                scratchSprite[i].GetComponent<Image>().sprite = _gameManager.scratchSprite[1, (int)Random.Range(1, 9)];

            randomData[i].Init(randomData[i].GetPercent(), 0, randomData[i].GetType()); //ERROR시 제거.

            randomPercent[i].text = randomData[i].GetPercent().ToString() + "%";
            skillImage[i].sprite = skillSprite[randomData[i].GetType()];
            mask[i].SetActive(false);
            _gameManager.TimeCountDown();
        }

        //Debug.Log(randomData[0].GetPercent() + " / " + randomData[0].GetDamage());
    }
}

public class RandomData
{
    int percent = -1;
    int damage = -1;
    int type = -1;

    public void Init(int percent, int damage, int type)
    {
        this.percent = percent;
        this.damage = damage;
        this.type = type;
    }

    public int GetPercent()
    {
        return percent;
    }

    public int GetDamage()
    {
        return damage;
    }

    public int GetType()
    {
        return type;
    }

}
