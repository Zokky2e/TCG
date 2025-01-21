using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CardDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Transform originalParent;
    private Canvas canvas;
    private float clickStartTime;
    private const float clickThreshold = 0.2f;
    private const float dragThreshold = 5f;
    private bool isDragging = false;
    private bool hasLeftDeck = false;
    public bool isFlipped = false;

    public RawImage frontImage;
    public RawImage backImage;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public bool HasLeftDeck { get { return hasLeftDeck; } }

    // Called when the user presses down on the card
    public void OnPointerDown(PointerEventData eventData)
    {
        clickStartTime = Time.time;
        this.transform.SetAsLastSibling();
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent; // Store the original parent
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    // Called while the card is being dragged
    public void OnDrag(PointerEventData eventData)
    {
        hasLeftDeck = true;
        Vector2 adjustedDelta = eventData.delta / canvas.scaleFactor;
        rectTransform.anchoredPosition += adjustedDelta;
        rectTransform.SetAsLastSibling();
        SetAsLastSiblingParentRecursively(transform);
        SetAsLastSiblingChildRecursivly(transform);
        if (Vector2.Distance(originalPosition, rectTransform.anchoredPosition) > dragThreshold)
        {
            isDragging = true;
        }
    }

    private void SetAsLastSiblingChildRecursivly(Transform parent)
    {
        foreach (Transform child in parent)
        {
            RectTransform childRectTransform = child.GetComponent<RectTransform>();
            if (childRectTransform != null)
            {
                SetAsLastSiblingChildRecursivly(child);
            }
        }
    }

    private void SetAsLastSiblingParentRecursively(Transform transform)
    {
        // Set the current transform as the last sibling
        transform.SetAsLastSibling();

        // If the parent has the tag "Card", call the function recursively on the parent
        if (transform.parent != null && transform.parent.CompareTag("Card"))
        {
            SetAsLastSiblingParentRecursively(transform.parent);
        }
    }

    private GameObject GetFinalChild(GameObject parent)
    {
        if (parent.transform.childCount == 0)
        {
            return parent;
        }

        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag("Card"))
            {
                return GetFinalChild(child.gameObject);
            }
        }
        return parent;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        float clickDuration = Time.time - clickStartTime;
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        if (clickDuration <= clickThreshold && !isDragging)
        {
            FlipCard();
        }

        isDragging = false;
    }
    public void FlipCard()
    {
        isFlipped = !isFlipped;
        SetCardFaces();
    }

    public void SetCardFaces()
    {
        frontImage.enabled = !isFlipped;
        backImage.enabled = isFlipped;
        backImage.gameObject.SetActive(isFlipped);
    }
    // Called when the dragging ends (when the user releases the card)
    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
        GameObject targetCard = GetDropTarget(eventData); 
        if (targetCard != null)
        {
            GameObject finalChild = GetFinalChild(targetCard);
            if (IsAboveSolitaireColumnsArea(eventData) && !finalChild.name.Contains("SolitaireColumn")) 
            {
                float yOffset = -35f;
                transform.SetParent(finalChild.transform);
                rectTransform.anchoredPosition = new Vector2(0, yOffset);
            }
            else
            {
                transform.SetParent(finalChild.transform);
                rectTransform.anchoredPosition = Vector2.zero;
            }
        }
        else
        {
            GameObject cardField = GameObject.Find("CardField");
            if (cardField != null)
            {
                transform.SetParent(cardField.transform);
            }
        }
    }
    private GameObject GetDropTarget(PointerEventData eventData)
    {
        // Raycast to detect potential drop targets
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        results.Where(r => r.gameObject.CompareTag("Card")).ToList();
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Card") && result.gameObject != this.gameObject)
            {
                return result.gameObject;
            }
        }
        return null;
    }

    public bool IsAboveSolitaireColumnsArea(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("SolitaireColumns"))
            {
                return true;
            }
        }

        return false;
    }
}
