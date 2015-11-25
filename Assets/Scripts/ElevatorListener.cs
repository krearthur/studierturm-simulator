using UnityEngine;
using System.Collections;

public interface ElevatorListener {

    void ElevatorOpening();
    void ElevatorClosing();
    void ElevatorOpened();
    void ElevatorClosed();
    void FloorArrived(Floor arrived);
    void FloorPassed(Floor passed);
    void DrivingUp();
    void DrivingDown();
}
