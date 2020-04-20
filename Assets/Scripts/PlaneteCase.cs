using UnityEngine;
using UnityEngine.EventSystems;

public class PlaneteCase : MonoBehaviour, IPointerClickHandler
{
	private SpriteRenderer spriteRenderer;
	public SpriteRenderer spriteRendererTop;

	public Planete planete;
	public CaseType type = CaseType.None;

	public LayerMask LayerMask;
	public int level = 0;
	private float targetDistance = 0;
	public float TargetDistance
	{
		get
		{
			if (type == CaseType.Cannon || type == CaseType.Rocket)
			{
				return targetDistance + planete.SatelliteCount * 5.0f;
			}
			return targetDistance;
		}
	}
	private float fireRate = 0.0f;
	public float FireRate
	{
		get
		{
			if (type == CaseType.DoubleCannon)
			{
				return fireRate - planete.SatelliteCount * 0.05f;
			}
			return fireRate;
		}
	}
	public float fireTime = 0.0f;

	private GameObject UniqueProjectile;
	private bool UniqueProjectileFired = false;

	public float ShieldStatus { get; private set; } = 1.0f;

	public bool swap = false;

	public bool HasShield
	{
		get
		{
			return type == CaseType.Shield && ShieldStatus >= 1.0f;
		}
	}

	public void UseShield()
	{
		if (ShieldStatus >= 1.0f)
		{
			ShieldStatus -= 1.0f;
		}
	}

	private void Awake()
	{
		if (planete)
		{
			planete.Cases.Add(this);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		SetType(type);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerCurrentRaycast.gameObject == this.gameObject)
		{
			if (CameraController.Instance.IsFocus(planete))
			{
				PanelCase.Instance.Show(this);
			}
			else
			{
				CameraController.Instance.SetFocus(planete);
			}
		}
	}

	private void Update()
	{
		if (Time.timeScale <= 0) return;
		if (type != CaseType.None)
		{
			if (type == CaseType.Shield)
			{
				float speed = 0.001f * (10.0f * (planete.SatelliteCount + 1.0f));
				ShieldStatus = Mathf.Min(ShieldStatus + Time.deltaTime * speed, 1.0f);
			}
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, TargetDistance, LayerMask.value);
			Collider2D focus = null;
			fireTime -= Time.deltaTime;
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject == gameObject) continue;
				if (focus == null)
				{
					focus = colliders[i];
					continue;
				}
				float a = (focus.gameObject.transform.position - transform.position).sqrMagnitude;
				float b = (colliders[i].gameObject.transform.position - transform.position).sqrMagnitude;
				if (b < a)
				{
					focus = colliders[i];
				}
			}
			if (focus != null)
			{
				Vector2 target = focus.gameObject.transform.position;
				Rigidbody2D rg2d = focus.gameObject.GetComponent<Rigidbody2D>();
				if (rg2d)
				{
					target = (Vector2)target + rg2d.velocity * Random.Range(0.5f, 1.1f);
				}

				transform.up = (target - (Vector2)transform.position).normalized;

				if (fireTime <= 0.0f)
				{
					Vector3 pos = transform.position + transform.up;
					GameObject go;
					Bullet bullet;
					switch (type)
					{
						case CaseType.Cannon:
							go = Instantiate(GameManager.Instance.BulletPrefab, pos, Quaternion.identity);
							go.GetComponent<Rigidbody2D>().velocity = transform.up * 15.0f;
							bullet = go.GetComponent<Bullet>();
							bullet.sender = planete;
							bullet.bulletDamage = 5;
							Instantiate(GameManager.Instance.LaserSound, pos, Quaternion.identity);
							break;
						case CaseType.DoubleCannon:
							if (swap)
								pos += transform.right * 0.25f;
							else
								pos -= transform.right * 0.25f;
							swap = !swap;
							go = Instantiate(GameManager.Instance.BulletPrefab, pos, Quaternion.identity);
							go.GetComponent<Rigidbody2D>().velocity = transform.up * 15.0f;
							bullet = go.GetComponent<Bullet>();
							bullet.sender = planete;
							bullet.bulletDamage = 1;
							Instantiate(GameManager.Instance.LaserSound, pos, Quaternion.identity);
							break;
						case CaseType.Rocket:
							if (!UniqueProjectile && UniqueProjectileFired)
							{
								spriteRendererTop.sprite = GameManager.Instance.Rocket;
								UniqueProjectileFired = false;
							}
							else
							{
								if (!UniqueProjectileFired)
								{
									go = Instantiate(GameManager.Instance.RocketPrefab, pos, Quaternion.identity);
									Rocket rocket = go.GetComponent<Rocket>();
									rocket.target = focus.transform;
									rocket.autofocus = level >= 2;
									rocket.sender = planete;
									UniqueProjectile = go;
									UniqueProjectileFired = true;
									spriteRendererTop.sprite = null;
									Instantiate(GameManager.Instance.RocketSound, pos, Quaternion.identity);
								}
							}
							break;
					}
				}
			}
			if (fireTime <= 0.0f)
			{
				fireTime += FireRate;
			}
		}
		else
		{
			transform.up = Vector2.up;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, TargetDistance);
	}

	public void SetType(CaseType nType)
	{
		type = nType;
		switch (type)
		{
			case CaseType.Cannon:
				level = 1;
				spriteRenderer.sprite = GameManager.Instance.WeaponBase;
				UpgradeWeapon();
				break;
			case CaseType.Rocket:
				level = 1;
				spriteRenderer.sprite = GameManager.Instance.RocketLauncher;
				UpgradeWeapon();
				break;
			case CaseType.Shield:
				level = 1;
				spriteRenderer.sprite = GameManager.Instance.WeaponBase;
				Solar.Instance.shields.Add(this);
				UpgradeWeapon();
				break;
			case CaseType.DoubleCannon:
				level = 1;
				spriteRenderer.sprite = GameManager.Instance.WeaponBase;
				UpgradeWeapon();
				break;
			case CaseType.Satellite:
				level = 1;
				spriteRenderer.sprite = GameManager.Instance.Antenna;
				UpgradeWeapon();
				break;
			case CaseType.None:
				Solar.Instance.shields.Remove(this);
				level = 0;
				targetDistance = 0.0f;
				spriteRenderer.sprite = GameManager.Instance.CaseSprite;
				spriteRendererTop.sprite = GameManager.Instance.CaseSprite;
				fireRate = 0.0f;
				break;
		}
	}

	public void UpgradeWeapon()
	{
		switch (type)
		{
			case CaseType.Cannon:
				if (level == 2)
				{
					targetDistance = 35.0f;
					spriteRendererTop.sprite = GameManager.Instance.CannonLongSprite;
					fireRate = 0.4f;
				}
				else
				{
					targetDistance = 25.0f;
					spriteRendererTop.sprite = GameManager.Instance.CannonSprite;
					fireRate = 0.5f;
				}
				break;
			case CaseType.Rocket:
				if (level == 2)
				{
					targetDistance = 40.0f;
					spriteRendererTop.sprite = UniqueProjectile ? null : GameManager.Instance.Rocket;
					fireRate = 1.0f;
				}
				else
				{
					targetDistance = 30.0f;
					spriteRendererTop.sprite = UniqueProjectile ? null : GameManager.Instance.Rocket;
					fireRate = 1.0f;
				}
				break;
			case CaseType.Shield:
				targetDistance = 30.0f;
				spriteRendererTop.sprite = GameManager.Instance.Shield;
				fireRate = 1.0f;
				break;
			case CaseType.DoubleCannon:
				if (level == 2)
				{
					targetDistance = 30.0f;
					spriteRendererTop.sprite = GameManager.Instance.DoubleCannonLongSprite;
					fireRate = 0.2f;
				}
				else
				{
					targetDistance = 20.0f;
					spriteRendererTop.sprite = GameManager.Instance.DoubleCannonSprite;
					fireRate = 0.25f;
				}
				break;
			case CaseType.Satellite:
				targetDistance = 0.0f;
				spriteRendererTop.sprite = null;
				fireRate = 0.0f;
				break;
		}
	}

	public int CaseScore()
	{
		int score = 0;
		switch (type)
		{
			case CaseType.Cannon:
				score = level * 200;
				break;
			case CaseType.Rocket:
				score = level * 400;
				break;
			case CaseType.Shield:
				score = level * 500;
				break;
			case CaseType.DoubleCannon:
				score = level * 300;
				break;
			case CaseType.Satellite:
				score = level * 600;
				break;
		}
		return score;
	}
}
