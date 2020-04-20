using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solar : MonoBehaviour
{
	public static Solar Instance { get; private set; }

	float scale = 1.0f;
	public LayerMask layer;

	private SpriteRenderer spriteRenderer;
	public List<Sprite> animationFrame = new List<Sprite>();
	private int frame = 0;
	private float timeFrame = 0;

	public List<PlaneteCase> shields = new List<PlaneteCase>();
	public GameObject Shield;


	public bool End = false;
	public float timer;
	public AnimationCurve endCurve;


	float absorbTime = 0.0f;

	public AudioSource AudioCritique;
	public AudioSource AudioEnd;

	public GameObject WarningUI;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(Instance.gameObject);
		}
		Instance = this;
	}

	void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
		WarningUI.SetActive(false);
	}

	private void Update()
	{
		if (!End)
		{
			absorbTime = Mathf.Max(absorbTime - Time.deltaTime, 0.0f);
			bool hasShieldActif = false;
			for (int j = 0; j < shields.Count; j++)
			{
				if (shields[j].HasShield)
				{
					hasShieldActif = true;
					break;
				}
			}
			Shield.SetActive(hasShieldActif);
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, (scale + 1.0f * absorbTime), layer.value);
			for (int i = 0; i < colliders.Length; i++)
			{
				int dmg = 1;
				if (colliders[i].gameObject == this.gameObject) continue;
				Planete pl = colliders[i].gameObject.GetComponent<Planete>();
				if (pl)
				{
					bool isPinky = colliders[i].gameObject.GetComponent<Pinky>() != null;
					if (isPinky)
					{
						GameManager.Instance.SpawnPlanete();
					}
					pl.Dead(true);
				}
				Asteroid at = colliders[i].gameObject.GetComponent<Asteroid>();
				if (at)
				{
					GameManager.Instance.AddScore(-at.maxLife);
					if (at.Big)
					{
						dmg++;
					}
				}
				Destroy(colliders[i].gameObject);
				if (hasShieldActif)
				{
					for (int j = 0; j < shields.Count; j++)
					{
						if (shields[j].HasShield)
						{
							shields[j].UseShield();
							dmg--;
							if (dmg <= 0) break;
						}
					}
				}
				if (dmg > 0)
				{
					absorbTime += dmg;
					scale += dmg;
					if (scale >= 20)
					{
						WarningUI.SetActive(true);
						if (AudioCritique.isPlaying == false)
						{
							AudioCritique.Play();
						}
						float pct = (scale - 20.0f) / 10.0f;
						AudioCritique.pitch = 1 + pct * 0.5f;
					}
					End = scale >= 30.0f;
					if (End)
					{
						timer = Time.time;
						AudioCritique.Stop();
					}
				}
			}
			transform.localScale = Vector3.one * (scale + 1.0f * absorbTime);
			UpdateAnimation();
		}
		else
		{
			float pct = Time.time - timer;
			scale = endCurve.Evaluate(pct > 1.5f ? 1.5f : pct) * 1.5f;
			transform.localScale = Vector3.one * scale;
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, scale, layer.value);
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject == this.gameObject) continue;
				Destroy(colliders[i].gameObject);
			}
			if (pct >= 0.5f && AudioEnd.isPlaying == false && PanelEnd.Instance.IsVisible == false)
			{
				AudioEnd.Play();
			}
			float pc = (pct - 1.5f) / 0.5f;
			pc = Mathf.Clamp01(pc);
			GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f - pc);
			if (pct > 2.0f)
			{
				if (PanelEnd.Instance.IsVisible == false)
				{
					GameSettings.AddScore();
					PanelEnd.Instance.Show();
				}
			}
		}
	}

	void UpdateAnimation()
	{
		timeFrame += Time.deltaTime;
		if (timeFrame >= 0.2f)
		{
			frame = (frame + 1) % animationFrame.Count;
			spriteRenderer.sprite = animationFrame[frame];
			timeFrame -= 0.2f;
		}

		if (WarningUI.activeSelf)
		{
			WarningUI.transform.localScale = Vector3.one * (1 + Mathf.PingPong(Time.time, 0.2f));
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, scale);
	}
}
