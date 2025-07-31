using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Вспомогательный класс для обработки изменения следующего Circle в GameplayController
/// </summary>
public class NextCircleUpdater : MonoBehaviour
{
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public void OnCircleUpdate(Circle circle)
    {
        _image.color = circle.Color;
    }
}
