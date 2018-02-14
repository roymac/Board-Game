using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{

    public Animator mainMenuAnim;

    public GameObject pauseMenu, mainMenu, gameoverMenu, pauseButton, noofPlayerScreen, playerSelectScene, gameModeScreen, MultiplayerMode, OfflineOnlineMode;
    public Text GameOverText;

    public static int numberOfPlayers = 0;

    public static bool isVersusBot = false;
    public static bool isOnline = false;

    // Use this for initialization
    void Start()
    {
        if(mainMenu != null)
            mainMenu.SetActive(true);

        if (MultiplayerMode != null)
            MultiplayerMode.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
    }

    public static void StartGame()
    {
        SceneManager.LoadScene("LudoLevel");
    }

    public void StartTheGame()
    {
        SceneManager.LoadScene("LudoLevel");
    }

    public void ExitGame()
    {
       // DiscoverNetworks.Instance.StopBroadcast();
       // NetworkManager.singleton.StopClient();
        SceneManager.LoadScene("LudoMenu");
        PlayerSelection.playerInfo.Clear();
        PlayerSelection.playerColor = PawnColor.c_null;

    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameOver(string in_GameOverText)
    {
        //mainMenu.SetActive(false);
        //pauseMenu.SetActive(false);
        gameoverMenu.SetActive(true);
        GameOverText.text = in_GameOverText;
    }

    public void OpenPlayerNumberScreen( bool isSP)
    {
        noofPlayerScreen.SetActive(true);
        mainMenu.SetActive(false);
        gameModeScreen.SetActive(false);
        isVersusBot = isSP;
    }

    public void SelectNumberOfPlayer(int playerNumber)
    {
        numberOfPlayers = playerNumber;
    }

    public void OpenPlayerSelectionScreen()
    {
        playerSelectScene.SetActive(true);
        noofPlayerScreen.SetActive(false);
    }

    public void OpenGamemodeScreen()
    {
        mainMenu.SetActive(false);
        gameModeScreen.SetActive(true);
    }

    public void OpenMultiplayerModeScreen()
    {
        PlayerSelection.isNetworkedGame = true;
        SceneManager.LoadScene("Main");
    }
    
    public void SetNetworkMode(bool value)
    {
        isOnline = value;
        MultiplayerMode.SetActive(false);
        OfflineOnlineMode.SetActive(true);
    }
}
