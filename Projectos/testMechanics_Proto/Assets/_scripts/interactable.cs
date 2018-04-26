
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactable : MonoBehaviour {

    //type of objects that player can interact
    public enum obj {ball, door};
    public obj typeOfObject;

    //trigger the object action
    private bool didTakeAction = false;
	
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
                    didTakeAction = false;
                    break;
                case obj.door:
                    this.GetComponent<door>().actionDoor();
                    Debug.Log("door take action");
                    didTakeAction = false;
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
