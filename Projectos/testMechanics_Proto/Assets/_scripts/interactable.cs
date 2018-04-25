using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactable : MonoBehaviour {

    //type of objects that player can interact
    public enum obj {ball, door};
    public obj typeOfObject;

    //trigger the object action
    private bool didTakeAction = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if action
        if (didTakeAction)
        {
            //switch case of type of obj
            switch(typeOfObject)
            {
                case obj.ball:
                    Debug.Log("ball take action");
                    Destroy(this.gameObject);
                    break;
                case obj.door:
                    Debug.Log("door take action");
                    break;
            }
        }
	}

    //active action by player interaction
    public void actionDone()
    {
        didTakeAction = true;
    }
}
