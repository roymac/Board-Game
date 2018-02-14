using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PawnColor
{
    c_Blue,
    c_Red,
    c_Green,
    c_Yellow,
    c_null
}
public enum nextDirection
{
    d_left,
    d_right,
    d_forward,
    d_back
}
public class WaypointScript : MonoBehaviour 
{
    public List<Transform> nextPoint;
    public Transform PreviousPoint;
    public nextDirection[] next_directions;
    public bool isDivergent;
    public  PawnColor safePassageColor;
    public int MoveConstraint;
    public List<GameObject> playerInBox;
    public bool isSafeBox, isHomeZone;
    AchievementsManager am;
    public bool isEndBox;
	// Use this for initialization
	void Start () 
    {
        am = GameObject.Find("GameManager").GetComponent<AchievementsManager>();
        if(next_directions.Length > 0)
        {
            nextPoint.Clear();
        }
        for (int i = 0; i < next_directions.Length; i++)
        {
            switch (next_directions[i])
            {
                case nextDirection.d_forward:
                    Debug.DrawRay(transform.position, Vector3.forward, Color.yellow, 10f);
                    getObject(Vector3.forward);
                    break;
                case nextDirection.d_back:
                    Debug.DrawRay(transform.position, Vector3.back, Color.blue, 10f);
                    getObject(Vector3.back);
                    break;
                case nextDirection.d_left:
                    Debug.DrawRay(transform.position, Vector3.left, Color.red, 10f);
                    getObject(Vector3.left);
                    break;
                case nextDirection.d_right:
                    Debug.DrawRay(transform.position, Vector3.right, Color.green, 10f);
                    getObject(Vector3.right);
                    break;
                
            }
        }
	}
	
    public void getObject(Vector3 Direction)
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, Direction, out hit, 10f))
        {
            nextPoint.Add(hit.transform);
            hit.transform.GetComponent<WaypointScript>().PreviousPoint = this.transform;
        }

    }
	// Update is called once per frame
	void Update () 
    {
		
	}

    public Transform returnNextPoint(PawnColor pawn_Color)
    {
        if (isDivergent)
        {
            if (pawn_Color == safePassageColor)
            {
                return nextPoint[1];
            }
            else
            {
                return nextPoint[0];
            }
        }
        else
        {
            return nextPoint[0];
        }
    }

    public int returnMoveConstraint()
    {
        return MoveConstraint;
    }

    public GameObject SetPlayerOccupy(GameObject player, bool toRemove)
    {
        if (toRemove)
        {
            playerInBox.Remove(player);
            for (int i = 0; i < playerInBox.Count; i++)
            {
                //playerInBox[i].GetComponent<PlayerMovement>().SetHeadMat(1);
                Vector3 posMax = GetComponent<MeshRenderer>().bounds.max;
                Vector3 posMin = GetComponent<MeshRenderer>().bounds.min;
                playerInBox[i].transform.position = new Vector3(Random.Range(posMin.x, posMax.x),playerInBox[i].transform.position.y,Random.Range(posMin.z, posMax.z));
                Debug.Log(playerInBox.Count / 4);
                Debug.Log(playerInBox.Count);

                playerInBox[i].transform.localScale = (Vector3.one * 50) / ((0.56f * playerInBox.Count) < 1 ? 1 : 0.56f * playerInBox.Count);
            }
            player.transform.localScale = Vector3.one * 50;
            //player.GetComponent<PlayerMovement>().SetHeadMat(0);
            return null;
        }

        if (playerInBox.Count == 0)
        {
            playerInBox.Add(player);
            return null;
        }
        else
        {
            if (player.GetComponent<PlayerMovement>().color != playerInBox[0].GetComponent<PlayerMovement>().color && !isSafeBox)
            {
                for (int i = 0; i < playerInBox.Count; i++)
                {
                    playerInBox[i].GetComponent<PlayerMovement>().MoveToStart();
                }
                playerInBox.Clear();
                playerInBox.Add(player);
                return player;
            }
            else
            {
                playerInBox.Add(player);
                for (int i = 0; i < playerInBox.Count; i++)
                {
                    playerInBox[i].transform.localScale = (Vector3.one * 50) / (0.60f * playerInBox.Count);
                }

                if (!isSafeBox)
                {
                    if (playerInBox.Count == 4)
                    {
                        if (playerInBox[0].GetComponent<PlayerMovement>().color == PlayerSelection.playerColor)
                        {
                            am.PawnPyramid();
                        }
                    }
                }
                else
                {
                    List<PawnColor> colorspresent = new List<PawnColor>();
                    for (int i = 0; i < playerInBox.Count; i++)
                    {
                        if (!colorspresent.Contains(playerInBox[i].GetComponent<PlayerMovement>().color))
                        {
                            colorspresent.Add(playerInBox[i].GetComponent<PlayerMovement>().color);
                        }
                    }

                    if (colorspresent.Count == 4)
                    {
                        am.DuckingForCover();
                    }
                }

                Vector3 posMax = GetComponent<MeshRenderer>().bounds.max;
                Vector3 posMin = GetComponent<MeshRenderer>().bounds.min;
                for (int i = 0; i < playerInBox.Count; i++)
                {
                    
                    Vector3 pos = new Vector3(Random.Range(posMin.x, posMax.x),playerInBox[i].transform.position.y,Random.Range(posMin.z, posMax.z));
                    if (i > 0)
                    {
                        int count = 0;
                        while (true)
                        {
                            if (count == playerInBox.Count - 1)
                            {
                                break;
                            }
                            else
                            {
                                if (!playerInBox[count].GetComponent<MeshRenderer>().bounds.Contains(pos))
                                {
                                    count++;
                                }
                                else
                                {
                                    pos = new Vector3(Random.Range(posMin.x, posMax.x),playerInBox[i].transform.position.y,Random.Range(posMin.z, posMax.z)); 
                                }
                            }
                        }
                    }
                    playerInBox[i].transform.position = pos;

                }
                return null;
            }
        }
    }
}
