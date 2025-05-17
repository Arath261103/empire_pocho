using UnityEngine;
using TMPro;

public class RecolectarMaiz : MonoBehaviour
{
    public int puntosAGanar = 5;
    public TextMeshProUGUI textoPuntos;

    private ClickGranja clickGranja;

    void Start()
    {
        // Buscamos el ClickGranja en la escena
        clickGranja = FindFirstObjectByType<ClickGranja>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && clickGranja != null)
        {
            clickGranja.cantidadMaiz += puntosAGanar;
            clickGranja.SendMessage("ActualizarTextoMaiz");
            Destroy(transform.parent.gameObject);

        }
    }
}
