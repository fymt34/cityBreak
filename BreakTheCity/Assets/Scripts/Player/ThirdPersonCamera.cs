using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
	[Header("カメラ回転設定")]
	private float cameraSensitivity;                            // カメラの感度
	[SerializeField] private Transform target;                  // 追従するターゲット
	[SerializeField] private Vector2 pitchMinMax = new Vector2(-45f, 30f); // 縦回転の制限角度
	[SerializeField] private float rotationSmoothing = 0.12f;   // 回転のスムージング係数
	[SerializeField] private float mouseSensitivity = 2.0f;     // マウスの感度設定
	[SerializeField] private float gamepadSensitivity = 100.0f; // ゲームパッドの感度設定

	[Header("カメラ距離設定")]
	[SerializeField] private float cameraDistanceMin = 0.5f;    // カメラの最短距離
	[SerializeField] private float cameraDistanceMax = 5f;      // カメラの最長距離
	[SerializeField] private float distanceSmoothing = 0.1f;    // 距離調整のスムージング係数

	[Header("障害物設定")]
	[SerializeField] private LayerMask obstacleLayer = -1;      // 障害物とするレイヤーマスク
	[SerializeField] private Transform cameraTransform;         // 実際のカメラのTransform

	// 回転制御用の変数
	private float currentYaw;           // 現在の水平回転角度
	private float currentPitch;         // 現在の垂直回転角度
	private Vector3 rotationVelocity;   // 回転用の速度
	private Vector3 currentRotation;    // 現在の回転値

	// カメラ距離制御用の変数
	private Vector3 cameraDirection;    // カメラの方向ベクトル
	private float currentDistance;      // 現在のカメラ距離
	private Vector3 distanceVelocity;   // 距離調整用の速度

	// 入力処理用変数
	private Vector2 lookInput = Vector2.zero; // 現在の視点移動入力値
	private bool isGamepadInput = false;      // 現在の入力がゲームパッドかどうかの判定

	void Start()
	{
		// カメラの初期方向を設定（正規化して保存）
		cameraDirection = cameraTransform.localPosition.normalized;

		// カメラの初期距離を最大距離に設定
		currentDistance = cameraDistanceMax;

		// 初期回転を現在の回転値で設定
		Vector3 initialRotation = transform.eulerAngles;
		currentYaw = initialRotation.y;
		currentPitch = initialRotation.x;
		currentRotation = initialRotation;
	}

	private void Update()
	{
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");

		lookInput=new Vector3(mouseX, mouseY);

		//感度
		cameraSensitivity = mouseSensitivity;
	}

	// カメラはLateUpdateで更新するのが一般的らしい
	void LateUpdate()
	{
		// カメラ回転の更新
		UpdateCameraRotation();

		// カメラ位置の更新（ターゲットに追従）
		UpdateCameraPosition();

		// 障害物との衝突をチェックしてカメラ距離を調整
		HandleCameraCollision();
	}

	private void UpdateCameraRotation()
	{
		// 入力に応じて回転角度を更新
		currentYaw += lookInput.x * cameraSensitivity * Time.deltaTime;
		currentPitch -= lookInput.y * cameraSensitivity * Time.deltaTime;

		// 垂直回転を制限
		// pitchMinMaxのXとYの名前に引っ張られないように
		// インスペクターにXとYとあるがどちらもYの最低値と最大値として考える
		currentPitch = Mathf.Clamp(currentPitch, pitchMinMax.x, pitchMinMax.y);

		// スムーズな回転角を取得するための処理
		// SmoothDamp　は　Lerp　のような関数。
		Vector3 targetRotation = new Vector3(currentPitch, currentYaw, 0f);
		currentRotation = Vector3.SmoothDamp(
			currentRotation,
			targetRotation,
			ref rotationVelocity,
			rotationSmoothing
		);

		// 回転を適用
		transform.eulerAngles = currentRotation;
	}

	private void UpdateCameraPosition()
	{
		// カメラピボットをターゲット位置に直接移動
		transform.position = target.position;
	}

	private void HandleCameraCollision()
	{
		// カメラとターゲットの間に障害物があった場合
		// カメラ位置を変更
		Vector3 desiredCameraPosition = transform.TransformPoint(cameraDirection * cameraDistanceMax);

		// レイキャストで障害物をチェック
		if (Physics.Linecast(transform.position, desiredCameraPosition, out RaycastHit hit, obstacleLayer))
		{
			// 障害物がある場合、距離を制限内でクランプ
			currentDistance = Mathf.Clamp(hit.distance * 0.9f, cameraDistanceMin, cameraDistanceMax);
		}
		else
		{
			// 障害物がない場合、最大距離に戻す
			currentDistance = cameraDistanceMax;
		}

		// スムーズにカメラ位置を調整
		Vector3 targetLocalPosition = cameraDirection * currentDistance;
		cameraTransform.localPosition = Vector3.SmoothDamp(
			cameraTransform.localPosition,
			targetLocalPosition,
			ref distanceVelocity,
			distanceSmoothing
		);
	}

	void OnDrawGizmosSelected()
	{
		if (target != null && cameraTransform != null)
		{
			// ターゲットからカメラへのレイを描画
			Gizmos.color = Color.red;
			Gizmos.DrawLine(target.position, cameraTransform.position);

			// カメラの範囲を描画
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(target.position, cameraDistanceMax);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(target.position, cameraDistanceMin);
		}
	}
}