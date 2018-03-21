using UnityEngine;
using UnityEngine.UI;

public class InternetChecker : MonoBehaviour
{
	private const bool allowCarrierDataNetwork = false;
	private const string pingAddress = "8.8.8.8"; // Google Public DNS server
	private const float waitingTime = 5.0f;
	public static bool internetConnectBool;
	private Ping ping;
	private float pingStartTime;

	public UIScript _ui;

    public Text connAvailabilityText;
    public GameObject ConnectionLostPanel;

	//public Text temp;

	public void Start()
	{
        if (ConnectionLostPanel != null)
            ConnectionLostPanel.SetActive(false);

        if (_ui == null && !NetworkTest.isLAN)
            InvokeRepeating("CheckInternetAvailability", 0.5f, 2f);
        //CheckInternetAvailability();
    }

    public void CheckInternetAvailability()
	{
		print ("Checking");
		bool internetPossiblyAvailable;
		switch (Application.internetReachability)
		{
		case NetworkReachability.ReachableViaLocalAreaNetwork:
			internetPossiblyAvailable = true;
			break;
		case NetworkReachability.ReachableViaCarrierDataNetwork:
			//internetPossiblyAvailable = allowCarrierDataNetwork;
			internetPossiblyAvailable = true;
			break;
		default:
			internetPossiblyAvailable = false;
			break;
		}

		if (!internetPossiblyAvailable)
		{
			InternetIsNotAvailable();
			return;
		}
		ping = new Ping(pingAddress);
		pingStartTime = Time.time;

		//AudioManager.Instance.UIClick();
	}

	public void Update()
	{
		if (ping != null)
		{
			bool stopCheck = true;
			if (ping.isDone)
				InternetAvailable();
			else if (Time.time - pingStartTime < waitingTime)
				stopCheck = false;
			else
				InternetIsNotAvailable();
			if (stopCheck)
				ping = null;
		}
	}

    public void InternetIsNotAvailable()
    {
        Debug.Log("No Internet");
        internetConnectBool = false;

        if (_ui != null)
        { 
            CheckIfInternet();
        }
        else
        {
            if(ConnectionLostPanel != null)
            {
                ConnectionLostPanel.SetActive(true);
                Invoke("WhenConnectionLost", 1f);
                Debug.Log("GoToMainMenu");
            }
        }


		//temp.text = internetConnectBool.ToString ();
	}

	public void InternetAvailable()
	{
		Debug.Log("Internet is available;)");

		internetConnectBool = true;

        if (_ui != null)
		    _ui.OpenMultiplayerModeScreen (false);	//open new scene only if internet is available.
		//temp.text = internetConnectBool.ToString ();
	}

    public void CheckIfInternet()
    {
        if (connAvailabilityText != null)
        {
            connAvailabilityText.gameObject.SetActive(true);
            Invoke("HideMessage", 3f);
        }
    }

    void HideMessage()
    {
        connAvailabilityText.gameObject.SetActive(false);
    }

    public void WhenConnectionLost()
    {
        //CancelInvoke("CheckInternetAvailability");
        DiscoverNetworks.Instance.GoToMainMenu();
    }

    public void OnDestroy()
    {
        CancelInvoke("CheckInternetAvailability");
    }
}