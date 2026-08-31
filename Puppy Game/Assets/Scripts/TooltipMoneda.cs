using UnityEngine;
using UnityEngine.EventSystems;

public enum TipoMoneda { Reputacion, Dinero }

/// <summary>
/// Va en el grupo (ícono + número) de una moneda, en la barra de arriba.
/// Al pasar el mouse, le pide a UIController que muestre el tooltip
/// compartido con el nombre completo y el número exacto de esa moneda.
///
/// El objeto donde se pone este script necesita un componente Image
/// (puede ser transparente) con "Raycast Target" activado, para que Unity
/// detecte el mouse encima de toda el área, no solo del texto o el ícono.
/// </summary>
public class TooltipMoneda : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TipoMoneda tipo;
    public string nombreMostrado; // "Reputación" o "Dinero"

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIController.Instance?.MostrarTooltipMoneda(tipo, nombreMostrado, (RectTransform)transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIController.Instance?.OcultarTooltipMoneda();
    }
}
