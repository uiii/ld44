using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LargeIntestine : MonoBehaviour
{
	public Body body;
	public Transform start;
	public Transform exit;

	public Stercus stercusPrefab;

	public float contractionPosition = float.MaxValue;
	public float contractionRange = 2f;
	public float contractionSpeed = 1f;
	public float maxContraction = 0.2f;
	public float stercusPosition = 0;
	public float stercusLength = 3;
	public float stercusMovementSpeed = 0.3f;

	protected LargeIntestinePart[] partsObsolete;
	public LargeIntestinePart2[] parts;
	public float[] stercusPositions = new float[] {0, 15};

	public Stercus[] stercuses
	{
		get => GetComponentsInChildren<Stercus>();
	}

	protected bool isContracting {
		get => contractionPosition < parts.Length + contractionRange;
	}

	void Start()
	{
		parts = GetComponentsInChildren<LargeIntestinePart2>();

		for (int i = 0; i < parts.Length - 1; ++i) {
			parts[i].nextPart = parts[i + 1];
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.S)) {
			if (! isContracting && stercuses.Length > 0) {
				contractionPosition = -contractionRange;
			}
		}

		Contract();
	}

	public void AddStercus()
	{
		var stercus = Instantiate(stercusPrefab, start.position, Quaternion.identity, transform);
		stercus.largeIntestine = this;
		stercus.position = -1f;
		stercus.transform.SetAsFirstSibling();
	}

	public void PushStercus(int index, float amount)
	{
		if (index < stercuses.Length - 1 && stercuses[index + 1].position - stercuses[index].position <= 1f) {
			PushStercus(index + 1, amount);
		}

		stercuses[index].position += amount;
	}

	protected void Contract()
	{
		if (isContracting) {
			contractionPosition += Time.deltaTime * contractionSpeed;
			/*if (contractionPosition >= 0 && contractionPosition < parts.Length) {
				Debug.Log((int) contractionPosition);
				parts[(int) contractionPosition].Contract();
				parts[(int) contractionPosition].SqueezeOutStercus(Time.deltaTime * stercusMovementSpeed);
			}*/

			for (int i = 0; i < parts.Length; ++i) {
				var part = parts[i];

				var contractionWeight = 1 - Mathf.Clamp01(Mathf.Abs(contractionPosition - i) / contractionRange);
				part.Contract(contractionWeight);
			}
			foreach (var stercus in stercuses) {
				stercus.position += Time.deltaTime * stercusMovementSpeed;
			}

			body.nutritionPercent += Time.deltaTime * 0.04f;

			//foreach (var stercus in stercuses.Where(s => s.transform.position == parts[parts.Length - 1].transform.position).ToList()) {
				//Destroy(stercus.gameObject);
			//}
		}

		return;

		for (int i = 0; i < partsObsolete.Length; ++i) {
			var part = partsObsolete[i];

			var scaleWieght = 1 - Mathf.Clamp01(Mathf.Abs(contractionPosition - i) / contractionRange);

			//part.SetStercusPosition(1 - Mathf.Clamp01(i - (stercusPosition - stercusLength / 2f)));
			//part.SetStercusPosition(Mathf.Clamp01(stercusPosition - i) + 0.5f);

			/*float ss = Mathf.Clamp01(stercusPosition - i - stercusLength / 2f + 0.5f);
			float se = Mathf.Clamp01(stercusPosition - i + stercusLength / 2f + 0.5f);
			float sp = (se + ss) / 2f;

			part.SetStercusPosition(sp);*/
			part.Scale(new Vector3(1f - maxContraction * scaleWieght, 1f, 1f));
		}

		MoveStercus();
	}

	protected void MoveStercus()
	{
		foreach (var part in partsObsolete) {
			part.SetStercusPosition(0);
		}

		for (int i = 0; i < stercusPositions.Length; ++i) {
			if (isContracting && Mathf.Abs(contractionPosition - stercusPositions[i]) <= stercusLength / 2f) {
				stercusPositions[i] += Time.deltaTime * stercusMovementSpeed;
			}

			int startPartIndex = Mathf.FloorToInt(Mathf.Max(0, stercusPositions[i] - stercusLength / 2f + 0.5f));
			int endPartIndex = Mathf.CeilToInt(Mathf.Min(partsObsolete.Length - 1, stercusPositions[i] + stercusLength / 2f + 0.5f));

			for (int j = startPartIndex; j < endPartIndex + 1; ++j) {
				float ss = Mathf.Clamp01(stercusPositions[i] - j - stercusLength / 2f + 0.5f);
				float se = Mathf.Clamp01(stercusPositions[i] - j + stercusLength / 2f + 0.5f);
				float sp = (se + ss) / 2f;

				partsObsolete[j].SetStercusPosition(sp);
			}
		}
	}
}
