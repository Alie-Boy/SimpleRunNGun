using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {

	[SerializeField] float walkSpeed = 6f;
	[SerializeField] LayerMask groundMask;

	Camera mainCamera;
	Rigidbody rigidbody;
	Vector3 moveDir;
	Vector3 velocity;

	void Start()
	{
		mainCamera = Camera.main;
		rigidbody = GetComponent<Rigidbody>();
	}

	void Update()
	{
		LookAtMousePosition();
		CalculateVelocity();
	}

	void FixedUpdate()
	{
		// moving with rigidbody instead of transform it has slightly higher performance.
		rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
	}

	private void CalculateVelocity()
	{
		moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
		velocity = moveDir * walkSpeed;
	}

	private void LookAtMousePosition()
	{
		Vector3 mousePos = Vector3.one;
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100f, groundMask))
		{
			mousePos = hit.point;
		}

		transform.LookAt(mousePos + Vector3.up * transform.position.y);
	}
}
