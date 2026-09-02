using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Une toda la lógica (DogManager, CurrencyManager, etc.) con los elementos
/// visuales de la escena. Es el único script que "sabe" de UI; los demás
/// managers no tienen ninguna referencia a botones ni textos.
///
/// Ver la guía paso a paso para saber exactamente qué arrastrar a cada campo
/// en el Inspector.
/// </summary>
public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [Header("Textos generales (arriba de la pantalla)")]
    public TMP_Text textoReputacion;
    public TMP_Text textoDinero;
    public TMP_Text textoProduccion;
    public TMP_Text textoDineroPorSegundo; // opcional: muestra que el Dinero también crece solo
    public Slider medidorPaseo;

    [Header("Lista de perros (panel izquierdo)")]
    public Transform contenedorBotonesPerro;
    public GameObject prefabBotonPerro; // debe tener un Button + al menos 2 TMP_Text hijos

    [Header("Mejoras (panel izquierdo, pestaña Mejoras)")]
    public Transform contenedorBotonesMejora;
    public GameObject prefabBotonMejora; // Button + TextoNombreBoton (el tooltip ahora es un panel compartido, no va adentro del prefab)

    [Header("Capa de tooltips")]
    [Tooltip("Un RectTransform vacío que sea el ÚLTIMO hijo directo del Canvas, para que cualquier cosa adentro se dibuje por encima de todo lo demás. Ahí adentro van el panel de tooltip de mejoras y el de monedas (ambos fijos, no se mueven de ahí).")]
    public RectTransform capaTooltips;

    [Header("Tooltip de mejoras (panel compartido)")]
    [Tooltip("Panel único (fondo + 3 textos) que vive fijo dentro de capaTooltips, empieza inactivo. Ya NO hace falta un panel de descripción por cada botón de mejora.")]
    public RectTransform panelTooltipMejora;
    public TMP_Text textoNombreTooltipMejora;
    public TMP_Text textoPrecioTooltipMejora;
    public TMP_Text textoDescripcionTooltipMejora;
    int? indiceMejoraTooltipActual;

    [Header("Tooltip de monedas (Reputación/Dinero)")]
    [Tooltip("Panel compartido (fondo + 2 textos) que vive dentro de capaTooltips, empieza inactivo. Mismo estilo visual que el de las mejoras.")]
    public RectTransform panelTooltipMoneda;
    public TMP_Text textoNombreTooltipMoneda;
    public TMP_Text textoValorTooltipMoneda;
    TipoMoneda? monedaTooltipActual;

    [Header("Piel visual (imágenes personalizadas)")]
    public GameSkin skin;
    public Image iconoReputacionUI;
    public Image iconoDineroUI;
    public Image fondoBotonPaseoUI;
    public Image fondoMedidorPaseoUI;
    public Image rellenoMedidorPaseoUI;
    public Image fondoBotonBibliografiaUI;

    [Header("Prestigio")]
    public Button botonRenacer;
    public TMP_Text textoRequisitoRenacer;

    void Awake() => Instance = this;

    void Start()
    {
        DogManager.Instance.OnCambio += RefrescarTodo;
        UpgradeManager.Instance.OnCambio += RefrescarTodo;
        WalkClickSystem.Instance.OnCambio += RefrescarTodo;
        AplicarSkin();
        RefrescarTodo();
    }

    void Update()
    {
        textoReputacion.text = "Reputación: " + NumberFormatter.Formatear(CurrencyManager.Instance.reputacion);
        textoDinero.text = "Dinero: " + NumberFormatter.Formatear(CurrencyManager.Instance.dinero);
        textoProduccion.text = NumberFormatter.Formatear(DogManager.Instance.ProduccionTotalPorSegundo()) + " Rep/s";
        if (textoDineroPorSegundo != null)
            textoDineroPorSegundo.text = NumberFormatter.Formatear(CurrencyManager.Instance.DineroPorSegundo()) + " $/s";
        if (medidorPaseo != null) medidorPaseo.value = WalkClickSystem.Instance.ProgresoPaseo01();

        ActualizarTooltipMoneda();
        ActualizarTooltipMejora();
    }

    // Reemplaza sprites genéricos por los de GameSkin, sin tocar ningún número.
    // Si algún campo quedó sin arrastrar (o skin no está asignado), simplemente
    // no cambia nada — el juego se ve con los placeholders de siempre.
    void AplicarSkin()
    {
        if (skin == null) return;
        if (iconoReputacionUI != null && skin.iconoReputacion != null) iconoReputacionUI.sprite = skin.iconoReputacion;
        if (iconoDineroUI != null && skin.iconoDinero != null) iconoDineroUI.sprite = skin.iconoDinero;
        if (fondoBotonPaseoUI != null && skin.fondoBotonPaseo != null) fondoBotonPaseoUI.sprite = skin.fondoBotonPaseo;
        if (fondoMedidorPaseoUI != null && skin.fondoMedidorPaseo != null) fondoMedidorPaseoUI.sprite = skin.fondoMedidorPaseo;
        if (rellenoMedidorPaseoUI != null && skin.rellenoMedidorPaseo != null) rellenoMedidorPaseoUI.sprite = skin.rellenoMedidorPaseo;
        if (fondoBotonBibliografiaUI != null && skin.fondoBotonBibliografia != null) fondoBotonBibliografiaUI.sprite = skin.fondoBotonBibliografia;
    }

    // Llamado por TooltipMoneda al pasar el mouse sobre Reputación o Dinero
    public void MostrarTooltipMoneda(TipoMoneda tipo, string nombre, RectTransform origen)
    {
        if (panelTooltipMoneda == null) return;
        monedaTooltipActual = tipo;
        if (textoNombreTooltipMoneda != null) textoNombreTooltipMoneda.text = nombre;
        panelTooltipMoneda.position = PosicionArribaDe(origen);
        panelTooltipMoneda.gameObject.SetActive(true);
        ActualizarTooltipMoneda();
    }

    public void OcultarTooltipMoneda()
    {
        monedaTooltipActual = null;
        if (panelTooltipMoneda != null) panelTooltipMoneda.gameObject.SetActive(false);
    }

    // Mientras el tooltip de una moneda está abierto, el número sigue
    // actualizándose en vivo (la Reputación crece sola, por ejemplo).
    void ActualizarTooltipMoneda()
    {
        if (monedaTooltipActual == null || textoValorTooltipMoneda == null) return;
        double valor = monedaTooltipActual == TipoMoneda.Reputacion
            ? CurrencyManager.Instance.reputacion
            : CurrencyManager.Instance.dinero;
        textoValorTooltipMoneda.text = NumberFormatter.FormatearCompleto(valor);
    }

    // Llamado por TooltipMejora al pasar el mouse sobre un botón de mejora
    public void MostrarTooltipMejora(int indice, RectTransform origen)
    {
        if (panelTooltipMejora == null) return;
        var mejoras = UpgradeManager.Instance.config.mejoras;
        if (indice < 0 || indice >= mejoras.Count) return;

        indiceMejoraTooltipActual = indice;
        var mejora = mejoras[indice];

        if (textoNombreTooltipMejora != null) textoNombreTooltipMejora.text = mejora.nombre;
        if (textoDescripcionTooltipMejora != null) textoDescripcionTooltipMejora.text = mejora.descripcion;

        panelTooltipMejora.position = PosicionArribaDe(origen);
        panelTooltipMejora.gameObject.SetActive(true);
        ActualizarTooltipMejora();
    }

    public void OcultarTooltipMejora()
    {
        indiceMejoraTooltipActual = null;
        if (panelTooltipMejora != null) panelTooltipMejora.gameObject.SetActive(false);
    }

    // El precio también se actualiza en vivo por si suben de nivel la mejora
    // mientras el tooltip sigue abierto.
    void ActualizarTooltipMejora()
    {
        if (indiceMejoraTooltipActual == null || textoPrecioTooltipMejora == null) return;
        textoPrecioTooltipMejora.text = NumberFormatter.Formatear(UpgradeManager.Instance.CostoSiguienteNivel(indiceMejoraTooltipActual.Value)) + " $";
    }

    // Ubica un tooltip justo ARRIBA del botón que lo disparó, no encima
    // (asume que el panel del tooltip tiene su Pivot en Y = 0, o sea que
    // "crece" hacia arriba desde ese punto). Así no tapa al propio botón
    // ni a sus vecinos.
    Vector3 PosicionArribaDe(RectTransform origen)
    {
        float alturaBoton = origen.rect.height * origen.lossyScale.y;
        return origen.position + new Vector3(0, alturaBoton / 2f + 10f, 0);
    }

    // Conectar este método al OnClick() del sprite/botón del perro principal
    public void OnClickPaseo() => WalkClickSystem.Instance.RegistrarClick();

    // Conectar este método al OnClick() del botón "Renacer"
    public void OnClickRenacer()
    {
        PrestigeManager.Instance.Renacer();
        RefrescarTodo();
    }

    void RefrescarTodo()
    {
        RefrescarListaPerros();
        RefrescarListaMejoras();
        RefrescarPrestigio();
    }

    void RefrescarListaPerros()
    {
        foreach (Transform hijo in contenedorBotonesPerro) Destroy(hijo.gameObject);

        foreach (var tier in DogManager.Instance.tiersDesbloqueados)
        {
            GameObject fila = Instantiate(prefabBotonPerro, contenedorBotonesPerro);
            var info = DogManager.Instance.InfoDe(tier.indice);

            var textos = fila.GetComponentsInChildren<TMP_Text>();
            // textos[0] = nombre y cantidad, textos[1] = costo. Ajustar según el orden
            // en que hayan puesto los TMP_Text dentro del prefab.
            textos[0].text = $"{info.nombre} (x{tier.cantidadComprada})";
            textos[1].text = NumberFormatter.Formatear(DogManager.Instance.CostoActual(tier)) + " Rep";

            // Busca un hijo llamado "IconoPerro" con un componente Image y le pone
            // la imagen del perro. Si el perro todavía no tiene imagen cargada
            // (o el prefab no tiene ese hijo), simplemente no hace nada — no rompe nada.
            Transform iconoTransform = fila.transform.Find("IconoPerro");
            if (iconoTransform != null && info.imagen != null)
            {
                var icono = iconoTransform.GetComponent<Image>();
                if (icono != null) icono.sprite = info.imagen;
            }

            fila.GetComponent<Button>().onClick.AddListener(() => DogManager.Instance.Comprar(tier));
        }
    }

    void RefrescarListaMejoras()
    {
        // Por si el tooltip está abierto justo cuando se refresca la lista
        // (por ejemplo, apenas comprás la mejora que estás mirando): lo
        // cerramos antes de borrar los botones, para que nunca quede
        // apuntando a algo que ya no existe.
        OcultarTooltipMejora();
        foreach (Transform hijo in contenedorBotonesMejora) Destroy(hijo.gameObject);

        var mejoras = UpgradeManager.Instance.config.mejoras;
        for (int i = 0; i < mejoras.Count; i++)
        {
            var mejora = mejoras[i];
            GameObject fila = Instantiate(prefabBotonMejora, contenedorBotonesMejora);

            // Aplica el fondo del botón de mejora (piel visual), si está cargado
            // Cada mejora recibe el fondo correspondiente según su índice (mejora 0, 3, 6... → fondo1; 1, 4, 7... → fondo2; 2, 5, 8... → fondo3)
            if (skin != null)
            {
                var fondo = fila.GetComponent<Image>();
                if (fondo != null)
                {
                    int indiceImagenMejora = (i % 3) + 1; // Mapea índices: 0,3,6→1  1,4,7→2  2,5,8→3
                    Sprite spriteAsignado = indiceImagenMejora switch
                    {
                        1 => skin.fondoBotonMejora1,
                        2 => skin.fondoBotonMejora2,
                        3 => skin.fondoBotonMejora3,
                        _ => null
                    };
                    if (spriteAsignado != null) fondo.sprite = spriteAsignado;
                }
            }

            // Texto corto, siempre visible sobre el botón
            var textoBoton = fila.transform.Find("TextoNombreBoton")?.GetComponent<TMP_Text>();
            if (textoBoton != null) textoBoton.text = mejora.nombre;

            // El tooltip (nombre/precio/descripción) ahora es un panel único y
            // compartido — el botón solo necesita saber SU índice para poder pedirlo.
            var hover = fila.GetComponent<TooltipMejora>();
            if (hover == null) hover = fila.AddComponent<TooltipMejora>();
            hover.indiceMejora = i;

            int indiceCapturado = i; // hay que capturar la variable para el lambda
            fila.GetComponent<Button>().onClick.AddListener(() => UpgradeManager.Instance.ComprarMejora(indiceCapturado));
        }
    }

    void RefrescarPrestigio()
    {
        double requisito = PrestigeManager.Instance.RequisitoParaSiguienteRenacer();
        textoRequisitoRenacer.text = $"Renacer {PrestigeManager.Instance.renacimientos + 1}: " +
            $"{DogManager.Instance.TotalPerrosComprados()}/{NumberFormatter.Formatear(requisito)} perros";
        botonRenacer.interactable = PrestigeManager.Instance.PuedeRenacer();
    }
}
