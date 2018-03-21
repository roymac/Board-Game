using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsRoll : MonoBehaviour {

	public UIScript _ui;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void DoneShowingCredits()
	{
		_ui.ShowCredits (false);
	}
}
