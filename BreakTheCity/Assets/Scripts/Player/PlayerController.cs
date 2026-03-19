using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
	// コンポーネント
	[SerializeField] private Rigidbody _rigidbody;

	// アクションからの入力値を取得するための変数
	private Vector2 pMove;

	// 移動用の変数
	[SerializeField] private float moveSpeed = 5f;
	private Vector3 moveDirection;

	// ジャンプ用の変数
	[SerializeField] private float jumpPower = 5f;
	[SerializeField] private LayerMask groundLayer = 1;
	[SerializeField] private int maxJumpCount = 1;
	[SerializeField] private float groundCheckDistance = 0.4f;
	[SerializeField] private Transform groundCheckPoint;
	private int jumpCount = 0;
	private bool isGrounded;
	private bool wasGrounded;

	//ダッシュ用の変数
	[SerializeField] private float dashSpeed = 10f;
	private bool isDashing;

	// カメラの方向情報を取得用
	[SerializeField] private Transform cameraTransform;

	[SerializeField] private float smoothRotate = 10f;

	//アニメーション用の変数
	private Animator _animator;
	private bool _hasAnimator;
	private int _animIDSpeed;
	private int _animIDGrounded;
	private int _animIDJump;
	private int _animIDFreeFall;
	private int _animIDMotionSpeed;
	private int _animIdAttack;
	private int _animIDFrontHit;
	private int _animIDBackHit;

	private bool isAttacking;

	[SerializeField] private Collider rightHand;
	[SerializeField] private Collider leftHand;

	public bool isHide;
	private bool isCanMove;

	private void Start()
	{
		Cursor.visible = false;

		Cursor.lockState = CursorLockMode.Locked;

		// カメラが設定されていない場合はメインカメラを使用
		if(cameraTransform == null)
		{
			cameraTransform = Camera.main.transform;
		}

		_hasAnimator = TryGetComponent(out _animator);
		AssignAnimationIDs();

		isGrounded = true;

		rightHand.enabled = false;
		leftHand.enabled = false;

		isAttacking = false;
		isHide = false;
		isCanMove = true;
	}

	private void Update()
	{
		pMove.x = Input.GetAxis("Horizontal");
		pMove.y = Input.GetAxis("Vertical");

		if(Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}

		isDashing = Input.GetKey (KeyCode.LeftShift);

		// 移動方向の計算
		CalculateMovement();

		// 地面接地判定
		CheckGrounded();

		// キャラクターの向きを回転
		if(moveDirection.sqrMagnitude > 0.001f && !isAttacking && isCanMove)
		{
			Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
			transform.rotation = Quaternion.Slerp(
				transform.rotation,
				targetRotation,
				smoothRotate * Time.deltaTime
			);
		}

		AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(1);

		if(Input.GetMouseButtonDown(0))
		{
			_animator.SetTrigger(_animIdAttack);
		}

		//攻撃すると当たり判定を表示
		if(stateInfo.IsTag("Right Punch"))
		{
			StartCoroutine("StartAttackRight");
			Invoke("ColliderReset", 0.8f);
		}
		else if(stateInfo.IsTag("Left Punch"))
		{
			StartCoroutine("StartAttackLeft");			
			Invoke("ColliderReset", 1.0f);
		}
	}

	IEnumerator StartAttackRight()
	{
		yield return new WaitForSeconds(0.5f);
		rightHand.enabled = true;
		isAttacking = true;
	}

	IEnumerator StartAttackLeft()
	{
		yield return new WaitForSeconds(0.3f);
		leftHand.enabled = true;
		isAttacking = true;
	}

	private void FixedUpdate()
	{
		// 移動処理を実行
		ApplyMovement();

		if(_hasAnimator)
		{
			float speed = _rigidbody.linearVelocity.magnitude;

			_animator.SetFloat(_animIDSpeed, speed);
			_animator.SetFloat(_animIDMotionSpeed, isDashing ? 2f : 1f);
		}

		if(_hasAnimator)
		{
			_animator.SetBool(_animIDFreeFall, !isGrounded);
			_animator.SetBool(_animIDJump, !isGrounded);
		}
	}

	private void CalculateMovement()
	{
		// カメラの前方向と右方向を取得（Y軸回転のみ考慮）
		Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
		Vector3 cameraRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

		// 移動方向を計算
		moveDirection = (cameraForward * pMove.y + cameraRight * pMove.x).normalized;
	}

	private void CheckGrounded()
	{
		wasGrounded = isGrounded;

		isGrounded = Physics.Raycast
		(
			groundCheckPoint.position,
			Vector3.down,
			groundCheckDistance,
			groundLayer
		);

		// 着地した瞬間
		if(!wasGrounded && isGrounded)
		{
			jumpCount = 0;

			if(_hasAnimator)
			{
				_animator.SetBool(_animIDGrounded, true);
				_animator.SetBool(_animIDFreeFall, false);
			}
		}

		// 空中にいる間
		if(_hasAnimator)
		{
			_animator.SetBool(_animIDGrounded, isGrounded);
			_animator.SetBool(_animIDFreeFall, !isGrounded);
		}
	}

	private void ApplyMovement()
	{

		//if (isGrounded)
		if(isCanMove)
		{
			float speed = isDashing ? dashSpeed : moveSpeed;
			Vector3 targetVelocity = moveDirection * speed;
			_rigidbody.linearVelocity = new Vector3(
				targetVelocity.x,
				_rigidbody.linearVelocity.y,
				targetVelocity.z
			);
		}
	}

	private void AssignAnimationIDs()
	{
		_animIDSpeed = Animator.StringToHash("Speed");
		_animIDGrounded = Animator.StringToHash("Grounded");
		_animIDJump = Animator.StringToHash("Jump");
		_animIDFreeFall = Animator.StringToHash("FreeFall");
		_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
		_animIdAttack = Animator.StringToHash("Attack");
		_animIDFrontHit = Animator.StringToHash("FrontHit");
		_animIDBackHit = Animator.StringToHash("BackHit");
	}

	public void OnFootstep(AnimationEvent animationEvent)
	{
	}

	public void OnLand(AnimationEvent animationEvent)
	{
	}

	public void Jump()
	{
		if(jumpCount >= maxJumpCount)
			return;
		if(!isGrounded && jumpCount == 0)
			return;
		if(!isCanMove)
			return;

		// 物理
		_rigidbody.linearVelocity = new Vector3
		(
			_rigidbody.linearVelocity.x,
			jumpPower,
			_rigidbody.linearVelocity.z
		);

		// 状態
		jumpCount++;
		isGrounded = false;

		// 入力した瞬間にジャンプアニメ
		if(_hasAnimator)
		{
			_animator.SetBool(_animIDJump, true);
			_animator.SetBool(_animIDGrounded, false);
		}
	}

	private void ColliderReset()
	{
		rightHand.enabled = false;
		leftHand.enabled = false;

		isAttacking = false;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(!collision.gameObject.tag.Contains("EnemyAttack"))
			return;

		SoundManager.Instance.PlayenemyAttackSE();

		Vector3 playerPos = gameObject.transform.position;
		Vector3 enemyPos = collision.gameObject.transform.position;

		Vector3 vec = enemyPos - playerPos;
		//Dotは内積を計算する
		float dot = Vector3.Dot(vec.normalized, gameObject.transform.forward);

		EffectManager.Instance.PlayEnemyEffect(transform.position);

		if(dot > 0)
		{
			if(_hasAnimator && !isHide)
			{
				_animator.SetTrigger(_animIDFrontHit);
				StartCoroutine("StartUnrivaled");
				StartCoroutine("StartCanMove");
			}
		}
		else if(dot <= 0)
		{

			if(_hasAnimator && !isHide)
			{
				_animator.SetTrigger(_animIDBackHit);
				StartCoroutine("StartUnrivaled");
				StartCoroutine("StartCanMove");
			}
		}
	}

	IEnumerator StartUnrivaled()
	{
		isHide = true;
		yield return new WaitForSeconds(5.0f);
		isHide = false;
	}

	IEnumerator StartCanMove()
	{
		isCanMove = false;
		yield return new WaitForSeconds(3.0f);
		isCanMove = true;
	}

	public void OnPlayerstep()
	{
		SoundManager.Instance.PlayplayerstepSE();
	}

	public void OnPlayerJump()
	{
		SoundManager.Instance.PlayplayerJumpSE();
	}

	public void OnPlayerlanding()
	{
		SoundManager.Instance.PlayplayerLandingSE();
	}
}