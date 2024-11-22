using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager1 : MonoBehaviour
{
    public bool isPuzzle;
    [SerializeField] List<Puzzles> puzzles;
    [SerializeField] GameObject dialog, resumeFrame, nextLevelFrame, gameOverFrame;
    [SerializeField] TextMeshProUGUI instructionText;
    [SerializeField] TMP_InputField answerText;
    [SerializeField] Button answerBtn, puzzleCloseBtn, nextLevelBtn, finalRestartBtn, resumeBtn, resumeRestartBtn, gameoverRestartBtn;
    [SerializeField] PlayerControl1 playerControl1;
    [SerializeField] string instruction;
    public Image interact;
    public TextMeshProUGUI questionText;
    int questionId;
    private void Awake()
    {
        resumeRestartBtn.onClick.AddListener(RestartGame);
        finalRestartBtn.onClick.AddListener(RestartGame);
        if (gameoverRestartBtn)
        {
            gameoverRestartBtn.onClick.AddListener(RestartGame);
        }
        if (nextLevelBtn)
        {
            nextLevelBtn.onClick.AddListener(NextLevel);
        }
        resumeBtn.onClick.AddListener(ResumeGame);
        if (answerBtn)
        {
            answerBtn.onClick.AddListener(Answer);
        }
        if (puzzleCloseBtn)
        {
            puzzleCloseBtn.onClick.AddListener(PuzzleClose);
        }
    }
    void Start()
    {
        instructionText.text = instruction;
    }
    private void Update()
    {
        if (((dialog && !dialog.gameObject.activeSelf) || !dialog) && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!resumeFrame.gameObject.activeSelf)
            {
                if (interact && interact.gameObject.activeSelf)
                {
                    interact.gameObject.SetActive(false);
                }
                resumeFrame.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                resumeFrame.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
    void ResumeGame()
    {
        resumeFrame.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void QuestionToAnswer()
    {
        if (!interact.gameObject.activeSelf && !dialog.gameObject.activeSelf && !resumeFrame.gameObject.activeSelf && isPuzzle)
        {
            interact.gameObject.SetActive(true);
        }
        if (!dialog.gameObject.activeSelf && isPuzzle)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                interact.gameObject.SetActive(false);
                isPuzzle = false;
                questionId = Random.Range(0, puzzles.Count);
                questionText.text = puzzles[questionId].question;
                dialog.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
    void Answer()
    {
        if (answerText.text == puzzles[questionId].answer)
        {
            dialog.SetActive(false);
            Time.timeScale = 1;
            playerControl1.transform.position = playerControl1.stepPos;
        }
    }
    void PuzzleClose()
    {
        dialog.SetActive(false);
        Time.timeScale = 1;
    }
    public void NextLevelFrameOpen()
    {
        Time.timeScale = 0;
        nextLevelFrame.SetActive(true);
    }
    public void GameoverFrameOpen()
    {
        Time.timeScale = 0;
        gameOverFrame.SetActive(true);
    }
    void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
[System.Serializable]
public class Puzzles
{
    public string question;
    public string answer;
}
