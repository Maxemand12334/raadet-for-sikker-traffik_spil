
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class selectable : MonoBehaviour
{

    public Camera cam;
    public float angleSelect = 50f;
    
    bool isHovering = false;
    public float hoverAmount = 0.05f;
    public float hoverTime = 2f;

    private Vector3 targetPosition;
    private Vector3 originalPos;
    bool active = false;
    public float newLocationSpeed = 5;

    void Start()
    {
        originalPos = this.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if(!active)
        InactiveBehaviour();
        else
        {
            if(Vector3.Magnitude(this.transform.position - targetPosition) > 0.1 && Vector3.Magnitude(this.transform.position - targetPosition) != 0)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * newLocationSpeed);
            } else
            {
                this.transform.position = targetPosition;
            }
        }
        

    }

    void InactiveBehaviour()
    {
             if (CanInteract() && !isHovering)
        {
            targetPosition = transform.position + new Vector3(0,hoverAmount,0);
            isHovering = true;
            cam.GetComponent<CameraMovementHospital>().activeInteractable = this.gameObject;
        }
        
        if (CanInteract() && isHovering)
        {
            LerpHover(targetPosition);
            
        }else if(!CanInteract() && isHovering)
        {
            targetPosition = originalPos;
            LerpHover(targetPosition);
            
            if (Vector3.Magnitude(this.transform.position - targetPosition) < 0.01 ) 
            {
            this.transform.position = targetPosition;
            isHovering = false;
            }
            cam.GetComponent<CameraMovementHospital>().activeInteractable = null;
        }
    }

    bool CanInteract()
    {
        Vector3 camToSelectableVector = this.transform.position-cam.transform.position;
        
        float angle = Mathf.Abs(Vector3.Angle(camToSelectableVector, cam.transform.forward));

        return angle <= angleSelect;
    }

    void LerpHover(Vector3 target)
    {
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * hoverTime);
    }

    public void Activate(Vector3 newPos)
    {
        targetPosition = newPos;
        active = true;
    }

        public void Deactivate()
    {
        targetPosition = originalPos;
        active = false;
    }

}
