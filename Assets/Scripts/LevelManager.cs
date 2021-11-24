using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CinemachineVirtualCameraBase cam;

    private void Awake() 
    {
        instance = this;
    }

    public void Respawn()
    {
        GameObject player = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        cam.Follow = player.transform;
    }

}
