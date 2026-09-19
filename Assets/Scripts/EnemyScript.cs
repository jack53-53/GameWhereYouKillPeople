using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyScript : MonoBehaviour
{
    public int HP;
    public float speed;
    public float timeBetweenAttacks;
    private float _timeBetweenAttacks;
    public Transform playerTransform;
    private float detectionRange = 5f;
    public NavMeshAgent _agent;
    private float viewAngle = 90f;
    private float losePlayerTime = 3f;
    private bool IsIdle = true;
    private bool Atacou;
    private float _timeSinceLostPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _timeBetweenAttacks = timeBetweenAttacks;
        _timeSinceLostPlayer = losePlayerTime;
    }

    // Update is called once per frame
    void Update()
    {
        var distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        Debug.Log(distanceToPlayer);
        if(distanceToPlayer <= detectionRange && canSeePlayer())
        {
            IsIdle = false;
        }

        if (!IsIdle)
        {
            FollowPlayer();
            if (!canSeePlayer())
            {
                _timeSinceLostPlayer += Time.deltaTime;
                if(_timeSinceLostPlayer >= losePlayerTime)
                {
                    Debug.Log("perdi o player");
                    IsIdle = true;
                }
            }
            else
            {
                _timeSinceLostPlayer = 0f;
            }
        }
        // Debug.Log("estou procurando pelo player");
        if(HP <= 0)
        {
            Destroy(gameObject);
        }
        if(_timeBetweenAttacks < 0)
        {
            _timeBetweenAttacks = timeBetweenAttacks;
            Atacou = true; //depois no codigo do inimigo tem que tirar isso aq
        }
        _timeBetweenAttacks -= timeBetweenAttacks;
    }

    private void FollowPlayer()
    {
        _agent.SetDestination(playerTransform.position);
        // Debug.Log("tentando seguir o jogador");
    }

    private bool canSeePlayer()
    {
        return isFacingPlayer() && HasClearPathToPlayer();
    }

    private bool isFacingPlayer()
    {
        var dirToPlayer = (playerTransform.position - transform.position).normalized;
        var angle = Vector3.Angle(transform.forward, dirToPlayer);
        // Debug.Log("vi o jogador");
        return angle <= viewAngle / 2f;
    }

    private bool HasClearPathToPlayer()
    {
        var directionToPlayer = playerTransform.position - transform.position;
        if(Physics.Raycast(transform.position, directionToPlayer.normalized, out RaycastHit hit, directionToPlayer.magnitude))
        {
            return hit.transform == playerTransform;
        }
        return true;
    }
}
