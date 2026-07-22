using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] InputActionReference _MovementAction;
	[SerializeField] InputActionReference _InteractionAction;
	[SerializeField] LayerMask _RaycastLayerMask;
	[SerializeField] float _Acceleration;
	[SerializeField] float _Speed;
	[SerializeField] float _InteractionRange;
	
	private Vector2 _LastMovementVector = Vector2.zero;
	private SpringJoint2D _SpringJoint;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		_MovementAction.action.Enable();
		_InteractionAction.action.Enable();
	}

	// Update is called once per frame
	void Update()
	{
		Vector2 movementVector = _MovementAction.action.ReadValue<Vector2>() * _Speed;

		Vector2 finalMovementVector = Vector2.MoveTowards(_LastMovementVector, movementVector, _Acceleration * Time.deltaTime);
		transform.position = transform.position +  (Vector3)(finalMovementVector * Time.deltaTime);
		_LastMovementVector = finalMovementVector;
		if (finalMovementVector.magnitude > 0.1 && Vector3.Distance(finalMovementVector, transform.up) > 0.1)
			transform.rotation = Quaternion.LookRotation(Vector3.forward, finalMovementVector);

		if (_InteractionAction.action.ReadValue<float>() >= 0.5f) 
		{
			RaycastHit2D hit = Physics2D.BoxCast(transform.position + (transform.up/2), Vector2.one * 0.5f, transform.eulerAngles.z, transform.up, _InteractionRange, _RaycastLayerMask);
			if (hit.collider != null) 
			{
				if (hit.transform.TryGetComponent(out SpringJoint2D spring)) 
				{
					_SpringJoint = spring;
				}
			}
		}
		if (_SpringJoint != null)
			_SpringJoint.connectedAnchor = transform.position;
		
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(transform.position, transform.position + (transform.up * _InteractionRange));
	}
}
