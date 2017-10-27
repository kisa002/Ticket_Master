using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {



    public GameObject[] timeGaugeObject;

    public GameObject[] refreshTicket;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartGame()
    {

    }
    


    public void NextTurn()
    {
        StopCoroutine("TimeCountDownCoroutine");
        StartCoroutine("TimeCountDownCoroutine");
    }
    WaitForSeconds timeCountDownDelay = new WaitForSeconds(1f);
    IEnumerator TimeCountDownCoroutine()
    {
        for (int i = 0; i < timeGaugeObject.Length; i++)
        {
            timeGaugeObject[i].SetActive(true);
        }
        for (int i = timeGaugeObject.Length - 1; i >= 0; i--)
        {
            timeGaugeObject[i].SetActive(false);
            yield return timeCountDownDelay;
        }
    }

    public void RefreshTicketUse()
    {
        for (int i = refreshTicket.Length - 1; i >= 0; i--)
        {
            if (refreshTicket[i].activeSelf)
            {
                refreshTicket[i].SetActive(false);
                break;
            }
        }
        Refresh();
    }
    public void Refresh()
    {

    }
}
