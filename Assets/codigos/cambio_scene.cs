using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorDeEscena : MonoBehaviour
{
    // Cambia a una escena cuyo nombre se pasa como argumento
    public void CambiarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    // Cierra el juego (funciona solo en builds, no en el editor)
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
