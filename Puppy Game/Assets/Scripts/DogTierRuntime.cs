/// <summary>
/// Estado de UN tier de perro mientras se juega (no es un asset, se crea en memoria).
/// No hay límite de tiers: cada vez que se compra el primer perro del tier más alto
/// desbloqueado, DogManager crea automáticamente el siguiente.
/// </summary>
[System.Serializable]
public class DogTierRuntime
{
    public int indice;                 // 1 = primer tier (Pug), 2 = segundo, etc.
    public int cantidadComprada;       // cuántas copias de este perro tiene el jugador
    public double multiplicadorCosto;  // multiplicador de precio acumulado (arranca en 1.0)

    public DogTierRuntime(int indice)
    {
        this.indice = indice;
        cantidadComprada = 0;
        multiplicadorCosto = 1.0;
    }
}
