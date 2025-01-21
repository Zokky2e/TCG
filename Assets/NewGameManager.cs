using NUnit.Framework.Constraints;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NewGameManager : CardDeckManager
{
    public Button dealButton;
    public GameObject discardPile;
    private List<GameObject> discardedCards;

    public MenuController menuController;
    new void Start()
    {
        base.Start();
        discardedCards = new List<GameObject>();
        ResetCardBoard();
        dealButton.onClick.AddListener(() => DealCard(true));
    }

    void Update()
    {
        
    }

    public GameObject DealCard(bool isFaceUp = true)
    {
        GameObject gameObject = SpawnCard(isFaceUp);
        if (remainingCards.Count == 0)
        {
            dealButton.enabled = false;
            dealButton.interactable = false;
        }
        return null;

    }
    public void HandleCardDropped(GameObject card, bool inDiscardArea)
    {
        if (inDiscardArea)
        {
            card.transform.SetParent(discardPile.transform);
            discardedCards.Add(card);
            card.SetActive(false);
            Debug.Log("Card discarded");
        }
    }

    public void DestroyDiscardedCards()
    {

        foreach (GameObject card in discardedCards)
        {
            Destroy(card);
        }
        discardedCards.Clear();
        remainingCards = new List<GameObject>(originalDeck);
        dealButton.enabled = true;
        dealButton.interactable = true;
    }


    public new void ResetCardBoard()
    {
        base.ResetCardBoard();
        DestroyDiscardedCards();
    }
}
