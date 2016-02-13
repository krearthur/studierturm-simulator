using UnityEngine;
using System.Collections;

/// <summary>
/// Calculates floor by position and triggers the 
/// right ToggleActivation instances for the floors
/// </summary>
public class ToggleFloorActivationByPosition : MonoBehaviour {

    public ToggleActivation[] enterFloorTogglers;
    public ToggleActivation[] exitFloorTogglers;

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
            // Player has changed floor
            for(int i=0; i < enterFloorTogglers.Length; i++) {
                if(currentFloorNumber-1 == i) {
                    enterFloorTogglers[i].Trigger();
                }
                else {
                    exitFloorTogglers[i].Trigger();
                }
                
            }
        }
	}
}
