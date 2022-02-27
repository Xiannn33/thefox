using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public Vector2 minPosition;
    public Vector2 maxPosition;

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos=player.position;
        targetPos.x = Mathf.Clamp(player.position.x, minPosition.x, maxPosition.x);
        targetPos.y = Mathf.Clamp(player.position.y, minPosition.y, maxPosition.y);
        transform.position = new Vector3(targetPos.x, targetPos.y, -10f);

    }
}
