using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ZLayer : MonoBehaviour
{
	SpriteRenderer spriteRenderer;

	void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();    
    }

    void Update()
    {
		int z = Mathf.RoundToInt(transform.position.y * 100);
		spriteRenderer.sortingOrder = -z;

	}
}
