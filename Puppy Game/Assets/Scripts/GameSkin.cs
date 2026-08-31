using UnityEngine;

/// <summary>
/// Todas las imágenes de "piel visual" del juego, en un solo lugar,
/// separado de GameConfig (que es solo balance/contenido: perros, mejoras,
/// costos). Esto es puramente estético — se puede vaciar y volver a llenar
/// sin tocar ningún número del juego.
///
/// Para crear el asset: click derecho en la carpeta Project → Create →
/// Incremental → Piel Visual (Imágenes).
/// </summary>
[CreateAssetMenu(fileName = "GameSkin", menuName = "Incremental/Piel Visual (Imagenes)")]
public class GameSkin : ScriptableObject
{
    [Header("Íconos de las monedas (arriba de la pantalla)")]
    public Sprite iconoReputacion;
    public Sprite iconoDinero;

    [Header("Botón de click (el perro que se pasea)")]
    public Sprite fondoBotonPaseo;

    [Header("Botón de mejora")]
    [Tooltip("Se aplica a los 3 botones de mejora automáticamente, no hace falta tocar el prefab.")]
    public Sprite fondoBotonMejora;

    [Header("Medidor de paseo (Slider)")]
    public Sprite fondoMedidorPaseo;   // la parte "vacía" del slider
    public Sprite rellenoMedidorPaseo; // la parte que se va llenando

    [Header("Botón de Bibliografía")]
    public Sprite fondoBotonBibliografia;
}
