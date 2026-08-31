using System;
using UnityEngine;

/// <summary>
/// Sistema de "paseo": cada click del jugador llena un medidor. Al llenarse,
/// se entrega Dinero según el tier más alto desbloqueado (sección 4 del doc).
/// </summary>
public class WalkClickSystem : MonoBehaviour
{
    public static WalkClickSystem Instance { get; private set; }
    public GameConfig config;

    int clicksActuales = 0;
    public event Action OnCambio;

    void Awake() => Instance = this;

    // Conectar este método al evento OnClick() del botón/sprite principal del perro
    public void RegistrarClick()
    {
        clicksActuales++;
        if (clicksActuales >= config.clicksParaPaseo)
        {
            double recompensa = config.dineroPorTierEnPaseo * DogManager.Instance.TierMasAltoDesbloqueado;
            CurrencyManager.Instance.dinero += recompensa;
            clicksActuales = 0;
        }
        OnCambio?.Invoke();
    }

    // Valor entre 0 y 1 para conectar a un Slider/Image de "fill" en la UI
    public float ProgresoPaseo01() => (float)clicksActuales / config.clicksParaPaseo;
}
