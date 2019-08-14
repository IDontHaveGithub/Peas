using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonB : MonoBehaviour
{
    public int id;

    public Animator anim;
    public AudioSource ding;

    SimonController simonController;

    //initialize all things
    void Start()
    {
        simonController = GetComponentInParent<SimonController>();
        ding = GetComponent<AudioSource>();
        id = transform.GetSiblingIndex();
        anim.enabled = false;
    }

    //click button to add it to the list
    void OnMouseDown()
    {
        if (!simonController.simonIsSaying)
        {
            Action();
            simonController.Instance.PlayerAction(this);
        }
    }

    //show that block is hit by lighting it up
    public void Action()
    {
        anim.enabled = true;
        anim.SetTrigger("pop");
        ding.Play();
    }
}
