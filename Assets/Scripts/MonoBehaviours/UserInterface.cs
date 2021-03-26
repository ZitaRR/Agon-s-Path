using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserInterface : MonoBehaviour
{
    [SerializeField]
    private List<Component> elements;

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
                SetChildren(element.transform, true);
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

    private void SetChildren(Transform transform, bool value)
    {
        if (transform.childCount <= 0)
            return;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(value);
            SetChildren(child, value);
        }
    }
}
