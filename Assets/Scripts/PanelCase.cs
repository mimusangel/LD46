using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelCase : MonoBehaviour
{
	public static PanelCase Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(Instance);
		}
		Instance = this;
	}

	public PlaneteCase planeteCase = null;

	public GameObject buyMenu;
	public GameObject infoMenu;

	public Image infoImage;
	public TextMeshProUGUI infoTitle;
	public TextMeshProUGUI infoDistance;
	public TextMeshProUGUI infoFireRate;
	public TextMeshProUGUI infoSell;
	public GameObject upgradeButton;
	public TextMeshProUGUI indoUpgrade;


	void Start()
    {
		Hide();

	}

	public bool IsVisible
	{
		get
		{
			return gameObject.activeSelf;
		}
	}

	public void Show(PlaneteCase c)
	{
		planeteCase = c;
		gameObject.SetActive(true);
		if (planeteCase.type == CaseType.None)
		{
			buyMenu.SetActive(true);
			infoMenu.SetActive(false);
		}
		else
		{
			ShowInfo();
			buyMenu.SetActive(false);
			infoMenu.SetActive(true);
		}
	}

	public void ShowInfo()
	{
		int rate;
		switch (planeteCase.type)
		{
			case CaseType.Cannon:
				infoImage.sprite = planeteCase.spriteRendererTop.sprite;
				infoTitle.text = $"Cannon lvl.{planeteCase.level}";
				infoDistance.text = $"Distance {planeteCase.targetDistance}";
				rate = Mathf.RoundToInt(1 / planeteCase.fireRate);
				if (rate <= 0)
					infoFireRate.text = $"Fire Rate < 1 per sec";
				else
					infoFireRate.text = $"Fire Rate {rate} per sec";
				infoSell.text = $"{planeteCase.level * 100} Gold";
				indoUpgrade.text = $"{(planeteCase.level + 1) * 200} Gold";
				upgradeButton.SetActive(planeteCase.level < 2);
				break;
			case CaseType.Rocket:
				infoImage.sprite = planeteCase.spriteRendererTop.sprite;
				infoTitle.text = $"Rocket Launcher lvl.{planeteCase.level}";
				infoDistance.text = $"Distance {planeteCase.targetDistance}";
				infoFireRate.text = $"Fire Rate: One By One";
				infoSell.text = $"{planeteCase.level * 200} Gold";
				indoUpgrade.text = $"{(planeteCase.level + 1) * 400} Gold";
				upgradeButton.SetActive(planeteCase.level < 2);
				break;
			case CaseType.Shield:
				infoImage.sprite = planeteCase.spriteRendererTop.sprite;
				infoTitle.text = $"Solar Shield";
				infoDistance.text = $"Charge {Mathf.RoundToInt(planeteCase.ShieldStatus * 100.0f)}%";
				infoFireRate.text = $"";
				infoSell.text = $"{planeteCase.level * 250} Gold";
				upgradeButton.SetActive(false);
				break;
			case CaseType.DoubleCannon:
				infoImage.sprite = planeteCase.spriteRendererTop.sprite;
				infoTitle.text = $"Double Cannon lvl.{planeteCase.level}";
				infoDistance.text = $"Distance {planeteCase.targetDistance}";
				rate = Mathf.RoundToInt(1 / planeteCase.fireRate);
				if (rate <= 0)
					infoFireRate.text = $"Fire Rate < 1 per sec";
				else
					infoFireRate.text = $"Fire Rate {rate} per sec";
				infoSell.text = $"{planeteCase.level * 150} Gold";
				indoUpgrade.text = $"{(planeteCase.level + 1) * 300} Gold";
				upgradeButton.SetActive(planeteCase.level < 2);
				break;
		}
	}

	public void Hide()
	{
		planeteCase = null;
		gameObject.SetActive(false);
	}

	public void BtnBuy(int type)
	{
		if (planeteCase)
		{
			CaseType t = (CaseType)type;

			switch (t)
			{
				case CaseType.Cannon:
					if (planeteCase.planete.HasGold(200))
					{
						planeteCase.planete.UseGold(200);
						buyMenu.SetActive(false);
						planeteCase.SetType(t);
						ShowInfo();
						infoMenu.SetActive(true);
					}
					break;
				case CaseType.Rocket:
					if (planeteCase.planete.HasGold(400))
					{
						planeteCase.planete.UseGold(400);
						buyMenu.SetActive(false);
						planeteCase.SetType(t);
						ShowInfo();
						infoMenu.SetActive(true);
					}
					break;
				case CaseType.Shield:
					if (planeteCase.planete.HasGold(500))
					{
						planeteCase.planete.UseGold(500);
						buyMenu.SetActive(false);
						planeteCase.SetType(t);
						ShowInfo();
						infoMenu.SetActive(true);
					}
					break;
				case CaseType.DoubleCannon:
					if (planeteCase.planete.HasGold(300))
					{
						planeteCase.planete.UseGold(300);
						buyMenu.SetActive(false);
						planeteCase.SetType(t);
						ShowInfo();
						infoMenu.SetActive(true);
					}
					break;
			}
			
		}
	}

	public void BtnUpgrade()
	{
		int price = 0;
		switch (planeteCase.type)
		{
			case CaseType.Cannon:
				price = (planeteCase.level + 1) * 200;
				break;
			case CaseType.Rocket:
				price = (planeteCase.level + 1) * 400;
				break;
			case CaseType.Shield:
				price = (planeteCase.level + 1) * 500;
				break;
			case CaseType.DoubleCannon:
				price = (planeteCase.level + 1) * 300;
				break;
		}
		if (price > 0)
		{
			if (planeteCase.planete.HasGold(price))
			{
				planeteCase.planete.UseGold(price);
				planeteCase.level++;
				planeteCase.UpgradeWeapon();
			}
		}
		ShowInfo();
	}

	public void BtnDestroy()
	{
		switch (planeteCase.type)
		{
			case CaseType.Cannon:
				planeteCase.planete.AddGold(planeteCase.level * 100);
				break;
			case CaseType.Rocket:
				planeteCase.planete.AddGold(planeteCase.level * 200);
				break;
			case CaseType.Shield:
				planeteCase.planete.AddGold(planeteCase.level * 250);
				break;
			case CaseType.DoubleCannon:
				planeteCase.planete.AddGold(planeteCase.level * 150);
				break;
		}
		infoMenu.SetActive(false);
		planeteCase.SetType(CaseType.None);
		buyMenu.SetActive(true);
	}
}
