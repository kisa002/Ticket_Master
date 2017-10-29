using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

    public int player = 1;

    int myDamage;
    int enemyDamage;

    bool stat = false;
    public bool isAttack = false;

    GameManager gameManager;
    PlayerControl playerControl1;
    PlayerControl playerControl2;

    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerControl1 = GameObject.Find("Player_Main").GetComponent<PlayerControl>();
        playerControl2 = GameObject.Find("Player_Other").GetComponent<PlayerControl>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetMyDamage()
    {
        return myDamage;
    }

    public int GetEnemyDamage()
    {
        return enemyDamage;
    }

    public IEnumerator SendDamage(int damage)
    {
        string url = "";

        if (player == 1)
            url = "http://haeyum.com/Game/TicketMaster/ticket_master.php?type=201&player=1&dmg=" + damage + "&status=ready";
        else
            url = "http://haeyum.com/Game/TicketMaster/ticket_master.php?type=201&player=2&dmg=" + damage + "&status=ready";

        Debug.Log(url);

        WWW wwwUrl = new WWW(url);

        yield return wwwUrl;

        myDamage = damage;

        playerControl1.num = -damage;
    }

    public IEnumerator GetDamage()
    {
        string url;

        if (player == 1)
            url = "http://haeyum.com/Game/TicketMaster/ticket_master.php?type=200&player=2";
        else
            url = "http://haeyum.com/Game/TicketMaster/ticket_master.php?type=200&player=1";

        WWW wwwUrl = new WWW(url);

        yield return wwwUrl;

        enemyDamage = int.Parse(wwwUrl.text);
        playerControl2.damage = enemyDamage;
    }

    public bool GetStatusCorutine()
    {
        return stat;
    }

    public IEnumerator GetStatus()
    {
        WWW wwwUrl = new WWW("http://haeyum.com/Game/TicketMaster/ticket_master.php?type=202&player=1");
        yield return wwwUrl;
    }

    public IEnumerator SetStatus()
    {
        string url;
        
        url = "http://haeyum.com/Game/TicketMaster/ticket_master.php?type=203&player=" + player;

        WWW wwwUrl = new WWW(url);

        yield return wwwUrl;

        if (wwwUrl.text.Equals("wait"))
            stat = true;
        else
            stat = false;
    }

    public IEnumerator CheckStatus()
    {
        string url;

        if(player == 1)
            url = "http://haeyum.com/Game/TicketMaster/ticket_master.php?type=202&player=2";
        else
            url = "http://haeyum.com/Game/TicketMaster/ticket_master.php?type=202&player=1";

        WWW wwwUrl = new WWW(url);

        yield return wwwUrl;

        if (wwwUrl.text.Equals("ready"))
        {
            if(!isAttack)
            {
                StartCoroutine(GetDamage());
                gameManager.FightStart();

                isAttack = true;
            }

        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(CheckStatus());
        }
    }
}
