using UnityEngine;

public class Torre : MonoBehaviour, IVida
{
    public int vidaMaxima = 100;
    private int vidaActual;

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDaño(int cantidad)
    {
        vidaActual -= cantidad;
        Debug.Log($"{gameObject.name} recibió {cantidad} de daño. Vida restante: {vidaActual}");

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        Debug.Log($"{gameObject.name} ha sido destruida.");
        Destroy(gameObject); // o pon animación de destrucción si lo prefieres
    }

    public float GetVida()
    {
        return vidaActual;
    }

    public float GetVidaMaxima()
    {
        return vidaMaxima;
    }
}
