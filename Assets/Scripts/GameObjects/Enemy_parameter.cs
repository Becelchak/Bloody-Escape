using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Assistant,
    Agent,
    Security,
    Commando
}

public class Enemy_parameter : MonoBehaviour
{
    [SerializeField] private EnemyType type;
    public SpriteRenderer sprite { get; private set; }

    [Header("Physics")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] protected float walkingSpeed, runningSpeed;
    [SerializeField] Transform upPos, downPos;
    protected Vector3 nextEdgePos;
    protected Vector3 direction;
    private int rotationAngle;
    protected float newAngle;
    protected Rigidbody2D rigidBody;
    [SerializeField] private Room room;

    [Header("States")]
    [SerializeField] private float idleTime;
    [SerializeField] protected float walkingTime, rotatingTime;
    protected float stateChangeTimer;
    protected EnemyState state;

    [Header("Attack")]
    [SerializeField] protected float attackModeTime;
    [SerializeField] protected float timeBetweenShots;
    protected float timerBetweenShots, attackModeTimer;
    protected bool isPlayerDetected;
    protected bool isAlive = true;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        timerBetweenShots = type == EnemyType.Commando ? 0 : timeBetweenShots;
        state = EnemyState.Idle;
        stateChangeTimer = idleTime;
        attackModeTimer = attackModeTime;
        nextEdgePos = upPos.localPosition;
        transform.localPosition = downPos.localPosition;
    }
    
    public bool EnemyAlive()
    {
        return isAlive;
    }

    public bool CanBeDevoured()
    {
        return isAlive && (type != EnemyType.Security || type == EnemyType.Security && Player_Control.GetBiomassSize() > 0.5f);
    }

    public void ChangeLiveStatus()
    {
        isAlive = !isAlive;
        if (!isAlive)
            Clear();
    }

    public void Clear()
    {
        room.ClearFromEnemy();
    }

    protected void MoveByState()
    {
        switch (state)
        {
            case EnemyState.Idle:
                rigidBody.velocity = Vector2.zero;
                break;
            case EnemyState.Walking:
                newAngle = (int)(Mathf.Atan2(transform.localPosition.x - nextEdgePos.x, nextEdgePos.y - transform.localPosition.y) * Mathf.Rad2Deg);
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
                if (!Player_Control.isInvisible)
                {
                    isPlayerDetected = !Player_Control.isDead;
                    Move(runningSpeed, Player_Control.GetPosition(), transform.position);
                    RotateAfterMovementDirection();
                }
                else
                {
                    isPlayerDetected = false;
                    rigidBody.velocity = Vector2.zero;
                    if (attackModeTimer <= 0)
                    {
                        ChangeState(EnemyState.Walking, 0);
                        attackModeTimer = attackModeTime;
                    }
                    else
                        attackModeTimer -= Time.deltaTime;
                }
                break;
        }
    }

    protected void Move(float speed, Vector3 targetPos, Vector3 enemyPos)
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

    protected void Rotate()
    {
        transform.rotation = Quaternion.RotateTowards(Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z),
            Quaternion.Euler(0, 0, newAngle), 1f);
        if (Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z) == Quaternion.Euler(0, 0, newAngle) && state != EnemyState.Walking)
            stateChangeTimer = 0;
    }

    protected void HandleEnemyState()
    {
        if (!isPlayerDetected && isAlive && stateChangeTimer <= 0)
        {
            ChangeState((EnemyState)Random.Range(0, 3), Random.Range(22, 136));
        }
        else
            stateChangeTimer -= Time.deltaTime;

        if (isPlayerDetected)
            state = EnemyState.Attacking;
        if (!isAlive)
            state = EnemyState.Idle;
    }

    protected void ChangeState(EnemyState nextState, int nextAngle)
    {
        state = nextState;
        rotationAngle = nextAngle;
        newAngle = transform.rotation.eulerAngles.z + rotationAngle;
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

    protected void HandleAttack()
    {
        if (state == EnemyState.Attacking && !Player_Control.isInvisible)
        {
            if (timerBetweenShots <= 0)
            {
                timerBetweenShots = timeBetweenShots;
                Attack();
            }
            else
                timerBetweenShots -= Time.deltaTime;

            isPlayerDetected = !Player_Control.isDead;
        }
    }

    protected virtual void Attack() {}

    protected virtual void Escape() {}

    protected virtual void Respawn() {}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !Player_Control.isInvisible)
        {
            isPlayerDetected = true;
            if (type == EnemyType.Assistant)
                Escape();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(upPos.position, downPos.position);
    }
}
