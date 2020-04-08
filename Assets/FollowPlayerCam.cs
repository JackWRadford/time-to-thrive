using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowPlayerCam : MonoBehaviour
{
    private GameObject player;
    private CinemachineVirtualCamera virtualCamera;
    // Start is called before the first frame update
    void Start()
    {
        var virtualCamera = GetComponent<CinemachineVirtualCamera>();
        player = GameObject.FindWithTag("Player");

        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
