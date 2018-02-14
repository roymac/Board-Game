using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour 
{

    public delegate void OnYellowPawnUnlock(int num);
    public delegate void OnRedPawnUnlock(int num);
    public delegate void OnBluePawnUnlock(int num);
    public delegate void OnGreenPawnUnlock(int num);

    public delegate void OnSelected();

    public delegate void OnOnePlayerout();

    public static event OnYellowPawnUnlock UnlockYellowPawns;
    public static event OnRedPawnUnlock UnlockRedPawns;
    public static event OnBluePawnUnlock UnlockBluePawns;
    public static event OnGreenPawnUnlock UnlockGreenPawns;
    public static event OnSelected onSelected;
    public static event OnOnePlayerout onePlayerOut;

    public static void PawnUnlockGreen(int num)
    {
        UnlockGreenPawns(num);
    }

    public static void PawnUnlockBlue(int num)
    {
        UnlockBluePawns(num);
    }

    public static void PawnUnlockRed(int num)
    {
        UnlockRedPawns(num);
    }

    public static void PawnUnlockYellow(int num)
    {
        UnlockYellowPawns(num);
    }

    public static void PlayerSelected()
    {
        onSelected();
    }

    public static void CallOnePlayerOut()
    {
        onePlayerOut();
    }

}
