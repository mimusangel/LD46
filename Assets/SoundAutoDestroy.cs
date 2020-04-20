using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAutoDestroy : MonoBehaviour
{
	AudioSource audioSource;

    void Start()
    {
		audioSource = GetComponent<AudioSource>();
		Destroy(gameObject, audioSource.clip.length + 0.5f);
	}
}
