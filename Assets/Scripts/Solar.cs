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

	}

	private void Update()
	{
		if (!End)
		{

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
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, scale, layer.value);
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject == this.gameObject) continue;
				Destroy(colliders[i].gameObject);
				bool absorb = false;
				if (hasShieldActif)
				{
					for (int j = 0; j < shields.Count; j++)
					{
						if (shields[j].HasShield)
						{
							shields[j].UseShield();
							absorb = true;
							break;
						}
					}
				}
				if (!absorb)
				{
					scale += 1.0f;
					End = scale >= 30.0f;
					if (End) timer = Time.time;
				}
			}
			transform.localScale = Vector3.one * scale;
			UpdateAnimation();
		}
		else
		{
			float pct = Time.time - timer;
			scale = endCurve.Evaluate(pct > 1.5f ? 1.5f : pct);
			transform.localScale = Vector3.one * scale;
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, scale, layer.value);
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject == this.gameObject) continue;
				Destroy(colliders[i].gameObject);
			}
			float pc = (pct - 1.5f) / 0.5f;
			pc = Mathf.Clamp01(pc);
			GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f - pc);
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
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, scale);
	}
}
