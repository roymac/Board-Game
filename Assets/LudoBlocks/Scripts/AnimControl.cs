using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimControl : MonoBehaviour {

	public Animator menuElement;

	public bool showingMenu;

	// Use this for initialization
	void Start () {
		//menuElement = this.GetComponent<Animator> ();
		showingMenu = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HideMenuBGAnim()
	{
		menuElement.SetTrigger ("MenuTransition");
	}

	public void GoToNextScreen(GameObject toScreen)
	{
		StartCoroutine(SwitchScreen(toScreen));
	}


	public void ShowMenuElementAnim()
	{
		menuElement.SetTrigger ("MenuTransition");
		AudioManager.Instance.UIClick ();
	}

	public IEnumerator SwitchScreen(GameObject toScreen)
	{
		HideMenuBGAnim ();

		yield return new WaitForSeconds (0.7f);

		toScreen.GetComponent<Animator>().SetTrigger ("ShowMenu");
	}



	public void SetSettingsMenu()
	{
		showingMenu = !showingMenu;

		ShowSettingsPanel (showingMenu);

		AudioManager.Instance.UIClick();
	}


	public void ShowSettingsPanel(bool showMenu)
	{
		if (showMenu) {
			menuElement.SetTrigger ("ShowMenu");
		}
		else if (!showMenu)
		{
			menuElement.SetTrigger("HideSettingMenu");
		}
	}

}
