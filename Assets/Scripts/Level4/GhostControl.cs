using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostControl : MonoBehaviour
{
    [SerializeField] float minPos, maxPos;
    [SerializeField] bool horizontal, vertical;
    bool min = true, max;
    void Start()
    {
        
    }
    void Update()
    {
        if (horizontal)
        {
            if (transform.position.x > maxPos)
            {
                min = true;
                max = false;
            }
            else if (transform.position.x < minPos)
            {
                min = false;
                max = true;
            }
            if (min)
            {
                transform.Translate(-Vector3.right * Time.deltaTime * 4);
            }
            else if (max)
            {
                transform.Translate(Vector3.right * Time.deltaTime * 4);
            }
        }
        else if (vertical)
        {
            if (transform.position.z > maxPos)
            {
                min = true;
                max = false;
            }
            else if (transform.position.z < minPos)
            {
                min = false;
                max = true;
            }
            if (min)
            {
                transform.Translate(-Vector3.up * Time.deltaTime * 4);
            }
            else if (max)
            {
                transform.Translate(Vector3.up * Time.deltaTime * 4);
            }
        }
    }
}
