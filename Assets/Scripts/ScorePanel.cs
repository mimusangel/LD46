using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePanel : MonoBehaviour
{
	public static ScorePanel Instance { get; private set; }
	public TextMeshProUGUI scoreResult;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		UpdateScore();
	}

	public void UpdateScore()
	{
		GameSettings.LoadScore();
		scoreResult.text = "";
		string[] diffs =
		{
			"E", "N", "H"
		};
		List<GameSettings.Score> list;
		switch (GameSettings.GameDifficulty)
		{
			case GameSettings.Difficulty.Easy:
				list = GameSettings.Scores.ScoresEasy;
				break;
			case GameSettings.Difficulty.Hard:
				list = GameSettings.Scores.ScoresHard;
				break;
			default:
			case GameSettings.Difficulty.Normal:
				list = GameSettings.Scores.ScoresNormal;
				break;
		}
		for (int i = 0; i < list.Count; i++)
		{
			scoreResult.text += $"{i + 1}:\t{list[i].score} in {Mathf.RoundToInt(list[i].time)}s\n";
		}
	}
}
