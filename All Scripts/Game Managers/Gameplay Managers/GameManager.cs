using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager instance ;

	public int activeBosses;
	public const int DEFAULT_BOSS_COUNT = 3; // Starting count of bosses
	
	
	private void Awake()
	{
		activeBosses = 3;
		instance = this;
	}
}
