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
        ItemManager _im = other.GetComponent<ItemManager>();

        if (_im.hasBought)
        {
            _im.CanSelectThisBoard(true);
        }
        else if (!_im.hasBought)
        {
            _im.CanSelectThisBoard(false);
        }


        this.GetComponent<SelectPlayField>().SelectThisBoard(other.GetComponent<ItemManager>().boardIndex);     //Set this as the board
		//AudioManager.Instance.songNumber = other.GetComponent<ItemManager>().boardIndex + 1;
        SnapNumbers ();
        
	}

	public void OnTriggerExit(Collider other)
	{
		contentName = "100";
	}

	public void OnTriggerStay(Collider other)
	{
        ItemManager _im = other.GetComponent<ItemManager>();
        //StopAtZero();
        if (_im.hasBought)
        {
            _im.CanSelectThisBoard(true);
        }

    }

	//void StopAtZero()
	//{
	//	if(contentName == "0" && _scrollRect.velocity.y < -300)
	//		_scrollRect.velocity = Vector2.Lerp(_scrollRect.velocity, Vector2.zero, lerpSpeed);
	//}

	void SnapNumbers()
	{
		
	}
}
