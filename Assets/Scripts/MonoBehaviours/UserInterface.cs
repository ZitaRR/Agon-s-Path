using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserInterface : MonoBehaviour
{
    [SerializeField]
    private List<Component> elements;

    private void Start()
    {
        GameManager.OnStateChange += OnStateChanged;
        OnStateChanged(GameManager.State);
    }

    public void Enable(string name)
    {
        foreach (var element in elements)
        {
            if (element.name == name)
            {
                element.gameObject.SetActive(true);
                return;
            }
        }
    }

    public void EnableSelfAndChildren(string name)
    {
        foreach (var element in elements)
        {
            if(element.name == name)
            {
                element.gameObject.SetActive(true);
                for (int i = 0; i < element.transform.childCount; i++)
                {
                    element.transform.GetChild(i).gameObject.SetActive(true);
                }
                return;
            }
        }
    }

    public void Disable(string name)
    {
        foreach (var element in elements)
        {
            if (element.name == name)
            {
                element.gameObject.SetActive(false);
                return;
            }
        }
    }

    public void DisableAll()
    {
        foreach (var element in elements)
        {
            element.gameObject.SetActive(false);
        }
    }

    public T GetElement<T>(string name)
    {
        foreach (var element in elements)
        {
            if (element.name == name)
                return element.GetComponentInChildren<T>();
        }
        return default;
    }

    private void OnStateChanged(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Menu:
                DisableAll();
                Enable("MainMenu");
                break;
            case GameManager.GameState.Loading:
                DisableAll();
                break;
            case GameManager.GameState.Combat:
            case GameManager.GameState.Idle:
                DisableAll();
                EnableSelfAndChildren("PlayerUI");
                break;
        }
    }
}
