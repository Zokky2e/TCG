using NUnit.Framework.Constraints;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button menuButton;
    public GameObject menu;
    public bool isMenuToggle;

    void Start()
    {
        menuButton.onClick.AddListener(ToggleMenu);
        menu.SetActive(isMenuToggle = false);
    }

    public void ToggleMenu()
    {
        menu.SetActive(isMenuToggle = !isMenuToggle);
    }
}
