using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScratchManager : MonoBehaviour {

    public GameObject imageHidden;

    float width = -1;
    float height = -1;

    float hiddenWidth = -1;
    float hiddenHeight = -1;

    public List<GameObject> hiddenPool = new List<GameObject>();
    public GameManager _gameManager;

    public Sprite test;

    // Use this for initialization
    void Start () {
        width = this.gameObject.GetComponent<RectTransform>().rect.width;
        height = this.gameObject.GetComponent<RectTransform>().rect.height;

        hiddenWidth = imageHidden.GetComponent<RectTransform>().rect.width;
        hiddenHeight = imageHidden.GetComponent<RectTransform>().rect.height;
        
        //this.gameObject.GetComponent<Image>().sprite = _gameManager.scratchSprite[0, 3];

        StartHidden();
	}
	

    public void Scratch()
    {
        int leftCount = hiddenPool.Count;
        for (int i = 0; i < hiddenPool.Count; i++)
        {
            if (!hiddenPool[i].activeSelf)
            {
                leftCount--;
            }
        }
        if(leftCount < 60)
        {
            if (!_gameManager.fight)
            {
                if(this.gameObject.transform.parent)
                _gameManager.FightStart();
                
                for (int i = 0; i < hiddenPool.Count; i++)
                {
                    hiddenPool[i].SetActive(false);
                }
            }
        }
    }

    GameObject blockPrefab;
    void StartHidden()
    {
        hiddenPool.Clear();
        for (int i = 0; i < width; i += (int)hiddenWidth)
        {
            for (int j = 0; j < height; j += (int)hiddenHeight)
            {
                GameObject hidden = (GameObject)Instantiate(imageHidden);
                hidden.transform.SetParent(this.gameObject.transform);
                hidden.GetComponent<RectTransform>().localPosition = new Vector2((-width / 2) + i, (-height / 2) + j);
                hidden.GetComponent<RectTransform>().localScale = Vector3.one;
                hiddenPool.Add(hidden);
            }
        }


                //Instantiate(imageHidden, new Vector2(transform.position.x - (width / 2) + i, transform.position.y - (height / 2) + j), Quaternion.identity).transform.SetParent(this.gameObject.transform);

        //for (float i = startHeight; i < stopHeight; i += width)
        //for (float i = startHeight; i < stopHeight; i += width)
        //    for (float j = startWidth; j < stopWidth; j += height)
        //        //Debug.Log(i);
        //        Instantiate(imageHidden, new Vector2(transform.position.x + i, transform.position.y + j), Quaternion.identity).transform.SetParent(this.gameObject.transform);
    }
}