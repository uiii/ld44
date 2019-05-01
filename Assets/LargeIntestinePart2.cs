using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeIntestinePart2 : MonoBehaviour
{
	public Transform inputStercusMask;
	public Transform outputStercusMask;
	public Transform stercus;
	public LargeIntestinePart2 nextPart;

	public float maxContraction = 0.2f;
	public float squeezeOutSpeed = 9f;

	public float inputStercusVolume;
	public float outputStercusVolume;

	protected Vector3 originalScale;
	protected Vector3 originalStercusMaskScale;

	// Start is called before the first frame update
	void Start()
	{
		originalScale = transform.localScale;
		originalStercusMaskScale = inputStercusMask.localScale;
	}

	// Update is called once per frame
	void Update()
	{
		return;
		float inputMaskScale = Mathf.Clamp01(inputStercusVolume);
		float outputMaskScale = Mathf.Clamp01(outputStercusVolume);

		inputStercusMask.localScale = new Vector3(
			originalStercusMaskScale.x,// * inputStercusVolume,
			originalStercusMaskScale.y * inputMaskScale,
			1f
		);

		outputStercusMask.localScale = new Vector3(
			originalStercusMaskScale.x,// * outputStercusVolume,
			originalStercusMaskScale.y * outputMaskScale,
			1f
		);

		inputStercusMask.localPosition = Vector3.down * stercus.localScale.y * (1 - inputMaskScale) / 2f;
		outputStercusMask.localPosition = Vector3.up * stercus.localScale.y * (1 - outputMaskScale) / 2f;
	}

	public void AddStercus(float volume)
	{
		inputStercusVolume = Mathf.Round((inputStercusVolume + volume) * 100f) / 100f;

		float totalStercusVolume = inputStercusVolume + outputStercusVolume;
		if (totalStercusVolume >= 0.99f) {
			outputStercusVolume += inputStercusVolume;
			inputStercusVolume = 0;
			SqueezeOutStercus(Mathf.Min(totalStercusVolume % 1, volume / 2f));
		}
	}

	public void SqueezeOutStercus(float volume)
	{
		volume = Mathf.Min(volume, outputStercusVolume);
		if (volume > 0) {
			Debug.Log(volume);
		}
		outputStercusVolume = Mathf.Round(Mathf.Max(0, outputStercusVolume - volume) * 100f) / 100f;

		if (nextPart) {
			nextPart.AddStercus(volume);
		}
	}

	public void Contract(float percent = 1f)
	{
		transform.localScale = new Vector3(
			originalScale.x * (1f - maxContraction * percent),
			originalScale.y,
			1f
		);

		Debug.Log(originalScale);
		Debug.Log(1f - maxContraction * percent);
		Debug.Log(transform.localScale);

		//SqueezeOutStercus(percent * squeezeOutSpeed * Time.deltaTime);
	}
}
