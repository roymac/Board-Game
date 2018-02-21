using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollCollisionController : MonoBehaviour {

    public GameManager gm;
    public UIManager um;
    public GameObject outerRing;
    public Material Ringmat;
    public ParticleSystem DiceSelectFeedbackParticle;
    public Animator animator;

    public Color red, blue, green, yellow;

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
        StopAllCoroutines();
        Ringmat.SetFloat("_EmissionStrength", 0);
        if (animator != null)
        animator.SetBool("RingAnime", false);

    }

    public void AnimateDiceHolder()
    {
        ParticleSystem.MainModule ma = DiceSelectFeedbackParticle.main;
        switch (gm.currentPlayerTurn)
        {
            case PawnColor.c_Blue:
                ma.startColor = Color.cyan;
                Ringmat.SetColor("_Color", blue);
                break;
            case PawnColor.c_Red:
                ma.startColor = Color.red;
                Ringmat.SetColor("_Color", red);
                break;
            case PawnColor.c_Yellow:
                ma.startColor = Color.yellow;
                Ringmat.SetColor("_Color", yellow);
                break;
            case PawnColor.c_Green:
                ma.startColor = Color.green;
                Ringmat.SetColor("_Color", green);
                break;

        }
        
        StartCoroutine(FadeMaterial(true, 0.5f));
        //DiceSelectFeedbackParticle.Play();
        if (animator != null)
            animator.SetBool("RingAnime", true);
    }

    IEnumerator FadeMaterial(bool fadeAway, float TimeToFade)
    {
        Debug.Log(fadeAway);
        // fade from opaque to transparent
        if (fadeAway)
        {
            for (float i = TimeToFade; i >= 0; i -= Time.deltaTime)
            {
                Ringmat.SetFloat("_EmissionStrength", i/TimeToFade );
                yield return null;
            }
            StartCoroutine(FadeMaterial(false,TimeToFade));
        }
        // fade from transparent to opaque
        else
        {
            for (float i = 0 ; i <= TimeToFade; i += Time.deltaTime)
            {
                Ringmat.SetFloat("_EmissionStrength", i/TimeToFade);
                yield return null;
            }
            StartCoroutine(FadeMaterial(true, TimeToFade));
        }
    }
}
