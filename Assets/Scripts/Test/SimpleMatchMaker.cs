using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class SimpleMatchMaker : MonoBehaviour
{
    //
    public Transform scrollTrans;
    public GameObject button;
    public GameObject showCavas;
    void Start()
    {
        NetworkManager.singleton.StartMatchMaker();
        PlayerSelection.isNetworkedGame = true;
    }

    //call this method to request a match to be created on the server
    public void CreateInternetMatch(string matchName)
    {
        NetworkManager.singleton.matchMaker.CreateMatch(matchName, 4, true, "", "", "", 0, 0, OnInternetMatchCreate);
        PlayerSelection.playerColor = PawnColor.c_Blue;
    }

    //this method is called when your request for creating a match is returned
    private void OnInternetMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            //Debug.Log("Create match succeeded");

            MatchInfo hostInfo = matchInfo;
            NetworkServer.Listen(hostInfo, 9000);

            NetworkManager.singleton.StartHost(hostInfo);
        }
        else
        {
            Debug.LogError("Create match failed");
        }
            
    }

    //call this method to find a match through the matchmaker
    public void FindInternetMatch(string matchName)
    {
        NetworkManager.singleton.matchMaker.ListMatches(0, 10, matchName, true, 0, 0, OnInternetMatchList);
    }

    //this method is called when a list of matches is returned
    private void OnInternetMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success)
        {
            if (matches.Count != 0)
            {
                //Debug.Log("A list of matches was returned");

                //join the last server (just in case there are two...)
                //NetworkManager.singleton.matchMaker.JoinMatch(matches[matches.Count - 1].networkId, "", "", "", 0, 0, OnJoinInternetMatch);
                showCavas.gameObject.SetActive(true);
                for (int i = 0; i < matches.Count; i++)
                {
                    GameObject obj = (GameObject)Instantiate(button, scrollTrans);
                    obj.transform.GetChild(0).GetComponent<Text>().text = matches[0].name;
                }
                    
                StartCoroutine(joinMatch(matches));
            }
            else
            {
                CreateInternetMatch("Match" + Time.time);
            }
        }
        else
        {
            Debug.LogError("Couldn't connect to match maker");
        }
    }

    //this method is called when your request to join a match is returned
    private void OnJoinInternetMatch(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.fixedDeltaTime;
            }
            MatchInfo hostInfo = matchInfo;
            NetworkManager.singleton.StartClient(hostInfo);
            PlayerSelection.playerColor = PawnColor.c_Green;
        }
        else
        {
            Debug.LogError("Join match failed");
        }
    }

    void Update()
    { 
        if(Input.GetKeyDown(KeyCode.A))
        {
            PlayerSelection.playerColor = PawnColor.c_Blue;
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            PlayerSelection.playerColor = PawnColor.c_Green;
            //CheckStatus();
        }
    }

    IEnumerator joinMatch(List<MatchInfoSnapshot> matches)
    {
        yield return new WaitForSeconds(2f);
        NetworkManager.singleton.matchMaker.JoinMatch(matches[0].networkId, "", "", "", 0, 0, OnJoinInternetMatch);
        Debug.Log(matches[0].name);
    }
     

} 