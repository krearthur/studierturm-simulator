using UnityEngine;
using System.Collections;
using System;

public class ElevatorLED : MonoBehaviour, ElevatorListener {

    public Elevator elevator;

    public Material[] numbers;

    private Material ledMat;

    public void DrivingDown() {
    }

    public void DrivingUp() {
    }

    public void ElevatorClosed() {
    }

    public void ElevatorClosing() {
    }

    public void ElevatorOpened() {
    }

    public void ElevatorOpening() {
    }

    public void FloorArrived(Floor arrived) {
    }

    public void FloorPassed(Floor passed) {
   //     Debug.Log("set mat");
     //   ledMat.mainTexture = numbers[FloorUtility.Index(passed)];
      //  ledMat.SetTexture("_EmissionMap", ledMat.mainTexture);
      //  ledMat.SetColor("_EmissionColor", Color.white);
      //  DynamicGI.SetEmissive(GetComponent<Renderer>(), Color.white);

        GetComponent<Renderer>().material = numbers[FloorUtility.Index(passed)]; 

    }

    // Use this for initialization
    void Start () {
        ledMat = new Material(GetComponent<Renderer>().material);
        elevator.AddListener(this);
	}
	
}
