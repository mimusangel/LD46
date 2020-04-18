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
				timerTxt.text = $"{Mathf.RoundToInt(GameManager.Instance.GameTime)}s (Pause)";
			}
			else
			{
				timerTxt.text = $"{Mathf.RoundToInt(GameManager.Instance.GameTime)}s";
			}
		}

	}
}
