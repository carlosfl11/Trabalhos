using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{


    public GameObject objToFollow;
    public float camDistance, maxCamDistanceFromObj = 0.2f, camMoveVel = 0.05f;

    private Transform camTarget;
    private Vector3 camPositionVec;
    private Camera mainCam;

    public bool sideLeft = true;
    private Vector3 currentCamTargetPos;
    private float currentCamTargetX;

    // Use this for initialization
    void Start()
    {
        // define currente x on camera target
        currentCamTargetX = -maxCamDistanceFromObj;

        // find mainCam & camTarget on the objToFollow childs
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camTarget = objToFollow.transform.Find("camTarget").transform;

        //adjust camTarget to left
        //use a vector to keep track of camTarget Position
        currentCamTargetPos = new Vector3(currentCamTargetX, camTarget.localPosition.y, camTarget.localPosition.z);
        camTarget.localPosition = currentCamTargetPos;

        // calculate the vector that puts the cam on position
        camPositionVec = -objToFollow.transform.forward * camDistance;


        // set position and lookAt for the cam
        mainCam.transform.position = camTarget.position + camPositionVec;
        mainCam.transform.LookAt(camTarget);

    }

    // Update is called once per frame
    void Update()
    {
        //target position function
        camSide();

        // update the vector that puts the cam on position
        camPositionVec = -objToFollow.transform.forward * camDistance;
        //!!Remake
        mainCam.transform.position = camTarget.position + camPositionVec;
        // update lookAt for the cam
        mainCam.transform.LookAt(camTarget);
    }

    private void camSide()
    {
        // change cam from shoulder
        if (Input.GetKeyDown(KeyCode.Q))
            sideLeft = true;
        else if (Input.GetKeyDown(KeyCode.E))
            sideLeft = false;

        // move cam to the left shoulder
        //on the left side
        if (sideLeft)
        {
            if (currentCamTargetX - camMoveVel * Time.deltaTime < -maxCamDistanceFromObj)
                currentCamTargetX = -maxCamDistanceFromObj;
            else
                currentCamTargetX -= camMoveVel * Time.deltaTime;
        }
        //on the right side
        else if (!sideLeft)
        {
            if (currentCamTargetX + camMoveVel * Time.deltaTime > maxCamDistanceFromObj)
                currentCamTargetX = maxCamDistanceFromObj;
            else
                currentCamTargetX += camMoveVel * Time.deltaTime;
        }

        currentCamTargetPos.x = currentCamTargetX;
        camTarget.localPosition = currentCamTargetPos;
    }
}
