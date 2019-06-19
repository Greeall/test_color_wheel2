using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorWheel : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public MainUI mainUI;
    public GameObject tooltip;
    public Camera camera;
    public Canvas canvas;
    Color[] Data;
    Image Image;

    public int Width { get { return Image.sprite.texture.width; } }
    public int Height { get { return Image.sprite.texture.height; } }

    void Awake()
    {
        Image = GetComponent<Image>();
        Data = Image.sprite.texture.GetPixels();
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        OnColorSelect(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnColorSelect(eventData);
    }

    void OnColorSelect(PointerEventData eventData)
    {
        Color color = GetWheelColor(eventData.position);
        if (color.a > 0.95f)
        {
            tooltip.SetActive(true);
            tooltip.transform.position = eventData.position;
            mainUI.CurrentColor = color;
            tooltip.GetComponent<Image>().color = color;
        }
    }

    Color GetWheelColor(Vector2 position)
    {
        RectTransform rt = GetComponent<RectTransform>();
        float wheelX = transform.position.x - ((rt.rect.width * canvas.scaleFactor) / 2);
        float wheelY = transform.position.y - ((rt.rect.height * canvas.scaleFactor) / 2);
        float xRelativePos = position.x - wheelX;
        float yRelativePos = position.y - wheelY;
        float resizeFactor = rt.rect.width * canvas.scaleFactor / Width;
        Debug.Log(resizeFactor);
        return Data[(int)(yRelativePos / resizeFactor) * Width + (int)(xRelativePos / resizeFactor)];
    }
}
