using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleControl : MonoBehaviour
{
    [SerializeField] float negativePosX, positivePosX,negativePosZ, positivePosZ;
    [SerializeField] GameObject applePrefab;
    [SerializeField] float startTime, repeatTime;
    void Start()
    {
        InvokeRepeating("SpawnApple", startTime, repeatTime);
    }
    void Update()
    {
        
    }
    void SpawnApple()
    {
        Vector3 spawnPos = new Vector3(Random.Range(negativePosX, positivePosX), 10, Random.Range(negativePosZ, positivePosZ));
        var apple = Instantiate(applePrefab, spawnPos, Quaternion.identity);
        Destroy(apple, 10);
    }
}
