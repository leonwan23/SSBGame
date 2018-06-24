using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadoukenScript : MonoBehaviour {

    public float speed = 10;
    public float damage;
    public MrSmileyScript caster;
    private Rigidbody2D rb;
    public float timeToDestroy;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();

        //if(transform.rotation.y > 0) //facing right
        //{ 
        //    rb.AddForce(Vector3.right * speed, ForceMode2D.Impulse);
        //} else
        //{
        //    rb.AddForce(Vector3.right * -speed, ForceMode2D.Impulse);
        //}
	}
	
	// Update is called once per frame
	void Update () {
        Destroy(gameObject, timeToDestroy);
	}
}
