using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public Planete sender;

	public int bulletDamage = 1;

	private void Start()
	{
		Destroy(gameObject, 5.0f);		
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Asteroid aste = collision.gameObject.GetComponent<Asteroid>();
		if (aste)
		{
			if (aste.Hit(bulletDamage))
			{
				sender.AddGold(Random.Range(40, 100));
			}
			else
			{
				sender.AddGold(Random.Range(20, 40));
			}
			Destroy(gameObject);
		}
	}
}
