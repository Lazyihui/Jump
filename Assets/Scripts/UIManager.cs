using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager S;

	void Awake()
	{
		S = this;
	}

	public Text scoreText;

	public void RefreshScore(int score)
	{
		scoreText.text = "Score: " + score.ToString();
	}

}
