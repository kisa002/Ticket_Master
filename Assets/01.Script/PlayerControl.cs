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
    public Image[] playerSprite;
    public Text totalHpLabel;
    public Text hpLabel;
    public Text effectLabel;

    public PlayerControl otherPlayerControl;

    public GameManager _gameManager;
    public GameObject[] O_image;
    public GameObject[] X_image;

    RandomManager randomManager;

    public Animator _animator;
    public bool MainPlayer;

    public int posNum;
    public GameObject[] attackEffect;

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
                    HpSet();
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
                    HpSet();
                    yield return hpDownDelay;
                }
            }                
        }
    }
    public void HpSet()
    {
        hpGauge.fillAmount = (float)hp / 200;
        hpLabel.text = hp.ToString();
    }

    WaitForSeconds hpDownColorDelay = new WaitForSeconds(0.1f);
    IEnumerator HpDownColorControl(int num)
    {
        if(num > 0)
        {
            for (int i = 0; i < playerSprite.Length; i++)
            {
                playerSprite[i].color = Color.green;
            }
            yield return hpDownColorDelay;
            for (int i = 0; i < playerSprite.Length; i++)
            {
                playerSprite[i].color = Color.white;
            }

        }
        else
        {
            for (int i = 0; i < playerSprite.Length; i++)
            {
                playerSprite[i].color = Color.red;
            }
            yield return hpDownColorDelay;
            for (int i = 0; i < playerSprite.Length; i++)
            {
                playerSprite[i].color = Color.white;
            }
        }
    }
    
    IEnumerator HurtEffectCoroutine()
    {
        
        yield return new WaitForSeconds(1.8f);
        effectLabel.text = "";
    }
    public void Fight()
    {
        if (!MainPlayer)
        {
            if(Random.Range(0,2) == 0)
            {
                effectLabel.text = "라이벌의 복권은 꽝이다!!";
                StartCoroutine("HurtEffectCoroutine");
            }
            else
            {
                int damage = -Random.Range(1, 4) * Random.Range(1, 3) * 10;
                otherPlayerControl.HpControl(damage);
                _animator.SetTrigger("Attack");
                effectLabel.text = "라이벌에게" + (-damage).ToString() + "만큼 피해를 입었다!!";
                StartCoroutine("HurtEffectCoroutine");
            }
        }
        else
        {//int num = Random.Range(-60, 60);

            int num = -randomManager.randomData[posNum].GetDamage();
            if (_gameManager.timeCountDownEnd)
            {
                num = -1;
            }
            //Debug.Log("Fight : " + num);
            if (num == -1)
            {
                effectLabel.text = "복권을 긁지 못했다!!";
                for (int i = 0; i < _gameManager._randomManager.mask.Length; i++)
                {
                    _gameManager._randomManager.mask[i].SetActive(true);
                }
                _gameManager.WarningSoundPlay();
            }
            else if (num == 0)
            {
                if (MainPlayer)
                {
                    effectLabel.text = "꽝!!";
                    X_image[posNum].SetActive(true);
                    StartCoroutine("XSoundPlayCoroutine");
                }

            }
            else if (num < 0)
            {
                otherPlayerControl.HpControl(num);
                _animator.SetTrigger("Attack");
                if (MainPlayer)
                {
                    O_image[posNum].SetActive(true);
                    effectLabel.text = _gameManager.effectString[randomManager.randomData[posNum].GetType()];
                    attackEffect[randomManager.randomData[posNum].GetType()].SetActive(true);
                    _gameManager.EffectSoundPlay(randomManager.randomData[posNum].GetType());
                    /*
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
                    */

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
    IEnumerator XSoundPlayCoroutine()
    {
        _gameManager._audioSource.PlayOneShot(_gameManager.xSound);
        yield return new WaitForSeconds(0.5f);
        _gameManager._audioSource.PlayOneShot(_gameManager.xSound);
    }
    




}
