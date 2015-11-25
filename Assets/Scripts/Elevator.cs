using UnityEngine;
using System.Collections.Generic;

public enum Floor {
    MinusOne = 1 << 0,
    Zero = 1 << 1,
    One = 1 << 2,
    Two = 1 << 3,
    Three = 1 << 4,
    Four = 1 << 5,
    Five = 1 << 6,
    Six = 1 << 7,
    Seven = 1 << 8,
    Eight = 1 << 9
}

public class Elevator : MonoBehaviour, SlidingDoorListener {

    private int targetFloors;
    public Floor currentFloor = Floor.Eight;
    private bool drivingUp;
    private bool drivingDown;
    private List<ElevatorListener> listeners = new List<ElevatorListener>();
    private Animator animator;

    public Collider doorColliders;

    public Vector3[] floorPositions = new Vector3[10] {
        new Vector3(4,0,4),
        new Vector3(4,2,4),
        new Vector3(4,4,4),
        new Vector3(4,6,4),
        new Vector3(4,8,4),
        new Vector3(4,10,4),
        new Vector3(4,12,4),
        new Vector3(4,14,4),
        new Vector3(4,16,4),
        new Vector3(4,18,4)
        };

    private Vector3 targetFloorPos;

    


    void Start() {
        animator = GetComponent<Animator>();
        foreach(SlidingDoorStateBehaviour behaviour in animator.GetBehaviours<SlidingDoorStateBehaviour>()) {
            behaviour.AddListener(this);
        }
        
        targetFloors = 0;
    }

    // Update is called once per frame
    void Update() {
        if (IsStanding()) {
            if (targetFloors>0 && IsInTargets(currentFloor)) {
                // remove current floor from target floor
                targetFloors &= ~(int)currentFloor;
                if (IsClosed()) {
                    animator.SetTrigger("OpenTrigger");
                }
                
                FloorArrived();
            }
        }
        else if (drivingUp) {
            // TODO move elevator up and check for target pos
        }
        else if (drivingDown) {
            // TODO move elevator down and check for target pos
        }
    }

    public void CallToFloor(Floor floor) {
        targetFloors |= (int)floor;
        if (IsStanding()) {
            if ((int)currentFloor > (int)floor) {
                drivingDown = true;
            }
            else if ((int)currentFloor < (int)floor) {
                drivingUp = true;
            }
        }
        CalculateTargetFloorPosition();
    }

    public void AddListener(ElevatorListener lis) {
        listeners.Add(lis);
    }

    private void CalculateTargetFloorPosition() {
        for (int i = 0; i < 9; i++) {
            int tmpFloor = MoveFromCurrentFloor(drivingUp, drivingDown, i);
            if (IsInTargets(tmpFloor)) {
                // found nearest target floor
                SetTargetFloorPosition((Floor)tmpFloor);
                break;
            }
        }
    }

    public void InterruptClosing(float normTime) {
        animator.Play("Opening", 0, normTime);
    }

    private int MoveFromCurrentFloor(bool moveUp, bool moveDown, int floors) {
        if (moveUp) {
            return (int)currentFloor << floors;
        }
        else if (moveDown) {
            return (int)currentFloor >> floors;
        }

        return (int)currentFloor;
    }

    private void SetTargetFloorPosition(Floor floor) {
        switch (floor) {
            case Floor.MinusOne: targetFloorPos = floorPositions[0]; break;
            case Floor.Zero: targetFloorPos = floorPositions[1]; break;
            case Floor.One: targetFloorPos = floorPositions[2]; break;
            case Floor.Two: targetFloorPos = floorPositions[3]; break;
            case Floor.Three: targetFloorPos = floorPositions[4]; break;
            case Floor.Four: targetFloorPos = floorPositions[5]; break;
            case Floor.Five: targetFloorPos = floorPositions[6]; break;
            case Floor.Six: targetFloorPos = floorPositions[7]; break;
            case Floor.Seven: targetFloorPos = floorPositions[8]; break;
            case Floor.Eight: targetFloorPos = floorPositions[9]; break;
        }
    }

    private bool IsInTargets(Floor floor) {
        return ((int)floor & targetFloors) > 0;
    }
    private bool IsInTargets(int floor) {
        return (floor & targetFloors) > 0;
    }

    public Floor GetCurrentFloor() {
        return currentFloor;
    }
    
    public bool IsStanding() {
        return drivingUp == false && drivingDown == false;
    }

    public bool IsClosed() {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Closed");
    }

    public bool IsDrivingUp() {
        return drivingUp;
    }

    public bool IsDrivingDown() {
        return drivingDown;
    }

    private void FloorArrived() {
        foreach (ElevatorListener listener in listeners) {
            listener.FloorArrived(currentFloor);
        }
    }

    void SlidingDoorListener.DoorOpening() {
        foreach (ElevatorListener listener in listeners) {
            listener.ElevatorOpening();
        }
    }

    void SlidingDoorListener.DoorClosing() {
        doorColliders.isTrigger = false;
        foreach (ElevatorListener listener in listeners) {
            listener.ElevatorClosing();
        }
    }

    void SlidingDoorListener.DoorClosed() {

    }

    void SlidingDoorListener.DoorOpened() {
        doorColliders.isTrigger = true;
    }
}
