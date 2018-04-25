using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    //public 
    public float animAcelaration = 0.04f;
    public float angularVel = 10.0f;
    public float jumpVel = 5.0f;

    // controll
    private Animator playerAnim;
    private float locSpeed = 0.5f;
    private CharacterController cc;
    //action
    private float startAction;
    private bool action = false;
    private bool canInteract = false;

    // controll of infected state
    private gameCharCtrl controller;

    //vertical movel
    private float jumpForce;



    // Use this for initialization
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        playerAnim.applyRootMotion = true;
        controller = GameObject.Find("gameCharCtrl").GetComponent<gameCharCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        //move if not interactiong
        if (!action)
            Movement();

        // interact when pressed f
        if (Input.GetKey(KeyCode.F) && canInteract)
            action = true;
        useAction();

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

        //rotation when walking
        if (cc.isGrounded)
        {
            if (locSpeed == 0.5f && playerAnim.GetBool("moving"))
            {
                if (Input.GetAxis("Horizontal") > 0)
                    this.transform.Rotate(this.transform.up, angularVel * Time.deltaTime);
                else if (Input.GetAxis("Horizontal") < 0)
                    this.transform.Rotate(this.transform.up, -angularVel * Time.deltaTime);
            }
            //rotation when running
            else if (locSpeed > 0.5f && playerAnim.GetBool("moving"))
            {
                if (Input.GetAxis("Horizontal") > 0)
                    this.transform.Rotate(this.transform.up, angularVel * Time.deltaTime * 0.8f);
                else if (Input.GetAxis("Horizontal") < 0)
                    this.transform.Rotate(this.transform.up, -angularVel * Time.deltaTime * 0.8f);
            }
            //rotation when moving back
            else if (locSpeed < 0.5f && playerAnim.GetBool("moving"))
            {
                if (Input.GetAxis("Horizontal") > 0)
                    this.transform.Rotate(this.transform.up, -angularVel * Time.deltaTime);
                else if (Input.GetAxis("Horizontal") < 0)
                    this.transform.Rotate(this.transform.up, angularVel * Time.deltaTime);
            }
        }
        //turn stoped
        if (!playerAnim.GetBool("moving") && Input.GetAxis("Horizontal") != 0)
            playerAnim.SetFloat("turn", Input.GetAxis("Horizontal"));
        else
            playerAnim.SetFloat("turn", 0.0f);




        //Jump
        if (cc.isGrounded)
        {
            //whne space is pressed
            if (Input.GetKey(KeyCode.Space))
            {
                if (!controller.getInfState())
                {
                    playerAnim.SetBool("jump", true);
                    jumpForce = 15.5f;
                    playerAnim.applyRootMotion = false;
                }
            }
        }// control  move when in air, cant change rotation
        else if (!playerAnim.hasRootMotion)
            if (Input.GetAxis("Vertical") > 0)
                if (Input.GetAxis("Fire3") > 0)
                    cc.Move(this.transform.forward.normalized * jumpVel * Time.deltaTime * 2.0f);
                else
                    cc.Move(this.transform.forward.normalized * jumpVel * Time.deltaTime);
            else if (Input.GetAxis("Vertical") < 0)
                cc.Move(-this.transform.forward.normalized * jumpVel * Time.deltaTime);



        //gravity
        gravityCalculation();

    }

    private void gravityCalculation()
    {
        //gravity
        if (jumpForce > 0)
            jumpForce -= 9.8f * Time.deltaTime;
        else
            jumpForce = 0.0f;

        Vector3 verticalVector = (Vector3.down * 9.8f) + (Vector3.up * jumpForce);
        cc.Move(verticalVector * Time.deltaTime); ;

        if (cc.isGrounded)
        {
            playerAnim.SetBool("jump", false);
            playerAnim.applyRootMotion = true;
        }
    }

    //actions
    private void useAction()
    {
        if (!action)
            //enable action and save the time of the action
            startAction = 0;

        else
        {
            locSpeed = 0.5f;
            playerAnim.SetFloat("LocSpeed", locSpeed);
            playerAnim.SetBool("moving", false);
            // if has not passed 2 s from the start
            if (startAction < 2.5f)
                startAction += Time.deltaTime;
            else
            {
                action = false;
            }
        }
        playerAnim.SetBool("action", action);
    }

    //set if player can interact
    public bool setInteract
    {
        set { canInteract = value; }
    }

    // retur action
    public bool getActionState { get { return (action); } }

}


