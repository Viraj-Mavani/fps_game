using System.Collections;
using UnityEngine;

public class DestroyAfterTimeParticle : MonoBehaviour {
	public float timeToDestroy = 0.8f;

	void Start () {
		Destroy (gameObject, timeToDestroy);
	}

}
