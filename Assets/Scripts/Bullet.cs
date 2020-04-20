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
				switch (GameSettings.GameDifficulty)
				{
					case GameSettings.Difficulty.Easy:
						sender.AddGold(Random.Range(80, 120));
						break;
					case GameSettings.Difficulty.Normal:
						sender.AddGold(Random.Range(40, 100));
						break;
					case GameSettings.Difficulty.Hard:
						sender.AddGold(Random.Range(20, 120));
						break;
				}
			}
			else
			{
				switch (GameSettings.GameDifficulty)
				{
					case GameSettings.Difficulty.Easy:
						sender.AddGold(Random.Range(20, 40));
						break;
					case GameSettings.Difficulty.Normal:
						sender.AddGold(Random.Range(0, 20));
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

			if (et.Hit(bulletDamage))
			{
				switch (GameSettings.GameDifficulty)
				{
					case GameSettings.Difficulty.Easy:
						sender.AddGold(Random.Range(120, 160));
						break;
					case GameSettings.Difficulty.Normal:
						sender.AddGold(Random.Range(80, 140));
						break;
					case GameSettings.Difficulty.Hard:
						sender.AddGold(Random.Range(60, 160));
						break;
				}
			}
			Destroy(gameObject);
			return;
		}
	}
}
