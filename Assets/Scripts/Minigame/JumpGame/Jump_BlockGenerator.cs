using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_BlockGenerator : MonoBehaviour
{
    public GameObject blockPrefab;
    void Start()
    {
        StartCoroutine(spawnCo());
    }

    void Update()
    {
        
    }

    IEnumerator spawnCo()
    {
        while (true)
        {
            yield return null;

            Instantiate(blockPrefab, new Vector2(Random.Range(-6.97f, 6.97f), 6.5f), Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(0.8f, 1.4f));
        }
    }
}
