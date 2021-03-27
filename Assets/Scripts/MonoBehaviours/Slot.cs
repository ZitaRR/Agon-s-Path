using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public sealed class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    private GameObject description;
    private Text title;
    private Text info;
    private Vector3 position;

    [SerializeField]
    private Button slot;
    [SerializeField]
    private Button remove;

    private void Start()
    {
        image = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        description = GameManager.UI.GetElement("SlotDescription");

        Text[] texts = description.GetComponentsInChildren<Text>(true);
        title = texts[0];
        info = texts[1];

        slot.onClick.AddListener(Use);
        remove.onClick.AddListener(() => Remove());
    }

    public void Use()
    {

    }

    public bool Add(Sprite sprite)
    {
        if (image.sprite != null)
            return false;

        image.sprite = sprite;
        image.enabled = true;
        remove.image.enabled = true;
        return true;
    }

    public bool Remove()
    {
        if (image is null)
            return false;

        image.sprite = null;
        image.enabled = false;
        remove.image.enabled = false;
        return true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.UI.Enable("SlotDescription");
        title.text = image.sprite.name;
        info.text = $"Texture name: {image.sprite.name}";

        bool left = Screen.width / 2 < Input.mousePosition.x;
        position = new Vector3(
            Input.mousePosition.x + (left ? -200f : 200f),
            Input.mousePosition.y,
            0f);

        description.transform.position = position;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.UI.Disable("SlotDescription");
    }
}
