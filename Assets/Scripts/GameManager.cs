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

	[Header("Game Prefab")]
	public GameObject panelHelp;
	public List<GameObject> PlanetePrefab = new List<GameObject>();
	public List<GameObject> MoonPrefab = new List<GameObject>();
	public List<GameObject> AsteroidPrefab = new List<GameObject>();
	public GameObject ETPrefab;

	[Header("Game UI")]
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

	public Sprite Satellite;
	public Sprite Antenna;


	[Header("Game Effect")]
	public List<Sprite> Cracks = new List<Sprite>();

	
	[Header("Sound")]
	public GameObject LaserSound;
	public GameObject RocketSound;



	public float GameTime { get; private set; } = 0.0f;
	public int GameScore { get; private set; } = 0;
	Vector2[] rotates;

	private void Start()
	{
		rotates = new Vector2[]
		{
			new Vector2(1.0f, 1.0f),
			new Vector2(1.0f, 0.5f),
			new Vector2(0.5f, 1.0f),
			new Vector2(0.75f, 1.0f),
			new Vector2(1.0f, 0.75f),
		};
		int count = Random.Range(8, 12);
		if (GameSettings.GameDifficulty == GameSettings.Difficulty.Hard)
		{
			count -= 2;
		}
		if (GameSettings.GameDifficulty == GameSettings.Difficulty.Easy)
		{
			count += 2;
		}
		for (int i = 0; i < count; i++)
		{
			SpawnPlanete();
		}

		StartCoroutine(GenerateAsteroid());
	}

	public void SpawnPlanete()
	{
		GameObject prefab = PlanetePrefab[Random.Range(0, PlanetePrefab.Count)];
		Vector2 position = new Vector2(
			Random.Range(-50.0f, 50.0f),
			Random.Range(-50.0f, 50.0f)
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
		rot.rotateSpeed = Random.Range(45.0f, 135.0f);
		Planete planete = go.GetComponent<Planete>();

		float moonChance = 10.0f;
		switch (GameSettings.GameDifficulty)
		{
			case GameSettings.Difficulty.Easy:
				moonChance = 15.0f;
				break;
			case GameSettings.Difficulty.Normal:
				moonChance = 12.5f;
				break;
			case GameSettings.Difficulty.Hard:
				moonChance = 10.0f;
				break;
		}
		if (Random.Range(0.0f, 100.0f) < moonChance)
		{
			SpawnMoon(planete);
		}
	}

	public void SpawnMoon(Planete planete)
	{
		GameObject goMoon = Instantiate(MoonPrefab[Random.Range(0, MoonPrefab.Count)]);
		Rotate rotMoon = goMoon.GetComponent<Rotate>();
		rotMoon.targetToRotate = planete.transform;
		rotMoon.worldPointToRotate = planete.GetComponent<Rotate>().worldPointToRotate;
		rotMoon.rotate = Vector2.one;
		rotMoon.rotateSpeed = 90.0f;
		Planete pMoon = goMoon.GetComponent<Planete>();
		pMoon.isMoon = true;
		pMoon.planete = planete;
	}

	private void Update()
	{
		GameTime += Time.deltaTime;
		panelHelp.SetActive(GameTime < 20.0f || Pause.Instance.IsVisible);
		if (Input.GetKeyDown(KeyCode.Escape) && Solar.Instance.End == false)
		{
			Pause.Instance.Show();
		}
	}

	IEnumerator GenerateAsteroid()
	{
		//yield return new WaitForSeconds(10.0f);
		while (gameObject.activeSelf && Solar.Instance.End == false)
		{
			Vector2 dir;
			if (Random.Range(0.0f, 100.0f) < 10.0f && Planete.Planetes.Count > 0 && GameTime > 45.0f)
			{
				dir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
				dir.Normalize();
				GameObject goET = Instantiate(ETPrefab, dir * Random.Range(150.0f, 250.0f), Quaternion.identity);
				ET et = goET.GetComponent<ET>();
				et.focus = Planete.Planetes[Random.Range(0, Planete.Planetes.Count)];
			}
			dir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
			dir.Normalize();
			GameObject go = Instantiate(AsteroidPrefab[Random.Range(0, AsteroidPrefab.Count)], dir * Random.Range(100.0f, 200.0f), Quaternion.identity);
			Asteroid asteroid = go.GetComponent<Asteroid>();
			asteroid.Big = Random.Range(0.0f, 100.0f) < (5.0f + GameTime / 60.0f) && GameTime > 30.0f;
			if (Planete.Planetes.Count > 0)
			{
				asteroid.dir = (Planete.Planetes[Random.Range(0, Planete.Planetes.Count)].transform.position - go.transform.position).normalized;
			}
			else
			{
				asteroid.dir = -dir;
			}
			float subTime = (GameTime / 30.0f);
			float min = Mathf.Max(3.0f - subTime, 0.0f);
			float max = Mathf.Max(6.0f - subTime, 1.0f);
			float timer = Random.Range(min, max);
			yield return new WaitForSeconds(timer);
		}
	}

	public void SpawnSmallAsteroid(Vector3 position)
	{
		GameObject go = Instantiate(AsteroidPrefab[Random.Range(0, AsteroidPrefab.Count)], position, Quaternion.identity);
		Rigidbody2D rg2d = go.GetComponent<Rigidbody2D>(); Asteroid asteroid = go.GetComponent<Asteroid>();
		if (Planete.Planetes.Count > 0)
		{
			asteroid.dir = (Planete.Planetes[Random.Range(0, Planete.Planetes.Count)].transform.position - go.transform.position).normalized;
		}
		else
		{
			asteroid.dir = (new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f))).normalized;
		}
	}

	public void AddScore(int score)
	{
		GameScore += score;
	}
}
