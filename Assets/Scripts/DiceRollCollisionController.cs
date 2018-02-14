using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollCollisionController : MonoBehaviour {

    public GameManager gm;
    public UIManager um;
    public GameObject outerRing;
    public Material Ringmat;
    public ParticleSystem DiceSelectFeedbackParticle;
	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void OnMouseDown()
    {
        gm.ThrowTheDice();
        um.DeactivateSpinButton();
        DiceSelectFeedbackParticle.Stop();

    }

    public void AnimateDiceHolder()
    {
        ParticleSystem.MainModule ma = DiceSelectFeedbackParticle.main;
        switch (gm.currentPlayerTurn)
        {
            case PawnColor.c_Blue:
                ma.startColor = Color.cyan;
                break;
            case PawnColor.c_Red:
                ma.startColor = Color.red;
                break;
            case PawnColor.c_Yellow:
                ma.startColor = Color.yellow;
                break;
            case PawnColor.c_Green:
                ma.startColor = Color.green;
                break;

        }
        //StartCoroutine(FadeMaterial(true, 0.5f));
        DiceSelectFeedbackParticle.Play();
    }

    IEnumerator FadeMaterial(bool fadeAway, float TimeToFade)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            for (float i = TimeToFade; i >= 0; i -= Time.deltaTime)
            {
                Ringmat.SetFloat("_Progress", i/TimeToFade );
                yield return null;
            }
            StartCoroutine(FadeMaterial(false,TimeToFade));
        }
        // fade from transparent to opaque
        else
        {
            for (float i = 0 ; i <= TimeToFade; i += Time.deltaTime)
            {
                Ringmat.SetFloat("_Progress", i/TimeToFade);
                yield return null;
            }
        }
    }
}
