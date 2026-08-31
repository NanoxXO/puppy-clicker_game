using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Va en cada botón de mejora instanciado. Al pasar el mouse, le avisa a
/// UIController qué mejora es la suya (por índice) para que muestre el
/// tooltip COMPARTIDO (un único panel que vive siempre fijo dentro de la
/// capa de arriba de todo). No mueve ni duplica ningún objeto — por eso no
/// se puede quedar "pegado" en pantalla aunque la lista de mejoras se
/// regenere mientras el mouse está encima.
/// </summary>
public class TooltipMejora : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public int indiceMejora;

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIController.Instance?.MostrarTooltipMejora(indiceMejora, (RectTransform)transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIController.Instance?.OcultarTooltipMejora();
    }
}
