using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class Timer : MonoBehaviour
{
	TextMeshProUGUI timerTxt;

	float stopTimer = 0;

	private void Start()
	{

		timerTxt = GetComponent<TextMeshProUGUI>();
	}

	private void Update()
	{
		if (Solar.Instance.End)
		{
			if (stopTimer <= 0)	stopTimer = Time.time;
			timerTxt.text = $"{Mathf.RoundToInt(stopTimer)}s";
		}
		else
		{
			if (Time.timeScale <= 0)
			{
				timerTxt.text = $"Time:\t\t{Mathf.RoundToInt(GameManager.Instance.GameTime)}s (Pause)\nScore:\t{GameManager.Instance.GameScore}";
			}
			else
			{
				timerTxt.text = $"Time:\t\t{Mathf.RoundToInt(GameManager.Instance.GameTime)}s\nScore:\t{GameManager.Instance.GameScore}";
			}
		}

	}
}
