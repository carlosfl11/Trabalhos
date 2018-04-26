using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fightDetect : MonoBehaviour
{

    private playerMove plMove;
    private void Awake()
    {
        plMove = GetComponentInParent<playerMove>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // when enemy is in range
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
            plMove.setCanHit = true;
    }

    //enemy leaves range
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "enemy")
            plMove.setCanHit = false;
    }

    private void OnTriggerStay(Collider other)
    {
        //in range and player time for hit 
        if (other.tag == "enemy" && plMove.hit)
        {
            other.GetComponentInParent<enemyCtrl>().takeDMG(25.0f);
            plMove.setCanHit = false;
        }
        else if (other.tag == "enemy")
            plMove.setCanHit = true;
    }
}
