using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {



    public GameObject[] timeGaugeObject;

    public GameObject[] refreshTicket;
    public PlayerControl[] _playerControl;

    public string[] effectString;

    public GameObject startBack;
    public GAui[] startPlayerGaui;

    // Use this for initialization
    void Start () {
        StartGame();

    }
    
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartGame()
    {
        StartCoroutine("StartCoroutine");
    }
    


    public void NextTurn()
    {
        StopCoroutine("TimeCountDownCoroutine");
        StartCoroutine("TimeCountDownCoroutine");
    }

    IEnumerator StartCoroutine()
    {
        startBack.SetActive(true);
        yield return new WaitForSeconds(1);
        for (int i = 0; i < startPlayerGaui.Length; i++)
        {
            startPlayerGaui[i].gameObject.SetActive(true);
            startPlayerGaui[i].MoveIn();
        }
        yield return new WaitForSeconds(3.6f);
        for (int i = 0; i < startPlayerGaui.Length; i++)
        {
            startPlayerGaui[i].MoveOut();
        }
        yield return new WaitForSeconds(0.6f);
        startBack.SetActive(false);
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

    public void FightStart()
    {
        StartCoroutine("FightCoroutine");
    }
    IEnumerator FightCoroutine()
    {
        yield return new WaitForSeconds(0.4f);
        _playerControl[0].Fight();
        yield return new WaitForSeconds(1f);
        _playerControl[1].Fight();
    }
}
