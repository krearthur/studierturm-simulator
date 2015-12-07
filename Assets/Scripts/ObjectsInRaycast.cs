using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectsInRaycast : MonoBehaviour {

    public Sprite defaultCursor;
    public Sprite interactCursor;

    public RaycastHit[] hits;
    public bool hasObjects;

    public Image cursorImage;

    public LayerMask interactiveObjectsMask;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width*0.5f, Screen.height*0.5f));

        hits = Physics.RaycastAll(ray, 1.1f, interactiveObjectsMask.value);
        Debug.DrawRay(ray.origin, ray.direction);

        if(hits != null && hits.Length>0) {
            hasObjects = true;
            cursorImage.sprite = interactCursor;
        }
        else {
            hasObjects = false;
            cursorImage.sprite = defaultCursor;
        }
        
	}
    
    void OnDisable() {
        cursorImage.enabled = false;
    }
    void OnEnable() {
        cursorImage.enabled = true;
    }
}
