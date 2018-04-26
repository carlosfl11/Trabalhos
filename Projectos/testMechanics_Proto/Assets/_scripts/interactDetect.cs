using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactDetect : MonoBehaviour
{
    private playerMove plMove;
    private bool actionStarted = false;
    private bool actionEnded = false;

    private void Awake()
    {
        plMove = GameObject.Find("player").GetComponent<playerMove>();
    }

    //when trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "interactable")
            plMove.setInteract = true;
    }
    //when trigger exits
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "interactable")
            plMove.setInteract = false;
        
    }

    private void OnTriggerStay(Collider other)
    {
        // when player is in contact with a object
        //when action started
        if (plMove.getActionState)
            actionStarted = true;
        //when action ended
        else if (actionStarted && !plMove.getActionState)
            actionEnded = true;

        // do action
        if (actionStarted && actionEnded && other.tag == "interactable")
        {
            other.GetComponent<interactable>().actionDone();

            // reset values
            actionStarted = false;
            actionEnded = false;
            plMove.setInteract = false;

        }
    }
}
