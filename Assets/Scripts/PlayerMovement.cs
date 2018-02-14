using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    //gameplay variables
    public float MoveSpeed;
    public Transform target;
    public int MoveCounter;
    public int diceRoll;
    public PawnColor color;
    public Vector3 originalPosition;
    public GameObject animationChild;
    public Transform StartingBlock;
    public bool ControlledByAI;

    //selection variables
    public bool isLocked;
    public bool canUnlock;
    public bool hasfinished;
    public GameManager gm;
    public AchievementsManager am;

    public int MoveConstraint;

    public Material mat;
    public List<MeshRenderer> renderers;
    public ParticleSystem onDeath, onReturn, showCanSelect,onFinish;

    //internal Variables
    protected bool canMove,isInHomeZone;
    protected Quaternion startingRot;
    int killcount, deathcount, statueCount;
    List<PawnColor> killedPawnColor;
	// Use this for initialization
	void Start () 
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }


        startingRot = transform.GetChild(0).transform.localRotation;
        originalPosition = this.gameObject.transform.position;
        animationChild = transform.GetChild(0).GetChild(1).gameObject;
        mat.enableInstancing = true;
        for (int i = 0; i < animationChild.transform.parent.childCount; i++)
        {
            renderers.Add(animationChild.transform.parent.GetChild(i).GetComponent<MeshRenderer>());
            renderers[i].material = mat;
        }
        StartingBlock = target;
        showCanSelect = transform.GetChild(1).GetComponent<ParticleSystem>();
        switch (color)
        {
            case PawnColor.c_Blue:
                EventManager.UnlockBluePawns += ShowUnlockAnimation;
                break;
            case PawnColor.c_Red:
                EventManager.UnlockRedPawns += ShowUnlockAnimation;
                break;
            case PawnColor.c_Yellow:
                EventManager.UnlockYellowPawns += ShowUnlockAnimation;
                break;
            case PawnColor.c_Green:
                EventManager.UnlockGreenPawns += ShowUnlockAnimation;
                break;
        }
        EventManager.onSelected += OnPlayerSelect;
        EventManager.onePlayerOut += OnePlayerOut;
        if (color == PlayerSelection.playerColor || PlayerSelection.playerColor == PawnColor.c_null)
        {
            ControlledByAI = false;
        }
        else
        {
            ControlledByAI = true;
        }

        am = gm.GetComponent<AchievementsManager>();
        mat.SetFloat("_Progress", 1f );
        onFinish = transform.GetChild(2).GetComponent<ParticleSystem>();
	}

    void OnDestroy()
    {
        //if (!gameObject.activeInHierarchy)
        //{
        //    return;
        //}

        switch (color)
        {
            case PawnColor.c_Blue:
                EventManager.UnlockBluePawns -= ShowUnlockAnimation;
                break;
            case PawnColor.c_Red:
                EventManager.UnlockRedPawns -= ShowUnlockAnimation;
                break;
            case PawnColor.c_Yellow:
                EventManager.UnlockYellowPawns -= ShowUnlockAnimation;
                break;
            case PawnColor.c_Green:
                EventManager.UnlockGreenPawns -= ShowUnlockAnimation;
                break;
        }
        EventManager.onSelected -= OnPlayerSelect;
        EventManager.onePlayerOut -= OnePlayerOut;
    }

    void OnePlayerOut()
    {
        //checks if AI Controlled or not
        if (!ControlledByAI)
        {
            if (PlayerSelection.isNetworkedGame)
            {
                if (PlayerSelection.playerColor == color)
                {
                    //Networked game, checks if it the players turn;
                    OnMouseDown();
                } 
            }
            else
            {
                if (gm.currentPlayerTurn == color)
                {
                    Debug.Log("On 3 players Locked");
                    OnMouseDown();   
                }
            }
        }

    }

    void ShowUnlockAnimation(int num)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        if (color == PlayerSelection.playerColor)
        {
            if (isLocked && !hasfinished)
            {
                if (!am.LockedPlayers.Contains(this.gameObject))
                {
                    am.LockedPlayers.Add(this.gameObject);
                }
            }
        }

        if (num == 6)
        {
            if (isLocked && !hasfinished)
            {
                canUnlock = true;
            }
        }
        else
        {
            canUnlock = false;
        }

        if (!ControlledByAI)
        {
            if (!isLocked|| canUnlock )
            {
                if (MoveConstraint == 0)
                {
                    GetComponent<BoxCollider>().enabled = true;
                    diceRoll = num;
                    animationChild.GetComponent<Animator>().SetBool("animate", true);
                    showCanSelect.Play();
                }
                else
                {
                    if (num <= MoveConstraint)
                    {
                        GetComponent<BoxCollider>().enabled = true;
                        diceRoll = num;
                        animationChild.GetComponent<Animator>().SetBool("animate", true);
                        showCanSelect.Play();
                    }
                    else
                    {
                        gm.SetLockedPlayers();
                    }
                }

            }
            else
            {
                if (isLocked && !canUnlock)
                {
                    gm.SetLockedPlayers();
                }
            }
        }
        else
        {
            if (!isLocked || canUnlock)
            {
                diceRoll = num;
                GetComponent<PawnAIController>().GetWeight();

            }
            else
            {
                if (isLocked && !canUnlock)
                {
                    gm.SetLockedPlayers();
                    GetComponent<PawnAIController>().GetWeight();
                }
            }
        }


    }

    void OnPlayerSelect()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        GetComponent<BoxCollider>().enabled = false;
        animationChild.GetComponent<Animator>().SetBool("animate", false);
        showCanSelect.Stop();

        //Statue Achievement Check
        if (color == PlayerSelection.playerColor)
        {
            if (gm.currentPlayer != this.gameObject)
            {
                statueCount++;
            }
            else
            {
                statueCount = 0;
            }

            if (statueCount == 10)
            {
                am.Statue();
            }
        }


    }
	
	// Update is called once per frame
	void Update () 
    {
        if (canMove)
        {
            if (Vector3.Distance(transform.position, target.position) > 0.001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, MoveSpeed * Time.deltaTime);
            }
            else
            {
                if (MoveCounter < diceRoll)
                {
                    target = target.gameObject.GetComponent<WaypointScript>().returnNextPoint(color);
                    MoveCounter++;
                }
                else
                {
                    canMove = false;
                    MoveConstraint = target.gameObject.GetComponent<WaypointScript>().returnMoveConstraint();
                    if (target.gameObject.GetComponent<WaypointScript>().isEndBox)
                    {
                        hasfinished = true;
                        isLocked = true;
                        canUnlock = false;
                        if (deathcount == 0)
                        {
                            if (color == PlayerSelection.playerColor)
                            {
                                am.GodLike();
                            }
                        }
                        am.homesafeCount--;
                        gm.CountPawnsAtHome();
                        onFinish.Play();
                        AudioManager.Instance.ReachedFinish();
                        StartCoroutine(FinalFade(3f));
                    }
                    else
                    {
                        GameObject obj = target.gameObject.GetComponent<WaypointScript>().SetPlayerOccupy(this.gameObject, false);
                        if (obj != null)
                        {
                            if (color == PlayerSelection.playerColor)
                            {
                                //Killstreak Achievement Check;
                                killcount++;
                                if (killcount == 5)
                                {
                                    am.KillStreak();
                                }

                                //EveryOneButMe achievement Check
                                if (!killedPawnColor.Contains(obj.GetComponent<PlayerMovement>().color))
                                {
                                    killedPawnColor.Add(obj.GetComponent<PlayerMovement>().color);
                                    if (killedPawnColor.Count == 3)
                                    {
                                        am.EveryOneButMe();
                                    }
                                }
                            }

                        }

                        //Homesafe Achievement Check
                        if (color == PlayerSelection.playerColor)
                        {
                            if (target.GetComponent<WaypointScript>().isHomeZone)
                            {
                                if (!isInHomeZone)
                                {
                                    if (deathcount == 0)
                                    {
                                        am.HomeSafe(); 
                                    }
                                    isInHomeZone = true;
                                }
                            }  
                        }

                        gm.OnMoveFinished(obj);
                        GetComponent<PawnAIController>().currentTravelledTiles += diceRoll;
                    }
                }
            } 
        }
	}

    public void MoveCharacter()
    {
        if (MoveConstraint == 0)
        {
            if (!isLocked)
            {
                if (diceRoll != 0)
                {
                    target.gameObject.GetComponent<WaypointScript>().SetPlayerOccupy(this.gameObject,true);
                }
                MoveCounter = 0;
                canMove = true;
            }
        }
        else
        {
            if (diceRoll <= MoveConstraint)
            {
                target.gameObject.GetComponent<WaypointScript>().SetPlayerOccupy(this.gameObject, true);
                MoveCounter = 0;
                canMove = true;
            }
            else
            {
                gm.SetLockedPlayers();
            }
        }
    }

    public void OnMouseDown()
    {
        if (!isLocked)
        {
            gm.SetCurrentPlayer(this.gameObject, color);
        }
        else
        {
            if (canUnlock)
            {
                isLocked = false;
                canUnlock = true;
                diceRoll = 0;
                gm.SetCurrentPlayer(this.gameObject, color);

                if (color == PlayerSelection.playerColor)
                {
                    if (isLocked && !hasfinished)
                    {
                        if (am.LockedPlayers.Contains(this.gameObject))
                        {
                            am.LockedPlayers.Remove(this.gameObject);
                        }
                    }
                }
            
            }
        }
    }

    public void MoveToStart()
    {
        
       //DeathStreak and TastOfVictory Achievements Check
        if(color == PlayerSelection.playerColor)
        {
            deathcount++;
            if (deathcount == 5)
            {
                am.DeathStreak();
            }

            if (GetComponent<PawnAIController>().currentTravelledTiles == 51)
            {
                am.TasteOfVictory();
            }
        }

        GetComponent<PawnAIController>().currentTravelledTiles = 0;
        StartCoroutine(FadeMaterial(true,1.5f));
        target = StartingBlock;
        isLocked = true;
        canUnlock = false;
    }

    IEnumerator FinalFade(float TimeToFade)
    {
        Debug.Log("Here");
        for (float i = TimeToFade; i >= TimeToFade/2; i -= Time.deltaTime)
        {
            if (i / TimeToFade < 0.5f)
            {
                break;
            }
            mat.SetFloat("_Progress", i/TimeToFade );
            yield return null;
        }
        gm.OnMoveFinished(null);
        GetComponent<PawnAIController>().currentTravelledTiles += diceRoll;
        GetComponent<MeshRenderer>().enabled = false;
    }

    IEnumerator FadeMaterial(bool fadeAway, float TimeToFade)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            AudioManager.Instance.PawnDeath();
            onDeath.Play();
            transform.localScale = Vector3.one * 50f;
            for (float i = TimeToFade; i >= TimeToFade/2; i -= Time.deltaTime)
            {
                if (i / TimeToFade < 0.5f)
                {
                    break;
                }
                mat.SetFloat("_Progress", i/TimeToFade );
                yield return null;
            }
            transform.position = originalPosition;
            StartCoroutine(FadeMaterial(false,TimeToFade));
        }
        // fade from transparent to opaque
        else
        {
            AudioManager.Instance.PawnSpawn();
            onReturn.Play();
            for (float i = TimeToFade/2; i <= TimeToFade; i += Time.deltaTime)
            {
                if (i / TimeToFade > 1)
                {
                    break;
                }
                mat.SetFloat("_Progress", i/TimeToFade);
                yield return null;
            }
        }
    }
}
