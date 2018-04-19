using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{

    public GameObject objToFollow;
    public float camDistance;

    private float camHeight;
    private Transform camTarget;
    private Vector3 camPositionVec;
    private Camera mainCam;

    // Use this for initialization
    void Start()
    {
        // find mainCam & camTarget on the objToFollow childs
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camTarget = objToFollow.transform.Find("camTarget").transform;

        // camHeight = local height of the target
        camHeight = camTarget.localPosition.y;

        // calculate the vector that puts the cam on position
        camPositionVec = -objToFollow.transform.forward.normalized * camDistance;
        camPositionVec.y = camTarget.position.y;
        

        // set position and lookAt for the cam
        mainCam.transform.position = objToFollow.transform.position + camPositionVec;
        mainCam.transform.LookAt(camTarget);

    }

    // Update is called once per frame
    void Update()
    {
        //!!Remake
        mainCam.transform.position = objToFollow.transform.position + camPositionVec;


        // update lookAt for the cam
        mainCam.transform.LookAt(camTarget);
    }
}
