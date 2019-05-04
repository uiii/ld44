using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lungs : MonoBehaviour
{
	public Body body;

	public Transform leftLung;
	public Transform rightLung;

	public float animationSpeed = 1f;

	protected bool isInhaling;
	protected float flipTime;
	protected float animationPercent;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.J)) {
			isInhaling = true;
			flipTime = Time.time;
		}

		if (Input.GetKeyUp(KeyCode.J)) {
			isInhaling = false;
			flipTime = Time.time;
		}

		var percentDelta = Time.deltaTime * animationSpeed;

		animationPercent += isInhaling ? percentDelta : -percentDelta;
		//animationPercent += percentDelta;
		animationPercent = Mathf.Clamp01(animationPercent);

		var exhaledScale = new Vector3(0.9f, 0.9f, 1f);
		var inhaledScale = new Vector3(1.1f, 1.1f, 1f);

		var exhaledPosition = new Vector3(-0.9f, 0f, 0f);
		var inhaledPosition = new Vector3(-1f, 0f, 0f);

		//Debug.Log(inhaling);
		//Debug.Log(targetScale);
		//Debug.Log(Time.time - time);

		var progress = animationPercent;

		if ((isInhaling && progress < 1f) || (!isInhaling && progress > 0f)) {
			body.oxygenPercent += percentDelta * Mathf.Clamp01(Time.time - flipTime) * 0.5f;
		}

		leftLung.localScale = Vector3.Lerp(exhaledScale, inhaledScale, progress);
		leftLung.localPosition = Vector3.Lerp(exhaledPosition, inhaledPosition, progress);

		rightLung.localScale = Vector3.Lerp(exhaledScale, inhaledScale, progress);
		rightLung.localPosition = Vector3.Lerp(-exhaledPosition, -inhaledPosition, progress);
	}
}
