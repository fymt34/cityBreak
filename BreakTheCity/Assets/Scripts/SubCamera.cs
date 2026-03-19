using UnityEngine;

public class SubCamera : Singleton<SubCamera>
{
	[SerializeField] private GameObject Camera;
	[SerializeField] private GameObject subCamera;

	// ’†S“_
	[SerializeField] private Transform _mapTransform;

	// ‰ñ“]²
	[SerializeField] private Vector3 _axis = Vector3.up;

	// ‰~‰^“®üŠú
	[SerializeField] private float _period = 2;

	private bool _isActive = false;

	void Start()
	{
		Camera.SetActive(true);
		subCamera.SetActive(false);
		_isActive = false;
	}

	void Update()
	{
		if(!_isActive)
			return;
		subCamera.transform.RotateAround(
		   _mapTransform.position,
		   _axis,
		   360 / _period * Time.unscaledDeltaTime
	   );
	}

	public void SwitchToSubCamera()
	{
		Camera.SetActive(false);
		subCamera.SetActive(true);

		_isActive = true;
	}
}
