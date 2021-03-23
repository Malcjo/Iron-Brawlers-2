using UnityEngine;
using UnityEngine.UI;

public class SlidableMask : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private RectTransform rectTransform;
    private Vector3 farLeft;
    private Vector3 farRight;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        farLeft = rectTransform.localPosition - new Vector3(rectTransform.rect.width, 0f);
        farRight = rectTransform.localPosition;
    }

    private void Update()
    {
        rectTransform.localPosition = Vector2.Lerp(farLeft, farRight, slider.value);
    }
}