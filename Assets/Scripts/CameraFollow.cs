using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform character;
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;
    public Vector3 minValue, maxValue;

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
            Mathf.Clamp(characterPosition.z, minValue.z, maxValue.z));

        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}
