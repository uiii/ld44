using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeIntestinePart : MonoBehaviour
{
	public float maxContraction = 0.2f;

	protected Vector3 originalScale;

	// Start is called before the first frame update
	void Start()
	{
		originalScale = transform.localScale;
	}

	public void Contract(float percent = 1f)
	{
		transform.localScale = new Vector3(
			originalScale.x * (1f - maxContraction * percent),
			originalScale.y,
			1f
		);
	}
}
