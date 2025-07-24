using UnityEngine;

public class AquaSpiderAI : MonoBehaviour
{
	// Định nghĩa các trạng thái của Nhện
	private enum SpiderState { Patrolling, Chasing, Attacking }
	private SpiderState currentState;

	[Header("Movement Settings")]
	public float moveSpeed = 2f;
	public float chaseSpeed = 3.5f; // Tốc độ khi đuổi theo player

	[Header("Behavior Ranges")]
	public float detectionRange = 10f; // Tầm phát hiện player để đuổi theo
	public float attackRange = 3f;
	public float patrolRange = 5f; // Phạm vi di chuyển tuần tra quanh vị trí gốc

	[Header("Attack Settings")]
	public float attackCooldown = 1.5f;
	private float lastAttackTime = -999f;

	[Header("References")]
	public Transform player;

	private Animator animator;
	private Vector2 originPos;
	private Vector2 targetPos;
	private Vector2 patrolTargetPos;
	private bool isFacingRight = false;
	private bool isAttacking = false;
	public float attackCooldown = 1f;
	private float lastAttackTime = 0f;

	void Start()
	{
		animator = GetComponent<Animator>();
		originPos = transform.position;
		ChooseNewTarget();
		FindPlayer();

		// Tự động xác định hướng ban đầu dựa vào scale của object.
		// Nếu scale.x dương, nó đang quay sang phải.
		isFacingRight = transform.localScale.x > 0;

		// Bắt đầu ở trạng thái tuần tra
		TransitionToState(SpiderState.Patrolling);
	}

	void Update()
	{
		if (player == null)
		{
			// Cố gắng tìm lại player nếu bị mất
			FindPlayer();
			if (player == null) return;
		}

		// Máy trạng thái (State Machine)
		switch (currentState)
		{
			case SpiderState.Patrolling:
				HandlePatrollingState();
				break;
			case SpiderState.Chasing:
				HandleChasingState();
				break;
			case SpiderState.Attacking:
				HandleAttackingState();
				break;
		}
	}

	// --- Xử lý các trạng thái ---

	private void HandlePatrollingState()
	{
		// Di chuyển tuần tra ngẫu nhiên
		MoveTo(patrolTargetPos, moveSpeed);

		// Nếu gần tới điểm đích thì chọn điểm mới
		if (Vector2.Distance(transform.position, patrolTargetPos) < 0.2f)
			ChooseNewPatrolTarget();
		

		// Nếu player đi vào tầm phát hiện, chuyển sang đuổi theo
		if (Vector2.Distance(transform.position, player.position) < detectionRange)
		{
			TransitionToState(SpiderState.Chasing);
		}
	}

	private void HandleChasingState()
	{
		// Đuổi theo player
		MoveTo(player.position, chaseSpeed);

		float distanceToPlayer = Vector2.Distance(transform.position, player.position);

		// Nếu vào tầm tấn công, chuyển sang tấn công
		if (distanceToPlayer <= attackRange)
		{
			TransitionToState(SpiderState.Attacking);
		}
		// Nếu player chạy quá xa, quay lại tuần tra
		else if (distanceToPlayer > detectionRange)
		{
			TransitionToState(SpiderState.Patrolling);
		}
	}

	private void HandleAttackingState()
	{
		// Dừng lại và nhìn về phía player
		FacePlayer();

		// Gây sát thương theo cooldown
		if (Time.time >= lastAttackTime + attackCooldown)
		{
			// Chơi animation tấn công
			animator.Play("attack");
			
			// Gây sát thương
			PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
			if (playerHealth != null)
			{
				playerHealth.TakeDamage(10);
			}
			lastAttackTime = Time.time;
		}

		// Nếu player di chuyển ra khỏi tầm tấn công, đuổi theo lại
		if (Vector2.Distance(transform.position, player.position) > attackRange)
		{
			TransitionToState(SpiderState.Chasing);
		}
	}

	// --- Các hàm hỗ trợ ---

	private void TransitionToState(SpiderState newState)
	{
		currentState = newState;
		// Logic khi chuyển trạng thái
		switch (currentState)
		{
			case SpiderState.Patrolling:
				animator.Play("run");
				ChooseNewPatrolTarget();
				break;
			case SpiderState.Chasing:
				animator.Play("run");
				break;
			case SpiderState.Attacking:
				// Animation tấn công sẽ được gọi trong HandleAttackingState
				break;
		}
	}

	private void MoveTo(Vector2 target, float speed)
	{
		transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
		// Lật sprite của nhện để luôn nhìn về hướng di chuyển
		if (target.x > transform.position.x && !isFacingRight)
		{
			Flip();
		}
		else if (target.x < transform.position.x && isFacingRight)
		{
			Flip();
		}
	}

	private void ChooseNewPatrolTarget()
	{
		float randomX = Random.Range(originPos.x - patrolRange, originPos.x + patrolRange);
		// Giữ nguyên Y nếu là game 2D platformer
		patrolTargetPos = new Vector2(randomX, originPos.y);
	}

	private void FacePlayer()
	{
		if (player.position.x > transform.position.x && !isFacingRight)
		{
			Flip();
		}
		else if (player.position.x < transform.position.x && isFacingRight)
		{
			Flip();
		}
	}

	private void Flip()
	{
		isFacingRight = !isFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void MoveRandomly()
	{
		// Nếu gần tới điểm đích thì chọn điểm mới
		if (Vector2.Distance(transform.position, targetPos) < 0.2f)
		{
			ChooseNewTarget();
		}
		// Di chuyển tới điểm đích
		transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
	}
	
	void ChooseNewTarget()
	{
		float randomX = Random.Range(originPos.x - moveRange, originPos.x + moveRange);
		float randomY = originPos.y; // Nếu chỉ muốn di chuyển ngang, giữ nguyên Y
		targetPos = new Vector2(randomX, randomY);
	}

	private void FindPlayer()
	{
		GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
		if (playerObj != null)
			player = playerObj.transform;
		else
			Debug.LogWarning("Không tìm thấy player trong scene!");
	}
}
