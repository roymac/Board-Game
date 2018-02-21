using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollRectSnap_CS : MonoBehaviour 
{
	public RectTransform panel;	// To hold the ScrollPanel
	public Image[] item;
	public RectTransform center;	// Center to compare the distance for each button

	public float[] distance;	// All buttons' distance to the center
	public bool dragging = false;	// Will be true, while we drag the panel
	public int itemDistance;	// Will hold the distance between the buttons
	public int minItemNum;	// To hold the number of the button, with smallest distance to center
	public int itemLength;

	void Start()
	{
		itemLength = item.Length;
		distance = new float[itemLength];

		// Get distance between buttons
		itemDistance  = (int)Mathf.Abs(item[1].GetComponent<RectTransform>().anchoredPosition.x - item[0].GetComponent<RectTransform>().anchoredPosition.x);
	}

	void Update()
	{
		for (int i = 0; i < item.Length; i++)
		{
			float dist = center.GetComponent<RectTransform>().position.y - item[i].GetComponent<RectTransform>().position.y;
			distance[i] = Mathf.Abs(dist);
		}
	
		float minDistance = Mathf.Min(distance);	// Get the min distance

		for (int a = 0; a < item.Length; a++)
		{
			if (minDistance == distance[a])
			{
				minItemNum = a;
			}
		}

		if (!dragging)
		{
			//LerpToitem(minButtonNum * -itemDistance);
			LerpToitem (-item[minItemNum].GetComponent<RectTransform>().anchoredPosition.y);
		}
	}

	void LerpToitem(float position)
	{
		float newX = Mathf.Lerp (panel.anchoredPosition.y, position, Time.deltaTime * 5f);
		Vector2 newPosition = new Vector2 (panel.anchoredPosition.x, newX);

		panel.anchoredPosition = newPosition;
	}

	public void StartDrag()
	{
		dragging = true;
	}

	public void EndDrag()
	{
		dragging = false;
	}

}
	