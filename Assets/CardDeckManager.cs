using NUnit.Framework.Constraints;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardDeckManager : MonoBehaviour
{
    public Button dealButton;
    public Button resetButton;
    public Button setupButton;
    public GameObject cardPrefab;
    public GameObject discardPile;
    private List<GameObject> discardedCards;
    public Transform cardParent;
    public List<GameObject> cardPrefabs;
    public List<GameObject> remainingCards;
    private List<GameObject> originalDeck = new List<GameObject>();
    public Texture frontTexture;
    public Texture backTexture;

    public MenuController menuController;
    public bool isSolitaire = false;
    public GameObject solitairePilePlacements;
    public GameObject dealCardsButton;
    public GameObject emptyDeckImage;
    public SolitaireGameManager solitaireGameManager;
    void Start()
    {
        remainingCards = new List<GameObject>(cardPrefabs);
        discardedCards = new List<GameObject>();
        originalDeck = new List<GameObject>(remainingCards);
        resetButton.onClick.AddListener(ResetGame);
        dealButton.onClick.AddListener(() => SpawnCard(true));
        HandleShowingObjects();
        setupButton.onClick.AddListener(() =>
        {
            ResetGame();
            solitaireGameManager.SetupSolitaireBoard();
        });

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
            GameObject selectedCardBackGroundPrefab = cardPrefab;

            // Remove the selected card from the deck
            remainingCards.RemoveAt(randomIndex);

            // Instantiate a new card and set its sprite
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
            if (remainingCards.Count == 0) 
            {
                dealButton.enabled = false;
                dealButton.interactable = false;
            }
            return newCard;
        }
        else
        {
            Debug.Log("No more cards left in the deck!");
            return null;
        }

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

    public void HandleShowingObjects()
    {
        isSolitaire = false;
        dealButton.gameObject.SetActive(true);
        discardPile.gameObject.SetActive(true);
        solitairePilePlacements.gameObject.SetActive(false);
        dealCardsButton.gameObject.SetActive(false);
        emptyDeckImage.gameObject.SetActive(false);
    }

    public void ResetGame()
    {
        HandleShowingObjects();
        menuController.menu.SetActive(menuController.isMenuToggle = false);
        foreach (Transform card in cardParent)
        {
            Destroy(card.gameObject);
        }
        foreach (GameObject card in discardedCards)
        {
            Destroy(card);
        }
        discardedCards.Clear();
        remainingCards = new List<GameObject>(originalDeck);

        dealButton.enabled = true;
        dealButton.interactable = true;

        Debug.Log("Game Reset. Deck is full again!");
    }
}
