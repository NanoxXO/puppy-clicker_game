using System.Collections;
using UnityEngine;

/// <summary>
/// Va en el MISMO GameObject que la Image del perro del botón principal
/// (no en el botón entero, solo en la imagen del perro). Cada vez que se
/// llama a Saltar(), la imagen sube y baja suavemente — es puramente
/// visual, no toca ninguna lógica del juego.
/// </summary>
public class SaltoPerro : MonoBehaviour
{
    [Tooltip("Cuánto sube el perro en píxeles de UI al saltar. Subilo si querés que se note más, bajalo si lo querés más sutil.")]
    public float alturaSalto = 12f;
    [Tooltip("Cuánto dura el salto completo (subida + bajada), en segundos.")]
    public float duracionSalto = 0.18f;

    RectTransform rect;
    Vector2 posicionOriginal;
    Coroutine saltoActual;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        posicionOriginal = rect.anchoredPosition;
    }

    public void Saltar()
    {
        if (saltoActual != null) StopCoroutine(saltoActual);
        saltoActual = StartCoroutine(RutinaSalto());
    }

    IEnumerator RutinaSalto()
    {
        float mitad = duracionSalto / 2f;

        float t = 0f;
        while (t < mitad)
        {
            t += Time.deltaTime;
            float progreso = Mathf.Sin((t / mitad) * Mathf.PI / 2f);
            rect.anchoredPosition = posicionOriginal + Vector2.up * progreso * alturaSalto;
            yield return null;
        }

        t = 0f;
        while (t < mitad)
        {
            t += Time.deltaTime;
            float progreso = 1f - Mathf.Sin((t / mitad) * Mathf.PI / 2f);
            rect.anchoredPosition = posicionOriginal + Vector2.up * progreso * alturaSalto;
            yield return null;
        }

        rect.anchoredPosition = posicionOriginal;
        saltoActual = null;
    }
}
