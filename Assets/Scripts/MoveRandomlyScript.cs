using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRandomlyScript : MonoBehaviour {

    private Vector3 StartPosition;
    private Vector3 destination;
    private float moveSpeed;

	// Use this for initialization
	void Awake () {
        StartPosition = gameObject.transform.position;
        StartCoroutine(ChangeDirection());
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 position = gameObject.transform.position;
        gameObject.transform.position = Vector3.MoveTowards(position, destination, moveSpeed * Time.deltaTime);
	}

    private IEnumerator ChangeDirection()
    {
        Vector3 startPosition = new Vector3(StartPosition.x, StartPosition.y, StartPosition.z);
        destination = startPosition += new Vector3(Random.Range(-13, 13), Random.Range(-7, 10));
        moveSpeed = Random.Range(5, 15);
        yield return new WaitForSeconds(Random.Range(2, 5));
        StartCoroutine(ChangeDirection());
    }
}
