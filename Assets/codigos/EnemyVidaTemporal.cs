using UnityEngine;

public class EnemyVida : MonoBehaviour
{
    public float vida = 100f;
    public Transform barraVida;
    private Vector3 escalaOriginal;

    void Start()
    {
        if (barraVida == null)
        {
            barraVida = transform.Find("BarraVida");
        }

        if (barraVida != null)
        {
            escalaOriginal = barraVida.localScale; // ← AQUÍ SE INICIALIZA

            float porcentaje = vida / 100f;

            barraVida.localScale = new Vector3(escalaOriginal.x * porcentaje, escalaOriginal.y, escalaOriginal.z);

            float diferencia = escalaOriginal.x - barraVida.localScale.x;
            barraVida.localPosition = new Vector3(-diferencia / 2f, barraVida.localPosition.y, barraVida.localPosition.z);
        }
        else
        {
            Debug.LogWarning("No se encontró la barra de vida en " + gameObject.name);
        }
    }


    public void RecibirDaño(float cantidad)
    {
        vida -= cantidad;
        vida = Mathf.Max(0, vida);

        if (barraVida != null)
        {
            float porcentaje = vida / 100f;
            barraVida.localScale = new Vector3(escalaOriginal.x * porcentaje, escalaOriginal.y, escalaOriginal.z);
        }

        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }
}
