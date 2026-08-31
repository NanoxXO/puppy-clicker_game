using System;
using UnityEngine;

/// <summary>
/// Sistema de prestigio "Renacer" (sección 6 del doc). Resetea el progreso
/// a cambio de un bono permanente que nunca se pierde.
/// </summary>
public class PrestigeManager : MonoBehaviour
{
    public static PrestigeManager Instance { get; private set; }
    public GameConfig config;

    public int renacimientos = 0;
    public double baseFlat = 0;

    public double MultiplicadorGlobal => 1 + config.renacerBonoMultiplicadorGlobal * renacimientos;

    void Awake() => Instance = this;

    public double RequisitoParaSiguienteRenacer()
    {
        int k = renacimientos + 1;
        return config.renacerRequisitoBase * Math.Pow(config.renacerRequisitoMultiplicador, k - 1);
    }

    public bool PuedeRenacer() => DogManager.Instance.TotalPerrosComprados() >= RequisitoParaSiguienteRenacer();

    public bool Renacer()
    {
        if (!PuedeRenacer()) return false;

        renacimientos++;
        baseFlat += config.renacerBonoBaseFlat;

        // Todo lo demás vuelve a cero; BaseFlat y MultiplicadorGlobal quedan para siempre
        DogManager.Instance.ReiniciarPorPrestigio();
        CurrencyManager.Instance.ReiniciarPorPrestigio();
        UpgradeManager.Instance.ReiniciarPorPrestigio();

        return true;
    }
}
