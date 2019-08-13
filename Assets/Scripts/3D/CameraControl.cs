using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float distance;

    public GameObject player;

    private Vector3 offset;
    private Vector3 playerPrevPos, playerMoveDir;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;

        distance = offset.magnitude-2;
        playerPrevPos = player.transform.position;
    }

    void LateUpdate()
    {
        //if "static" you freeze the position and turn around player
        //player still moves tho, so it's pretty fucky
        if (Input.GetButton("Static"))
        {
            transform.RotateAround(player.transform.position, Vector3.up, Input.GetAxis("Mouse X"));
        }
        else
        {
            CameraFollow();
        }
    }

    //get position of target and follow
    void CameraFollow()
    {
        playerMoveDir = player.transform.position - playerPrevPos;

        var currentPos = transform.position;
        var targetPos = player.transform.position - player.transform.forward * distance;

        transform.position = Vector3.Slerp(currentPos, targetPos, 0.2f);
        transform.position = new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z);

        transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y+1.5f, player.transform.position.z));

        //if stand still use last positions.
        if (playerMoveDir != Vector3.zero)
        {
            playerMoveDir.Normalize();

            playerPrevPos = player.transform.position;
        }
    }
}
