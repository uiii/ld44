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
	public float stercusMovementSpeed = 0.3f;

	public LargeIntestinePart[] parts;

	public Stercus[] stercuses
	{
		get => GetComponentsInChildren<Stercus>();
	}

	protected bool isContracting {
		get => contractionPosition < parts.Length + contractionRange;
	}

	void Start()
	{
		parts = GetComponentsInChildren<LargeIntestinePart>();
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

			for (int i = 0; i < parts.Length; ++i) {
				var part = parts[i];

				var contractionWeight = 1 - Mathf.Clamp01(Mathf.Abs(contractionPosition - i) / contractionRange);
				part.Contract(contractionWeight);
			}
			foreach (var stercus in stercuses) {
				stercus.position += Time.deltaTime * stercusMovementSpeed;
			}

			body.nutritionPercent += Time.deltaTime * 0.04f;
		}
	}
}
