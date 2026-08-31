using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mejoras permanentes compradas con Dinero. A diferencia de la primera
/// versión, acá NO hay tipos fijos (Comida/Correa/Collar): las mejoras son
/// una lista de datos libre en GameConfig (ver MejoraDefinicion). Cada una
/// declara a qué TAMAÑO de perro afecta; este script solo suma el bono de
/// las mejoras cuyo tamaño coincide con el del perro consultado.
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    public GameConfig config;

    // índice dentro de config.mejoras -> nivel actual comprado
    Dictionary<int, int> niveles = new Dictionary<int, int>();

    public event Action OnCambio;

    void Awake() => Instance = this;

    int NivelActual(int indiceMejora)
    {
        niveles.TryGetValue(indiceMejora, out int nivel);
        return nivel;
    }

    public double CostoSiguienteNivel(int indiceMejora)
    {
        int nivel = NivelActual(indiceMejora);
        return config.costoMejoraBase * Math.Pow(config.costoMejoraMultiplicador, nivel);
    }

    public bool ComprarMejora(int indiceMejora)
    {
        int nivel = NivelActual(indiceMejora);
        if (nivel >= config.nivelMaximoMejora) return false;

        double costo = CostoSiguienteNivel(indiceMejora);
        if (!CurrencyManager.Instance.GastarDinero(costo)) return false;

        niveles[indiceMejora] = nivel + 1;
        OnCambio?.Invoke();
        return true;
    }

    // Bono total de producción para UN perro: suma el aporte de cada mejora
    // cuyo tamanoObjetivo coincide con el tamaño de ese perro (definido en
    // su InfoPerro dentro de GameConfig).
    public double BonoParaPerro(int indiceTier)
    {
        TamanoPerro tamano = DogManager.Instance.InfoDe(indiceTier).tamano;
        double bono = 0;
        for (int i = 0; i < config.mejoras.Count; i++)
        {
            if (config.mejoras[i].tamanoObjetivo != tamano) continue;
            bono += NivelActual(i) * config.mejoras[i].porcentajePorNivel;
        }
        return bono;
    }

    public void ReiniciarPorPrestigio()
    {
        niveles.Clear();
        OnCambio?.Invoke();
    }
}
