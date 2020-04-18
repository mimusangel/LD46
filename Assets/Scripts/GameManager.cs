using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; } = null;
	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(Instance);
		}
		Instance = this;
	}

	public GameObject panelHelp;

	public List<GameObject> PlanetePrefab = new List<GameObject>();
	public GameObject MoonPrefab;
	public List<GameObject> AsteroidPrefab = new List<GameObject>();

	public Sprite CaseSprite;
	public Sprite WeaponBase;
	public Sprite CannonSprite;
	public Sprite CannonLongSprite;
	public GameObject BulletPrefab;

	public Sprite DoubleCannonSprite;
	public Sprite DoubleCannonLongSprite;

	public Sprite RocketLauncher;
	public Sprite Rocket;
	public GameObject RocketPrefab;
	
	public Sprite Shield;


	public float GameTime { get; private set; } = 0.0f;

	private void Start()
	{
		Vector2[] rotates = new Vector2[]
		{
			new Vector2(1.0f, 1.0f),
			new Vector2(1.0f, 0.5f),
			new Vector2(0.5f, 1.0f),
			new Vector2(0.75f, 1.0f),
			new Vector2(1.0f, 0.75f),
		};
		int count = Random.Range(4, 9);
		for (int i = 0; i < count; i++)
		{
			GameObject prefab = PlanetePrefab[Random.Range(0, PlanetePrefab.Count)];
			Vector2 position = new Vector2(
				Random.Range(-35.0f, 35.0f),
				Random.Range(-35.0f, 35.0f)
			);
			position.x += position.x < 0 ? -5 : 5;
			position.y += position.y < 0 ? -5 : 5;

			GameObject go = Instantiate(prefab, position, Quaternion.identity);
			Rotate rot = go.GetComponent<Rotate>();
			rot.worldPointToRotate = new Vector2(
				Random.Range(-5.0f, 5.0f),
				Random.Range(-5.0f, 5.0f)
			);
			rot.rotate = rotates[Random.Range(0, rotates.Length)];
			rot.rotateSpeed = 90.0f;
			if (Random.Range(0.0f, 100.0f) < 10.0f)
			{
				CircleCollider2D circle = go.GetComponent<CircleCollider2D>();
				Vector2 dir = new Vector2(
					Random.Range(-1.0f, 1.0f),
					Random.Range(-1.0f, 1.0f)
				).normalized;
				GameObject goMoon = Instantiate(MoonPrefab, position + dir * (circle.radius + 1.5f), Quaternion.identity);
				Rotate rotMoon = goMoon.GetComponent<Rotate>();
				rotMoon.targetToRotate = go.transform;
				rotMoon.worldPointToRotate = rot.worldPointToRotate;
				rotMoon.rotate = Vector2.one;
				rotMoon.rotateSpeed = 90.0f;
			}
		}

		StartCoroutine(GenerateAsteroid());
	}

	private void Update()
	{
		GameTime += Time.deltaTime;
		panelHelp.SetActive(GameTime < 20.0f || Pause.Instance.IsVisible);
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Pause.Instance.Show();
		}
	}

	IEnumerator GenerateAsteroid()
	{
		//yield return new WaitForSeconds(10.0f);
		while (gameObject.activeSelf && Solar.Instance.End == false)
		{
			Vector2 dir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
			dir.Normalize();
			GameObject go = Instantiate(AsteroidPrefab[Random.Range(0, AsteroidPrefab.Count)], dir * Random.Range(100.0f, 200.0f), Quaternion.identity);
			Rigidbody2D rg2d = go.GetComponent<Rigidbody2D>();
			rg2d.velocity = -dir * 10.0f;
			rg2d.AddTorque(Random.Range(-10.0f, 10.0f), ForceMode2D.Impulse);
			Asteroid asteroid = go.GetComponent<Asteroid>();
			asteroid.Big = Random.Range(0.0f, 100.0f) < 5.0f;
			float timer = Random.Range(2.0f, 5.0f);
			yield return new WaitForSeconds(timer);
		}
	}

	public void SpawnSmallAsteroid(Vector3 position)
	{
		Vector2 dir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
		dir.Normalize();
		GameObject go = Instantiate(AsteroidPrefab[Random.Range(0, AsteroidPrefab.Count)], position, Quaternion.identity);
		Rigidbody2D rg2d = go.GetComponent<Rigidbody2D>();
		rg2d.velocity = dir * 5.0f;
		rg2d.AddTorque(Random.Range(-10.0f, 10.0f), ForceMode2D.Impulse);
	}
}
