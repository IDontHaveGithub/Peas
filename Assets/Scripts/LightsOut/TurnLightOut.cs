using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLightOut : MonoBehaviour
{
    LightsOut lightsOut;
    AudioSource note;

    void Start()
    {
        note = GetComponent<AudioSource>();
        lightsOut = GetComponentInParent<LightsOut>();
    }
    
    void OnMouseDown()
    {
        lightsOut.currentCell = int.Parse(gameObject.name);
        note.Play();
        lightsOut.LightsOff();
    }


}
