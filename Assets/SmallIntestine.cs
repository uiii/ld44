using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallIntestine : MonoBehaviour
{
	public Stomach stomach;
	public LargeIntestine largeIntestine;

	public Animator animator;

	public float stercusPushingSpeed = 0.2f;

	protected bool isPushingStercus;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.D)) {
			if (stomach.digestedBoluses.Length > 0) {
				animator.SetTrigger("digest");

				stomach.Empty();

				isPushingStercus = true;
				largeIntestine.AddStercus();
			}
		}

		if (isPushingStercus) {
			largeIntestine.PushStercus(0, Time.deltaTime * stercusPushingSpeed);
		}
	}

	void DigestingStopped()
	{
		isPushingStercus = false;
	}
}
