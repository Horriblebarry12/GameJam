using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] InputActionReference _MovementAction;
	[SerializeField] InputActionReference _InteractionAction;
	[SerializeField] LayerMask _ProductRaycastLayer;
	[SerializeField] LayerMask _OutputTileRaycastLayer;
	[SerializeField] LayerMask _MashineInputRaycastLayer;
	[SerializeField] float _Acceleration;
	[SerializeField] float _Speed;
	[SerializeField] float _InteractionRange;

	private Vector2 _LastMovementVector = Vector2.zero;
	private SpringJoint2D _SpringJoint;

	private Rigidbody2D _Rigidbody;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		_MovementAction.action.Enable();
		_InteractionAction.action.Enable();
		_InteractionAction.action.performed += InteractionPerformed;

		_Rigidbody = GetComponent<Rigidbody2D>();
	}

	private void InteractionPerformed(InputAction.CallbackContext obj)
	{
		Debug.Log("Action Prformed!");

		RaycastHit2D hit = Physics2D.BoxCast(transform.position + (transform.up / 2), Vector2.one * 0.5f, transform.eulerAngles.z, transform.up, _InteractionRange, _ProductRaycastLayer);
		if (hit.collider != null)
		{
			if (hit.transform.TryGetComponent(out SpringJoint2D spring))
			{
				if (_SpringJoint != null)
					_SpringJoint.enabled = false;
				_SpringJoint = spring;
				_SpringJoint.enabled = true;
				//_SpringJoint.GetComponent<Rigidbody2D>().simulated = true;
			}
		}
		else
		{
			hit = Physics2D.BoxCast(transform.position + (transform.up / 2), Vector2.one * 0.5f, transform.eulerAngles.z, transform.up, _InteractionRange, _OutputTileRaycastLayer);
			if (hit.collider != null && _SpringJoint != null)
			{
				_SpringJoint.transform.position = hit.transform.position;
				_SpringJoint.enabled = false;
				_SpringJoint.GetComponent<Rigidbody2D>().simulated = false;
				hit.transform.GetComponent<OutputConvayor>().Product = _SpringJoint.GetComponent<ProductComponent>();
				_SpringJoint = null;
			}
			else
			{
				hit = Physics2D.BoxCast(transform.position + (transform.up / 2), Vector2.one * 0.5f, transform.eulerAngles.z, transform.up, _InteractionRange, _MashineInputRaycastLayer);
				if (hit.collider != null && _SpringJoint != null && hit.transform.parent != null)
				{
					if (hit.transform.parent.TryGetComponent(out BasicMashine mashine))
					{
						_SpringJoint.enabled = false;
						_SpringJoint.GetComponent<Rigidbody2D>().simulated = false;
						mashine.InputProduct(_SpringJoint.GetComponent<ProductHolder>(), hit.transform.GetComponent<MashineInput>());
						_SpringJoint = null;
					}
				}
				else if (hit.collider != null && hit.transform.TryGetComponent(out GeneratorScript generator))
				{
					generator.Generate();
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		Vector2 movementVector = _MovementAction.action.ReadValue<Vector2>() * _Speed;

		Vector2 finalMovementVector = Vector2.MoveTowards(_LastMovementVector, movementVector, _Acceleration * Time.deltaTime);
		_Rigidbody.linearVelocity = (finalMovementVector);
		_LastMovementVector = finalMovementVector;
		if (finalMovementVector.magnitude > 0.1 && Vector3.Distance(finalMovementVector, transform.up) > 0.1)
			transform.rotation = Quaternion.LookRotation(Vector3.forward, finalMovementVector);

		if (_SpringJoint != null)
			_SpringJoint.connectedAnchor = transform.position;

	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(transform.position, transform.position + (transform.up * _InteractionRange));
	}
}
