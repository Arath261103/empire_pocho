using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    private GameObject previewInstance;
    private bool isPlacing = false;
    private GameObject buildingToPlace;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (isPlacing)
        {
            UpdatePreviewPosition();

            if (Input.GetMouseButtonDown(0)) // Click izquierdo
            {
                PlaceBuilding();
            }
            else if (Input.GetMouseButtonDown(1)) // Click derecho para cancelar
            {
                CancelPlacement();
            }
        }
    }

    public void StartPlacing(GameObject prefab)
    {
        if (previewInstance != null)
        {
            Destroy(previewInstance);
        }

        buildingToPlace = prefab;
        previewInstance = Instantiate(buildingToPlace);
        SetPreviewMode(previewInstance, true);

        isPlacing = true;
    }

    void UpdatePreviewPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Ground")) // Asegúrate que tu terreno tenga el tag "Ground"
            {
                previewInstance.transform.position = hit.point;
            }
        }
    }

    void PlaceBuilding()
    {
        Instantiate(buildingToPlace, previewInstance.transform.position, Quaternion.identity);
        Destroy(previewInstance);
        isPlacing = false;
    }

    void CancelPlacement()
    {
        Destroy(previewInstance);
        isPlacing = false;
    }

    void SetPreviewMode(GameObject building, bool isPreview)
    {
        Renderer[] renderers = building.GetComponentsInChildren<Renderer>();
        foreach (var rend in renderers)
        {
            Color color = rend.material.color;
            color.a = isPreview ? 0.5f : 1f; // Transparente cuando es preview
            rend.material.color = color;
        }
    }
}
