using UnityEngine;

public class CameraMovementHospital : MonoBehaviour
{
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public float yawMin = 25;
    public float yawMax = 165;
    public float pitchMin = -80;
    public float pitchMax = 10;

    void Update () {
        
        
        yaw += speedH * Input.GetAxis("Mouse X");

        yaw = yaw < yawMin ? yawMin : yaw;
        yaw = yaw > yawMax ? yawMax : yaw;
        pitch = pitch < pitchMin ? pitchMin : pitch;
        pitch = pitch > pitchMax ? pitchMax : pitch;


        pitch -= speedV * Input.GetAxis("Mouse Y");
        
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);


        Cursor.visible = false;
        Screen.lockCursor = true;

    }
}
