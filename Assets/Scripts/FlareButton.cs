using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FlareButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public bool flaringOut;


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
        flaringOut = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        flaringOut = false;
    }
}
