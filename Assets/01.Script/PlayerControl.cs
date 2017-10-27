using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {


    public int hp;
    public GameObject hpDownEffect;
    public Text hpDownNumLabel;
    public Image hpGauge;
    public Image playerSprite;
    public Text totalHpLabel;
    public Text hpLabel;

    void Start()
    {
        totalHpLabel.text = "/ " + hp.ToString();
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
                hp++;
                hpGauge.fillAmount = (float)hp / 200;
                hpLabel.text = hp.ToString();
                yield return hpDownDelay;
            }
        }
        else
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




}
