using System;

/// <summary>
/// Convierte números grandes en texto legible: 1500 -> "1.5K", 2300000 -> "2.3M", etc.
/// Fundamental en un incremental, porque los números escalan muy rápido (ver tabla
/// de referencia del doc: tier 10 ya produce 40 millones/seg).
/// </summary>
public static class NumberFormatter
{
    static readonly string[] sufijos = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };

    public static string Formatear(double valor)
    {
        if (double.IsInfinity(valor) || double.IsNaN(valor)) return "∞";
        if (valor < 1000) return Math.Floor(valor).ToString("0");

        int indiceSufijo = 0;
        while (valor >= 1000 && indiceSufijo < sufijos.Length - 1)
        {
            valor /= 1000;
            indiceSufijo++;
        }
        return valor.ToString("0.##") + sufijos[indiceSufijo];
    }

    // Número completo, sin abreviar, con separador de miles (para el tooltip
    // de las monedas, donde queremos ver el valor exacto).
    public static string FormatearCompleto(double valor)
    {
        if (double.IsInfinity(valor) || double.IsNaN(valor)) return "∞";
        return Math.Floor(valor).ToString("N0");
    }
}
