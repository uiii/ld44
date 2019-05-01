using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeIntestinePart : MonoBehaviour
{
	public Transform stercusMask;

	private Vector3 originalScale;
	private Vector3 originalMaskScale;

	public float stercusPosition;

	// Start is called before the first frame update
	void Start()
	{
		originalScale = transform.localScale;
		originalMaskScale = stercusMask.localScale;

		stercusMask.localScale = new Vector3(
			stercusMask.localScale.x,
			0.2f,
			1f
		);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Scale(Vector3 scale) {
		transform.localScale = new Vector3(
			originalScale.x * scale.x,
			originalScale.y * scale.y,
			1f
		);
	}

	public void SetStercusPosition(float percent)
	{
		stercusPosition = percent;
		float maskPercent = -2f * Mathf.Abs(percent - 0.5f) + 1f;

		stercusMask.localScale = new Vector3(originalMaskScale.x * maskPercent, originalMaskScale.y * maskPercent, 1f);
		stercusMask.localPosition = new Vector3(0f, percent * 0.482f - 0.241f, 0f);
	}
}
