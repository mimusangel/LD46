using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Planete : MonoBehaviour, IPointerClickHandler
{
	public static List<Planete> Planetes = new List<Planete>();
	public List<PlaneteCase> Cases = new List<PlaneteCase>();

	private int gold;
	public TextMeshProUGUI infoGold;

	public GameObject Satellite;
	public int SatelliteCount = 0;

	public int Life { get; private set; }
	public bool isMoon = false;
	public Planete planete = null;

	CircleCollider2D circleCollider2D;

	public SpriteRenderer crack;

	public bool HasShield
	{
		get
		{
			foreach (PlaneteCase c in Cases)
			{
				if (c.type == CaseType.Shield)
				{
					return true;
				}
			}
			return false;
		}
	}

	private void Start()
	{
		Planetes.Add(this);
		SetLife(Cases.Count + 1);
		if (isMoon)
		{
			if (GameSettings.MoneyByDifficulty)
			{
				switch (GameSettings.GameDifficulty)
				{
					case GameSettings.Difficulty.Easy:
						AddGold(300);
						break;
					case GameSettings.Difficulty.Normal:
						AddGold(200);
						break;
					case GameSettings.Difficulty.Hard:
						AddGold(200);
						break;
				}
			}
			else
			{
				AddGold(200);
			}
			infoGold.gameObject.SetActive(false);
		}
		else
		{
			if (GameSettings.MoneyByDifficulty)
			{
				switch (GameSettings.GameDifficulty)
				{
					case GameSettings.Difficulty.Easy:
						AddGold(Cases.Count * 150);
						break;
					case GameSettings.Difficulty.Normal:
						AddGold(Cases.Count * 100);
						break;
					case GameSettings.Difficulty.Hard:
						int a = Cases.Count * 75;
						if (a < 200) a = 200;
						AddGold(a);
						break;
				}
			}
			else
			{
				AddGold(Cases.Count * 100);
			}
		}
		HideCase();
		Satellite.SetActive(false);
		circleCollider2D = GetComponent<CircleCollider2D>();
	}

	private void OnDestroy()
	{
		Planetes.Remove(this);
	}

	private void Update()
	{
		bool hasSatellite = false;
		SatelliteCount = 0;
		foreach (PlaneteCase c in Cases)
		{
			if (c.type == CaseType.Satellite)
			{
				hasSatellite = true;
				SatelliteCount++;
			}
		}
		Satellite.SetActive(hasSatellite);
		if (hasSatellite)
		{
			Satellite.transform.position = transform.position - transform.position.normalized * circleCollider2D.radius;
		}
	}

	public void NoMoon()
	{
		isMoon = false;
		planete = null;
		infoGold.gameObject.SetActive(true);
		UpdateGold();
	}

	public void ShowCase()
	{
		foreach (PlaneteCase c in Cases)
		{
			if (c.type != CaseType.None)
			{
				continue;
			}
			c.gameObject.SetActive(true);
		}
	}

	public void HideCase()
	{
		foreach (PlaneteCase c in Cases)
		{
			if (c.type != CaseType.None)
			{
				continue;
			}
			c.gameObject.SetActive(false);
		}
	}
	
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerCurrentRaycast.gameObject == this.gameObject)
		{
			CameraController.Instance.SetFocus(this);
			transform.localScale = Vector3.one;
		}
	}

	public bool HasGold(int g)
	{
		if (isMoon)
		{
			return planete.HasGold(g);
		}
		return g <= gold;
	}

	public void UseGold(int g)
	{
		if (isMoon)
		{
			planete.UseGold(g);
			return;
		}
		gold -= g;
		UpdateGold();
	}

	public void AddGold(int g)
	{
		if (isMoon)
		{
			planete.AddGold(g);
			return;
		}
		gold += g;
		UpdateGold();
	}

	public void UpdateGold()
	{
		if (isMoon)
		{
			infoGold.text = $"{planete.gold} Gold";
			return;
		}
		infoGold.text = $"{gold} Gold";
	}

	public void SetLife(int nLife)
	{
		float mLife = Cases.Count + 1;
		Life = Mathf.Max(nLife, 0);
		float pct = (float)Life / mLife;
		int v = Mathf.RoundToInt((1.0f - pct) * GameManager.Instance.Cracks.Count);
		if (v > 0)
		{
			crack.gameObject.SetActive(true);
			crack.sprite = GameManager.Instance.Cracks[v - 1];
		}
		else
		{
			crack.gameObject.SetActive(false);
		}
	}

	public void Dead(bool instante)
	{
		if (instante)
		{
			GameManager.Instance.AddScore(GetScore());
			Destroy(gameObject);
		}
		else
		{
			for (int i = 0; i < Cases.Count; i++)
			{
				Destroy(Cases[i].gameObject);
			}
			Cases.Clear();
			Destroy(circleCollider2D);
			StartCoroutine(DeadAnim());
		}
	}

	IEnumerator DeadAnim()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		for (int i = 0; i < 10; i++)
		{
			sr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - (i / 10.0f));
			yield return new WaitForSeconds(0.1f);
		}
		yield return null;
		Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Asteroid ast = collision.gameObject.GetComponent<Asteroid>();
		if (ast && ast.Big)
		{
			SetLife(Life - Random.Range(2, 5));
		}
		else
		{
			SetLife(Life - 1);
		}
		Destroy(collision.gameObject);
		if (Life <= 0)
		{
			Dead(false);
		}
	}

	public int GetScore()
	{
		int score = 0;

		if (Cases.Count <= 0)
		{
			return Random.Range(500, 1501) + gold;
		}

		for (int i = 0; i < Cases.Count; i++)
		{
			score += Cases[i].CaseScore();
		}

		return score + gold;
	}

	private void OnMouseEnter()
	{
		if (Pause.Instance.IsVisible) return;

		if (CameraController.Instance.IsFocus(null))
		{
			transform.localScale = Vector3.one * 1.2f;
		}
	}

	private void OnMouseExit()
	{
		transform.localScale = Vector3.one;
	}
}
