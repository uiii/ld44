using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stomach : MonoBehaviour
{
	public Body body;

	public Transform waterMask;
	public Transform gastricAcidMask;
	public GameObject waterStream;

	public Animator animator;

	public float fillingSpeed = 0.6f;
	public float emptyingSpeed = 0.8f;
	public float digestingSpeed = 0.1f;

	public Transform bolusSpawnPoint;

	public float waterLevel = 0f;
	public float gastricAcidLevel = 0f;
	public List<Bolus> boluses = new List<Bolus>();

	protected bool isDigesting = false;
	protected bool isEmptying = false;

	public Bolus[] digestedBoluses
	{
		get => boluses.Where(b => b.digested).ToArray();
	}

	public void AddWater(float volume)
	{
		waterLevel = Mathf.Clamp01(waterLevel + volume);
	}

	public void AddBolus(Bolus bolus)
	{
		boluses.Add(bolus);
		bolus.transform.position = bolusSpawnPoint.position;
	}

	public void Empty()
	{
		isEmptying = true;
	}

	void Update()
	{
		if (isEmptying) {
			gastricAcidLevel = Mathf.Clamp01(gastricAcidLevel - Time.deltaTime * emptyingSpeed);
			body.nutritionPercent += Time.deltaTime * emptyingSpeed * 0.1f;

			if (gastricAcidLevel == 0) {
				isEmptying = false;

				foreach (var bolus in boluses.Where(b => b.digested).ToList()) {
					boluses.Remove(bolus);
					Destroy(bolus.gameObject);
				}
			}
		} else {
			if (Input.GetKeyDown(KeyCode.K)) {
				if (gastricAcidLevel == 1f) {
					animator.SetTrigger("churn");
				}

				isDigesting = true;
			}

			Digest();
		}

		waterMask.localPosition = new Vector3(
			0,
			-1.15f + 0.6f * waterLevel,
			0f
		);

		gastricAcidMask.localPosition = new Vector3(
			0,
			-1.15f + 0.6f * gastricAcidLevel,
			0f
		);
	}

	void Digest()
	{
		if (! isDigesting) {
			return;
		}

		isDigesting = false;

		if (waterLevel > 0f) {
			waterLevel = Mathf.Clamp01(waterLevel - Time.deltaTime * emptyingSpeed);
			body.waterPercent += Time.deltaTime * emptyingSpeed * 0.5f;
			isDigesting = waterLevel > 0f;
		}

		if (boluses.Count > 0) {
			if (gastricAcidLevel < 1f) {
				gastricAcidLevel = Mathf.Clamp01(gastricAcidLevel + Time.deltaTime * fillingSpeed);

				isDigesting = gastricAcidLevel < 1f;
			} else {
				foreach (var bolus in boluses) {
					bolus.Digest(Time.deltaTime * digestingSpeed);
				}

				isDigesting = true;
			}
		}
	}

	void ChurningStopped()
	{
		isDigesting = false;
	}
}
