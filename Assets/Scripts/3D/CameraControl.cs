using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    float distance;
    Vector3 playerPrevPos, playerMoveDir;

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
        if (Input.GetButton("Static"))
        {
           // Debug.Log("WAAAAAAAAA");
            transform.RotateAround(player.transform.position, Vector3.up, Input.GetAxis("Mouse X"));
        }
        else
        {
            CameraFollow();
        }
    }

    void CameraFollow()
    {
        playerMoveDir = player.transform.position - playerPrevPos;

        var currentPos = transform.position;
        var targetPos = player.transform.position - player.transform.forward * distance;
        transform.position = Vector3.Slerp(currentPos, targetPos, 0.2f);

        //var tPos = transform.position;
        //tPos.y += 1f; // required height
        transform.position = new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z);

        transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y+1.5f, player.transform.position.z));

        if (playerMoveDir != Vector3.zero)
        {
            playerMoveDir.Normalize();

            playerPrevPos = player.transform.position;
        }
    }
}
