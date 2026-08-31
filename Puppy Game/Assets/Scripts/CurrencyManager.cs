using UnityEngine;

/// <summary>
/// Guarda las dos monedas del juego (Reputación y Dinero) y hace que la
/// Reputación crezca sola cada frame según la producción total de los perros.
/// </summary>
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public GameConfig config;

    public double reputacion = 0;
    public double dinero = 0;

    void Awake() => Instance = this;

    void Update()
    {
        reputacion += DogManager.Instance.ProduccionTotalPorSegundo() * Time.deltaTime;
        dinero += DineroPorSegundo() * Time.deltaTime;
    }

    // Dinero generado solo, sin clickear. Escala lineal con el tier (no exponencial
    // como la Reputación) para que sea un ingreso mucho más chico y predecible.
    public double DineroPorSegundo() => config.dineroPasivoPorTier * DogManager.Instance.TierMasAltoDesbloqueado;

    public bool GastarReputacion(double cantidad)
    {
        if (reputacion < cantidad) return false;
        reputacion -= cantidad;
        return true;
    }

    public bool GastarDinero(double cantidad)
    {
        if (dinero < cantidad) return false;
        dinero -= cantidad;
        return true;
    }

    public void ReiniciarPorPrestigio()
    {
        reputacion = 0;
        dinero = 0;
    }
}
