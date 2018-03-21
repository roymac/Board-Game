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

    public float lerpSpeed;
    float minDistance;

    void Start()
	{
		itemLength = item.Length;
		distance = new float[itemLength];

		// Get distance between buttons
		itemDistance  = (int)Mathf.Abs(item[1].GetComponent<RectTransform>().anchoredPosition.y - item[0].GetComponent<RectTransform>().anchoredPosition.y);
	}

	void Update()
	{
		for (int i = 0; i < item.Length; i++)
		{
			float dist = center.GetComponent<RectTransform>().position.x - item[i].GetComponent<RectTransform>().position.x;
			distance[i] = Mathf.Abs(dist);
		}
	
		minDistance = Mathf.Min(distance);	// Get the min distance

		for (int a = 0; a < item.Length; a++)
		{
			if (minDistance == distance[a])
			{
				minItemNum = a;
			}
		}

        if (minDistance <= 450f)
        {
            //LerpToitem(minButtonNum * -itemDistance);
            LerpToitem(-item[minItemNum].GetComponent<RectTransform>().anchoredPosition.x);
        }

    }   

	void LerpToitem(float position)
	{
		float newX = Mathf.Lerp (panel.anchoredPosition.x, position, Time.deltaTime * lerpSpeed);
		Vector2 newPosition = new Vector2 (newX, panel.anchoredPosition.y);

		panel.anchoredPosition = newPosition;
	}

	public void OnMouseDown()
	{
		dragging = true;
    }

	public void OnMouseUp()
	{
		dragging = false;
    }

}
	