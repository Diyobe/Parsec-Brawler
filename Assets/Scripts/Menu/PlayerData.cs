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
	private int teamID;
	public int TeamID
	{
		get { return teamID; }
		set { teamID = value; }
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
	TwoVsTwo,
}