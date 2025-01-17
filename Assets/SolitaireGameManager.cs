using UnityEngine;
using System.Collections.Generic;

public class SolitaireGameManager : MonoBehaviour
{
    public Transform cardParent;
    private List<GameObject> remainingCards;
    public CardDeckManager cardDeckManager;

    public void HandleHidingObjects()
    {
        cardDeckManager.isSolitaire = true;
        cardDeckManager.dealButton.gameObject.SetActive(false);
        cardDeckManager.discardPile.gameObject.SetActive(false);
        cardDeckManager.solitairePilePlacements.gameObject.SetActive(true);
    }

    public void SetupSolitaireBoard()
    {
        HandleHidingObjects();
        int[] cardsInColumns = { 1, 2, 3, 4, 5, 6, 7 };
        float yOffset = -35f;
        float cardWidth = 160f; // Adjust this based on your card size
        float startingXOffset = -((cardsInColumns.Length - 1) * cardWidth) / 2; // Centered starting position

        for (int i = 0; i < cardsInColumns.Length; i++)
        {
            float xOffset = startingXOffset + i * cardWidth; // Adjust the x offset per column
            for (int j = 0; j < cardsInColumns[i]; j++)
            {
                bool isFaceUp = (j == cardsInColumns[i] - 1);

                // Use SpawnCard from CardDeckManager
                GameObject newCard = cardDeckManager.SpawnCard(isFaceUp);

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
        while (cardDeckManager.remainingCards.Count > 0) 
        {
            // Spawn face-down cards
            GameObject stockCard = cardDeckManager.SpawnCard(false);

            if (stockCard != null)
            {
                stockCard.transform.SetParent(cardParent, false);
                RectTransform rect = stockCard.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(stockpileXOffset, stockpileYOffset);
                rect.localScale = Vector3.one;
            }
        }

    }

}
