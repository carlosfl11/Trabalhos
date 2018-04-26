using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    //public 
    public float animAcelaration = 0.04f;
    public float angularVel = 10.0f;
    public float jumpVel = 5.0f;
    private float infectedAngularSpeed = 1.5f;

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
    private float currentSpeed = 1.0f;

    //vertical movel
    private float jumpForce;

    //fighting
    private bool isAttacking = false;
    private bool isBlocking = false;
    private bool giveHit = false, canHit = false;
    private float hitTimer = 0.0f;

    //hp
    private float hp = 100.0f;
    private bool isAlive = true;

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

        //fight function
        if (controller.getInfState())
            fighting();

        //hp check
        if (hp <= 0.0f)
            isAlive = false;
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

        //rotation
        if (cc.isGrounded)
        {
            if (controller.getInfState())
                currentSpeed = infectedAngularSpeed;
            else
                currentSpeed = 1.0f;

            //when walking
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
                    this.transform.Rotate(this.transform.up, angularVel * Time.deltaTime * 0.8f * currentSpeed);
                else if (Input.GetAxis("Horizontal") < 0)
                    this.transform.Rotate(this.transform.up, -angularVel * Time.deltaTime * 0.8f * currentSpeed);
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
                if (!controller.getInfState() && Input.GetAxis("Vertical") >= 0.0f)
                {
                    playerAnim.SetBool("jump", true);
                    jumpForce = 15.5f;
                    playerAnim.applyRootMotion = false;
                }
                else if (controller.getInfState() && Input.GetAxis("Vertical") >= 0.0f)
                {
                    playerAnim.SetBool("jump", true);
                    jumpForce = 18.5f;
                    playerAnim.applyRootMotion = false;
                }

            }
        }// control  move when in air, cant change rotation
        else if (!playerAnim.hasRootMotion)
        {
            if (!controller.getInfState())
            {
                if (Input.GetAxis("Vertical") > 0)
                    if (Input.GetAxis("Fire3") > 0)
                        cc.Move(this.transform.forward.normalized * jumpVel * Time.deltaTime * 2.0f);
                    else
                        cc.Move(this.transform.forward.normalized * jumpVel * Time.deltaTime);

            }
            else if (controller.getInfState())
            {
                if (Input.GetAxis("Vertical") > 0)
                    if (Input.GetAxis("Fire3") > 0)
                        cc.Move(this.transform.forward.normalized * jumpVel * Time.deltaTime * 2.0f * 10.0f);
                    else
                        cc.Move(this.transform.forward.normalized * jumpVel * Time.deltaTime * 10.0f);
            }
        }

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

    private void fighting()
    {
        //has to be on infected char
        if (controller.getInfState() && cc.isGrounded && !playerAnim.GetBool("jump"))
        {
            //left mouse 
            if (Input.GetAxis("Fire1") > 0.0f)
            {
                isAttacking = true;
                isBlocking = false;

                if (canHit)
                {
                    hitTimer += Time.deltaTime;
                    if (hitTimer > 1.0f)
                    {
                        giveHit = true;
                        hitTimer = 0.0f;
                    }
                }
                else
                {
                    giveHit = false;
                }
            }
            //right mouse
            else if (Input.GetAxis("Fire2") > 0.0f)
            {
                isAttacking = false;
                isBlocking = true;
            }
            else
            {
                hitTimer = 0.0f;
                isAttacking = false;
                isBlocking = false;
                giveHit = false;
            }
            //set animator parameters
            playerAnim.SetBool("block", isBlocking);
            playerAnim.SetBool("attack", isAttacking);
        }
    }

    //set if player can interact
    public bool setInteract
    {
        set { canInteract = value; }
    }

    // retur action
    public bool getActionState { get { return (action); } }


    //fighting
    public bool setCanHit { set { canHit = value; } }
    public bool hit { get { return (giveHit); } }

    //take damage
    public void takeDamage(float amount)
    {
        hp -= amount;
    }
}


