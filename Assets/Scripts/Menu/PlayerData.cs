using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField]
	private List<int> playerID;
	public List<int> PlayerID
	{
		get { return playerID; }
		set { playerID = value; }
	}

	[SerializeField]
	private int numberOfLives = 3;
	public int NumberOfLives
	{
		get { return numberOfLives; }
		set { numberOfLives = value; }
	}

}
