using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public bool touchingJoystick;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        touchingJoystick = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        touchingJoystick = false;
    }
}
