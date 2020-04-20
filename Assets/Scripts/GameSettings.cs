using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class GameSettings
{
	public enum Difficulty
	{
		Easy = 0,
		Normal = 1,
		Hard = 2
	}

	public class Score
	{
		public int score;
		public float time;
	}

	public class ScoreList
	{
		public List<Score> ScoresEasy = new List<Score>();
		public List<Score> ScoresNormal = new List<Score>();
		public List<Score> ScoresHard = new List<Score>();
	}

	public static Difficulty GameDifficulty = Difficulty.Normal;

	public static bool MoneyByDifficulty = true;

	public static ScoreList Scores = new ScoreList();

	public static float MusicVolume
	{
		set
		{
			PlayerPrefs.SetFloat("MusicVolume", value);
		}
		get
		{
			return PlayerPrefs.GetFloat("MusicVolume", 0.5f);
		}
	}
	public static float FXVolume
	{
		set
		{
			PlayerPrefs.SetFloat("FXVolume", value);
		}
		get
		{
			return PlayerPrefs.GetFloat("FXVolume", 0.8f);
		}
	}

	public static void LoadScore()
	{
		string xmlText = PlayerPrefs.GetString("Scores");
		if (string.IsNullOrEmpty(xmlText) == false)
		{
			XmlSerializer xml = new XmlSerializer(typeof(ScoreList));
			using (StringReader reader = new StringReader(xmlText))
			{
				try
				{
					Scores = xml.Deserialize(reader) as ScoreList;
					if (Scores == null)
						Scores = new ScoreList();
				}
				catch
				{
					PlayerPrefs.SetString("Scores", "");
					Scores = new ScoreList();
				}
			}
		}
	}

	public static void AddScore()
	{
		LoadScore();
		switch (GameDifficulty)
		{
			case Difficulty.Easy:
				Scores.ScoresEasy.Add(new Score()
				{
					score = GameManager.Instance.GameScore,
					time = GameManager.Instance.GameTime
				});
				Scores.ScoresEasy = Scores.ScoresEasy.OrderByDescending(x => x.score).ToList<Score>();
				if (Scores.ScoresEasy.Count > 10)
				{
					Scores.ScoresEasy.RemoveAt(10);
				}
				break;
			case Difficulty.Normal:
				Scores.ScoresNormal.Add(new Score()
				{
					score = GameManager.Instance.GameScore,
					time = GameManager.Instance.GameTime
				});
				Scores.ScoresNormal = Scores.ScoresNormal.OrderByDescending(x => x.score).ToList<Score>();
				if (Scores.ScoresNormal.Count > 10)
				{
					Scores.ScoresNormal.RemoveAt(10);
				}
				break;
			case Difficulty.Hard:
				Scores.ScoresHard.Add(new Score()
				{
					score = GameManager.Instance.GameScore,
					time = GameManager.Instance.GameTime
				});
				Scores.ScoresHard = Scores.ScoresHard.OrderByDescending(x => x.score).ToList<Score>();
				if (Scores.ScoresHard.Count > 10)
				{
					Scores.ScoresHard.RemoveAt(10);
				}
				break;
		}
		
		SaveScore();
	}

	public static void SaveScore()
	{
		XmlSerializer xml = new XmlSerializer(typeof(ScoreList));
		StringBuilder stringXML = new StringBuilder();
		using (XmlWriter writer = XmlWriter.Create(stringXML))
		{
			xml.Serialize(writer, Scores);
		}
		string xmlText = stringXML.ToString();
		PlayerPrefs.SetString("Scores", xmlText);
	}
}
