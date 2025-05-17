using UnityEngine;

public enum TipoPersonajeJugador
{
    Espadachin,
    Barbaro,
    Arquero
}

public class PersonajeBase : MonoBehaviour
{
    public int saludMaxima = 100;
    public int saludActual;
    public TipoPersonajeJugador tipoPersonaje;  // <-- Este campo nuevo

    protected virtual void Start()
    {
        saludActual = saludMaxima;
    }

    public virtual void RecibirDa�o(int da�o)
    {
        saludActual -= da�o;
        if (saludActual <= 0)
        {
            Morir();
        }
    }

    protected virtual void Morir()
    {
        Debug.Log(gameObject.name + " ha muerto.");
        Destroy(gameObject);
    }

    public int ObtenerSaludActual()
    {
        return saludActual;
    }

    public int ObtenerSaludMaxima()
    {
        return saludMaxima;
    }
}