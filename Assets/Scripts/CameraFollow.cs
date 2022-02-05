using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    public Transform character;
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;
    Vector3 minValue, maxValue;

    [SerializeField] Tilemap level;

    private void Start()
    {
        Bounds bound = level.localBounds;
        minValue = bound.min;
        maxValue = bound.max;
        Debug.Log(minValue.y);
        Debug.Log(maxValue.y);

        minValue.x += Camera.main.orthographicSize * Screen.width / Screen.height;
        minValue.y += Camera.main.orthographicSize;

        maxValue.x -= Camera.main.orthographicSize * Screen.width / Screen.height;
        maxValue.y -= Camera.main.orthographicSize;
    }

    private void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        Vector3 characterPosition = character.position + offset;

        //Keeping the camera within the level bounds
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(characterPosition.x, minValue.x, maxValue.x),
            Mathf.Clamp(characterPosition.y, minValue.y, maxValue.y),
            transform.position.z);

        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}
