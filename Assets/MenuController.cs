using NUnit.Framework.Constraints;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private static MenuController instance;

    public Button resetButton;
    public Button setupButton;
    public CardDeckManager cardDeckManager;
    public static MenuController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MenuController();
                instance.LoadNewCardGameScene();
            }
            return instance;
        }
    }

    public Button menuButton;
    public GameObject menu;
    public bool isMenuToggle;
    public bool isSolitaire = false;
    void Start()
    {
        menuButton.onClick.AddListener(ToggleMenu);
        menu.SetActive(isMenuToggle = false);
        resetButton.onClick.AddListener(LoadNewCardGameScene);
        setupButton.onClick.AddListener(LoadSolitaireScene);
    }

    public void ToggleMenu()
    {
        menu.SetActive(isMenuToggle = !isMenuToggle);
    }

    public void ResetGame()
    {
        menu.SetActive(isMenuToggle = false);
        Debug.Log("Game Reset. Deck is full again!");
    }

    public void LoadSolitaireScene()
    {
        SceneManager.LoadScene("SolitaireScene");
        ResetGame();
    }

    public void LoadNewCardGameScene()
    {
        SceneManager.LoadScene("NewGameScene");
        ResetGame();
    }
}
