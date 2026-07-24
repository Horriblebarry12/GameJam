using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MashineInput : MonoBehaviour
{
	[field: SerializeField] public int InputIndex { get; private set; }
	[field: SerializeField] public Collider2D _Collider { get; private set; }
	[field: SerializeField] public Vector2 _InputStartPos { get; private set; }
	[field: SerializeField] public Vector2 _InputEndPos { get; private set; }

	void Start()
	{
		_Collider.GetComponent<Collider2D>();
	}
}
