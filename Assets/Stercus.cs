using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stercus : MonoBehaviour
{
	public LargeIntestine largeIntestine;
	public float position;

	void Update()
	{
		int index = Mathf.FloorToInt(position);

		if (index < largeIntestine.parts.Length) {
			Vector3 previousPart = index < 0
				? largeIntestine.start.position
				: largeIntestine.parts[index].transform.position;

			Vector3 nextPart = index < largeIntestine.parts.Length - 1
				? largeIntestine.parts[index + 1].transform.position
				: largeIntestine.exit.position;

			transform.position = Vector3.Lerp(previousPart, nextPart, (position + 1) % 1);
		} else {
			Destroy(gameObject);
		}
	}
}
