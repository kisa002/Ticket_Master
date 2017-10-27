using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {


    public int hp;
    public GameObject hpDownEffect;
    public Text hpDownNumLabel;

    public void Damage(int num)
    {
        hp = hp - num;
        hpDownNumLabel.text = num.ToString();
        hpDownEffect.SetActive(true);
    }


}
