﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyCtrl : MonoBehaviour
{
    //get player
    private GameObject player;
    private Rigidbody rb;
    private Vector3 playerDir;

    //sethp
    private float hp = 100.0f;

    private NavMeshAgent agent;
    private NavMeshPath path;
    private float force = 0.0f;

    //idle/ follow / attack
    private bool isIdle, isFollowing, isAttacking;
    private float attackTimer = 0.0f;

    //Ragdoll
    private bool isRagdollDisable = true;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("player");
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();


        isIdle = true;
        isFollowing = false;
        isAttacking = false;

        Ragdoll();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0.0f)
        {
            isRagdollDisable = false;
            Ragdoll();

        }
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

            agent.Move(playerDir * force * Time.deltaTime);
            force -= force * Time.deltaTime;
            if (force < 0)
                force = 0.0f;

            follow();
            attack();
        }
    }

    public void takeDMG(float amount)
    {
        hp -= amount;
        force = 25.0f;
        playerDir = transform.position - player.transform.position;
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
            if (agent.remainingDistance < 1.2f)
            {
                agent.isStopped = true;
                isFollowing = false;
                isAttacking = true;
            }
        }

    }

    //atack
    private void attack()
    {
        if (isAttacking)
        {
            if (attackTimer > 2.0f)
                attackTimer = 0.0f;
            else
                attackTimer += Time.deltaTime;
        }
    }

    private void Ragdoll()
    {
        Rigidbody[] ragDollRb = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody r in ragDollRb) { r.isKinematic = isRagdollDisable; }
    }
}
