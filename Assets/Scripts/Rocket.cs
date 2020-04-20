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
				switch (GameSettings.GameDifficulty)
				{
					case GameSettings.Difficulty.Easy:
						sender.AddGold(Random.Range(60, 100));
						break;
					case GameSettings.Difficulty.Normal:
						sender.AddGold(Random.Range(20, 80));
						break;
					case GameSettings.Difficulty.Hard:
						sender.AddGold(Random.Range(0, 100));
						break;
				}
			}
			else
			{
				switch (GameSettings.GameDifficulty)
				{
					case GameSettings.Difficulty.Easy:
						sender.AddGold(Random.Range(10, 30));
						break;
					case GameSettings.Difficulty.Normal:
						sender.AddGold(Random.Range(0, 10));
						break;
					case GameSettings.Difficulty.Hard:
						break;
				}
			}
			Destroy(gameObject);
			return;
		}
		ET et = collision.gameObject.GetComponent<ET>();
		if (et)
		{
			if (et.Hit(Random.Range(10, 21)))
			{
				switch (GameSettings.GameDifficulty)
				{
					case GameSettings.Difficulty.Easy:
						sender.AddGold(Random.Range(100, 160));
						break;
					case GameSettings.Difficulty.Normal:
						sender.AddGold(Random.Range(60, 120));
						break;
					case GameSettings.Difficulty.Hard:
						sender.AddGold(Random.Range(40, 140));
						break;
				}
			}
			Destroy(gameObject);
			return;
		}
	}
}
