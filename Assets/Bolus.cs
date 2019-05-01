using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolus : MonoBehaviour
{
	public SpriteRenderer sprite;

	protected float digestProgress = 0f;

	public bool digested
	{
		get => digestProgress == 1f;
	}

	void Update()
	{
		var color = sprite.color;
		color.a = 1 - digestProgress;
		sprite.color = color;
	}

	public void Digest(float digestAmountDelta)
	{
		digestProgress = Mathf.Clamp01(digestProgress + digestAmountDelta);
	}
}
