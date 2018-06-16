using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupCamScript : MonoBehaviour {

    public LevelFocusScript focusLevel;
    public List<GameObject> players;
    public float DepthUpdateSpeed = 5f; //how quickly camera zooms in and out
    public float AngleUpdateSpeed = 7f; //how quickly camera pans up and down
    public float PositionUpdateSpeed = 5f; //how quickly camera moves left and right, top to bottom

    public float DepthMax = -10f; //how far out camera can zoom at most
    public float DepthMin = -22f; //how far in camera can zoom at most

    public float AngleMax = 11f;
    public float AngleMin = 3f;

    private float CameraEulerX;
    private Vector3 CameraPosition;

    // Use this for initialization
    void Start () {
        players.Add(focusLevel.gameObject);
	}
	
	// Update is called once per frame
	void LateUpdate () {  //update after everything drawn to screen
        CalculateCameraLocations();
        MoveCamera();
	}

    private void MoveCamera()
    {
        Vector3 position = gameObject.transform.position;
        if(position != CameraPosition)
        {
            Vector3 targetPosition = Vector3.zero;
            targetPosition.x = Mathf.MoveTowards(position.x, CameraPosition.x, PositionUpdateSpeed * Time.deltaTime);
            targetPosition.y = Mathf.MoveTowards(position.y, CameraPosition.y, PositionUpdateSpeed * Time.deltaTime);
            targetPosition.z = Mathf.MoveTowards(position.z, CameraPosition.z, DepthUpdateSpeed * Time.deltaTime);
            gameObject.transform.position = targetPosition;
        }

        Vector3 localEulerAngles = gameObject.transform.localEulerAngles;
        if(localEulerAngles.x != CameraEulerX)
        {
            Vector3 targetEulerAngles = new Vector3(CameraEulerX, localEulerAngles.y, localEulerAngles.z);
            gameObject.transform.localEulerAngles = Vector3.MoveTowards(localEulerAngles, targetEulerAngles, AngleUpdateSpeed * Time.deltaTime);
        }
    }

    private void CalculateCameraLocations()
    {
        Vector3 averageCenter = Vector3.zero;
        Vector3 totalPositions = Vector3.zero;
        Bounds playerBounds = new Bounds();

        //cycle through every player
        for (int i =0; i<players.Count; i++)
        {
            Vector3 playerPosition = players[i].transform.position;
            if (!focusLevel.FocusBounds.Contains(playerPosition))
            {
                float PlayerX = Mathf.Clamp(playerPosition.x, focusLevel.FocusBounds.min.x, focusLevel.FocusBounds.max.x);
                float PlayerY = Mathf.Clamp(playerPosition.y, focusLevel.FocusBounds.min.y, focusLevel.FocusBounds.max.y);
                playerPosition = new Vector3(PlayerX, PlayerY);
            }

            totalPositions += playerPosition;
            playerBounds.Encapsulate(playerPosition);
        }
        averageCenter = (totalPositions / players.Count);

        float extents = (playerBounds.extents.x + playerBounds.extents.y);
        float lerpPercent = Mathf.InverseLerp(0, (focusLevel.HalfXBounds + focusLevel.HalfYBounds)/2, extents);

        float depth = Mathf.Lerp(DepthMax, DepthMin, lerpPercent);
        float angle = Mathf.Lerp(AngleMax, AngleMin, lerpPercent);

        CameraEulerX = angle;
        CameraPosition = new Vector3(averageCenter.x, averageCenter.y, depth);
    }
}
