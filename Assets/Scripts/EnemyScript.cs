using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public int HP = 100;
    public int DamageIDeal = 10;
    public float timeBetweenAttacks = 1.5f;
    public float DistanceThatEnemyCanAttack = 8f;
    public float speed = 3.5f;
    public float retreatDistance = 3f;
    public float retreatStopDistance = 5f;
    public float maxRetreatTime = 2f;
    public float retreatCooldown = 1.5f;
    public float extraRotationSpeed = 360f;
    public float detectionRange = 20f;
    public float viewAngle = 180f;
    public float timeToDetectPlayer = 2f;
    public float losePlayerTime = 3f;
    public float AmmountOfKnockBack = 5f;
    public float knockbackDuration = 0.15f;
    public Rigidbody RB;
    public NavMeshAgent _agent;

    private Transform playerTransform;
    private playerscript playerScript;

    private float detectionTimer;
    private float attackTimer;
    private float lostPlayerTimer;
    private float retreatTimer;
    private float retreatCooldownTimer;

    private bool hasDetectedPlayer;
    private bool hasSeenPlayerBefore;
    private bool isRetreating;
    private bool isSearching;
    private bool isKnockedBack;

    private Vector3 lastKnownPlayerPos;

    private void Start()
    {
        GameObject player = GameObject.Find("Player");

        if (player == null)
        {
            enabled = false;
            return;
        }

        playerTransform = player.transform;
        playerScript = player.GetComponent<playerscript>();

        detectionTimer = timeToDetectPlayer;
        attackTimer = timeBetweenAttacks;

        _agent.speed = speed;
        _agent.updateRotation = false;

        if (RB != null)
        {
            RB.isKinematic = true;
        }
    }

    private void Update()
    {
        if (HP <= 0 || isKnockedBack)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        bool inDetectionRange = distance <= detectionRange;
        bool facingPlayer = IsFacingPlayer();
        bool hasLineOfSight = HasClearPathToPlayer();

        if (hasLineOfSight)
        {
            lastKnownPlayerPos = playerTransform.position;
        }

        attackTimer -= Time.deltaTime;
        retreatCooldownTimer -= Time.deltaTime;

        if (!hasDetectedPlayer)
        {
            if (inDetectionRange && (facingPlayer || hasSeenPlayerBefore) && hasLineOfSight)
            {
                detectionTimer -= Time.deltaTime;

                if (detectionTimer <= 0f)
                {
                    hasDetectedPlayer = true;
                    hasSeenPlayerBefore = true;
                    isSearching = false;
                    lostPlayerTimer = 0f;
                }
            }
            else
            {
                detectionTimer = timeToDetectPlayer;
            }

            if (!hasDetectedPlayer)
            {
                UpdateSearching();
                return;
            }
        }

        if (hasLineOfSight)
        {
            lostPlayerTimer = 0f;
        }
        else
        {
            lostPlayerTimer += Time.deltaTime;

            if (lostPlayerTimer >= losePlayerTime)
            {
                hasDetectedPlayer = false;
                detectionTimer = timeToDetectPlayer;
                isRetreating = false;
                isSearching = true;
                _agent.isStopped = false;
                _agent.SetDestination(lastKnownPlayerPos);
                return;
            }
        }

        if (!isRetreating && distance < retreatDistance && retreatCooldownTimer <= 0f)
        {
            isRetreating = true;
            retreatTimer = maxRetreatTime;
        }
        else if (isRetreating && distance >= retreatStopDistance)
        {
            EndRetreat();
        }

        if (isRetreating)
        {
            retreatTimer -= Time.deltaTime;

            bool foundSpot = RetreatFromPlayer();
            bool arrived = !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance + 0.1f;

            if (!foundSpot || arrived || retreatTimer <= 0f)
            {
                EndRetreat();
            }

            RotateTowardsPoint(playerTransform.position);

            TryAttack(hasLineOfSight);
            return;
        }

        RotateTowardsPoint(playerTransform.position);

        if (distance > DistanceThatEnemyCanAttack || !hasLineOfSight)
        {
            _agent.isStopped = false;
            _agent.speed = speed;
            _agent.SetDestination(playerTransform.position);
        }
        else
        {
            StopMoving();
            TryAttack(hasLineOfSight);
        }
    }

    private void TryAttack(bool hasLineOfSight)
    {
        if (!hasLineOfSight || attackTimer > 0f)
        {
            return;
        }

        if (playerScript != null)
        {
            playerScript.takeDamage(DamageIDeal, transform.position);
        }

        attackTimer = timeBetweenAttacks;
    }

    private void EndRetreat()
    {
        isRetreating = false;
        retreatCooldownTimer = retreatCooldown;
    }

    private void UpdateSearching()
    {
        if (!isSearching)
        {
            StopMoving();
            return;
        }

        RotateTowardsPoint(_agent.steeringTarget);

        bool arrived = !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance + 0.1f;

        if (arrived)
        {
            isSearching = false;
            StopMoving();
        }
    }

    private void StopMoving()
    {
        if (!_agent.isOnNavMesh)
        {
            return;
        }

        _agent.isStopped = true;
        _agent.ResetPath();
    }

    private bool RetreatFromPlayer()
    {
        Vector3 directionAway = transform.position - playerTransform.position;
        directionAway.y = 0f;

        if (directionAway.sqrMagnitude < 0.001f)
        {
            directionAway = -transform.forward;
        }

        directionAway.Normalize();
        Vector3 retreatPosition = playerTransform.position + directionAway * retreatStopDistance;

        if (NavMesh.SamplePosition(retreatPosition, out NavMeshHit hit, 3f, NavMesh.AllAreas))
        {
            _agent.isStopped = false;
            _agent.speed = speed;
            _agent.SetDestination(hit.position);
            return true;
        }

        return false;
    }

    private bool IsFacingPlayer()
    {
        Vector3 direction = playerTransform.position - transform.position;
        return Vector3.Angle(transform.forward, direction) <= viewAngle / 2f;
    }

    private bool HasClearPathToPlayer()
    {
        Vector3 origin = transform.position + Vector3.up;
        Vector3 target = playerTransform.position + Vector3.up;
        Vector3 direction = target - origin;

        if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit, direction.magnitude))
        {
            return hit.transform == playerTransform || hit.transform.IsChildOf(playerTransform);
        }

        return true;
    }

    private void RotateTowardsPoint(Vector3 point)
    {
        Vector3 direction = point - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,extraRotationSpeed * Time.deltaTime);
    }

    public bool TakeDamage(int dano, Transform Father)
    {
        HP -= dano;

        if (HP <= 0)
        {
            Destroy(gameObject);
            return true;
        }

        if (Father != null)
        {
            Vector3 knockbackDirection = transform.position - Father.position;
            knockbackDirection.y = 0f;
            StartCoroutine(KnockBack(knockbackDirection.normalized));
        }

        hasDetectedPlayer = true;
        hasSeenPlayerBefore = true;
        isSearching = false;
        lostPlayerTimer = 0f;

        LookAtPlayer();

        return false;
    }

    private IEnumerator KnockBack(Vector3 direction)
    {
        isKnockedBack = true;
        float timer = 0f;

        while (timer < knockbackDuration)
        {
            timer += Time.deltaTime;

            if (_agent != null && _agent.isOnNavMesh)
            {
                _agent.Move(direction * AmmountOfKnockBack * Time.deltaTime);
            }

            yield return null;
        }

        isKnockedBack = false;
    }

    public void LookAtPlayer()
    {
        if (playerTransform != null && _agent.isOnNavMesh)
        {
            _agent.isStopped = false;
            _agent.SetDestination(playerTransform.position);
        }
    }
}