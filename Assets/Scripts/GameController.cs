using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public Canvas hud;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
        hud.gameObject.SetActive(true);
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
