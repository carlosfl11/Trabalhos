using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControllerDouglas : MonoBehaviour
{
    //Rotation
    public float acelaration, angularVelocity;
    private float actualAVel;

    //Animation
    private Animator playerAnim;
    private float idleAnim, changevalue;

    //Camera
    public float CameraHeight, CameraDistance, CameraVelocity;
    private Transform mainCamera, cameraTarget;
    private Vector3 cameraPositionTarget;


    // Use this for initialization
    void Start()
    {
        //animator
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        cameraTarget = GameObject.Find("CameraTarget").transform;

        playerAnim = GetComponent<Animator>();
        actualAVel = angularVelocity;

        //Camera
        cameraPositionTarget = transform.position - (transform.forward * CameraDistance) + (transform.up * CameraHeight);

        mainCamera.LookAt(cameraTarget);

    }

    void Update()
    {
        // idle
        if (!playerAnim.GetBool("isfighting") && !playerAnim.GetBool("isMoving"))
        {
            if (idleAnim <= 0)
                changevalue = 0.001f;
            else if (idleAnim >= 1)
                changevalue = -0.001f;

            idleAnim += changevalue;
            playerAnim.SetFloat("randIdle", idleAnim);
        }

        // move
        if (Input.GetAxis("Vertical") > 0)
        {
            //walking forward
            playerAnim.SetBool("isMoving", true);
            // turn faster when wallking
            actualAVel = angularVelocity * 2;

            //run
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // turn velocity return to base value
                actualAVel = angularVelocity;
                // blend transition to running
                if (playerAnim.GetFloat("LocX") < 1.0f)
                    playerAnim.SetFloat("LocX", playerAnim.GetFloat("LocX") + acelaration * Time.deltaTime);
            }
            else if (playerAnim.GetFloat("LocX") > 0.0f)
                playerAnim.SetFloat("LocX", playerAnim.GetFloat("LocX") - acelaration * Time.deltaTime);

            // turn while moving
            if (Input.GetAxis("Horizontal") > 0)
                transform.Rotate(transform.up, actualAVel * Time.deltaTime);
            else if (Input.GetAxis("Horizontal") < 0)
                transform.Rotate(transform.up, -actualAVel * Time.deltaTime);

        }
        else
            playerAnim.SetBool("isMoving", false);

        //CameraPosition
        cameraPositionTarget = transform.position - (transform.forward * CameraDistance) + (transform.up * CameraHeight);

        mainCamera.position = cameraPositionTarget;
        mainCamera.LookAt(cameraTarget);


    }

    //private Vector3 calculatecameraPos()
    //{

    //}
}
