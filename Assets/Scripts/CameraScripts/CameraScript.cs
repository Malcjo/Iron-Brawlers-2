using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public FocusLevel focusLevel;
    public List<GameObject> Players;

    public float depthUpdateSpeed = 5f;
    public float angleUpdateSpeed = 7f;
    public float positionUpdateSpeed = 5f;

    public float depthMax = -10f;
    public float depthMin = -22f;

    public float angleMax = 11f;
    public float angleMin = 3f;

    private float cameraEulerX;
    private Vector3 cameraPosition;


    void Start()
    {
        Players.Add(focusLevel.gameObject);
        GameManager.instance.SetCameraScript(this);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CalculateCameraLocations();
        MoveCamera();
    }

    public void AddPlayers(GameObject player)
    {
        Players.Add(player);
    }


    void MoveCamera()
    {
        Vector3 position = gameObject.transform.position;
        if (position != cameraPosition)
        {
            Vector3 targetPosition = Vector3.zero;
            targetPosition.x = Mathf.MoveTowards(position.x,cameraPosition.x, positionUpdateSpeed * Time.deltaTime);
            targetPosition.y = Mathf.MoveTowards(position.y, cameraPosition.y, positionUpdateSpeed * Time.deltaTime);
            targetPosition.z = Mathf.MoveTowards(position.z, cameraPosition.z, depthUpdateSpeed * Time.deltaTime);
            gameObject.transform.position = targetPosition;
        }
        Vector3 localEulerAngles = gameObject.transform.localEulerAngles;
        if(localEulerAngles.x != cameraEulerX)
        {
            Vector3 targetEulerAngles = new Vector3(cameraEulerX, localEulerAngles.y, localEulerAngles.z);
            gameObject.transform.localEulerAngles = Vector3.MoveTowards(localEulerAngles, targetEulerAngles, angleUpdateSpeed * Time.deltaTime);
        }
    }

    void CalculateCameraLocations()
    {
        Vector3 averageCenter = Vector3.zero;
        Vector3 totalPosition = Vector3.zero;
        Bounds playerBounds = new Bounds();
        
        for (int i = 0; i < Players.Count; i++)
        {
            if(Players[i] == null)
            {
                return;
            }
            Vector3 playerPosition = Players[i].transform.position;

            if (!focusLevel.focusBounds.Contains(playerPosition))
            {
                float playerX = Mathf.Clamp(playerPosition.x, focusLevel.focusBounds.min.x, focusLevel.focusBounds.max.x);
                float playerY = Mathf.Clamp(playerPosition.y, focusLevel.focusBounds.min.y, focusLevel.focusBounds.max.y);
                float playerZ = Mathf.Clamp(playerPosition.z, focusLevel.focusBounds.min.z, focusLevel.focusBounds.max.z);
                playerPosition = new Vector3(playerX, playerY, playerZ);
            }
            totalPosition += playerPosition;
            playerBounds.Encapsulate(playerPosition);
        }
        averageCenter = (totalPosition / Players.Count);

        float extents = (playerBounds.extents.x + playerBounds.extents.y);
        float lerpPercent = Mathf.InverseLerp(0, (focusLevel.halfXBounds + focusLevel.halfYBounds) / 2, extents);

        float depth = Mathf.Lerp(depthMax, depthMin, lerpPercent);
        float angle = Mathf.Lerp(angleMax, angleMin, lerpPercent);

        cameraEulerX = angle;
        cameraPosition = new Vector3(averageCenter.x, averageCenter.y, depth);
    }
}
