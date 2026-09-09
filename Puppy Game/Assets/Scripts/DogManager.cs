using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maneja la lista de perros del jugador, sus costos, su producción y el
/// desbloqueo del siguiente tier. Implementa las fórmulas de las secciones
/// 2 y 3 del documento de diseño. No tiene un límite de tiers "hardcodeado":
/// el tier i se calcula con una fórmula, así que escala infinito.
/// </summary>
public class DogManager : MonoBehaviour
{
    public static DogManager Instance { get; private set; }

    public GameConfig config;
    public List<DogTierRuntime> tiersDesbloqueados = new List<DogTierRuntime>();

    // La UI se suscribe a este evento para refrescarse cada vez que algo cambia
    public event Action OnCambio;

    void Awake()
    {
        Instance = this;
        // Siempre arrancamos con el tier 1 disponible para comprar
        tiersDesbloqueados.Add(new DogTierRuntime(1));
    }

    public double CostoBase(int i) => config.costoBaseInicial * Math.Pow(config.costoMultiplicadorTier, i - 1);
    public double ProduccionBase(int i) => config.produccionBaseInicial * Math.Pow(config.produccionMultiplicadorTier, i - 1);
    public double CostoActual(DogTierRuntime tier) => CostoBase(tier.indice) * tier.multiplicadorCosto;

    // Punto único de acceso a nombre/imagen/bibliografía de un tier.
    // Si el equipo de arte todavía no cargó ese perro en GameConfig, se genera
    // un placeholder para que el juego jamás se rompa por faltar contenido.
    public InfoPerro InfoDe(int i)
    {
        if (config.perros != null && i - 1 < config.perros.Count && i >= 1)
            return config.perros[i - 1];
        return new InfoPerro { nombre = $"Perro Tier {i}", imagen = null, bibliografia = "" };
    }

    public string NombrePerro(int i) => InfoDe(i).nombre;

    public bool Comprar(DogTierRuntime tier)
    {
        double costo = CostoActual(tier);
        if (!CurrencyManager.Instance.GastarReputacion(costo)) return false;

        // r(n) usando n = cantidad ya comprada ANTES de esta compra
        double r = Math.Max(config.multiplicadorPiso, config.multiplicadorInicial - config.multiplicadorDecremento * tier.cantidadComprada);
        tier.multiplicadorCosto *= r;
        tier.cantidadComprada++;

        DesbloquearSiguienteSiCorresponde(tier);
        OnCambio?.Invoke();
        return true;
    }

    void DesbloquearSiguienteSiCorresponde(DogTierRuntime tier)
    {
        bool esElUltimoDeLaLista = tier.indice == tiersDesbloqueados[tiersDesbloqueados.Count - 1].indice;
        if (esElUltimoDeLaLista && tier.cantidadComprada == 1)
        {
            tiersDesbloqueados.Add(new DogTierRuntime(tier.indice + 1));
        }
    }

    public int TierMasAltoDesbloqueado => tiersDesbloqueados[tiersDesbloqueados.Count - 1].indice;

    // A diferencia de TierMasAltoDesbloqueado (que incluye el próximo perro
    // disponible para comprar aunque tengas 0 copias todavía), esto devuelve
    // el tier más alto del que YA tenés al menos 1 comprado de verdad.
    public int TierMasAltoObtenido()
    {
        for (int i = tiersDesbloqueados.Count - 1; i >= 0; i--)
        {
            if (tiersDesbloqueados[i].cantidadComprada > 0)
                return tiersDesbloqueados[i].indice;
        }
        return 1; // todavía no compraste ningún perro
    }

    // Fórmula final de producción (sección 7 del doc)
    public double ProduccionTotalPorSegundo()
    {
        double suma = 0;
        foreach (var tier in tiersDesbloqueados)
        {
            if (tier.cantidadComprada <= 0) continue;
            double bonoCategoria = UpgradeManager.Instance.BonoParaPerro(tier.indice);
            suma += tier.cantidadComprada * ProduccionBase(tier.indice) * (1 + bonoCategoria);
        }
        return suma * PrestigeManager.Instance.MultiplicadorGlobal + PrestigeManager.Instance.baseFlat;
    }

    public int TotalPerrosComprados()
    {
        int total = 0;
        foreach (var tier in tiersDesbloqueados) total += tier.cantidadComprada;
        return total;
    }

    public void ReiniciarPorPrestigio()
    {
        tiersDesbloqueados.Clear();
        tiersDesbloqueados.Add(new DogTierRuntime(1));
        OnCambio?.Invoke();
    }
}
