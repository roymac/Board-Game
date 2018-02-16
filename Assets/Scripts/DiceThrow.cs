using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class DiceThrow : MonoBehaviour
{
    public Rigidbody diceObject;
    public List<Vector3> directions;
    public List<int> sideValues;
    public GameManager gm;
    public GameObject child;
    public float rollSpeed;
    bool rolled = false;
    public List<int> framveValues;

    public int EventFrameCount;
    public int currentEventCount = -1;
    int indexnum;
    int dicecount;
    void Awake()
    {
        GetComponent<Animator>().speed = 0;
        currentEventCount = -1;
        //diceObject = transform.GetComponent<Rigidbody>();
        SetDiceFaceValues(); 
        //diceObject.maxAngularVelocity = 90000000000;
    }

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.dice = this;
        if (GetComponent<NetworkIdentity>() != null)
        {
            if (!GetComponent<NetworkIdentity>().isServer)
            {
                GetComponent<Animator>().enabled = false;
            }
        }
        StartCoroutine(setSize());
    }

    IEnumerator setSize()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject pos = GameObject.Find("DicePosition");
        transform.parent = pos.transform;
        pos.transform.localScale = Vector3.one * 0.65f;
        transform.localPosition = Vector3.zero;
    }

    private void FixedUpdate()
    {
        
    }

    public void RemoveAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }

    void CheckAnims(int num)
    {
        if (currentEventCount == -1)
        {
            return;
        }
        if (currentEventCount < EventFrameCount)
        {
            currentEventCount++;
            float percentageDone = currentEventCount / EventFrameCount * 100f;
            if (percentageDone > 60)
            {
                GetComponent<Animator>().speed = 8 / percentageDone * 8;
            }
        }
        else
        {
            if (currentEventCount == EventFrameCount)
            {
                GetComponent<Animator>().speed = 0;
                currentEventCount = 0;
                gm.SetDiceVale(framveValues[num]);
            }
        }
    }

    public void ThrowDice()
    {
        EventFrameCount = Random.Range(10, 28);
        GetComponent<Animator>().speed = 8;
        currentEventCount = 0;

    }

    public void SetDiceFaceValues()
    {
        if (directions.Count == 0)
        {
            // Object space directions
            directions.Add(Vector3.up);
            sideValues.Add(4); // up
            directions.Add(Vector3.down);
            sideValues.Add(2); // down

            directions.Add(Vector3.left);
            sideValues.Add(1); // left
            directions.Add(Vector3.right);
            sideValues.Add(6); // right

            directions.Add(Vector3.forward);
            sideValues.Add(3); // fw
            directions.Add(Vector3.back);
            sideValues.Add(4); // back
        }

        // Assert equal side of lists
        if (directions.Count != sideValues.Count)
        {
            Debug.LogError("Not consistent list sizes");
        }
    }


    public int GetNumber(Vector3 referenceVectorUp, float epsilonDeg = 5f)
    {
        Vector3 referenceObjectSpace = transform.InverseTransformDirection(referenceVectorUp);

        //float min = float.MaxValue;
        int mostSimilarDirectionIndex = -1;

        for (int i = 0; i < directions.Count; ++i)
        {
            float a = Vector3.Angle(referenceObjectSpace, directions[i]);
            if (a <= epsilonDeg)
            {
                //min = a;
                mostSimilarDirectionIndex = i;
            }
        }

        // -1 as error code for not within bounds
        return (mostSimilarDirectionIndex >= 0) ? sideValues[mostSimilarDirectionIndex] : -1;
    }

    IEnumerator FadeMaterial(bool fadeAway, float TimeToFade)
    {
        Material mat = GetComponent<MeshRenderer>().material;
        // fade from opaque to transparent
        if (fadeAway)
        {
            yield return new WaitForSeconds(0.8f);
            for (float i = TimeToFade; i >= 0.3f; i -= Time.deltaTime)
            {
                mat.SetFloat("_Progress", i/TimeToFade );
                yield return null;
            }
           
        }
        // fade from transparent to opaque
        else
        {
            if (mat.GetFloat("_Progress") < 1)
            {
                for (float i = 0.3f ; i <= TimeToFade; i += Time.deltaTime)
                {
                    mat.SetFloat("_Progress", i/TimeToFade);
                    yield return null;
                }
            }
            yield return null;
        }
    }

    public void ShowDice(bool fadeout)
    {
        StopAllCoroutines();
        StartCoroutine(FadeMaterial(fadeout, 1f));
    }
}