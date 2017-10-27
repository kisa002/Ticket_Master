using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckMouse();
	}

    void CheckMouse()
    {
        if(Input.GetMouseButton(0))
            if (Input.mousePosition.x > this.gameObject.transform.position.x - this.gameObject.GetComponent<RectTransform>().rect.width / 2 && Input.mousePosition.x < this.gameObject.transform.position.x + this.gameObject.GetComponent<RectTransform>().rect.width / 2)
                if (Input.mousePosition.y > this.gameObject.transform.position.y - this.gameObject.GetComponent<RectTransform>().rect.height / 2 && Input.mousePosition.y < this.gameObject.transform.position.y + this.gameObject.GetComponent<RectTransform>().rect.height / 2)
                    Destroy(this.gameObject);
    }
}
