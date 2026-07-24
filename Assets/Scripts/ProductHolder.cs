using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D), typeof(Rigidbody2D), typeof(SpringJoint2D))]
public class ProductHolder : MonoBehaviour
{
	GameObject topComponent;
	Vector2 centerOfMassOffset;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		GetComponent<SpringJoint2D>().enabled = false;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
