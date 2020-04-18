using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Planete : MonoBehaviour, IPointerClickHandler
{
	public List<PlaneteCase> Cases = new List<PlaneteCase>();

	private int gold;
	public TextMeshProUGUI infoGold;

	public int Life { get; private set; }

	private void Start()
	{
		SetLife(Cases.Count + 1);
		gold = Cases.Count * 100;
		if (gold < 200) gold = 200;
		infoGold.text = $"{gold} Gold";
		HideCase();
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
		}
	}

	public bool HasGold(int g)
	{
		return g <= gold;
	}

	public void UseGold(int g)
	{
		gold -= g;
		infoGold.text = $"{gold} Gold";
	}

	public void AddGold(int g)
	{
		gold += g;
		infoGold.text = $"{gold} Gold";
	}

	public void SetLife(int nLife)
	{
		float mLife = Cases.Count + 1;
		Life = nLife;
		float pct = (float)Life / mLife;
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		sr.color = Color.Lerp(Color.red, Color.white, pct);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		SetLife(Life - 1);
		Destroy(collision.gameObject);
		if (Life <= 0)
		{
			Destroy(gameObject);
		}
	}
}
