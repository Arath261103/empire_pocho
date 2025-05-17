using UnityEngine;
using UnityEngine.SceneManagement; // Necesario si quieres cambiar de escena

public class Flor : MonoBehaviour, IVida
{
    public int salud = 30;
    public int saludMaxima = 30;
    public string nombreEscenaAlMorir = "EscenaFinal";

    private bool estaMuerta = false;

    public void RecibirDaño(int daño)
    {
        if (estaMuerta) return; // Ya murió, no seguir procesando

        salud -= daño;
        salud = Mathf.Max(0, salud); // Evita que sea menor a 0

        if (salud <= 0)
        {
            Morir();
        }
    }

    public float GetVida() => salud;

    public float GetVidaMaxima() => saludMaxima;

    void Morir()
    {
        if (estaMuerta) return;

        estaMuerta = true;
        Debug.Log("Flor ha muerto");

        // Aquí puedes agregar animaciones, efectos, etc.

        // Cambiar de escena
        SceneManager.LoadScene(nombreEscenaAlMorir); // Asegúrate que la escena esté en Build Settings
    }
}
