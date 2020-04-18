using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
	public Planete sender;

	public Transform target;
	public bool autofocus = false;


	Rigidbody2D rg2d;

	private void Start()
	{
		Destroy(gameObject, 10.0f);
		rg2d = GetComponent<Rigidbody2D>();
		rg2d.velocity = (target.position - transform.position).normalized * 12.5f;
	}

	private void Update()
	{
		if (autofocus && target != null)
		{
			rg2d.velocity = (target.position - transform.position).normalized * 15f;
		}
		transform.up = rg2d.velocity;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Asteroid aste = collision.gameObject.GetComponent<Asteroid>();
		if (aste)
		{
			if (aste.Hit(Random.Range(10, 21)))
			{
				sender.AddGold(Random.Range(20, 80));
			}
			else
			{
				sender.AddGold(Random.Range(0, 20));
			}
			Destroy(gameObject);
		}
	}
}
