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
	}
	
	// Update is called once per frame
	void Update () {
        Destroy(gameObject, timeToDestroy);
	}
}
