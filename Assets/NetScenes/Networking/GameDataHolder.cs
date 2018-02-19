using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class GameDataHolder : MonoBehaviour {

    public string name;
	public string ip;
    public MatchInfoSnapshot matchDetails;

	public PawnColor color;
    public bool isSelected;

    public int playerValue;
}
