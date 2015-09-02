using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

    public GameObject Hand;

    public GameObject[] SpawnPrefabs;

    public float SpawnEveryXSeconds;

    public GameObject CurrentObject;
    public Rigidbody2D Rigid;

    public float StartPositionX;
    public float EndPositionX;

    public float MoveSpeed = 20;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private float highestOfHigh = 0;

    private float distance = 0;
    private bool doTheLerp = false;
    private float newY = 0;

    private int howManyDied = 0;

	// Use this for initialization
	IEnumerator Start () {
        //distance = Mathf.Abs(GameObject.Find("plank").transform.position.y - gameObject.transform.position.y) / 2;
        yield return new WaitForSeconds( 3 );

        distance = 2;
        Spawn();
	}

    public void Spawn()
    {        
        var position = new Vector3(Random.Range(StartPositionX, EndPositionX), 18, transform.position.z);
        Hand.transform.position = new Vector3( position.x, 22, transform.position.z );
        CurrentObject = Instantiate(SpawnPrefabs[Random.Range(0, SpawnPrefabs.Length)], position, Quaternion.identity) as GameObject;
        spawnedObjects.Add(CurrentObject);
        Rigid = CurrentObject.GetComponent<Rigidbody2D>();
        Rigid.isKinematic = true;
        CurrentObject.transform.parent = Hand.transform;
        
        iTween.MoveTo( Hand, iTween.Hash( "time", 2, "y", transform.position.y + 2, "easetype", iTween.EaseType.linear, "oncomplete", "OnCompleteHand", "oncompletetarget", gameObject ) );
    }

    public void OnCompleteHand() {
        CurrentObject.transform.parent = null;
        Rigid.isKinematic = false;

        iTween.MoveTo( Hand, iTween.Hash( "time", 1, "y", 22, "easetype", iTween.EaseType.linear, "oncomplete", "OnCompleteHand", "oncompletetarget", gameObject ) );
    }

    public void RemoveMe(GameObject gameObject)
    {
        spawnedObjects.Remove(gameObject);
        howManyDied++;
    }

    void Update()
    {
        GameObject.Find("Text").GetComponent<Text>().text = "Lives left: " + (10 - howManyDied);
        GameObject.Find("Score").GetComponent<Text>().text = "Score: " + (highestOfHigh + 20).ToString("#.00");

        highestOfHigh = GameObject.Find("plank").transform.position.y;
        foreach (var item in spawnedObjects)
        {
            if (item == CurrentObject) continue;
            if (item == null) continue;
            if (item.transform.position.y > highestOfHigh)
            {
                highestOfHigh = item.transform.position.y;
            }
        }        

        var distanceToHigh = transform.position.y - highestOfHigh;

        Debug.Log(distanceToHigh - distance);

        if (Mathf.Abs(distanceToHigh - distance) < 8 || Mathf.Abs(distanceToHigh - distance) > 15)
        {
            if (!doTheLerp)
            {
                doTheLerp = true;
                newY = highestOfHigh + distance + 15;
            }
        }


        if (doTheLerp)
        {
            var pos = transform.position;
            pos.y = Mathf.Lerp(transform.position.y, newY, Time.deltaTime);
            transform.position = pos;

            if (pos.y >= newY - 0.5f)
            {
                doTheLerp = false;
            }
        }
        //Debug.Log(Mathf.Abs(transform.position.y - highestOfHigh));

        //if (Mathf.Abs(transform.position.y - highestOfHigh) < distance - 5)
        //{
        //    if (!doTheLerp)
        //    {
        //        doTheLerp = true;
        //        newY = highestOfHigh;
        //    }
        //}
        //else if (Mathf.Abs(transform.position.y - highestOfHigh) > distance + 5)
        //{
        //    if (!doTheLerp)
        //    {
        //        doTheLerp = true;
        //        newY = highestOfHigh - 7.5f;
        //    }
        //}

        //if (doTheLerp)
        //{
        //    var pos = transform.position;
        //    pos.y = Mathf.Lerp(transform.position.y, newY + distance + 2.5f, Time.deltaTime);
        //    transform.position = pos;

        //    if (pos.y >= newY + distance + 2.45f)
        //    {
        //        doTheLerp = false;
        //    }
        //}
    }
	
	// Update is called once per frame
	void FixedUpdate () {        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Rigid.AddForce(Vector3.left * Rigid.mass * MoveSpeed);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Rigid.AddForce(Vector3.right * Rigid.mass * MoveSpeed);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Rigid.AddForce(Vector3.up * Rigid.mass * 10);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            Rigid.AddForce(Vector3.down * Rigid.mass * 5);
        }


        if (Input.GetKey(KeyCode.Q))
        {
            Rigid.AddTorque(MoveSpeed / Rigid.mass);
        }

        if (Input.GetKey(KeyCode.E))
        {
            Rigid.AddTorque(-MoveSpeed / Rigid.mass);
        }
	}
}
