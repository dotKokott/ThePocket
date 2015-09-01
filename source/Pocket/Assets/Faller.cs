using UnityEngine;
using System.Collections;

public class Faller : MonoBehaviour {

    private bool ignore = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < -15)
        {
            GameObject.Find("Spawner").GetComponent<Spawner>().RemoveMe(gameObject);
            if (!ignore)
            {
                GameObject.Find("Spawner").GetComponent<Spawner>().Spawn();
            }
            
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (ignore) return;
        
        GameObject.Find("Spawner").GetComponent<Spawner>().Spawn();
        ignore = true;
    }
}
