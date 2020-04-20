using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ET : MonoBehaviour
{
	int life;
	public int maxLife { get; private set; }

	public Planete focus;
	Rigidbody2D rg2d;
	public float vel;
	
	float missionTime = 0.0f;
	Vector3 startPoint;

	public TextMeshProUGUI textMission;

	private void Start()
	{
		maxLife = Random.Range(400, 501);
		life = maxLife;
		rg2d = GetComponent<Rigidbody2D>();
		vel = Random.Range(12.0f, 14.0f) + GameManager.Instance.GameTime / 60.0f;
		startPoint = transform.position;
		textMission.text = "";
	}

	public void Update()
	{
		if (focus != null)
		{
			rg2d.velocity = ((focus.transform.position + Vector3.up * 3.5f) - transform.position).normalized * vel;
			if (Vector3.Distance(transform.position, focus.transform.position) < 4.0f)
			{
				missionTime += Time.deltaTime;
				textMission.text = $"{Mathf.RoundToInt((missionTime / 10.0f) * 100.0f)}%";
				transform.position = focus.transform.position + Vector3.up * 3.5f;
				rg2d.velocity = Vector3.zero;
				if (missionTime >= 10.0f)
				{
					focus.GetComponent<Rotate>()?.ET_TP();
					focus = null;
				}
			}
		}
		else
		{
			textMission.text = "";
			rg2d.velocity = (startPoint - transform.position).normalized * vel;
			if (Vector3.Distance(startPoint, transform.position) < 10.0f)
			{
				Destroy(gameObject);
			}
		}

	}

	public bool Hit(int dmg = 1)
	{
		life -= dmg;
		if (life <= 0)
		{
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
