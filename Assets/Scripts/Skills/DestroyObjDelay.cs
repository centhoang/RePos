using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjDelay : MonoBehaviour
{
    [SerializeField] private float destroyObjAfter = 18f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy());
    }

    public IEnumerator Destroy() 
    {
        yield return new WaitForSeconds(destroyObjAfter);
        Destroy(this.gameObject);
    }
}
