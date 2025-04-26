using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	// public GameManager gameManager;

	// void Awake()
	// {
	// 	// Awake 比 Start 先执行
	// 	gameManager = this;
	// }

	public static GameManager S;

	void Awake()
	{
		S = this;
	}

	public bool PlayerIsFacingXAxis;
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
