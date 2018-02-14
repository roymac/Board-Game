using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour 
{
    public List<GameObject> players;
	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void SelectPawnToMove(GameObject in_player)
    {
        if (players.Count < 4)
        {
            players.Add(in_player); 
        }
        if (players.Count == 4)
        {
            for (int i = 0; i < players.Count-1; i++)
            {
                for (int j = i + 1; j < players.Count; j++)
                {
                    if (players[i].GetComponent<PawnAIController>().weight < players[j].GetComponent<PawnAIController>().weight)
                    {
                        GameObject temp = players[j];
                        players[j] = players[i];
                        players[i] = temp;
                    }
                }
            }
                
            Debug.Log("choosing Player");
            players[0].GetComponent<PlayerMovement>().OnMouseDown();
            players.Clear();
        }
      

    }
}
