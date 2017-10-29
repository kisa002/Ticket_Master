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

    public int num;
    public int damage;

    NetworkManager networkManager;

    void Start()
    {
        totalHP = hp;
        totalHpLabel.text = "/ " + totalHP.ToString();

        randomManager = GameObject.Find("RandomManager").GetComponent<RandomManager>();
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
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

    WaitForSeconds hpDownDelay = new WaitForSeconds(0.001f);
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
            for (int i = 0; i < -num; i++)
            {
                if (hp > 0)
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

    WaitForSeconds hpDownColorDelay = new WaitForSeconds(0.06f);
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
            //int damage = -Random.Range(1, 4) * Random.Range(1, 3) * 10;
            networkManager.StartCoroutine(networkManager.GetDamage());
            int damage = -networkManager.GetEnemyDamage();

            otherPlayerControl.HpControl(damage);
            _animator.SetTrigger("Attack");
            effectLabel.text = "라이벌에게" + (-damage).ToString() + "만큼 피해를 입었다!!";
            StartCoroutine("HurtEffectCoroutine");
            _gameManager.HurtSoundPlay();
        }
        else
        {
            //int num = Random.Range(-60, 60);
            //int num = -randomManager.randomData[posNum].GetDamage();
            
            //Debug.Log("Fight : " + num);
            //
            /*
            if (num == -1)
            {
                effectLabel.text = "복권을 긁지 못했다!!";
                for (int i = 0; i < _gameManager._randomManager.mask.Length; i++)
                {
                    _gameManager._randomManager.mask[i].SetActive(true);
                }
                _gameManager.WarningSoundPlay();
            }
            */
            if (num == 0)
            {
                if (MainPlayer)
                {
                    effectLabel.text = "꽝!!";
                    X_image[posNum].SetActive(true);
                    StartCoroutine("XSoundPlayCoroutine");
                    attackEffect[6].SetActive(true);
                    _gameManager.EffectSoundPlay(6);
                }

            }
            else if (num < 0)
            {
                
                _animator.SetTrigger("Attack");
                if (MainPlayer)
                {
                    O_image[posNum].SetActive(true);
                    effectLabel.text = _gameManager.effectString[randomManager.randomData[posNum].GetType()];
                    if(randomManager.randomData[posNum].GetType() == 5)
                    {
                        StartCoroutine(OnePunch(num));
                    }
                    else
                    {
                        otherPlayerControl.HpControl(num);
                        attackEffect[randomManager.randomData[posNum].GetType()].SetActive(true);
                        _gameManager.EffectSoundPlay(randomManager.randomData[posNum].GetType());

                    }
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
                //HpControl(num);
                //_animator.SetTrigger("Attack");
                //if (MainPlayer)
                //{
                //    effectLabel.text = _gameManager.effectString[6];
                //}
            }
        }        
    }


    IEnumerator OnePunch(int num)
    {

        attackEffect[7].SetActive(true);
        yield return new WaitForSeconds(1.2f);
        attackEffect[5].SetActive(true);
        otherPlayerControl.HpControl(num);
        _gameManager.EffectSoundPlay(5);
    }
    IEnumerator XSoundPlayCoroutine()
    {
        _gameManager._audioSource.PlayOneShot(_gameManager.xSound);
        yield return new WaitForSeconds(0.5f);
        _gameManager._audioSource.PlayOneShot(_gameManager.xSound);
    }
    




}
