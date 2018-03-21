using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAIController : MonoBehaviour 
{
    public int MaxTilesToTravel = 57;
    public int currentTravelledTiles;
    public float weight;
    public int ChaseDistance;
    public PlayerMovement player;
    public AIManager ai_Manager;
    public bool showDebug;
	// Use this for initialization
	void Start () 
    {
        player = GetComponent<PlayerMovement>();
        ChaseDistance = 10;
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void GetWeight()
    {
        weight = 0;
        if (GetComponent<PlayerMovement>().isLocked && !GetComponent<PlayerMovement>().canUnlock)
        {
            weight = -100;
            ai_Manager.SelectPawnToMove(this.gameObject);
            return;
        }

        //else, if it is a six and the pawn is locked, move this pawn out of the jail area
        if (player.diceRoll == 6 && player.canUnlock && player.isLocked)
        {
            weight += 150;
        }
        //else, if this pawn is the furthest forward, move the pawn forward
        if (player.MoveConstraint == 0)
        {
            weight += ((float)currentTravelledTiles / (float)MaxTilesToTravel) * 100f;
        } 
        else
        {
            if (player.diceRoll > player.MoveConstraint)
            {
                weight = -100000;
            } 
            else
            {
                if (player.diceRoll == player.MoveConstraint)
                {
                    weight += 100000;
                }
                else
                {
                    weight += 30;
                }
               
            }
        }
            

        //if there is anyone within a dice roll away from the pawn, add 100 to the weight.
        WaypointScript point = player.target.GetComponent<WaypointScript>().PreviousPoint.GetComponent<WaypointScript>();
        //check the last 6 squares, that is the maximum dice roll
        for (int i = 0; i <= 6; i++)
        {
            //check if there are any player in the square
            if (point.playerInBox.Count > 0)
            {
                //check if the pawn present is the same as current pawn or not
                if (point.playerInBox[0].GetComponent<PlayerMovement>().color != GetComponent<PlayerMovement>().color && !player.target.GetComponent<WaypointScript>().isSafeBox)
                {
                    //if it not the same as current pawn, increase weight of this pawn 100
                    weight += 100 * i;
                    if(showDebug)
                        Debug.Log("Being Chased" + gameObject.name + weight);
                }
               
            }
            //get the tile before the current tile
            point = point.PreviousPoint.GetComponent<WaypointScript>();
        }
       
        //else, if there is a chance to knock out an enemy within a certain distance, add 10 to the weight
        point = player.target.GetComponent<WaypointScript>();//.nextPoint[0].GetComponent<WaypointScript>();
        int placeToKnockDown = 0;
        //check for X number of squares, where X is ChaseDistance
        for (int i = 0; i < ChaseDistance; i++)
        {
            //check if there are any players in the current square
            if (point.playerInBox.Count > 0)
            {
                Debug.Log(point.playerInBox[0].GetComponent<PlayerMovement>().color);
                //check if the player present is of the same type as this player
                if (point.playerInBox[0].GetComponent<PlayerMovement>().color != GetComponent<PlayerMovement>().color && !point.isSafeBox)
                { 
                    //save the position of the square
                    placeToKnockDown = i;
                    Debug.Log("Found enemy in " + placeToKnockDown);
                    //check if the dice roll is higher than where the players are located, if it is lower, or the diceroll is a six, then add weight according to the number of pawns present
                    if (player.diceRoll <= placeToKnockDown && !point.isSafeBox || player.canUnlock)
                    {
                        //number of players present in the target box
                        for (int j = 0; j < point.playerInBox.Count; j++)
                        {
                            //increase weight by 25
                            weight += 150; 
                        }
                        if (showDebug)
                            Debug.Log("Chasing" + gameObject.name + weight);
                    }
                    else
                    {
                        //decrease the weight of this as it will have a higher chance of dying if moved ahead of an opposition
                        weight = -10;
                        if (showDebug)
                            Debug.Log("Will be eaten if chased" + gameObject.name + weight);
                        //weight = Mathf.Clamp(weight, 0, 999999);
                        point = point.nextPoint[0].GetComponent<WaypointScript>();
                    }
                    //break;
                }
                else
                {
                    if (point.nextPoint[0] == null)
                    {
                        break;          
                    }
                    point = point.nextPoint[0].GetComponent<WaypointScript>();
                }
            }
            //if no enemy was found
            else
            {
                //move to the next waypoint
				if (point.nextPoint[0] == null)
				{
					break;			
				}
                point = point.nextPoint[0].GetComponent<WaypointScript>();
            }
        }


      

        //set this pawn in the list of pawns with weight in AIManager
        ai_Manager.SelectPawnToMove(this.gameObject);
    }
}
