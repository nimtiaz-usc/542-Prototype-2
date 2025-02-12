using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSway : MonoBehaviour
{
    //public Transform cameraPosition;

    public float amplitudeX = 0.1f;
    public float periodX = 1f;

    public float amplitudeY = 0.15f;
    public float periodY = 1.5f;

    void Update() {

        float thetaX = Time.timeSinceLevelLoad / periodX;
        float distanceX = amplitudeX * Mathf.Sin(thetaX);

        float thetaY = Time.timeSinceLevelLoad / periodY;
        float distanceY = amplitudeY * Mathf.Sin(thetaY);

        transform.localPosition = transform.localPosition + (Vector3.right * distanceX) + (Vector3.up * distanceY);
    }


}
