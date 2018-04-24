using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameCharCtrl : MonoBehaviour {

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


	// Use this for initialization
	void Start () {
        anim = player.GetComponent<Animator>();

        anim.avatar = douglasAvatar;
        anim.runtimeAnimatorController = douglasCtrl;

        douglas.SetActive(true);
        douglasInf.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
