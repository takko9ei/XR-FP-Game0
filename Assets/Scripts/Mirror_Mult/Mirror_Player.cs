using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Mirror_Player : NetworkBehaviour
{
    private GameObject following;

    private void Start()
    {
        StartCoroutine(waitReadyAndAuthor());
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three))
        {
            var go = GameObject.Find("electric bass");
            var id = go.GetComponent<NetworkIdentity>();
            CmdAutho(id, connectionToClient);
        }
        if (following != null && isLocalPlayer)
        {
            transform.position = following.transform.position;
        }
        else return;//make player pf follow camera
    }
    // Start is called before the first frame update
    public override void OnStartLocalPlayer()
    {
        following = GameObject.Find("CenterEyeAnchor");
    }

    void getAuth()
    {
        var go = GameObject.Find("electric guitar");
        var id = go.GetComponent<NetworkIdentity>();
        CmdAutho(id, connectionToClient);
    }

    [Command]
    void CmdAutho(NetworkIdentity identity, NetworkConnectionToClient connClient)
    {
        identity.RemoveClientAuthority();
        identity.AssignClientAuthority(connClient);
    }

    void authoObj(GameObject go)
    {
        var id = go.GetComponent<NetworkIdentity>();
        CmdAutho(id, connectionToClient);
    }

    IEnumerator waitReadyAndAuthor()
    {
        while (!NetworkClient.ready)
        {
            yield return new WaitForEndOfFrame();
        }

        getAuth();
    }
}
