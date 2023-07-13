using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assistant : Enemy_parameter
{
    [Header("Physics")]
    [SerializeField] private Transform[] escapePoints;

    void Update()
    {
        HandleEnemyState();
    }

    private void FixedUpdate()
    {
        switch(state)
        {
            case EnemyState.Idle:
                rigidBody.velocity = Vector2.zero;
                break;
            case EnemyState.Rotating:
                rigidBody.velocity = Vector2.zero;
                Rotate();
                break;
            case EnemyState.Attacking:
                newAngle = (int)(Mathf.Atan2(transform.localPosition.x - nextEdgePos.x, nextEdgePos.y - transform.localPosition.y) * Mathf.Rad2Deg);
                if ((int)(Mathf.Abs(Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z).z) * 1000) !=
                    (int)(Mathf.Abs(Quaternion.Euler(0, 0, newAngle).z) * 1000))
                {
                    transform.rotation = Quaternion.Euler(0, 0, newAngle);
                }
                else
                {
                    Move(runningSpeed, nextEdgePos, transform.localPosition);
                    CheckPosition();
                }
                Move(runningSpeed, nextEdgePos, transform.localPosition);
                CheckPosition();
                break;
        }
    }

    protected override void Escape()
    {
        float maxDistance = 0;
        foreach (var point in escapePoints)
        {
            var distance = Vector2.Distance(point.position, Player_Control.GetPosition());

            if(Mathf.Sign(transform.position.x - Player_Control.GetPosition().x) == Mathf.Sign(point.position.x - Player_Control.GetPosition().x) 
                && Mathf.Sign(transform.position.y - Player_Control.GetPosition().y) == Mathf.Sign(point.position.y - Player_Control.GetPosition().y) 
                && distance >= maxDistance)
            {
                maxDistance = distance;
                nextEdgePos = point.localPosition;
            }
        }
    }

    private void CheckPosition()
    {
        if(transform.localPosition.x > nextEdgePos.x - 0.1f && transform.localPosition.x < nextEdgePos.x + 0.1f 
            && transform.localPosition.y > nextEdgePos.y - 0.1f && transform.localPosition.y < nextEdgePos.y + 0.1f)
        {
            isPlayerDetected = false;
            ChangeState(EnemyState.Idle, 0);
        }
    }
}
