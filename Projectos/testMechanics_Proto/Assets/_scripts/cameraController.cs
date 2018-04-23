using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{

    //pref
    public GameObject objToFollow;
    public float camDistance, maxCamDistanceFromObj = 0.2f, camMoveVel = 0.05f;
    public float anglePerSec = 5.0f;
    public float RunDistanceMulti = 1.1f;

    // on run
    private Animator objToFollowAnim;
    private float onRunCurrentDistance = 0.0f;

    // target
    private Transform camTarget;
    private Vector3 camPositionVec;
    private Camera mainCam;
    private Vector3 targetRotationVec;

    // side
    public bool sideLeft = true;
    private Vector3 currentCamTargetPos;
    private float currentCamTargetX;

    //collision
    private bool isColliding = false;
    private Collision coll;


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

        //get animator from target
        objToFollowAnim = objToFollow.GetComponent<Animator>();

        // set position and lookAt for the cam
        mainCam.transform.position = camTarget.position + camPositionVec;
        mainCam.transform.LookAt(camTarget);

    }

    // Update is called once per frame
    void Update()
    {
        //target position function
        camSide();
        //smoth ajust camPosition
        camAjust();

        // update set the new position and lookAt for the cam
        mainCam.transform.position = camTarget.position + camPositionVec;
        mainCam.transform.LookAt(camTarget);

        //Debug
        camDebug();
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
        //updete camera target x position
        currentCamTargetPos.x = currentCamTargetX;
        camTarget.localPosition = currentCamTargetPos;
    }

    //camera rotation debug
    private void camDebug()
    {
        Debug.DrawRay(objToFollow.transform.position, -objToFollow.transform.forward, Color.red);
        Debug.DrawRay(objToFollow.transform.position, camPositionVec, Color.green);

    }

    private void camAjust()
    {
        // update the vector that puts the cam on position
        targetRotationVec = -objToFollow.transform.forward * camDistance;
        // test the angle between old camPostion and the target one, smooth transition
        if (Vector3.Angle(targetRotationVec, -mainCam.transform.forward) > 0.5f &&
            Vector3.Angle(targetRotationVec, -mainCam.transform.forward) <= 30.0f)
        {
            // using croos funcition can identify if new target is on the left or right
            if (Vector3.Cross(targetRotationVec, -mainCam.transform.forward).y > 0.0f)
                camPositionVec = Quaternion.AngleAxis(-anglePerSec * Time.deltaTime, mainCam.transform.up) * camPositionVec;
            else if (Vector3.Cross(targetRotationVec, -mainCam.transform.forward).y < 0.0f)
                camPositionVec = Quaternion.AngleAxis(anglePerSec * Time.deltaTime, mainCam.transform.up) * camPositionVec;
        }
        //limit max angle offset to 30º
        else if (Vector3.Angle(targetRotationVec, -mainCam.transform.forward) > 30.0f)
        {
            if (Vector3.Cross(targetRotationVec, -mainCam.transform.forward).y > 0.0f)
                camPositionVec = Quaternion.AngleAxis(29.0f, mainCam.transform.up) * targetRotationVec;
            else if (Vector3.Cross(targetRotationVec, -mainCam.transform.forward).y < 0.0f)
                camPositionVec = Quaternion.AngleAxis(-29.0f, mainCam.transform.up) * targetRotationVec;
        }

        // in and out on moving fast
        if (objToFollowAnim.GetFloat("LocSpeed") >= 0.75f)
        {
            //if camera distance is less than running offset
            if (onRunCurrentDistance < camDistance * RunDistanceMulti)
                onRunCurrentDistance += camMoveVel * 0.5f * Time.deltaTime;
            else
                onRunCurrentDistance = camDistance * RunDistanceMulti;
        }
        else // not running camera ajust
        {
            if (onRunCurrentDistance > 0.0f)
                onRunCurrentDistance -= camMoveVel * 0.5f * Time.deltaTime;
            else
                onRunCurrentDistance = 0.0f;
        }


        //set camPositionVec distance to camDistance
        camPositionVec = camPositionVec.normalized * (camDistance + onRunCurrentDistance);
        camCollision();
    }

    // camera collision
    private void camCollision()
    {
        if (isColliding)
        {
            
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        isColliding = true;
        coll = collision;
        Debug.DrawRay(coll.contacts[0].point, Vector3.up, Color.blue);

    }
    private void OnCollisionEnter(Collision collision)
    {
        isColliding = true;
        coll = collision;
        Debug.DrawRay(coll.contacts[0].point, Vector3.up, Color.blue);


    }
    private void OnCollisionExit(Collision collision)
    {
        isColliding = false;
        coll = null;
    }
}
