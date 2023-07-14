using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

public class Hook_logic : MonoBehaviour
{
    public Camera mainCamera;
    public LineRenderer tongue;
    public DistanceJoint2D distance { get; private set; }
    public CircleCollider2D hookCollider;
    public GameObject player;

    private float liveTimeHook = 0.5f;
    private bool isActive;
    
    void Start()
    {
        distance = player.GetComponent<DistanceJoint2D>();
        distance.enabled = false;
    }

    
    public void Update()
    {

        if (Input.GetMouseButtonDown(0) && isActive)
        {
            distance.connectedAnchor = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            hookCollider.transform.position = distance.connectedAnchor;
            tongue.SetPosition(0, distance.connectedAnchor);
            tongue.SetPosition(1, transform.position);
            distance.enabled = true;
            tongue.enabled = true;
            isActive = false;
        }
        if (tongue.positionCount == 2)
        {
            liveTimeHook -= Time.deltaTime;
            if (liveTimeHook <= 0)
            {
                distance.enabled = false;
                tongue.enabled = false;
                liveTimeHook = 0.5f;
                hookCollider.transform.position = player.transform.position;
            }
        }

        if (distance.enabled)
        {
            tongue.SetPosition(1, transform.position);
        }
    }

    public void ActiveHook()
    {
        isActive = true;
        Physics2D.IgnoreLayerCollision(8, 9, false);
    }
}
