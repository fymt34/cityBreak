using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
	enum State
	{
		Patrol,
		Chase,
		Attack
	}

	[Header("References")]
	public Transform player;
	public NavMeshAgent agent;
	public Animator animator;

	[Header("View")]
	public float viewDistance = 25f;
	public float viewAngle = 120f;
	public LayerMask obstacleMask;

	[Header("Random Patrol")]
	public float patrolRadius = 10f;   // ‚Ç‚ê‚­‚ç‚¢‚Ì”ÍˆÍ‚ð•à‚­‚©
	public float waitTime = 2f;         // —§‚¿Ž~‚Ü‚éŽžŠÔ

	private float waitTimer;
	private State state = State.Patrol;
	bool isStopped = false;
	bool isPlayerInSafeRoom = false;

	private float dis;
	[SerializeField] private float freezeTime = 0.1f;
	private float time = 0.0f;

	[SerializeField] private Collider Collider;

	AudioSource audio;

	void Start()
	{
		animator = GetComponent<Animator>();
		audio = GetComponent<AudioSource>();
		agent.autoBraking = true;
		SetRandomDestination();
		time = 0.0f;
	}

	void Update()
	{
		if(isStopped)
			return;

		float speed = agent.velocity.magnitude;

		animator.SetFloat("Speed", speed);

		dis = Vector3.Distance(transform.position, player.transform.position);

		switch(state)
		{
			case State.Patrol:
				RandomPatrol();

				if(CanSeePlayer())
				{
					state = State.Chase;
				}
				break;

			case State.Chase:
				agent.SetDestination(player.position);

				if(!CanSeePlayer())
				{
					state = State.Patrol;
					SetRandomDestination();
				}
				else if(dis <= 5f && !PlayerController.Instance.isHide)
				{
					state = State.Attack;
					agent.isStopped = true;
					animator.SetTrigger("Attack");
					Collider.enabled = true;
					Invoke("ColliderReset", 1.0f);
					time = 0f;
				}
				break;

			case State.Attack:

				time += Time.deltaTime;

				if(time >= freezeTime)
				{
					state = State.Chase;
					agent.isStopped = false;
				}

				break;
		}
	}

	void RandomPatrol()
	{
		if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
		{
			waitTimer += Time.deltaTime;

			if(waitTimer >= waitTime)
			{
				SetRandomDestination();
				waitTimer = 0f;
			}
		}
	}

	void SetRandomDestination()
	{
		Vector3 randomDir = Random.insideUnitSphere * patrolRadius;
		randomDir += transform.position;

		if(NavMesh.SamplePosition(randomDir, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
		{
			agent.SetDestination(hit.position);
		}
	}

	public void StopChaseAndReturnPatrol()
	{
		state = State.Patrol;

		agent.ResetPath();
		agent.isStopped = false;

		SetRandomDestination();
	}

	public void SetPlayerInSafeRoom(bool value)
	{
		isPlayerInSafeRoom = value;

		if(value && state == State.Chase)
		{
			state = State.Patrol;
			agent.ResetPath();

			SetRandomDestination();
		}
	}

	bool CanSeePlayer()
	{
		if(isPlayerInSafeRoom)
			return false;

		Vector3 toPlayer = player.position - transform.position;
		float dist = toPlayer.magnitude;

		if(dist > viewDistance)
			return false;

		float angle = Vector3.Angle(transform.forward, toPlayer);
		if(angle > viewAngle * 0.5f)
			return false;

		if(Physics.Raycast(
			transform.position + Vector3.up,
			toPlayer.normalized,
			dist,
			obstacleMask))
			return false;

		return true;
	}

	public void StopEnemy()
	{
		isStopped = true;
		agent.isStopped = true;
		agent.ResetPath();

		animator.speed = 0f;
	}

	private void ColliderReset()
	{
		Collider.enabled = false;
	}

	public void OnFootstep()
	{
		audio.Play();
	}
}