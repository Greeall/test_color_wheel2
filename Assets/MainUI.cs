using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MainUI : MonoBehaviour {
    public string imageUrl;
    public GameObject imagePrefab;
    public GameObject newImageWindow;
    public Button confirmButton;
    public ScrollRect list;

    Texture2D currentImage;
    Color _currentColor;
    public Color CurrentColor
    {
        get { return _currentColor; }
        set { _currentColor = value; }
    }

    public void OpenNewImageWindow()
    {
        newImageWindow.SetActive(true);
        StartCoroutine(DownloadImage());
    }

    public void CloseNewImageWindow()
    {
        newImageWindow.SetActive(false);
        // Reset Image
        confirmButton.interactable = false;
        currentImage = null;
    }

    public void AddNewImage()
    {
        // Create Image
        GameObject newImage = Instantiate<GameObject>(imagePrefab);
        newImage.transform.parent = list.content.transform;
        // Assign Sprite
        Sprite sprite = Sprite.Create(currentImage, new Rect(0, 0, currentImage.width, currentImage.height), new Vector2(0.5f, 0.5f));
        Image imgCmp = newImage.GetComponent<Image>();
        imgCmp.sprite = sprite;
        imgCmp.color = CurrentColor;
        // Scroll list to the bottom where newest image is
        Canvas.ForceUpdateCanvases();
        list.verticalNormalizedPosition = 0;
        // Close window
        CloseNewImageWindow();
    }

    IEnumerator DownloadImage()
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                currentImage = DownloadHandlerTexture.GetContent(uwr);
                confirmButton.interactable = true;
            }
        }
    }
}
