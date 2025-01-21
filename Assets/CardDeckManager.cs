using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardDeckManager : MonoBehaviour
{
    public GameObject cardBack;
    public Transform cardParent;
    public List<GameObject> cardPrefabs;
    public List<GameObject> remainingCards;
    public List<GameObject> originalDeck = new List<GameObject>();
    public void Start()
    {
        cardPrefabs = new List<GameObject>(Resources.LoadAll<GameObject>("Prefab/BackColor_Red"));
        cardPrefabs = cardPrefabs.Where
            (x => 
                x.name.Contains("PlayingCard")
                && 
                !(x.name.Contains("Blank") || x.name.Contains("Joker"))
            )
            .ToList();
        remainingCards = new List<GameObject>(cardPrefabs);
        originalDeck = new List<GameObject>(remainingCards);

    }

    void Update()
    {
        
    }

    public GameObject SpawnCard(bool isFaceUp = true)
    {
        if (remainingCards.Count > 0)
        {
            // Get a random card index
            int randomIndex = Random.Range(0, remainingCards.Count);
            GameObject selectedCardPrefab = remainingCards[randomIndex];
            GameObject selectedCardBackGroundPrefab = cardBack;

            // Remove the selected card from the deck
            remainingCards.RemoveAt(randomIndex);

            GameObject newCard = Instantiate(selectedCardPrefab, cardParent);
            newCard.tag = "Card";
            GameObject newCardBack = Instantiate(selectedCardBackGroundPrefab, newCard.transform);
            newCardBack.name = "CardBack";
            RectTransform rect = newCard.GetComponent<RectTransform>();
            RectTransform rectBack = newCardBack.GetComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.anchoredPosition = Vector3.zero;
            rect.sizeDelta = new Vector2(150, 200);
            rectBack.localScale = Vector3.one;
            rectBack.anchoredPosition = Vector3.zero;
            rectBack.sizeDelta = new Vector2(150, 200);
            CardDrag cardDragScript = newCard.GetComponent<CardDrag>();
            if (cardDragScript == null)
            {
                cardDragScript = newCard.AddComponent<CardDrag>();
            }
            // Set the RawImage components
            RawImage frontImage = newCard.GetComponent<RawImage>();
            RawImage backImage = newCardBack.GetComponent<RawImage>();

            if (backImage != null)
            {
                cardDragScript.frontImage = frontImage;
                cardDragScript.backImage = backImage;
                cardDragScript.isFlipped = !isFaceUp;
                cardDragScript.SetCardFaces();
            }
            return newCard;
        }
        else
        {
            Debug.Log("No more cards left in the deck!");
            return null;
        }

    }
    public void ResetCardBoard()
    {
        foreach (Transform card in cardParent)
        {
            Destroy(card.gameObject);
        }
    }
}
