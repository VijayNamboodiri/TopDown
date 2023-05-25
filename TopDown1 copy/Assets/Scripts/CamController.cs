using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CamController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothing;
    
    public Transform mouseHelper;
    public Transform camHelper;
    Vector3 mHelper;
    Vector3 cHelper;
    public float percentage = 0.25f;
    // Update is called once per frame
    private void Update()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        

        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if(Physics.Raycast(ray,out RaycastHit hitdata,100))
        {
            //when we hit with our ray
            mHelper = hitdata.point;
            mHelper.y = target.position.y;
        }

        mouseHelper.position = mHelper;
        cHelper = Vector3.Lerp(target.position,mHelper,percentage);
        camHelper.position = cHelper;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = cHelper + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothing * Time.deltaTime);
        transform.position = smoothedPosition;

      
        
    }
}
