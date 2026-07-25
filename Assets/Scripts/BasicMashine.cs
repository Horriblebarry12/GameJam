using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BasicMashine : MonoBehaviour
{

	[SerializeField] protected Vector2 _OutputEndPos;
	[SerializeField] protected Vector2 _OutputStartPos;
	[SerializeField] protected float _OutputSpeed;
	[SerializeField] protected int _NumInputs;

	protected ProductHolder _InputProduct;
	protected MashineInput _MashineInput;


	//protected abstract ProductHolder CalculateOutputProduct();

	protected virtual void Start()
	{
		_InputProduct = null;
		_MashineInput = GetComponentInChildren<MashineInput>();
	}

	public virtual void InputProduct(ProductHolder productHolder)
	{
		_InputProduct = productHolder;
		StartCoroutine(InputCoroutine(productHolder, _MashineInput));

	}

	IEnumerator InputCoroutine(ProductHolder inputProduct, MashineInput mashineInput)
	{
		inputProduct.transform.position = mashineInput._InputStartPos;

		while (inputProduct.transform.position != (Vector3)mashineInput._InputEndPos)
		{
			yield return new WaitForEndOfFrame();
			inputProduct.transform.position = Vector3.MoveTowards(inputProduct.transform.position, mashineInput._InputEndPos, _OutputSpeed * Time.deltaTime);
		}
	}

	protected virtual void OutputProduct(ProductHolder productHolder)
	{
		StartCoroutine(OutputCoroutine(productHolder));
	}

	IEnumerator OutputCoroutine(ProductHolder outputProduct)
	{
		outputProduct.transform.position = _OutputStartPos;

		while (outputProduct.transform.position != (Vector3)_OutputEndPos)
		{
			yield return new WaitForEndOfFrame();
			outputProduct.transform.position = Vector3.MoveTowards(outputProduct.transform.position, _OutputEndPos, _OutputSpeed * Time.deltaTime);
		}
	}

}
