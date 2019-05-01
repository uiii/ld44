using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cardio : MonoBehaviour
{
	public Animator animator;
	public Body body;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F)) {
			animator.SetTrigger("heartBeat");

			body.bloodCirculationPercent = 1f;
		}
	}
}
