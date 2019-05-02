using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cardio : MonoBehaviour
{
	public Animator animator;
	public Body body;

	protected float lastBeatTime;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F)) {
			animator.SetTrigger("heartBeat");

			if (Time.time - lastBeatTime > 0.3f) {
				body.bloodCirculationPercent = 1f;
			}

			lastBeatTime = Time.time;
		}
	}
}
