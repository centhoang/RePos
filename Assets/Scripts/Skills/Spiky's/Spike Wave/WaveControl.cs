using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveControl : MonoBehaviour
{
    [SerializeField] private float nextSpikeDelay = 0.3f;
    [SerializeField] private float destroyObjDelay = 1f;
    private List<GameObject> spikeList;

    void Awake() 
    {
        spikeList = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            spikeList.Add(transform.GetChild(i).gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(StartWave());
    }

    IEnumerator StartWave()
    {
        foreach (var spike in spikeList)
        {
            spike.gameObject.SetActive(true);
            yield return new WaitForSeconds(nextSpikeDelay);
        }

        yield return new WaitForSeconds(destroyObjDelay);
        Destroy(this.gameObject);
    }
}
