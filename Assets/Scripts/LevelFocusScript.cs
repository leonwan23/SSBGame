using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFocusScript : MonoBehaviour {

    public float HalfXBounds = 3f;
    public float HalfYBounds = 2.5f;
   
    public Bounds FocusBounds;

    // Update is called once per frame
    void Update () {
        Vector3 position = gameObject.transform.position;
        Bounds bounds = new Bounds();

        //adding position to bounds
        bounds.Encapsulate(new Vector3(position.x - HalfXBounds, position.y - HalfYBounds));
        bounds.Encapsulate(new Vector3(position.x + HalfXBounds, position.y + HalfYBounds));
        FocusBounds = bounds;
    }
}
