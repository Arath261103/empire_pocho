using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VidaUIController : MonoBehaviour
{
    public GameObject panelVidaUI;       // Panel del Canvas que contiene la barra y el texto
    public Image barraVida;              // Imagen tipo Filled
    public TMP_Text textoVida;           // Texto que muestra nombre del objeto y su vida

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                IVida objetoVida = hit.collider.GetComponent<IVida>();

                if (objetoVida != null)
                {
                    float vidaActual = objetoVida.GetVida();
                    float vidaMaxima = objetoVida.GetVidaMaxima();
                    float porcentaje = vidaActual / vidaMaxima;

                    // Depuración
                    Debug.Log($"Vida Actual: {vidaActual}, Vida Maxima: {vidaMaxima}, Porcentaje: {porcentaje}");

                    // Mostrar nombre del objeto y su vida
                    textoVida.text = $"<b>{hit.collider.gameObject.name}</b>\nVida: {vidaActual}/{vidaMaxima}";

                    // Reducir la barra
                    barraVida.fillAmount = porcentaje;

                    // Mostrar el panel
                    panelVidaUI.SetActive(true);
                }
                else
                {
                    // Oculta el panel si el objeto no tiene vida
                    panelVidaUI.SetActive(false);
                }
            }
        }
    }
}
