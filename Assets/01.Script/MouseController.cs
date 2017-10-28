using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {
    Vector2 preMousePosition;

    // Use this for initialization
    void Start () {
        preMousePosition = Vector2.zero;
	}

    // Update is called once per frame
    /*
    void Update()
    {
        CheckMouse();
    }
    */

    void CheckMouse()
    {
        //if(Input.GetMouseButton(0))
        //    if (Input.mousePosition.x > this.gameObject.transform.position.x - this.gameObject.GetComponent<RectTransform>().rect.width / 2 && Input.mousePosition.x < this.gameObject.transform.position.x + this.gameObject.GetComponent<RectTransform>().rect.width / 2)
        //        if (Input.mousePosition.y > this.gameObject.transform.position.y - this.gameObject.GetComponent<RectTransform>().rect.height / 2 && Input.mousePosition.y < this.gameObject.transform.position.y + this.gameObject.GetComponent<RectTransform>().rect.height / 2)
        //            Destroy(this.gameObject);
        if (Input.GetMouseButtonDown(0))
        {
            preMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            if (preMousePosition == Vector2.zero)
                return;

            RaycastHit2D[] hits;
            Vector2 interval = (Vector2)Input.mousePosition - preMousePosition;
            hits = Physics2D.RaycastAll(Input.mousePosition, interval.normalized, interval.magnitude);
            if (hits == null)
                return;
            else
            {
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.tag == "UIMask")
                    {
                        hit.collider.gameObject.SetActive(false);
                    }
                }
            }

            Debug.DrawLine(preMousePosition, Input.mousePosition, Color.green, 10f);
            preMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            preMousePosition = Vector2.zero;
        }
    }
}
