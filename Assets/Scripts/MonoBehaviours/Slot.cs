using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public sealed class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item Item
    {
        get => item;
        private set
        {
            item = value;

            if(item is null)
            {
                image.enabled = false;
                remove.image.enabled = false;
                GameManager.UI.Disable("SlotDescription");
                return;
            }

            image.sprite = item.Sprite;
            image.enabled = true;
            remove.image.enabled = true;
        }
    }
    public bool IsEmpty { get => Item is null; }

    private Image image;
    private GameObject description;
    private Text title;
    private Text info;
    private Item item;
    private bool hover;

    [SerializeField]
    private Button slot;
    [SerializeField]
    private Button remove;

    private void Start()
    {
        image = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        description = GameManager.UI.GetElement("SlotDescription");

        if (Item is null)
            remove.image.enabled = false;

        Text[] texts = description.GetComponentsInChildren<Text>(true);
        title = texts[0];
        info = texts[1];

        slot.onClick.AddListener(Use);
        remove.onClick.AddListener(() => Remove());
    }

    private void Update()
    {
        if (!hover)
            return;

        var x = Input.mousePosition.x < Screen.width / 2
            ? Input.mousePosition.x + Screen.width / 6
            : Input.mousePosition.x - Screen.width / 6;

        description.transform.position = new Vector3(x, Input.mousePosition.y);
    }

    public void Use()
    {

    }

    public bool Add(Item item)
    {
        if (!IsEmpty)
            return false;

        Item = item;
        return true;
    }

    public bool Remove()
    {
        if (IsEmpty)
            return false;

        Item = null;
        return true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsEmpty)
            return;

        GameManager.UI.Enable("SlotDescription");
        title.text = image.sprite.name;
        info.text = $"Texture name: {image.sprite.name}";
        hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsEmpty)
            return;

        GameManager.UI.Disable("SlotDescription");
        hover = false;
    }
}
