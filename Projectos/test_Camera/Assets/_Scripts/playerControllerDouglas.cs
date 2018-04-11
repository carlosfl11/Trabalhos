using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControllerDouglas : MonoBehaviour
{
    public float animationVel,animationAcel ;

    //Animation
    private Animator playerAnim;
    private float idleAnim, changevalue;
    private float LocX, LocY;

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

        //Camera
        cameraPositionTarget = transform.position - (transform.forward * CameraDistance) + (transform.up * CameraHeight);

        mainCamera.LookAt(cameraTarget);

    }

    void Update()
    {
        // idle
        if (!playerAnim.GetBool("isMoving"))
        {
            if (idleAnim <= 0)
                changevalue = 0.001f;
            else if (idleAnim >= 1)
                changevalue = -0.001f;

            idleAnim += changevalue;
            playerAnim.SetFloat("randIdle", idleAnim);
        }

        // move
        Movement();

        //CameraPosition
        cameraPositionTarget = transform.position - (transform.forward * CameraDistance) + (transform.up * CameraHeight);

        mainCamera.position = cameraPositionTarget;
        mainCamera.LookAt(cameraTarget);


    }

    private void Movement()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            //moving
            playerAnim.SetBool("isMoving", true);

            // blendTree smooth
            //walk
            if (Input.GetAxis("Horizontal") > 0)
            {
                if (LocX < 1.0f)
                    LocX += animationVel;
                else
                    LocX = 1.0f;
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                if (LocX > -1.0f)
                    LocX -= animationVel;
                else
                    LocX = -1.0f;
            }
            else
            {
                if (LocX > animationVel)
                    LocX -= animationVel;
                else if (LocX < 0.0f)
                    LocX += animationVel;
                else if (LocX - animationVel < 0.0f || LocX + animationVel > 0.0f)
                    LocX = 0.0f;
            }

            //running

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (LocY < 1.0f)
                    LocY += animationAcel;
                else
                    LocY = 1.0f;
            }
            else
            {
                if (LocY - animationAcel > 0.0f)
                    LocY -= animationAcel;
                else
                    LocY = 0.0f;
            }
            playerAnim.SetFloat("LocY", LocY);
            playerAnim.SetFloat("LocX", LocX);
        }
        else
            //not Moving
            playerAnim.SetBool("isMoving", false);
    }

    //private Vector3 calculatecameraPos()
    //{

    //}
}
