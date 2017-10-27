using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchManager : MonoBehaviour {

    public GameObject imageHidden;

    float width = -1;
    float height = -1;

    float hiddenWidth = -1;
    float hiddenHeight = -1;

	// Use this for initialization
	void Start () {
        width = this.gameObject.GetComponent<RectTransform>().rect.width;
        height = this.gameObject.GetComponent<RectTransform>().rect.height;

        hiddenWidth = imageHidden.GetComponent<RectTransform>().rect.width;
        hiddenHeight = imageHidden.GetComponent<RectTransform>().rect.height;

        StartHidden();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartHidden()
    {
        for (int i=0; i<width; i += (int)hiddenWidth)
            for(int j=0; j<height; j += (int)hiddenHeight)
                Instantiate(imageHidden, new Vector2(transform.position.x - (width / 2) + i, transform.position.y - (height / 2) + j), Quaternion.identity).transform.SetParent(this.gameObject.transform);

        //for (float i = startHeight; i < stopHeight; i += width)
        //    for (float j = startWidth; j < stopWidth; j += height)
        //        //Debug.Log(i);
        //        Instantiate(imageHidden, new Vector2(transform.position.x + i, transform.position.y + j), Quaternion.identity).transform.SetParent(this.gameObject.transform);
    }
}