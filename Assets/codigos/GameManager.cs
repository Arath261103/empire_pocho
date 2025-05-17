using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI textoPuntos;
    private int puntosActuales = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AumentarPuntos(int cantidad)
    {
        puntosActuales += cantidad;
        textoPuntos.text = puntosActuales.ToString();
    }

    public int ObtenerPuntos()
    {
        return puntosActuales;
    }
}
