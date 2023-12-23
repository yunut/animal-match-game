using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{


    [SerializeField]
    private SpriteRenderer cardRenderer;

    [SerializeField]
    private Sprite animalSprite; // 카드를 뒤집었을때 나오는 동물 이미지

    [SerializeField]
    private Sprite backSprite;

    private bool isFlipped = false;
    private bool isFlipping = false;

    private bool isMatched = false;

    public int cardId;

    public void SetCardId(int id)
    {
        this.cardId = id;
    }

    public void SetMatched()
    {
        this.isMatched = true;
    }

    public void SetAnimalSprite(Sprite sprite)
    {
        this.animalSprite = sprite;
    }

    
    // 카드 뒤집기 처리
    public void FlipCard()
    {

        isFlipping = true;

        // dotween 라이브러리를 이용해 x축 좌표 변경
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(0f, originalScale.y, originalScale.z);

        transform.DOScale(targetScale, 0.2f)  
        .OnComplete(() =>
        {
            // 카드 렌더링의 sprite 변경
            isFlipped = !isFlipped;
            if (isFlipped)
            {
                cardRenderer.sprite = animalSprite;
            }
            else
            {
                cardRenderer.sprite = backSprite;
            }

            transform.DOScale(originalScale, 0.2f)
            .OnComplete(() =>
            {
                isFlipping = false;
            });
        });
    }

    // 해당 오브젝트에 마우스 클릭시 처리
    void OnMouseDown()
    {
        if (!isFlipping && !isMatched && !isFlipped)
        {
            GameManager.instance.CardClicked(this);
        }
    }
}
