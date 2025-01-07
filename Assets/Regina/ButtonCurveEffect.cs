using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonCurveEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AnimationCurve curve;  // Кривая для изменения размера
    public float animationDuration = 0.2f;  // Длительность анимации

    private RectTransform rectTransform;
    private Vector3 normalScale;
    private Vector3 highlightedScale;
    private Coroutine currentAnimation;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        normalScale = rectTransform.localScale;
        highlightedScale = normalScale * 1.2f;  // Например, увеличение на 20%
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(AnimateScale(highlightedScale));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(AnimateScale(normalScale));
    }

    private System.Collections.IEnumerator AnimateScale(Vector3 targetScale)
    {
        Vector3 initialScale = rectTransform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            float curveValue = curve.Evaluate(t);  // Получаем значение из кривой
            rectTransform.localScale = Vector3.LerpUnclamped(initialScale, targetScale, curveValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.localScale = targetScale;
    }
}