using UnityEngine;
using System.Collections;

public class Zoomer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        iTween.ValueTo(gameObject, iTween.Hash("from", 135, "to", 15, "time", 5f, "easetype", iTween.EaseType.easeOutBounce, "onupdate", "updatecamera"));
	}

    void updatecamera(float a)
    {
        Camera.main.orthographicSize = a;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
