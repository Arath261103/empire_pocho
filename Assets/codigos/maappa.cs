using UnityEngine;
using UnityEngine.UI;

public class ToggleMinimap : MonoBehaviour
{
    public RawImage minimap;

    private void Start()
    {
        if (minimap != null)
            minimap.enabled = false; // asegúrate que empiece oculto
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (minimap != null)
                minimap.enabled = !minimap.enabled; // alterna visibilidad
        }
    }
}
