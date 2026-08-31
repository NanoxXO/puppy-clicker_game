using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contiene TODOS los números de balance del juego, en un solo lugar.
/// Los compañeros pueden crear un asset de este tipo (click derecho en la
/// carpeta Project > Create > Incremental > Configuración de Balance) y
/// modificar los valores desde el Inspector, SIN tocar código.
///
/// Estos valores corresponden 1 a 1 con las fórmulas del documento de diseño
/// ("Sistema Matemático — Incremental de Perros").
/// </summary>
[CreateAssetMenu(fileName = "GameConfig", menuName = "Incremental/Configuracion de Balance")]
public class GameConfig : ScriptableObject
{
    [Header("Reputación - Costo y producción por tier")]
    [Tooltip("Costo del primer perro del tier 1. CostoBase(i) = costoBaseInicial * costoMultiplicadorTier^(i-1)")]
    public double costoBaseInicial = 10;
    [Tooltip("Cuánto se multiplica el costo base de un tier al siguiente")]
    public double costoMultiplicadorTier = 50;
    [Tooltip("Producción por segundo del primer perro del tier 1. ProduccionBase(i) = produccionBaseInicial * produccionMultiplicadorTier^(i-1)")]
    public double produccionBaseInicial = 1;
    [Tooltip("Cuánto se multiplica la producción base de un tier al siguiente")]
    public double produccionMultiplicadorTier = 7;

    [Header("Costo por copias adicionales del mismo perro")]
    [Tooltip("r(n) = max(multiplicadorPiso, multiplicadorInicial - multiplicadorDecremento * n)")]
    public double multiplicadorInicial = 1.5;
    public double multiplicadorDecremento = 0.05;
    public double multiplicadorPiso = 1.1;

    [Header("Dinero - Sistema de paseo (click)")]
    [Tooltip("Cuántos clicks llenan el medidor de paseo")]
    public int clicksParaPaseo = 50;
    [Tooltip("Dinero_ganado = dineroPorTierEnPaseo * TierMasAltoDesbloqueado")]
    public double dineroPorTierEnPaseo = 5;

    [Header("Dinero pasivo (AFK)")]
    [Tooltip("Dinero por segundo, incluso sin clickear = dineroPasivoPorTier * TierMasAltoDesbloqueado. A propósito escala LINEAL (no exponencial como la Reputación) para que crezca mucho más lento. Con el valor por defecto (0.1), 50 segundos AFK ≈ 1 paseo completo de 50 clicks.")]
    public double dineroPasivoPorTier = 0.1;

    [Header("Mejoras permanentes (compradas con Dinero)")]
    [Tooltip("CostoMejora(nivel) = costoMejoraBase * costoMejoraMultiplicador^nivel. Este costo es el mismo para todas las mejoras; lo que cambia entre mejoras es el porcentaje y a qué tamaño apuntan (ver lista 'Mejoras' más abajo).")]
    public double costoMejoraBase = 100;
    public double costoMejoraMultiplicador = 2.2;
    public int nivelMaximoMejora = 20;

    [Tooltip("Cada mejora tiene nombre, descripción, el TAMAÑO de perro al que afecta (Pequeño/Mediano/Grande) y cuánto % de producción suma por nivel. Para agregar una mejora nueva, agregar un elemento a esta lista — no hace falta tocar código. Una mejora solo afecta a los perros cuyo campo 'tamaño' (en la lista Perros, más abajo) coincida con el tamaño objetivo de la mejora.")]
    public List<MejoraDefinicion> mejoras = new List<MejoraDefinicion>
    {
        new MejoraDefinicion { nombre = "Comida (Pequeño)", tamanoObjetivo = TamanoPerro.Pequeno, porcentajePorNivel = 0.10, descripcion = "Mejor alimento para razas pequeñas." },
        new MejoraDefinicion { nombre = "Correa (Pequeño)", tamanoObjetivo = TamanoPerro.Pequeno, porcentajePorNivel = 0.10, descripcion = "Correas más livianas para razas pequeñas." },
        new MejoraDefinicion { nombre = "Collar (Pequeño)", tamanoObjetivo = TamanoPerro.Pequeno, porcentajePorNivel = 0.10, descripcion = "Collares cómodos para razas pequeñas." },
        new MejoraDefinicion { nombre = "Comida (Mediano)", tamanoObjetivo = TamanoPerro.Mediano, porcentajePorNivel = 0.10, descripcion = "Mejor alimento para razas medianas." },
        new MejoraDefinicion { nombre = "Correa (Mediano)", tamanoObjetivo = TamanoPerro.Mediano, porcentajePorNivel = 0.10, descripcion = "Correas resistentes para razas medianas." },
        new MejoraDefinicion { nombre = "Collar (Mediano)", tamanoObjetivo = TamanoPerro.Mediano, porcentajePorNivel = 0.10, descripcion = "Collares cómodos para razas medianas." },
        new MejoraDefinicion { nombre = "Comida (Grande)", tamanoObjetivo = TamanoPerro.Grande, porcentajePorNivel = 0.10, descripcion = "Mejor alimento para razas grandes." },
        new MejoraDefinicion { nombre = "Correa (Grande)", tamanoObjetivo = TamanoPerro.Grande, porcentajePorNivel = 0.10, descripcion = "Correas reforzadas para razas grandes." },
        new MejoraDefinicion { nombre = "Collar (Grande)", tamanoObjetivo = TamanoPerro.Grande, porcentajePorNivel = 0.10, descripcion = "Collares reforzados para razas grandes." },
    };

    [Header("Prestigio - Renacer")]
    [Tooltip("RequisitoRenacer(k) = renacerRequisitoBase * renacerRequisitoMultiplicador^(k-1)")]
    public double renacerRequisitoBase = 100;
    public double renacerRequisitoMultiplicador = 1.5;
    [Tooltip("Cuánto BaseFlat se gana permanentemente por cada renacer")]
    public double renacerBonoBaseFlat = 1;
    [Tooltip("MultiplicadorGlobal = 1 + renacerBonoMultiplicadorGlobal * renacimientos")]
    public double renacerBonoMultiplicadorGlobal = 0.5;

    [Header("Perros (nombre, imagen, bibliografía)")]
    [Tooltip("El ORDEN de esta lista define la cadena del incremental: el elemento 0 es el tier 1, el elemento 1 es el tier 2, etc. Para agregar un perro nuevo al final de la cadena, solo hay que agregar un elemento acá — el costo y la producción se calculan solos con la fórmula, no hay que tocar código. Si el juego llega a un tier más alto que la cantidad de elementos de esta lista (por ejemplo, mientras se prueba o mientras el equipo de arte todavía no agregó todos los perros), el sistema genera automáticamente un perro placeholder ('Perro Tier N') para que el juego nunca se rompa.")]
    public List<InfoPerro> perros = new List<InfoPerro>
    {
        new InfoPerro { nombre = "Pug" },
        new InfoPerro { nombre = "Chihuahua" },
        new InfoPerro { nombre = "Yorkshire" },
        new InfoPerro { nombre = "Toy Poodle" },
        new InfoPerro { nombre = "Corgi" },
        new InfoPerro { nombre = "Beagle" },
        new InfoPerro { nombre = "Border Collie" },
        new InfoPerro { nombre = "Labrador" },
        new InfoPerro { nombre = "Pastor Alemán" },
        new InfoPerro { nombre = "Golden Retriever" },
    };
}

/// <summary>
/// Toda la información "de contenido" de UN perro de la cadena (no de balance).
/// Se edita 100% desde el Inspector, sin tocar código. Agregar un perro nuevo =
/// agregar un elemento a la lista "perros" de GameConfig, en la posición que
/// le corresponda dentro de la cadena.
/// </summary>
[System.Serializable]
public class InfoPerro
{
    [Tooltip("Nombre que se muestra en la UI")]
    public string nombre;

    [Tooltip("Imagen del perro (foto o el primer frame si es un GIF/animación). Si se deja vacío, la UI usa un ícono genérico y el juego sigue funcionando igual.")]
    public Sprite imagen;

    [Tooltip("Tamaño de este perro. Decide qué mejoras le afectan: una mejora con tamanoObjetivo = Mediano solo suma producción a los perros que tengan este campo puesto en Mediano.")]
    public TamanoPerro tamano = TamanoPerro.Pequeno;

    [Tooltip("Opcional por ahora: texto real sobre la raza, para el panel de bibliografía que se arma más adelante.")]
    [TextArea(2, 6)]
    public string bibliografia;
}

/// <summary>
/// Tamaño de un perro. Se usa para decidir qué mejoras le aplican.
/// Si más adelante hace falta un tamaño nuevo (por ejemplo "Gigante"),
/// se agrega acá como una línea más — es el único cambio de código
/// que requiere esta parte del sistema.
/// </summary>
public enum TamanoPerro { Pequeno, Mediano, Grande }

/// <summary>
/// Una mejora permanente comprable con Dinero. Se edita 100% desde el
/// Inspector: para agregar una mejora nueva, agregar un elemento a la
/// lista "mejoras" de GameConfig con su nombre, descripción, a qué
/// tamaño de perro apunta y qué % de producción suma por nivel.
/// </summary>
[System.Serializable]
public class MejoraDefinicion
{
    [Tooltip("Nombre que se muestra en la UI")]
    public string nombre;

    [Tooltip("Para el panel de detalle/tooltip (no obligatorio para que el juego funcione)")]
    [TextArea(2, 4)]
    public string descripcion;

    [Tooltip("A qué tamaño de perro afecta esta mejora")]
    public TamanoPerro tamanoObjetivo;

    [Tooltip("Cuánto % de producción suma CADA nivel de esta mejora a los perros de ese tamaño (0.10 = +10% por nivel, acumulativo)")]
    public double porcentajePorNivel = 0.10;
}
