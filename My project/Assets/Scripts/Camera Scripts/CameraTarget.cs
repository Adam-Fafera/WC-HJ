using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform player;
    [SerializeField] float threshold;

    void Update()
    {
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPosition = (player.position + mousePosition) / 2F;

        //constraining the amount of camera movement
        targetPosition.x = Mathf.Clamp(targetPosition.x, player.position.x - threshold, threshold + player.position.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -threshold + player.position.y, threshold + player.position.y);

        this.transform.position = targetPosition;
    }
}
