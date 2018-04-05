using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControllerDouglas : MonoBehaviour
{

    private Animator playerAnim;
    private float idleAnim, targetIdle;

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
            if (idleAnim >= targetIdle)
                

            idleAnim = Mathf.Lerp(idleAnim, targetIdle, 0.001f);
            playerAnim.SetFloat("randIdle", idleAnim);
            Debug.Log(idleAnim + "," + targetIdle);
        }



    }
}
