using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterInfos
{
	[SerializeField]
	private int playerID;
	public int PlayerID
	{
		get { return playerID; }
		set { playerID = value; }
	}

	[SerializeField]
	private Team team;
	public Team Team
	{
		get { return team; }
		set { team = value; }
	}

	[SerializeField]
	private int characterColorID;
	public int CharacterColorID
	{
		get { return characterColorID; }
		set { characterColorID = value; }
	}

	[SerializeField]
	private CharacterData characterData;
	public CharacterData CharacterData
	{
		get { return characterData; }
		set { characterData = value; }
	}
}
public enum Team
{
	NoTeam = 0,
	TeamRed = 1,
	TeamBlue = 2,
	TeamGreen = 3,
	TeamPurple = 4,
}

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField]
	private List<CharacterInfos> characterInfos;
	public List<CharacterInfos> CharacterInfos
	{
		get { return characterInfos; }
		set { characterInfos = value; }
	}

	[SerializeField]
	private int numberOfLives = 3;
	public int NumberOfLives
	{
		get { return numberOfLives; }
		set { numberOfLives = value; }
	}

	[SerializeField]
	private TypeOfGameMode gameMode = TypeOfGameMode.FreeForAll;
	public TypeOfGameMode GameMode
	{
		get { return gameMode; }
		set { gameMode = value; }
	}
}

public enum TypeOfGameMode{
	FreeForAll,
	TeamVsTeam,
}
