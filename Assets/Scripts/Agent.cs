using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : Enemy_parameter
{
    [Header("Physics")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float walkingSpeed, runningSpeed;
    [SerializeField] Transform upPos, downPos;
    private Vector3 nextEdgePos;
    private Vector3 direction;
    private int rotationAngle, resultAngle;
    private float newAngle;
    private Rigidbody2D rigidBody;

    [Header("States")]
    [SerializeField] private float idleTime;
    [SerializeField] private float walkingTime, rotatingTime;
    private float stateChangeTimer;
    private int rotationDirection;
    private EnemyState state;

    [Header("Attack")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private float attackModeTime, timeBetweenShots;
    private float timerBetweenShots;
    private bool isPlayerDetected;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        timerBetweenShots = timeBetweenShots;
        state = EnemyState.Idle;
        stateChangeTimer = idleTime;
        nextEdgePos = upPos.localPosition;
    }

    void Update()
    {
        HandleEnemyState();

        HandleShooting();
    }

    private void FixedUpdate()
    {
        switch(state)
        {
            case EnemyState.Idle:
                rigidBody.velocity = Vector2.zero;
                break;
            case EnemyState.Walking:
                newAngle = (int)(Mathf.Atan2(nextEdgePos.x - transform.localPosition.x, nextEdgePos.y - transform.localPosition.y) * Mathf.Rad2Deg);
                if ((int)(Mathf.Abs(Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z).z) * 1000) != 
                    (int)(Mathf.Abs(Quaternion.Euler(0, 0, newAngle).z) * 1000))
                {
                    rigidBody.velocity = Vector2.zero;
                    Rotate();
                }
                else
                {
                    if (stateChangeTimer > walkingTime)
                        stateChangeTimer = walkingTime;
                    Move(walkingSpeed, nextEdgePos, transform.localPosition);
                    ChangeDirection();
                }
                break;
            case EnemyState.Rotating:
                rigidBody.velocity = Vector2.zero;
                Rotate();
                break;
            case EnemyState.Attacking:
                Move(runningSpeed, PlayerControl.GetPosition(), transform.position);
                RotateAfterMovementDirection();
                break;
        }
    }

    private void Move(float speed, Vector3 targetPos, Vector3 enemyPos)
    {
        direction = Vector3.Normalize(targetPos - enemyPos);
        rigidBody.AddForce(speed * direction, ForceMode2D.Force);
        rigidBody.velocity = direction * speed;
    }

    private void RotateAfterMovementDirection()
    {
        var angle = Mathf.Atan2(-rigidBody.velocity.x, rigidBody.velocity.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void ChangeDirection()
    {
        if (transform.localPosition.x < upPos.localPosition.x && upPos.localPosition.x <= 0 
            || transform.localPosition.x > upPos.localPosition.x && upPos.localPosition.x > 0
            || transform.localPosition.y > upPos.localPosition.y)
        {
            nextEdgePos = downPos.localPosition;
            stateChangeTimer = 0;
        }
        if (transform.localPosition.x > downPos.localPosition.x && downPos.localPosition.x >= 0 
            || transform.localPosition.x < downPos.localPosition.x && downPos.localPosition.x < 0 
            || transform.localPosition.y < downPos.localPosition.y)
        {
            nextEdgePos = upPos.localPosition;
            stateChangeTimer = 0;
        }
    }

    private void Rotate()
    {
        //transform.Rotate(0, 0, rotationAngle * Time.deltaTime * 0.5f * rotationDirection);
        transform.rotation = Quaternion.RotateTowards(Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z), 
            Quaternion.Euler(0, 0, newAngle), 1f);
        if (Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z) == Quaternion.Euler(0, 0, newAngle) && state != EnemyState.Walking)
            stateChangeTimer = 0;
        /*transform.rotation = Quaternion.Euler(0, 0, Mathf.Ceil(rotationDirection + transform.rotation.eulerAngles.z));
        if (isAngleReached())
        {
            stateChangeTimer = 0;
            //print('s');
        }*/
        //print($"{transform.rotation.eulerAngles.z} {positiveEnemyAngle} {resultAngle}");
    }

    private bool isAngleReached()
    {
        var positiveEnemyAngle = transform.rotation.eulerAngles.z < 0 ? 360f + transform.rotation.eulerAngles.z : transform.rotation.eulerAngles.z;
        return positiveEnemyAngle >= resultAngle - 0.5f && positiveEnemyAngle <= resultAngle + 0.5f;
    }

    private void HandleEnemyState()
    {
        if (!isPlayerDetected && isAlive && stateChangeTimer <= 0)
        {
            rotationDirection = Random.Range(0, 2) == 0 ? -1 : 1;
            ChangeState((EnemyState)Random.Range(0, 3), Random.Range(22, 135), Random.Range(0, 2) == 0 ? -1 : 1);
            print(state);
        }
        else
            stateChangeTimer -= Time.deltaTime;

        if (isPlayerDetected)
            state = EnemyState.Attacking;
        if (!isAlive)
            state = EnemyState.Idle;
    }

    private void ChangeState(EnemyState nextState, int nextAngle, int nextDirection)
    {
        state = nextState;
        rotationAngle = nextAngle;
        rotationDirection = nextDirection;
        newAngle = transform.rotation.eulerAngles.z + rotationAngle;
        var positiveEnemyAngle = (int)Mathf.Ceil(transform.rotation.eulerAngles.z < 0 ? 360f + transform.rotation.eulerAngles.z : transform.rotation.eulerAngles.z);
        resultAngle = positiveEnemyAngle + rotationAngle - 360 * ((positiveEnemyAngle + rotationAngle) / 360);
        stateChangeTimer = GetStateTime();
    }

    private float GetStateTime()
    {
        if (state == EnemyState.Idle)
            return idleTime;
        else if (state == EnemyState.Walking)
            return walkingTime;
        return 10f;
    }

    private void HandleShooting()
    {
        if (state == EnemyState.Attacking)
        {
            if (timerBetweenShots <= 0)
            {
                timerBetweenShots = timeBetweenShots;
                Shoot();
            }
            else
                timerBetweenShots -= Time.deltaTime;

            isPlayerDetected = !PlayerControl.isDead;
        }
    }

    private void Shoot()
    {
        //We could create a pool of bullet objects,
        //but it would be pretty hard and long, so I use Instantiate method
        var newBullet = Instantiate(bullet, transform.position, transform.rotation).GetComponent<Bullet>();
        newBullet.Shoot(direction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            isPlayerDetected = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(upPos.position, downPos.position);
    }
}
