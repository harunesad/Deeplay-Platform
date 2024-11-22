using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl1 : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 10;
    [SerializeField] LayerMask jumpLayer, puzzleLayer, appleLayer, collectableLayer;
    [SerializeField] UIManager1 uIManager;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] int score;
    [SerializeField] List<CollectableItems> itemCollects;
    public bool isGround = false, isStarted = false;
    Rigidbody rb;
    Vector3 startPos;
    public Vector3 stepPos;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }
    private void Start()
    {
        Time.timeScale = 1;
    }
    void Update()
    {
        RaycastHit hit;
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            uIManager.isPuzzle = true;
        }
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2, puzzleLayer))
        {
            uIManager.QuestionToAnswer();
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                uIManager.questionText.text = hit.transform.name;
            }
        }
        else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4, appleLayer) && hit.transform.position.y > 1 && score != 100)
        {
            uIManager.interact.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                hit.rigidbody.GetComponent<BoxCollider>().isTrigger = true;
                hit.rigidbody.AddForce(transform.forward * 10000);
                score += 10;
                scoreText.text = score.ToString();
                if (score == 100)
                {
                    uIManager.NextLevelFrameOpen();
                }
            }
        }
        else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4, collectableLayer))
        {
            uIManager.interact.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                for (int i = 0; i < itemCollects.Count; i++)
                {
                    if (itemCollects[i].items == hit.transform.gameObject)
                    {
                        hit.transform.gameObject.SetActive(false);
                        itemCollects[i].oldBuild.transform.gameObject.SetActive(false);
                        itemCollects[i].newBuild.transform.gameObject.SetActive(true);
                        if (itemCollects[i].newBuildItem)
                        {
                            itemCollects[i].newBuildItem.transform.gameObject.SetActive(true);
                        }
                        itemCollects.RemoveAt(i);
                        if (itemCollects.Count == 0)
                        {
                            uIManager.NextLevelFrameOpen();
                        }
                    }
                }
            }
        }
        else
        {
            uIManager.interact.gameObject.SetActive(false);
        }
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 2, Color.red);
        Jump();
    }
    void FixedUpdate()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            MovePlayer();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            uIManager.NextLevelFrameOpen();
        }
        else if (collision.gameObject.layer == 7)
        {
            isStarted = true;
            stepPos = transform.position;
        }
        else if (collision.gameObject.layer == 6)
        {
            if (isStarted)
            {
                uIManager.isPuzzle = true;
            }
            isStarted = false;
        }
        else if (collision.gameObject.layer == 9)
        {
            isStarted = false;
            transform.position = startPos;
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
    void MovePlayer()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement = cameraForward * Input.GetAxisRaw("Vertical") + cameraRight * Input.GetAxisRaw("Horizontal");
        movement.Normalize();

        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
[Serializable]
public class CollectableItems
{
    public GameObject items;
    public GameObject newBuild;
    public GameObject oldBuild;
    public GameObject newBuildItem;
}
