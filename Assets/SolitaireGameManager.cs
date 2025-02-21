using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class SolitaireGameManager : CardDeckManager
{
    public Transform solitaireColumns;
    private List<GameObject> dealtCards;
    private List<GameObject> remainingDeckCards;
    public GameObject cardDeck;

    public GameObject solitairePilePlacements;

    new void Start()
    {
        base.Start();
        ResetCardBoard();
    }

    public void DealCardFromStack()
    {
        GameObject firstCard = remainingDeckCards.FirstOrDefault();
        if (firstCard != null) 
        {
            dealtCards.Add(firstCard);
            float cardWidth = 150f;
            float xOffset = 10f + cardWidth;
            DealCard(firstCard, xOffset);
            remainingDeckCards.Remove(firstCard);
        }
        else
        {
            ReverseDealtCards();
        }
    }

    public void ReverseDealtCards()
    {
        foreach (GameObject card in dealtCards) 
        {
            float cardWidth = -150f;
            float xOffset = -10f + cardWidth;
            RectTransform rect = card.GetComponent<RectTransform>();

            CardDrag cardDragScript = card.GetComponent<CardDrag>();
            RectTransform deckRect = cardDeck.GetComponent<RectTransform>();

            if (cardDragScript != null)
            {
                if (!cardDragScript.HasLeftDeck)
                {
                    remainingDeckCards.Add(card);
                    rect.anchoredPosition = deckRect.anchoredPosition;
                    cardDragScript.FlipCard();
                }
            }
        }
        dealtCards.Clear();
    }

    public void DealCard(GameObject card, float xOffset)
    {
        RectTransform rect = card.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x + xOffset, rect.anchoredPosition.y);

        CardDrag cardDragScript = card.GetComponent<CardDrag>();
        if (cardDragScript != null)
        {
            cardDragScript.FlipCard();
        }
    }

    public void SetupSolitaireBoard()
    {
        dealtCards = new List<GameObject>();
        remainingDeckCards = new List<GameObject>();
        int[] cardsInColumns = { 1, 2, 3, 4, 5, 6, 7 };
        float yOffset = -35f;
        float cardWidth = 150f; // Adjust this based on your card size
        float startingXOffset = -((cardsInColumns.Length - 1) * cardWidth) / 2; // Centered starting position

        for (int i = 0; i < cardsInColumns.Length; i++)
        {
            float xOffset = startingXOffset + i * (10 + cardWidth); // Adjust the x offset per column
            for (int j = 0; j < cardsInColumns[i]; j++)
            {
                bool isFaceUp = (j == cardsInColumns[i] - 1);

                // Use SpawnCard from CardDeckManager
                GameObject newCard = SpawnCard(isFaceUp);

                if (newCard != null)
                {
                    newCard.transform.SetParent(cardParent, false);
                    RectTransform rect = newCard.GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(xOffset, yOffset * j);
                    rect.localScale = Vector3.one;
                }
            }
        }
        float stockpileXOffset = startingXOffset;
        float stockpileYOffset = 250f;
        while (remainingCards.Count > 0) 
        {
            // Spawn face-down cards
            GameObject stockCard = SpawnCard(false);
            remainingDeckCards.Add(stockCard);

            if (stockCard != null)
            {
                stockCard.transform.SetParent(cardParent, false);
                RectTransform rect = stockCard.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(stockpileXOffset, stockpileYOffset);
                rect.localScale = Vector3.one;
            }
        }
    }

    public new void ResetCardBoard()
    {   
        base.ResetCardBoard();
        Debug.Log("Game Reset. Deck is full again!");
        SetupSolitaireBoard();
    }
}
