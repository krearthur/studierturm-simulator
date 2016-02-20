using UnityEngine;
using System.Collections;

/// <summary>
/// Calculates floor by position and triggers the 
/// right ToggleActivation instances for the floors
/// </summary>
public class ToggleFloorActivationByPosition : MonoBehaviour {

    public ToggleActivation[] floorTogglers;

    public float floorHeight = 3.103f;
    public float floor1PositionY = 0;
    
    public int currentFloorNumber;
    private int lastFloorNumber;

    public Transform observedObject;
    
	
	// Update is called once per frame
	void Update () {

        lastFloorNumber = currentFloorNumber;
        currentFloorNumber = 1+ (int)((observedObject.position.y - floor1PositionY) / floorHeight);


	    if(currentFloorNumber != lastFloorNumber) {
//            Debug.Log("changed from floor " + lastFloorNumber + " to " + currentFloorNumber);
            // Player has changed floor
            for(int i=0; i < floorTogglers.Length; i++) {
                if(i - 1 == currentFloorNumber - 1 || i == currentFloorNumber - 1 || i + 1 == currentFloorNumber - 1 ) {
                    //Debug.Log("activate floor " + (i+1));
                    floorTogglers[i].Trigger(1);
                }
                else {
                    //Debug.Log("deactivate floor " + (i+1));
                    floorTogglers[i].Trigger(0);
                }
                
            }
        }
	}
}
