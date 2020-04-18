using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
	int life;
	int maxLife;

	public bool Big = false;

    void Start()
    {
		maxLife = Random.Range(10, 21);
		if (Big)
		{
			maxLife *= 2;
			transform.localScale = Vector3.one * 2;
		}
		life = maxLife;
	}

	private void Update()
	{
		if (Solar.Instance.End)
		{
			Destroy(gameObject);
		}
		if (transform.position.sqrMagnitude > 300 * 300)
		{
			Destroy(gameObject);
		}
	}

	public bool Hit(int dmg = 1)
	{
		life -= dmg;
		if (life <= 0)
		{
			if (Big)
			{
				int count = Random.Range(1, 4);
				for (int i = 0; i < count; i++)
				{
					Vector2 offset = new Vector2(
						Random.Range(-1.0f, 1.0f),
						Random.Range(-1.0f, 1.0f)
					);
					GameManager.Instance.SpawnSmallAsteroid((Vector2)transform.position + offset);
				}
			}
			Destroy(gameObject);
			return true;
		}
		else
		{
			SpriteRenderer sr = GetComponent<SpriteRenderer>();
			float pct = (float)life / (float)maxLife;
			sr.color = Color.Lerp(Color.red, Color.white, pct);
		}
		return false;
	}
}
