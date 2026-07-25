using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
class InteractionRenderer : MonoBehaviour
{
	public static InteractionRenderer Instance;

	LineRenderer _LineRenderer;

	private void Start()
	{
		Instance = this;
		_LineRenderer = GetComponent<LineRenderer>();
	}


	static void DrawBorder(Collider2D collider)
	{
		if ((CompositeCollider2D)collider)
		{
			List<Vector2> points = new List<Vector2>();
			((CompositeCollider2D)collider).GetPath(0, points);

			Instance._LineRenderer.positionCount = points.Count + 1;

			for (int i = 0; i < points.Count; i++)
			{
				Instance._LineRenderer.SetPosition(i, (Vector3)points[i] + (Vector3.forward * -0.01f));
			}
			Instance._LineRenderer.SetPosition(points.Count, (Vector3)points[0] + (Vector3.forward * -0.01f));

		}
	}

}