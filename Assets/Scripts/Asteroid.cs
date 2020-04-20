using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
	int life;
	public int maxLife { get; private set; }

	public bool Big = false;
	Rigidbody2D rg2d;
	public Vector2 dir;
	public float vel;

	List<Vector3> points = new List<Vector3>();

	void Start()
    {
		maxLife = Random.Range(15, 31);
		if (Big)
		{
			maxLife *= 4;
			transform.localScale = Vector3.one * 2;
		}
		life = maxLife;
		rg2d = GetComponent<Rigidbody2D>();
		vel = Random.Range(8.0f, 12.0f) + GameManager.Instance.GameTime / 30.0f;
		rg2d.velocity = dir * vel;
		rg2d.AddTorque(Random.Range(-10.0f, 10.0f), ForceMode2D.Impulse);
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
		Vector2 solarDir = (-(Vector2)transform.position).normalized;
		rg2d.velocity = (rg2d.velocity + solarDir * 3.0f * Time.deltaTime).normalized * vel;
		points.Add(transform.position);
	}

	private void OnDrawGizmos()
	{
		for (int i = 0; i < points.Count - 1; i++)
		{
			Gizmos.DrawLine(points[i], points[i + 1]);
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
			GameManager.Instance.AddScore(maxLife);
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
