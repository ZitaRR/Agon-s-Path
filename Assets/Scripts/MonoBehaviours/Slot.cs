using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public sealed class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsEmpty { get => image.sprite is null; }

    private Image image;
    private GameObject description;
    private Text title;
    private Text info;
    private Vector3 position;
    private bool hover;

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
        GameManager.UI.Disable("SlotDescription");
        return true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image.sprite is null)
            return;

        GameManager.UI.Enable("SlotDescription");
        title.text = image.sprite.name;
        info.text = $"Texture name: {image.sprite.name}";
        hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (image.sprite is null)
            return;

        GameManager.UI.Disable("SlotDescription");
        hover = false;
    }
}
