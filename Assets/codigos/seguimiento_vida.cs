using UnityEngine;

public class ManagerBarraVida : MonoBehaviour
{
    private Camera camara;
    private GameObject[] barras;

    void Start()
    {
        camara = Camera.main;
        barras = GameObject.FindGameObjectsWithTag("barra_vida");
    }

    void Update()
    {
        foreach (GameObject barra in barras)
        {
            if (barra == null) continue; // ← Esto evita el error

            Vector3 toObject = barra.transform.position - camara.transform.position;
            Vector3 forward = camara.transform.forward;

            if (Vector3.Dot(forward, toObject) > 0)
            {
                barra.transform.LookAt(camara.transform);
                barra.transform.rotation = Quaternion.Euler(0, barra.transform.rotation.eulerAngles.y, 0); // Solo Y
            }
        }
    }

}
