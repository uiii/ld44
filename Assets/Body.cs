using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Body : MonoBehaviour
{
	public Stomach stomach;
	public Bolus bolusPrefab;

	public GameObject gameOver;
	public TextMeshPro timeText;
	public TextMeshPro gameOverTimeText;

	public TextMeshPro bloodCirculationPercentText;
	public TextMeshPro oxygenPercentText;
	public TextMeshPro waterPercentText;
	public TextMeshPro nutritionPercentText;

	public Transform bloodCirculationBar;
	public Transform oxygenBar;
	public Transform waterBar;
	public Transform nutritionBar;

	public float bloodCirculationPercent;
	public float oxygenPercent;
	public float waterPercent;
	public float nutritionPercent;

	public float bloodCirculationPercentFallSpeed = 0.5f;
	public float oxygenPercentFallSpeed = 0.25f;
	public float waterPercentFallSpeed = 0.1f;
	public float nutritionPercentFallSpeed = 0.05f;

	public float drinkingSpeed = 0.5f;

	void Start()
	{
		bloodCirculationPercent = 1f;
		oxygenPercent = 1f;
		waterPercent = 1f;
		nutritionPercent = 1f;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			return;
		}

		if (gameOver.activeSelf) {
			return;
		}

		Drink();

		FallStats();
		DisplayStats();

		timeText.text = $"time: {Mathf.Round(Time.timeSinceLevelLoad)} s";
	}

	protected void FallStats()
	{
		bloodCirculationPercent = Mathf.Clamp01(bloodCirculationPercent - Time.deltaTime * bloodCirculationPercentFallSpeed);
		oxygenPercent = Mathf.Clamp01(oxygenPercent - Time.deltaTime * oxygenPercentFallSpeed);
		waterPercent = Mathf.Clamp01(waterPercent - Time.deltaTime * waterPercentFallSpeed);
		nutritionPercent = Mathf.Clamp01(nutritionPercent - Time.deltaTime * nutritionPercentFallSpeed);

		if (bloodCirculationPercent == 0 || oxygenPercent == 0 || waterPercent == 0 || nutritionPercent == 0) {
			gameOver.SetActive(true);
			gameOverTimeText.text = $"{Mathf.Round(Time.time)} seconds";
		}
	}

	protected void DisplayStats()
	{
		bloodCirculationPercentText.text = $"{Mathf.Round(bloodCirculationPercent * 100f)}%";
		oxygenPercentText.text = $"{Mathf.Round(oxygenPercent * 100f)}%";
		waterPercentText.text = $"{Mathf.Round(waterPercent * 100f)}%";
		nutritionPercentText.text = $"{Mathf.Round(nutritionPercent * 100f)}%";

		bloodCirculationBar.localScale = new Vector3(bloodCirculationPercent, 1f, 1f);
		oxygenBar.localScale = new Vector3(oxygenPercent, 1f, 1f);
		waterBar.localScale = new Vector3(waterPercent, 1f, 1f);
		nutritionBar.localScale = new Vector3(nutritionPercent, 1f, 1f);
	}

	protected void Drink()
	{
		stomach.waterStream.SetActive(false);

		if (waterPercent < 0.8f && stomach.waterLevel < 1f && stomach.gastricAcidLevel == 0f) {
			stomach.waterStream.SetActive(true);

			float waterVolumeDelta = Time.deltaTime * drinkingSpeed;
			stomach.AddWater(waterVolumeDelta);
		}

		if (nutritionPercent < 0.8f && stomach.gastricAcidLevel == 0f && stomach.boluses.Count < 3) {
			Eat();
		}
	}

	protected void Eat()
	{
		var bolus = Instantiate(bolusPrefab, Vector3.zero, Quaternion.identity);
		stomach.AddBolus(bolus);
	}
}
