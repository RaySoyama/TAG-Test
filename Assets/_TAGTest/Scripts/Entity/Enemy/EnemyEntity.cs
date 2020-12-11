using UnityEngine;

public class EnemyEntity : Entity
{
    public enum EnemyState
    {
        idle = 0,
        chase = 1,
        attack = 2,
    }

    [Header("AI Stats")]
    [SerializeField]
    protected bool useAI = true;

    [SerializeField]
    protected EnemyState currentState = EnemyState.idle;

    [SerializeField, Min(0.0f)]
    protected float aggroRange = 10.0f;

    [SerializeField, Min(0.0f)]
    protected float attackRange = 7.0f;

    [SerializeField, ReadOnlyField]
    protected GameObject attackTarget = null;

    [SerializeField]
    private float stunLockTime = 1.0f;

    [Header("References")]
    [SerializeField]
    private Rigidbody rb = null;
    [SerializeField]
    private UnityEngine.AI.NavMeshAgent navMeshAgent = null;
    private UnityEngine.AI.NavMeshPath navMeshPath;

    [SerializeField]
    private GameObject bulletPrefab = null;

    [SerializeField, ReadOnlyField]
    private float timeSinceLastBullet = -100;

    void Start()
    {
        navMeshPath = new UnityEngine.AI.NavMeshPath();
    }

    void Update()
    {
        AILoop();

    }


    protected void AILoop()
    {
        if (useAI == false)
        {
            return;
        }

        switch (currentState)
        {
            case EnemyState.idle:
                OnIdle();
                break;
            case EnemyState.chase:
                OnChase();
                break;
            case EnemyState.attack:
                OnAttack();
                break;
        }
    }

    protected virtual void OnEnterIdle()
    {
        //Play Idle Anim
    }
    protected virtual void OnIdle()
    {
        //See if player is in aggro range

        Collider[] overlapQuery = Physics.OverlapSphere(transform.position, aggroRange); //We can also check every x frames, instead of every frame for optimization

        //check for player
        foreach (Collider obj in overlapQuery)
        {
            if (obj.CompareTag("Player")) //currently using a tag based check. Is flexible
            {
                attackTarget = obj.gameObject;
                ChangeState(EnemyState.chase);
            }
        }

        //we can add things like "if player hits enemy, instant aggro here"
    }
    protected virtual void OnExitIdle()
    {
        //We can do something like change colors here
    }

    protected virtual void OnEnterChase()
    {

    }
    protected virtual void OnChase()
    {
        if (attackTarget == null)
        {
            //it forgot who it was chasing??
            ChangeState(EnemyState.idle);
        }

        LookAt(attackTarget.transform.position - transform.position);

        if (Vector3.Distance(transform.position, attackTarget.transform.position) < attackRange) //see if player is in attack range
        {
            ChangeState(EnemyState.attack);
        }
        else //chase
        {
            if (navMeshAgent == null)
            {
                Debug.LogError($"MISSING REF TO NAVMESH AGENT: {gameObject.name}");
                return;
            }

            navMeshAgent.CalculatePath(attackTarget.transform.position, navMeshPath);

            if (navMeshPath.corners.Length >= 2)
            {
                Vector3 direction = navMeshPath.corners[1] - transform.position;
                Move(direction.normalized);
            }
        }
    }
    protected virtual void OnExitChase()
    {

    }


    protected virtual void OnEnterAttack()
    {

    }
    protected virtual void OnAttack()
    {
        if (attackTarget == null)
        {
            //it forgot who it was chasing??
            ChangeState(EnemyState.idle);
        }

        LookAt(attackTarget.transform.position - transform.position);

        if (Vector3.Distance(transform.position, attackTarget.transform.position) > attackRange) //see if player is in attack range
        {
            ChangeState(EnemyState.chase);
        }
        else //attack
        {
            Attack();
        }
    }
    protected virtual void OnExitAttack()
    {

    }

    protected void ChangeState(EnemyState targetState)
    {
        switch (currentState) //Exit
        {
            case EnemyState.idle:
                OnExitIdle();
                break;
            case EnemyState.chase:
                OnExitChase();
                break;
            case EnemyState.attack:
                OnExitAttack();
                break;
        }

        switch (targetState) //Enter
        {
            case EnemyState.idle:
                OnEnterIdle();
                break;
            case EnemyState.chase:
                OnEnterChase();
                break;
            case EnemyState.attack:
                OnEnterAttack();
                break;
        }

        currentState = targetState;
    }


    public override void Move(Vector3 direction)
    {
        if (canMove == false)
        {
            return;
        }

        if (rb == null)
        {
            Debug.LogError("Missing Ref to RigidBody on Player");
            return;
        }

        //Since I don't really want to do the mental math of converting world space input into a isometric transform, I'm just gonna rotate the environment. (RIP Artists) 
        //FULL DISCALIMER: If I was working at a company I would do this in a heart beat, but I have like 3 other programming tests that are all due this week and I need to get this done.

        rb.AddForce(direction * speed * Time.deltaTime);
    }

    public override void LookAt(Vector3 direction)
    {
        //Things do get funky when you go leave worldPos.y = 0
        direction = new Vector3(direction.x, transform.position.y, direction.z);

        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    public override void Attack()
    {
        if (canAttack == false)
        {
            return;
        }

        if (bulletPrefab == null)
        {
            Debug.LogError($"MISSING REF TO BULLET PREFAB ON: {gameObject.name}");
            return;
        }

        if (Time.time - timeSinceLastBullet > attackSpeed)
        {
            //spawn bullet
            timeSinceLastBullet = Time.time;

            //I could object pool this but it should be fine
            //I also hard code in the Vec3.up*1.5f because i'm too lazy atm to create a spawn point for every enemy.
            Instantiate(bulletPrefab, transform.position + Vector3.up * 1.5f, transform.rotation);
        }
    }

    protected override void OnTakeDamage()
    {
        base.OnTakeDamage();
        //I stun lock the enemy shooting so I dont take damage when I hit them
        timeSinceLastBullet = Time.time + stunLockTime;
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        //can play animations or effects here
        Destroy(gameObject);

        TelemetryManager.Instance.enemiesKilled++;
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, aggroRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        if (navMeshPath != null)
        {
            for (int i = 0; i < navMeshPath.corners.Length - 1; i++)
            {
                Gizmos.DrawLine(navMeshPath.corners[i], navMeshPath.corners[i + 1]);
            }
        }

    }
}
