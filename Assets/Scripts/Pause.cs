using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public static Pause Instance { get; private set; }
	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(Instance.gameObject);
		}
		Instance = this;
	}

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
	}

	public void Hide()
	{
		if (PanelCase.Instance.IsVisible == false) Time.timeScale = 1.0f;
		gameObject.SetActive(false);
	}

	public void Resume()
	{
		Hide();
	}

	public void Restart()
	{
		Time.timeScale = 1.0f;
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.buildIndex);
	}

	public void Exit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
