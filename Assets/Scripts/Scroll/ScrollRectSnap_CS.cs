using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollRectSnap_CS : MonoBehaviour 
{
    public ScrollRect _scrollRect;
	public RectTransform panel;	// To hold the ScrollPanel
	public Image[] item;
	public RectTransform center;	// Center to compare the distance for each button

	public float[] distance;	// All buttons' distance to the center
	public int dragging = 0;	// Will be true, while we drag the panel
	public int itemDistance;	// Will hold the distance between the buttons
	public int minItemNum;	// To hold the number of the button, with smallest distance to center
	public int itemLength;
    public int itemNum;

    public float lerpSpeed;
    public float minDistance;
    public bool clicked = false;

    void Start()
	{
		itemLength = item.Length;
		distance = new float[itemLength];

		// Get distance between buttons
		//itemDistance  = (int)Mathf.Abs(item[1].GetComponent<RectTransform>().anchoredPosition.y - item[0].GetComponent<RectTransform>().anchoredPosition.y);
	}

	void Update()
	{
        for (int i = 0; i < item.Length; i++)
        {
            float dist = center.GetComponent<RectTransform>().position.x - item[i].GetComponent<RectTransform>().position.x;
            distance[i] = Mathf.Abs(dist);
        }

        minDistance = Mathf.Min(distance);	// Get the min distance

        if (dragging == -1)
        {
            LerpToitem(-item[itemNum].GetComponent<RectTransform>().anchoredPosition.x);
            if (minDistance < 1 && !clicked)
            {
                //LerpToitem(minButtonNum * -itemDistance);
                dragging = 0;
            }
        }
        else if (dragging == 1)
        {
            LerpToitem(-item[itemNum].GetComponent<RectTransform>().anchoredPosition.x);
            if (minDistance < 1 && !clicked)
            {
                //LerpToitem(minButtonNum * -itemDistance);
                dragging = 0;
            }
        }

    }   

	void LerpToitem(float position)
	{
		float newX = Mathf.Lerp (panel.anchoredPosition.x, position, Time.deltaTime * lerpSpeed);
		Vector2 newPosition = new Vector2 (newX, panel.anchoredPosition.y);

		panel.anchoredPosition = newPosition;

        if(minDistance > 5)
        {
            clicked = false;
        }
    }

	public void MoveLeft()
	{
        if (itemNum > 0 && itemNum <= item.Length - 1)
        {
            itemNum--;
            dragging = 1;
            clicked = true;
        }
        //_scrollRect.horizontalNormalizedPosition += (0.5f);

    }

	public void MoveRight()
	{
        if (itemNum >= 0 && itemNum < item.Length - 1)
        {
            itemNum++;
            dragging = -1;
            clicked = true;
        }
        // _scrollRect.horizontalNormalizedPosition -= (0.5f);
    }

}
	