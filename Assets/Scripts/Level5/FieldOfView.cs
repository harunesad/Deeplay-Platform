using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] UIManager1 uIManager;
    Transform parent;

    private bool rotatingForward = false;
    public float minAngle = 0f; 
    public float maxAngle = 90f; 
    public float speed = 50f; 
    void Start()
    {
        parent = transform.parent;
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),  
            new Vector3(7, 0, 0),  
            new Vector3(0, 0, 7)   
        };

        int[] triangles = new int[]
        {
            0, 1, 2 
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }
    void Update()
    {
        float targetAngle = rotatingForward ? maxAngle : minAngle;
        float step = speed * Time.deltaTime;
        float newAngle = Mathf.MoveTowardsAngle(transform.parent.eulerAngles.y, targetAngle, step);
        transform.parent.eulerAngles = new Vector3(transform.parent.eulerAngles.x, newAngle, transform.parent.eulerAngles.z);

        if (Mathf.Abs(newAngle - targetAngle) < 0.1f)
        {
            rotatingForward = !rotatingForward; 
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 14)
        {
            uIManager.GameoverFrameOpen();
        }
    }
}
