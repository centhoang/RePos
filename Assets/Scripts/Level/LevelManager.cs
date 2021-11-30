using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{   
    [Header("-- Player Respawn --")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CinemachineVirtualCameraBase cam;

    [System.Serializable]
    public class ButtonGatePair 
    {
        public GameObject button;
        public GameObject gate;
    }
    [SerializeField] private List<ButtonGatePair> buttonGatePairs;

    [System.Serializable]
    public class KeyGatePair 
    {
        public GameObject key;
        public GameObject gate;
    }
    [SerializeField] private List<KeyGatePair> keyGatePairs;
    

    public static LevelManager instance;

    private void Awake() 
    {
        instance = this;
    }

    public void Respawn()
    {
        GameObject player = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        cam.Follow = player.transform;
    }

    private void FixedUpdate() 
    {
        if (buttonGatePairs.Count > 0)
        {
            for (int i = 0; i < buttonGatePairs.Count; i++)
            {
                if (buttonGatePairs[i].button.GetComponent<ButtonPressed>().pressed)
                {
                    buttonGatePairs[i].gate.GetComponent<GateControl>().buttonPressed = true;
                }
                else
                {
                    buttonGatePairs[i].gate.GetComponent<GateControl>().buttonPressed = false;
                }
            }
        }

        if (keyGatePairs.Count > 0)
        {
            for (int i = 0; i < keyGatePairs.Count; i++)
            {
                if (keyGatePairs[i].key.gameObject.activeSelf == false)
                {
                    keyGatePairs[i].gate.GetComponent<GateControl>().buttonPressed = true;
                }
            }
        }
    }

}
