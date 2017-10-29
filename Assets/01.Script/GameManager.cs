using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {



    public GameObject[] timeGaugeObject;

    public GameObject[] refreshTicket;
    public GameObject[] refreshTicketEffect;
    public PlayerControl[] _playerControl;
    public RandomManager _randomManager;

    public string[] effectString;
    public bool fight;

    public GameObject startBack;
    public GameObject reTicketButton;

    public Sprite[,] scratchSprite = new Sprite[2,9];



    public static List<GameObject> hiddenEffectPool = new List<GameObject>();
    public GameObject hiddenEffectPrefab;
    public Transform objectBox;

    public GAui gameOverGaui;
    public GameObject gameOverMask;
    public Text gameOverLabel;

    public GameObject[] ticket;

    public AudioSource _audioSource;

    public AudioSource scratchAudioSource;
    public AudioClip[] effectSound;
    public AudioClip xSound;
    public AudioClip warningSound;
    public AudioClip hurtSound;
    public AudioClip scratchSound;

    public AudioSource musicManager;

    NetworkManager networkManager;


    // Use this for initialization
    void Awake () {
        //Application.targetFrameRate = 60;
        //StartGame();

        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        CreateHiddenEffect();

        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 9; j++)
                if (i == 0)
                    scratchSprite[i, j] = Resources.Load<Sprite>("ticket_inside/img.hide.get.0" + (j+1));
                else
                    scratchSprite[i, j] = Resources.Load<Sprite>("ticket_inside/img.hide.lose.0" + (j+1));
    }
    
	


    void CreateHiddenEffect()
    {
        for (int i = 0; i < 60; i++)
        {
            GameObject hiddenEffect = (GameObject)Instantiate(hiddenEffectPrefab);
            hiddenEffect.name = "hidden_" + i.ToString();
            hiddenEffect.SetActive(false);
            hiddenEffect.transform.SetParent(objectBox);
            hiddenEffectPool.Add(hiddenEffect);
        }
    }
    void StartGame()
    {
        StartCoroutine("StartCoroutine");
    }
    


    public void TimeCountDown()
    {
        StopCoroutine("TimeCountDownCoroutine");
        StartCoroutine("TimeCountDownCoroutine");

    }

    IEnumerator StartCoroutine()
    {
        StopCoroutine("TimeCountDownCoroutine");
        startBack.SetActive(true);

        yield return new WaitForSeconds(3f);

        startBack.SetActive(false);
        _randomManager.RandomMix();
        
    }
    public bool timeCountDownEnd;
    WaitForSeconds timeCountDownDelay = new WaitForSeconds(0.8f);
    IEnumerator TimeCountDownCoroutine()
    {
        timeCountDownEnd = false;
        for (int i = 0; i < timeGaugeObject.Length; i++)
        {
            timeGaugeObject[i].SetActive(true);
        }
        for (int i = timeGaugeObject.Length - 1; i >= 0; i--)
        {
            timeGaugeObject[i].SetActive(false);
            yield return timeCountDownDelay;
        }
        timeCountDownEnd = true;
        ////FightStart();

    }

    public void RefreshTicketUse()
    {
        for (int i = 0; i < ticket.Length; i++)
        {
            if (ticket[i].activeSelf)
            {
                _randomManager.refresh = true;
                for (int j = refreshTicket.Length - 1; j >= 0; j--)
                {
                    if (refreshTicket[j].activeSelf)
                    {
                        refreshTicket[j].SetActive(false);
                        refreshTicketEffect[j].SetActive(true);
                        break;
                    }
                }
                Refresh();
                break;
            }
        }
        if (!refreshTicket[0].activeSelf)
        {
            reTicketButton.SetActive(true);
        }
    }
    public void Refresh()
    {
        _randomManager.RandomMix();
    }

    public void FightStart()
    {
        fight = true;
        StopCoroutine("TimeCountDownCoroutine");
        for (int i = 0; i < timeGaugeObject.Length; i++)
        {
            timeGaugeObject[i].SetActive(false);
        }

        StartCoroutine("FightCoroutine");
    }
    public GameObject confetti;
    IEnumerator FightCoroutine()
    {
        yield return new WaitForSeconds(0.4f);
        _playerControl[0].Fight();
        yield return new WaitForSeconds(2f);
        _playerControl[1].Fight();
        yield return new WaitForSeconds(2f);
        if (_randomManager.randomData[_playerControl[0].posNum].GetType() == 5)
        {
            if(_randomManager.randomData[_playerControl[0].posNum].GetDamage() != 0)
            {
                yield return new WaitForSeconds(3.2f);
            }
        }
            fight = false;
        if(_playerControl[0].hp <= 0)
        {
            gameOverLabel.text = "죽었습니다..";
            GameOver();
        }
        else if (_playerControl[1].hp <= 0)
        {
            gameOverLabel.text = "승리!!";
            GameOver();
            _playerControl[1]._animator.SetTrigger("Die");
            confetti.SetActive(true);
        }
        else
        {
            Refresh();
        }        
    }

    public void GameOver()
    {
        gameOverMask.SetActive(true);
        gameOverGaui.gameObject.SetActive(true);
        gameOverGaui.MoveIn();
    }

    public void ReStart()
    {
        StartGame();
        musicManager.Stop();
        musicManager.Play();
        for (int i = 0; i < 2; i++)
        {
            _playerControl[i]._animator.SetTrigger("ReStart");
            _playerControl[i].hp = 200;
            _playerControl[i].HpSet();
        }
        for (int i = 0; i < refreshTicket.Length; i++)
        {
            refreshTicket[i].SetActive(true);
        }
    }

    public void EffectSoundPlay(int num)
    {
        _audioSource.PlayOneShot(effectSound[num]);
    }
    public void WarningSoundPlay()
    {
        _audioSource.PlayOneShot(warningSound);
    }
    public void HurtSoundPlay()
    {
        _audioSource.PlayOneShot(hurtSound);
    }
    public void ScratchSoundPlay()
    {
        scratchAudioSource.Stop();
        scratchAudioSource.PlayOneShot(scratchSound);
    }

}
