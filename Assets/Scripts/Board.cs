using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    private Sprite[] cardSprites;

    private List<int> cardIdList = new List<int>();

    private List<Card> cardList = new List<Card>();


    // Start is called before the first frame update
    void Start()
    {
        GenerateCardId();
        ShuffleCardId();
        InitBoard();
    }

    // 카드 매칭을 위한 고유 id 부여
    void GenerateCardId()
    {
        for (int i = 0; i < cardSprites.Length; i++)
        {
            cardIdList.Add(i);
            cardIdList.Add(i);
        }
    }

    // 카드 섞기
    void ShuffleCardId()
    {
        for (int i = 0; i < cardIdList.Count; i++)
        {
            int randomIndex = Random.Range(i, cardIdList.Count);
            int temp = cardIdList[randomIndex];
            cardIdList[randomIndex] = cardIdList[i];
            cardIdList[i] = temp;
        }
    }

    // 게임판 초기화 카드 세팅
    void InitBoard()
    {
        float spaceY = 1.8f;
        float spaceX = 1.3f;


        int rowCount = 5;
        int colCount = 4;

        int cardIndex = 0;

        for (int row = 0; row < rowCount; row++)
        {
            float posY = (row - (int)(rowCount / 2)) * spaceY;
            for (int col = 0; col < colCount; col++)
            {
                float posX = (col - (int)(colCount / 2)) * spaceX + (spaceX / 2);
                Vector3 pos = new Vector3(posX, posY, 0f);
                GameObject cardObject = Instantiate(cardPrefab, pos, Quaternion.identity); // 카드 인스턴스 화
                Card card = cardObject.GetComponent<Card>();
                int cardId = cardIdList[cardIndex++];
                card.SetCardId(cardId);
                card.SetAnimalSprite(cardSprites[cardId]);

                cardList.Add(card);
            }
        }
    }

    public List<Card> getCardList()
    {
        return cardList;
    }
}
