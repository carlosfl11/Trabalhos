using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyCtrl : MonoBehaviour
{
    //get player
    private GameObject player;
    private Rigidbody rb;

    //sethp
    private float hp = 100.0f;

    private NavMeshAgent agent;
    private NavMeshPath path;

    //idle/ follow / attack
    private bool isIdle, isFollowing, isAttacking;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("player");
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();


        isIdle = true;
        isFollowing = false;
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0.0f)
            Destroy(this.gameObject);
        if (hp > 0)
        {
            // check for distance with target
            if (Vector3.Distance(this.transform.position, player.transform.position) < 50.0f)
            {
                isFollowing = true;
                isIdle = false;
                isAttacking = false;
            }
            else
            {
                isIdle = true;
                isFollowing = false;
                isAttacking = false;
                agent.isStopped = true;
            }

            follow();
            //Debug.Log(hp);
        }
    }

    public void takeDMG(float amount)
    {
        hp -= amount;
        rb.AddForce(player.transform.forward * 300.0f + Vector3.up * 100.0f);
    }

    //moving
    private void follow()
    {
        if (isFollowing)
        {
            agent.isStopped = false;
            if (agent.CalculatePath(player.transform.position, path))
            {
                agent.SetDestination(player.transform.position);
            }
            if (agent.remainingDistance < 2.0f)
            {
                agent.isStopped = true;
                Debug.Log("attack");
            }
        }
        
    }
}
