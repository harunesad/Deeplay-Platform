using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerControl2 : MonoBehaviour
{
    public bool isGround = false, pressed = true, move;
    public float turnSmoothTime = .5f, turnSmoothVelocity;
    public Vector3 playerOffset, desiredPosition;
    public float smoothSpeed = 0.125f;
    public int targetId;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 10;
    [SerializeField] LayerMask jumpLayer, itemLayer;
    [SerializeField] GameObject player2, cat;
    [SerializeField] List<ItemCollect> items;
    [SerializeField] UIManager1 uIManager;
    Rigidbody rb;
    [SerializeField] List<Transform> target;
    float inputX;
    float inputZ;
    Vector3 movement;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        Time.timeScale = 1;
        for (int i = 0; i < items.Count; i++) 
        {
            items[i].itemPos = items[i].item.transform.position;
        }
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) ||
    Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            pressed = true;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            pressed = false;

        Jump();
        desiredPosition = transform.position + playerOffset;
        Vector3 smoothedPosition = Vector3.Lerp(Camera.main.transform.position, desiredPosition, smoothSpeed * 10);
        Camera.main.transform.position = smoothedPosition;
        Camera.main.transform.LookAt(transform.position);

        player2.GetComponent<NavMeshAgent>().SetDestination(transform.position - transform.forward - transform.right);
        cat.GetComponent<NavMeshAgent>().SetDestination(transform.position - transform.forward + transform.right);
        player2.transform.LookAt(transform.position);
        player2.transform.localEulerAngles = new Vector3(0, player2.transform.localEulerAngles.y - 90, 0);
        cat.transform.LookAt(transform.position);
        cat.transform.localEulerAngles = new Vector3(0, cat.transform.localEulerAngles.y - 180, 0);
    }
    void FixedUpdate()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputZ = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(-inputZ, 0, inputX).normalized;
        if (direction.magnitude >= .1f)
        {
            float targetAngle = (float)Math.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        if (!pressed && !move)
        {
            movement = Vector3.forward;
            transform.Translate(movement * moveSpeed * Time.deltaTime);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 12)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!items[i].isPlace && items[i].item == collision.gameObject)
                {
                    collision.gameObject.SetActive(false);
                    collision.transform.position = items[i].itemPos;
                    items[i].isPlace = true;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].isPlace)
                {
                    items[i].item.SetActive(true);
                    items.RemoveAt(i);
                    i--;
                    if (items.Count != 2)
                    {
                        for (int j = 0; j < target.Count; j++)
                        {
                            target[j].transform.GetChild(targetId).gameObject.SetActive(false);
                        }
                        targetId++;
                        for (int j = 0; j < target.Count; j++)
                        {
                            target[j].transform.GetChild(targetId).gameObject.SetActive(true);
                        }
                    }
                    if (items.Count == 0)
                    {
                        uIManager.NextLevelFrameOpen();
                    }
                }
            }
        }
    }
    void Jump()
    {
        IsGround();
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }
    void IsGround()
    {
        if (Physics.Raycast(transform.position + (transform.forward * .25f), Vector2.down, 2.1f, jumpLayer))
        {
            isGround = true;
        }
        else if (Physics.Raycast(transform.position + (transform.forward * -1 * .25f), Vector2.down, 2.1f, jumpLayer))
        {
            isGround = true;
        }
        else if (Physics.Raycast(transform.position + (transform.right * .25f), Vector2.down, 2.1f, jumpLayer))
        {
            isGround = true;
        }
        else if (Physics.Raycast(transform.position + (transform.right * -1 * .25f), Vector2.down, 2.1f, jumpLayer))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }
}
[Serializable]
public class ItemCollect
{
    public bool isPlace;
    public GameObject item;
    public Vector3 itemPos;
}
