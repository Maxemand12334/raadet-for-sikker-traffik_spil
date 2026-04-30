using UnityEngine;
using UnityEngine.InputSystem;


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

    public SpoonGameManager spoonManager;

    public GameObject activeInteractable;

    void Update () {
        
        yaw += speedH * Input.GetAxis("Mouse X");

        yaw = yaw < yawMin ? yawMin : yaw;
        yaw = yaw > yawMax ? yawMax : yaw;
        pitch = pitch < pitchMin ? pitchMin : pitch;
        pitch = pitch > pitchMax ? pitchMax : pitch;


        pitch -= speedV * Input.GetAxis("Mouse Y");
        
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);


        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if(activeInteractable != null && Mouse.current["leftButton"].IsPressed() && spoonManager.getGameState() != SpoonGameManager.GameStates.active)
        {
            Debug.Log("pressed");
            spoonManager.SetGameStateActive();
        }
    }
}
