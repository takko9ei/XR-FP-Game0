using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static OVRPlugin;

public class menufollow : MonoBehaviour
{

    public Camera sceneCamera;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float step; 
    // Start is called before the first frame update
    void Start()
    {
        transform.position = sceneCamera.transform.position + sceneCamera.transform.forward * 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        step = 5.0f * Time.deltaTime;
        targetPosition = sceneCamera.transform.position + sceneCamera.transform.forward * 3.0f;
        targetRotation = Quaternion.LookRotation(transform.position - sceneCamera.transform.position);

        transform.position = Vector3.Lerp(transform.position, targetPosition, step);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
    }
}
