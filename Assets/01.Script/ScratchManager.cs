using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchManager : MonoBehaviour {

    public GameObject imageHidden;

    int width = 500;
    int height = 300;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartHidden()
    {
        for(int i=0; i<width; i+= 10)
            for(int j=0; j<height; j+= 10)
                Instantiate(imageHidden, new Vector2(transform.position.x + i, transform.position.y + j), Quaternion.identity);
    }
}
