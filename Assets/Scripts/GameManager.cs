using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private List<Card> allCards;

    private Card flippedCard;
    private bool isFlipping = false;

    [SerializeField]
    private Slider timeoutSlider;

    [SerializeField]
    private TextMeshProUGUI timeoutText;

    [SerializeField]
    private float timeLimit = 60f;

    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private TextMeshProUGUI gameOverText;

    private float currentTime;

    private int totalMatches = 10;

    private float matchesFound = 0;

    private Boolean isGameOver = false;

    // 시작시 인스턴스 초기화

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Board board = FindObjectOfType<Board>();
        allCards = board.getCardList();

        currentTime = timeLimit;

        SetCurrentTimeText();

        StartCoroutine("FlipAllCardsRoutine");
    }

    // 시간 텍스트 설정
    void SetCurrentTimeText()
    {
        int timeSecond = Mathf.CeilToInt(currentTime);
        timeoutText.SetText(timeSecond.ToString());
    }

    // 시작시 전체 카드를 보여주기
    IEnumerator FlipAllCardsRoutine()
    {
        isFlipping = true;
        yield return new WaitForSeconds(0.5f);
        FlipAllCards();
        yield return new WaitForSeconds(3f);
        FlipAllCards();
        yield return new WaitForSeconds(0.5f);
        isFlipping = false;

        yield return StartCoroutine("CountdownTimerRoutine");
    }

    // 카운트 다운 로직
    IEnumerator CountdownTimerRoutine()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timeoutSlider.value = currentTime / timeLimit;
            SetCurrentTimeText();
            yield return null;
        }

        GameOver(false);
    }

    // 전체 카드 뒤집기
    void FlipAllCards()
    {
        foreach (Card card in allCards)
        {
            card.FlipCard();
        }
    }

    // 카드 클릭 시 카드 매칭의 확인
    public void CardClicked(Card card)
    {
        if (isFlipping || isGameOver)
        {
            return;
        }

        card.FlipCard();

        if (flippedCard != null)
        {
            StartCoroutine(CheckMatchRoutine(flippedCard, card));
        }
        else
        {
            flippedCard = card;
        }
    }

    // 카드 매칭 실 로직
    IEnumerator CheckMatchRoutine(Card card1, Card card2)
    {
        isFlipping = true;

        if (card1.cardId == card2.cardId)
        {
            card1.SetMatched();
            card2.SetMatched();
            matchesFound++;

            if (matchesFound == totalMatches)
            {
                GameOver(true);
            }
        }
        else
        {
            yield return new WaitForSeconds(1f);

            card1.FlipCard();
            card2.FlipCard();

            yield return new WaitForSeconds(0.4f);
        }

        isFlipping = false;
        flippedCard = null;
    }

    // 게임 오버 처리
    void GameOver(bool success)
    {

        if (!isGameOver)
        {

            isGameOver = true;

            StopCoroutine("CountdownTimerRoutine");

            if (success)
            {
                gameOverText.SetText("SUCCESS !");
            }
            else
            {
                gameOverText.SetText("GAME OVER");
            }


            Invoke("ShowGameOverPanel", 2f);
        }
    }

    // 게임 오버 페널 보여주기
    void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    // 재시작
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
