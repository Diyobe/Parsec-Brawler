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

}
