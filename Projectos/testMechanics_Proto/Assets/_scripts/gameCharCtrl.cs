using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameCharCtrl : MonoBehaviour
{

    //Animator
    private Animator anim;
    public GameObject player;

    //models
    public GameObject douglas;
    public GameObject douglasInf;

    //avatar
    public Avatar douglasAvatar, douglasInfAvatar;

    //controllers
    public RuntimeAnimatorController douglasCtrl, douglasInfCtrl;
    private GameObject fightTrigger;

    private bool isInfected = false;


    // Use this for initialization
    void Start()
    {
        anim = player.GetComponent<Animator>();
        fightTrigger = GameObject.Find("fightTrigger");

        anim.avatar = douglasAvatar;
        anim.runtimeAnimatorController = douglasCtrl;

        douglas.SetActive(true);
        douglasInf.SetActive(false);
        fightTrigger.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //when is infected change mesh, controller and avatar
        if (isInfected)
        {
            douglas.SetActive(false);
            douglasInf.SetActive(true);

            anim.avatar = douglasInfAvatar;
            anim.runtimeAnimatorController = douglasInfCtrl;

            fightTrigger.SetActive(true);
        }
        if (!isInfected)
        {
            anim.avatar = douglasAvatar;
            anim.runtimeAnimatorController = douglasCtrl;

            douglas.SetActive(true);
            douglasInf.SetActive(false);
            fightTrigger.SetActive(false);
        }

    }

    // return if is infected
    public bool getInfState()
    {
        return isInfected;
    }

    public void switchChars()
    {
        if (isInfected)
            isInfected = false;
        else
            isInfected = true;
                
    }
}
