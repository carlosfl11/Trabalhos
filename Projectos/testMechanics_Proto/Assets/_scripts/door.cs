using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour {

    public bool isOpen = false;

    private float rot;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (isOpen)
            transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        else
            transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void actionDoor()
    {
        if (isOpen)
            isOpen = false;
        else
            isOpen = true;
    }
}
