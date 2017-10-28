using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchController : MonoBehaviour {

    GameObject hiddenMask;

    float prev = -1;
    float current = -1;
    RandomManager _randomManager;
    int posNum;
    ScratchManager _scratchManager;

    // Use this for initialization
    void Start () {
        float width = transform.parent.transform.position.x;
        float height = transform.parent.transform.position.y;

        hiddenMask = this.gameObject.transform.Find("ImageHiddenMask").gameObject;
        hiddenMask.transform.position = new Vector2(width, height);
        _randomManager = GameObject.Find("RandomManager").GetComponent<RandomManager>();
        if(transform.parent.parent.name == "Random0")
        {
            posNum = 0;
        }
        else if (transform.parent.parent.name == "Random1")
        {
            posNum = 1;
        }
        else if (transform.parent.parent.name == "Random2")
        {
            posNum = 2;
        }
        _scratchManager = transform.parent.GetComponent<ScratchManager>();
    }

    public void Explode()
    {
        for (int j = 0; j < GameManager.hiddenEffectPool.Count; j++)
        {
            if (!GameManager.hiddenEffectPool[j].activeSelf)
            {
                GameManager.hiddenEffectPool[j].transform.position = transform.position;
                GameManager.hiddenEffectPool[j].SetActive(true);
                break;
            }
        }
        for (int i = 0; i < _randomManager.mask.Length; i++)
        {
            _randomManager.mask[i].SetActive(true);
        }
        _randomManager.mask[posNum].SetActive(false);
        _scratchManager.Scratch();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        //CheckMouse();
	}

    //void CheckMouse()
    //{
    //    //if(Input.GetMouseButton(0))
    //    //    if (Input.mousePosition.x > this.gameObject.transform.position.x - this.gameObject.GetComponent<RectTransform>().rect.width / 2 && Input.mousePosition.x < this.gameObject.transform.position.x + this.gameObject.GetComponent<RectTransform>().rect.width / 2)
    //    //        if (Input.mousePosition.y > this.gameObject.transform.position.y - this.gameObject.GetComponent<RectTransform>().rect.height / 2 && Input.mousePosition.y < this.gameObject.transform.position.y + this.gameObject.GetComponent<RectTransform>().rect.height / 2)
    //    //            Destroy(this.gameObject);

    //    if (Input.GetMouseButton(0))
    //    {
    //        RaycastHit2D[] hits;
    //        Vector2 interval = (Vector2)Input.mousePosition - preMousePosition;
    //        hits = Physics2D.RaycastAll(Input.mousePosition, interval, interval.magnitude);
    //        if (hits == null)
    //            return;
    //        else
    //        {
    //            foreach(RaycastHit2D hit in hits)
    //            {
    //                if(hit.collider.tag == "UIMask")
    //                {
    //                    hit.collider.gameObject.SetActive(false);
    //                }
    //            }
    //        }
    //    }
    //}
}
