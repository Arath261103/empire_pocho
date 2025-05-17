using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClickGranja : MonoBehaviour
{
    public Material materialSeleccionado;
    public GameObject botonCultivar;
    public TextMeshProUGUI contadorMaizUI;
    public GameObject temporizadorUI;
    public float tiempoCultivo = 2f;
    public Camera camaraPrincipal;

    private GameObject granjaSeleccionada;
    private Material materialOriginal;
    private bool cultivando = false;
    public int cantidadMaiz = 0;
    public int maizPorCultivo = 20;


    private TextMeshProUGUI textoTemporizador;

    void Start()
    {
        if (botonCultivar != null)
            botonCultivar.SetActive(false);

        if (temporizadorUI != null)
        {
            temporizadorUI.SetActive(false);
            textoTemporizador = temporizadorUI.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (camaraPrincipal == null)
            camaraPrincipal = Camera.main;

        ActualizarTextoMaiz();
    }


    void Update()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0) && !cultivando)
        {
            Ray ray = camaraPrincipal.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Granja"))
                {
                    SeleccionarGranja(hit.collider.gameObject);
                }
                else
                {
                    DeseleccionarGranja();
                }
            }
            else
            {
                DeseleccionarGranja();
            }
        }



    }

    void SeleccionarGranja(GameObject granja)
    {
        if (granjaSeleccionada != null && granja != granjaSeleccionada)
            RestaurarMaterial();

        granjaSeleccionada = granja;

        Renderer rend = granjaSeleccionada.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            materialOriginal = rend.material;
            rend.material = materialSeleccionado;
        }

        if (botonCultivar != null)
            botonCultivar.SetActive(true);
    }

    void DeseleccionarGranja()
    {
        RestaurarMaterial();
        granjaSeleccionada = null;

        if (botonCultivar != null)
            botonCultivar.SetActive(false);
    }

    void RestaurarMaterial()
    {
        if (granjaSeleccionada == null) return;

        Renderer rend = granjaSeleccionada.GetComponentInChildren<Renderer>();
        if (rend != null && materialOriginal != null)
            rend.material = materialOriginal;
    }

    public void IniciarCultivo()
    {
        if (granjaSeleccionada == null || cultivando) return;

        botonCultivar.SetActive(false);
        temporizadorUI.SetActive(true);
        StartCoroutine(TemporizadorCultivo());
    }

    System.Collections.IEnumerator TemporizadorCultivo()
    {
        cultivando = true;
        float tiempoRestante = tiempoCultivo;

        while (tiempoRestante > 0f)
        {
            if (textoTemporizador != null)
                textoTemporizador.text = $"{tiempoRestante:F1}s";

            tiempoRestante -= Time.deltaTime;
            yield return null;
        }

        if (textoTemporizador != null)
            textoTemporizador.text = "0.0s";

        temporizadorUI.SetActive(false);
        cantidadMaiz += maizPorCultivo;

        ActualizarTextoMaiz();
        Debug.Log("Cantidad de maíz: " + cantidadMaiz);

        cultivando = false;
        DeseleccionarGranja();
    }


    void ActualizarTextoMaiz()
    {
        if (contadorMaizUI != null)
        {
            contadorMaizUI.text = $"Maíz: {cantidadMaiz}";
        }
    }
}