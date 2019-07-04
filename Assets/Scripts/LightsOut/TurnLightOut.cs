using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLightOut : MonoBehaviour
{
    //reference tot other script and the sound
    LightsOut lightsOut;
    AudioSource note;

    void Start()
    {
        note = GetComponent<AudioSource>();
        lightsOut = GetComponentInParent<LightsOut>();
    }
    
    // ifClicked
    void OnMouseDown()
    {
        lightsOut.currentCell = int.Parse(gameObject.name);
        note.Play();
        lightsOut.LightsOff();
    }


}
