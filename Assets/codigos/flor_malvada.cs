using UnityEngine;
using UnityEngine.SceneManagement;

public class FlorMalvada : MonoBehaviour, IVida
{
    public int salud = 100;
    private int saludMaxima;

    public string nombreEscenaAlMorir = "Victoria";

    void Start()
    {
        saludMaxima = salud;
    }

    public void RecibirDaño(int daño)
    {
        salud -= daño;
        if (salud <= 0)
        {
            Morir();
        }
    }

    public float GetVida()
    {
        return salud;
    }

    public float GetVidaMaxima()
    {
        return saludMaxima;
    }

    void Morir()
    {
        Debug.Log("Flor Malvada ha muerto");
        SceneManager.LoadScene(nombreEscenaAlMorir);
    }
}
