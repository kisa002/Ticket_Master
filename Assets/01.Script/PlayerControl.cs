using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {


    public int hp;
    int totalHP;
    public GameObject hpDownEffect;
    public Text hpDownNumLabel;
    public Image hpGauge;
    public Image playerSprite;
    public Text totalHpLabel;
    public Text hpLabel;
    public Text effectLabel;

    public PlayerControl otherPlayerControl;

    public GameManager _gameManager;

    RandomManager randomManager;

    public Animator _animator;
    public bool MainPlayer;

    public int posNum;

    void Start()
    {
        totalHP = hp;
        totalHpLabel.text = "/ " + totalHP.ToString();

        randomManager = GameObject.Find("RandomManager").GetComponent<RandomManager>();
    }

    public void HpControl(int num)
    {
        if(num > 0)
        {
            hpDownNumLabel.color = Color.green;
            hpDownNumLabel.text = "+" + num.ToString();
        }
        else
        {
            hpDownNumLabel.color = Color.red;
            hpDownNumLabel.text = num.ToString();
        }
        hpDownEffect.SetActive(false);
        hpDownEffect.SetActive(true);

        StartCoroutine(HpDownCoroutine(num));
        StartCoroutine(HpDownColorControl(num));
    }

    WaitForSeconds hpDownDelay = new WaitForSeconds(0.016f);
    IEnumerator HpDownCoroutine(int num)
    {
        if(num > 0)
        {
            for (int i = 0; i < num; i++)
            {
                if(hp < totalHP)
                {
                    hp++;
                    hpGauge.fillAmount = (float)hp / 200;
                    hpLabel.text = hp.ToString();
                    yield return hpDownDelay;
                }
            }
        }
        else
        {
            if (hp > 0)
            {
                for (int i = 0; i < -num; i++)
                {
                    hp--;
                    hpGauge.fillAmount = (float)hp / 200;
                    hpLabel.text = hp.ToString();
                    yield return hpDownDelay;
                }
            }                
        }
    }

    WaitForSeconds hpDownColorDelay = new WaitForSeconds(0.4f);
    IEnumerator HpDownColorControl(int num)
    {
        if(num > 0)
        {
            playerSprite.color = Color.green;
            yield return hpDownColorDelay;
            playerSprite.color = Color.white;

        }
        else
        {
            playerSprite.color = Color.red;
            yield return hpDownColorDelay;
            playerSprite.color = Color.white;
        }
    }

    public void Fight()
    {
        //int num = Random.Range(-60, 60);
        int num = -randomManager.randomData[posNum].GetDamage();
        //Debug.Log("Fight : " + num);

        if (num < 0)
        {
            otherPlayerControl.HpControl(num);
            _animator.SetTrigger("Attack");
            if (MainPlayer)
            {
                if (num < -50)
                {
                    effectLabel.text = _gameManager.effectString[0];
                }
                else if (num < -40)
                {
                    effectLabel.text = _gameManager.effectString[1];

                }
                else if (num < -30)
                {
                    effectLabel.text = _gameManager.effectString[2];

                }
                else if (num < -20)
                {
                    effectLabel.text = _gameManager.effectString[3];
                }
                else if (num < -10)
                {
                    effectLabel.text = _gameManager.effectString[4];
                }
                else
                {
                    effectLabel.text = _gameManager.effectString[5];
                }

            }            
        }
        else
        {
            HpControl(num);
            _animator.SetTrigger("Attack");
            if (MainPlayer)
            {
                effectLabel.text = _gameManager.effectString[6];
            }
        }
    }
    

    




}
