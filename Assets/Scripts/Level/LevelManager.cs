using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{   
    [Header("-- Player Respawn --")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CinemachineVirtualCameraBase cam;

    // Button Gate pairs
    [System.Serializable]
    public class ButtonGatePair 
    {
        public GameObject button;
        public GameObject gate;
    }
    [SerializeField] private List<ButtonGatePair> buttonGatePairs;

    // Key Gate pairs
    [System.Serializable]
    public class KeyGatePair 
    {
        public GameObject key;
        public GameObject gate;
    }
    [SerializeField] private List<KeyGatePair> keyGatePairs;

    // Key Spikes pairs
    [System.Serializable]
    public class KeySpikesPair 
    {
        public GameObject key;
        public List<GameObject> listOfSpikes;
    }
    [SerializeField] private List<KeySpikesPair> keySpikesPairs;
    
    // Reverse Gates pairs
    [System.Serializable]
    public class ReverseGatesPair
    {
        public GameObject controlGate;
        public GameObject controlledGate;
    }
    [SerializeField] private List<ReverseGatesPair> ReverseGatesPairs;

    public static LevelManager instance;

    private void Awake() 
    {
        instance = this;
    }

    public void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // GameObject player = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        // cam.Follow = player.transform;
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

        if (keySpikesPairs.Count > 0)
        {
            for (int i = 0; i < keySpikesPairs.Count; i++)
            {
                if (keySpikesPairs[i].key.gameObject.activeSelf == false)
                {
                    foreach (var spikeHolder in keySpikesPairs[i].listOfSpikes)
                    {
                        spikeHolder.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
            }
        }

        if (ReverseGatesPairs.Count > 0)
        {
            foreach (var pair in ReverseGatesPairs)
            {
                pair.controlledGate.GetComponent<GateControl>().buttonPressed = !pair.controlGate.GetComponent<GateControl>().buttonPressed;
            }
        }
    }
}
