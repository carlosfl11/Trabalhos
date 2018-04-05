using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControllerDouglas : MonoBehaviour
{

    private Animator playerAnim;
    private float idleAnim, changevalue;

    private float animationTimer;

    // Use this for initialization
    void Start()
    {
        //animator
        playerAnim = GetComponent<Animator>();

    }

    void Update()
    {
        // idle
        if (!playerAnim.GetBool("isfighting") && !playerAnim.GetBool("isMoving"))
        {
            if (idleAnim <= 0)
                changevalue = 0.001f;
            else if(idleAnim >=1)
                changevalue = -0.001f;

            idleAnim += changevalue;
            playerAnim.SetFloat("randIdle", idleAnim);
        }

        if(Input.GetButtonDown(KeyCode.W))
        {
            playerAnim.SetBool("isMoving", true);
        }
        else
            playerAnim.SetBool("isMoving", true);



    }
}
