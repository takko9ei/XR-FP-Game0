using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;



public class handlealign : MonoBehaviour
{
    public GameObject anchorPf;
    public GameObject anchorGo;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(placeAnchor());
        }
        else
        {
            StartCoroutine(clientGetAnchor());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var anchor = anchorGo.GetComponent<OVRSpatialAnchor>();
        if (anchor.Created)
        {
            Align(anchor);
        }
    }

    IEnumerator placeAnchor()
    {
        anchorGo = PhotonNetwork.Instantiate("ssaPrefab",new Vector3(0,0,0),Quaternion.identity);
        var anchor = anchorGo.GetComponent<OVRSpatialAnchor>();
        yield return new WaitUntil(() => anchor.Created);
        Align(anchor);
    }

    IEnumerator clientGetAnchor()
    {
        var anchor = anchorGo.GetComponent<OVRSpatialAnchor>();
        yield return new WaitUntil(() => anchor.Created);
        Align(anchor);
        //syncro
        //load
        //align
    }

    private void Align(OVRSpatialAnchor anchor)
    {
        // Align the scene by transforming the camera.
        // The inverse anchor pose is used to move the camera so that the scene appears as if it was parented to the anchor.

        // Get the anchor's raw tracking space pose to align the camera.
        // Note that the anchor's world space pose is dependent on the camera position, in order to maintain consistent world-locked rendering.
        OVRPose trackingSpacePose;
        if (!TryGetPose(anchor, out trackingSpacePose))
        {
            this.enabled = false;
            return;
        }

        // Position the anchor in tracking space
        Transform anchorTransform = anchor.transform;
        anchorTransform.SetPositionAndRotation(trackingSpacePose.position, trackingSpacePose.orientation);

        // Transform the camera to the inverse of the anchor pose to align the scene
        transform.position = anchorTransform.InverseTransformPoint(Vector3.zero);
        transform.eulerAngles = new Vector3(0, -anchorTransform.eulerAngles.y, 0);

        // Update the world space position of the anchor so it renders in a consistent world-locked position.
        OVRPose worldSpacePose = trackingSpacePose.ToWorldSpacePose(Camera.main);
        anchorTransform.SetPositionAndRotation(worldSpacePose.position, worldSpacePose.orientation);
    }

    private static bool TryGetPose(OVRSpatialAnchor anchor, out OVRPose pose)
    {
        if (anchor == null)
        {
            Debug.Log(" Unable to get anchor pose, anchor is null.");
            pose = OVRPose.identity;
            return false;
        }

        if (!OVRPlugin.TryLocateSpace(anchor.Space, OVRPlugin.GetTrackingOriginType(), out var posef))
        {
            Debug.Log("Unable to get anchor pose for anchor {anchor.Space}.");
            pose = OVRPose.identity;
            return false;
        }

        pose = posef.ToOVRPose();
        return true;
    }
}
