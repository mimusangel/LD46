using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;



public class PanelEnd : MonoBehaviour
{
	public static PanelEnd Instance { get; private set; }
	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(Instance.gameObject);
		}
		Instance = this;
	}


	public TextMeshProUGUI ScoreText;
	public TextMeshProUGUI TimeText;


	private void Start()
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

	public void Show()
	{
		Time.timeScale = 0.0f;
		gameObject.SetActive(true);
		GameManager manager = GameManager.Instance;

		ScoreText.text = $"Score:\t{GameManager.Instance.GameScore}";
		TimeText.text = $"Time:\t\t{Mathf.RoundToInt(GameManager.Instance.GameTime)}s";
	}

	public void Hide()
	{
		if (PanelCase.Instance.IsVisible == false) Time.timeScale = 1.0f;
		gameObject.SetActive(false);
	}
	
	public void Restart()
	{
		Time.timeScale = 1.0f;
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.buildIndex);
	}

	public void Exit()
	{
		SceneManager.LoadScene(0);
	}
}
