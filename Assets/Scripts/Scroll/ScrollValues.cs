using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollValues: MonoBehaviour {

	public string contentName; 
	public ScrollRect _scrollRect;
	public float threshold = 1;

	public int currentSlot;
	// Use this for initialization
	void Start () {
		_scrollRect = this.GetComponentInParent<ScrollRect> ();
		contentName = "0";
	}
	
	// Update is called once per frame
	void Update () {
		//print (_scrollRect.velocity);
	}

	public void OnTriggerEnter(Collider other)
	{
		contentName = other.GetComponent<Collider> ().name;
		SnapNumbers ();
	}

	public void OnTriggerExit(Collider other)
	{
		contentName = "100";
	}

	public void OnTriggerStay(Collider other)
	{
		StopAtZero();
	}

	void StopAtZero()
	{
		if(contentName == "0" && _scrollRect.velocity.y < -300)
			_scrollRect.velocity = Vector2.Lerp(_scrollRect.velocity, Vector2.zero, 25f);
	}

	void SnapNumbers()
	{
		
	}
}
