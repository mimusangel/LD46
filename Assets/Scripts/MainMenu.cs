using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public AudioMixer audioMixer;

	public Slider musicSlider;
	public Slider fxSlider;
	public TextMeshProUGUI musicText;
	public TextMeshProUGUI fxText;

	public Toggle Easy;
	public Toggle Normnal;
	public Toggle Hard;

	void Start()
	{
		musicSlider.value = GameSettings.MusicVolume;
		musicText.text = $"Music: {Mathf.RoundToInt(GameSettings.MusicVolume * 100.0f)}";
		audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-80.0f, 20.0f, GameSettings.MusicVolume));
		fxSlider.value = GameSettings.FXVolume;
		fxText.text = $"Effect: {Mathf.RoundToInt(GameSettings.FXVolume * 100.0f)}";
		audioMixer.SetFloat("FXVolume", Mathf.Lerp(-80.0f, 20.0f, GameSettings.FXVolume));

		ToggleGroup tg = GetComponent<ToggleGroup>();
		tg.SetAllTogglesOff();
		if (GameSettings.GameDifficulty == GameSettings.Difficulty.Easy)
		{
			Easy.isOn = true;
			tg.NotifyToggleOn(Easy);
		}
		if (GameSettings.GameDifficulty == GameSettings.Difficulty.Normal)
		{
			Normnal.isOn = true;
			tg.NotifyToggleOn(Normnal);
		}
		if (GameSettings.GameDifficulty == GameSettings.Difficulty.Hard)
		{
			Hard.isOn = true;
			tg.NotifyToggleOn(Hard);
		}
	}
	
    void Update()
    {
        
    }

	public void ChangeValueMusic()
	{
		GameSettings.MusicVolume = musicSlider.value;
		musicText.text = $"Music: {Mathf.RoundToInt(GameSettings.MusicVolume * 100.0f)}";
		audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-80.0f, 20.0f, GameSettings.MusicVolume));
	}

	public void ChangeValueFx()
	{
		GameSettings.FXVolume = fxSlider.value;
		fxText.text = $"Effect: {Mathf.RoundToInt(GameSettings.FXVolume * 100.0f)}";
		audioMixer.SetFloat("FXVolume", Mathf.Lerp(-80.0f, 20.0f, GameSettings.FXVolume));
	}

	public void PlaySoundTest(GameObject go)
	{
		Instantiate(go);
	}

	public void ChooseDifficulty(int d)
	{
		GameSettings.GameDifficulty = (GameSettings.Difficulty)d;
		ScorePanel.Instance.UpdateScore();
	}

	public void Play()
	{
		PlayerPrefs.Save();
		SceneManager.LoadScene(1);
	}

	public void Exit()
	{
		PlayerPrefs.Save();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
