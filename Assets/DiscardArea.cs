using UnityEngine;
using UnityEngine.EventSystems;

public class DiscardArea : MonoBehaviour, IDropHandler
{
    public CardDeckManager cardDeckManager;
    private void Start()
    {
        if (cardDeckManager == null)
        {
            cardDeckManager = FindObjectOfType<CardDeckManager>();
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedCard = eventData.pointerDrag;

        if (droppedCard != null)
        {
            Debug.Log("Card dropped in discard area");

            HandleCardDrop(droppedCard);
        }
    }

    private void HandleCardDrop(GameObject card)
    {
        card.transform.SetParent(transform, false);
        card.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        card.SetActive(false);
    }
}
