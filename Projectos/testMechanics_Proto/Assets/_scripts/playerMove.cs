using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{

    public float animAcelaration = 0.04f;
    public float angularVel = 10.0f;

    private Animator playerAnim;
    private float locSpeed = 0.5f;
    private CharacterController cc;

    // Use this for initialization
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        playerAnim.applyRootMotion = true;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {

        //if w  is pressed
        if (Input.GetAxis("Vertical") > 0)
        {
            playerAnim.SetBool("moving", true);

            // if left shift is pressed
            if (Input.GetAxis("Fire3") > 0.0f)
            {
                if (locSpeed < 1.0f)
                    locSpeed += animAcelaration;
                else
                    locSpeed = 1.0f;
            }
            else
            {
                if (locSpeed > 0.5f)
                    locSpeed -= animAcelaration;
                else
                    locSpeed = 0.5f;
            }
        }

        // if s is pressed
        else if (Input.GetAxis("Vertical") < 0)
        {
            playerAnim.SetBool("moving", true);
            if (locSpeed > 0)
                locSpeed -= animAcelaration;
            else
                locSpeed = 0.0f;
        }

        //if no vertical buttons are pressed
        else if (Input.GetAxis("Vertical") == 0)
        {
            playerAnim.SetBool("moving", false);

            if (locSpeed < 0.5f && locSpeed + animAcelaration > 0.5f)
                locSpeed = 0.5f;
            else if (locSpeed < 0.5f && locSpeed + animAcelaration < 0.5f)
                locSpeed += animAcelaration;
            else if (locSpeed > 0.5f && locSpeed - animAcelaration < 0.5f)
                locSpeed = 0.5f;
            else if (locSpeed > 0.5f && locSpeed - animAcelaration > 0.5f)
                locSpeed -= animAcelaration;
        }

        // set LocSpeed
        playerAnim.SetFloat("LocSpeed", locSpeed);

        if (locSpeed == 0.5f && playerAnim.GetBool("moving"))
            if (Input.GetAxis("Horizontal") > 0)
                this.transform.Rotate(this.transform.up, angularVel);
            else if (Input.GetAxis("Horizontal") < 0)
                this.transform.Rotate(this.transform.up, -angularVel);


        if (cc.isGrounded && Input.GetKeyDown(KeyCode.Space)) { }
            
    }

}
