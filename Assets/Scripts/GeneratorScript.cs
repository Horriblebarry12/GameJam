using UnityEngine;

public class GeneratorScript : BasicMashine
{
	[SerializeField] float _OutputCooldown;
	//References
	ProductManager _ProductManager;

	float _LastOutputTime;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	protected override void Start()
	{
		_InputProduct = null;
		_MashineInput = GetComponentInChildren<MashineInput>();
		_ProductManager = GameObject.FindWithTag("ProductManager").GetComponent<ProductManager>();
		Generate();
	}

	// Update is called once per frame
	void Update()
	{

	}


	public void Generate()
	{
		GenerateProduct("square(triangle(square,0),0,square(0,triangle(square,0),triangle),semicircle)");
	}

	void GenerateProduct(string blueprint)
	{
		if (_LastOutputTime + _OutputCooldown > Time.time)
			return;
		_ProductManager.GenerateProduct(blueprint, transform.position + (Vector3)_OutputStartPos, transform.rotation, false);
		//animation
		_LastOutputTime = Time.time;
	}
}
