using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayfabManager : MonoBehaviour
{
    void Start()
    {

    }

    public IEnumerator LinkGoogleAccount()
    {
        string url = "https://titleId.playfabapi.com/Client/LinkGoogleAccount";

        UnityWebRequest.Post(url, "");

        yield return new WaitForEndOfFrame();
    }

    void Update()
    {

    }
}
