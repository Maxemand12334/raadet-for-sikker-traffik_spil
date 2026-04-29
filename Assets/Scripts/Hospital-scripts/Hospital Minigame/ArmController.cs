
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArmController : MonoBehaviour
{
    private Vector3 speedVector = new Vector3(0,0,0);
    private Vector3 targetPos;
    public Camera cam;
    public float targetPosSpeed = 3;
    public float movementSpeed = 3;

    public float maxLength = 40;

    void Start()
    {
        targetPos = this.transform.position;
    }

    void Update()
    {
        move(Key.A, -cam.transform.right);
        move(Key.D, cam.transform.right);
        rot(Key.W, cam.transform.forward);
        rot(Key.S, -cam.transform.forward);
        
        this.transform.RotateAround(this.transformHandle.position, cam.transform.forward,);

        this.transform.position = Vector3.Lerp(this.transform.position,targetPos, Time.deltaTime * movementSpeed);

      //  if (maxLength > targetPos - cam.transform.position)
        {
            
        }
    }

    void move(Key k, Vector3 dir)
    {
        if (Keyboard.current[k].IsPressed())
        targetPos += dir * Time.deltaTime * targetPosSpeed;
    }

    void rot(Key K, Vector3 axis, float angle)
    {
        
    }
}
