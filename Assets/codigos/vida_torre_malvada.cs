using UnityEngine;

public class TorreMala : PersonajeBase, IVida
{
    public string tipoTorre = "torre_espadachin_malo";

    protected override void Start()
    {
        base.Start();

        if (tipoTorre == "torre_arquero_malo") saludMaxima = 1000;
        else if (tipoTorre == "torre_barbaro_malo") saludMaxima = 1000;
        else if (tipoTorre == "torre_espadachin_malo") saludMaxima = 1000;

        saludActual = saludMaxima;
    }

    public override void RecibirDaño(int cantidad)
    {
        base.RecibirDaño(cantidad);

        if (saludActual <= 0)
        {
            Morir();
        }
    }

    public float GetVida()
    {
        return saludActual;
    }

    public float GetVidaMaxima()
    {
        return saludMaxima;
    }

    protected override void Morir()
    {
        Debug.Log($"{gameObject.name} ha sido destruida.");
        Destroy(gameObject);
    }
}
