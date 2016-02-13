using UnityEngine;
using System.Collections;


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
    Eight = 1 << 9,
    Roof = 1 << 10
}

/// <summary>
/// Provides static utility functions for Floor Enum
/// </summary>
public class FloorUtility {
    

    public static int Index(Floor floor) {
        switch (floor) {
            case Floor.MinusOne: return 0;
            case Floor.Zero: return 1;
            case Floor.One: return 2;
            case Floor.Two: return 3;
            case Floor.Three: return 4;
            case Floor.Four: return 5;
            case Floor.Five: return 6;
            case Floor.Six: return 7;
            case Floor.Seven: return 8;
            case Floor.Eight: return 9;
            case Floor.Roof: return 10;
        }
        // impossible to go here
        return 0;
    }


    public static int Number(Floor floor) {
        switch (floor) {
            case Floor.MinusOne: return -1;
            case Floor.Zero: return 0;
            case Floor.One: return 1;
            case Floor.Two: return 2;
            case Floor.Three: return 3;
            case Floor.Four: return 4;
            case Floor.Five: return 5;
            case Floor.Six: return 6;
            case Floor.Seven: return 7;
            case Floor.Eight: return 8;
            case Floor.Roof: return 9;
        }
        // impossible to go here
        return 0;
    }

    public static Floor FloorForNumber(int number) {
        switch (number) {
            case -1: return Floor.MinusOne;
            case 0: return Floor.Zero;
            case 1: return Floor.One;
            case 2: return Floor.Two;
            case 3: return Floor.Three;
            case 4: return Floor.Four;
            case 5: return Floor.Five;
            case 6: return Floor.Six;
            case 7: return Floor.Seven;
            case 8: return Floor.Eight;
            case 9: return Floor.Roof;
            default: throw new UnityException("Invalid Floor Number: " + number);
        }
    }

    public static Floor FloorForIndex(int index) {
        return FloorForNumber(index - 1);
    }
}
