using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingSelector : MonoBehaviour
{
    [System.Serializable]
    public struct BuildingOption
    {
        public string name;
        public Sprite image;
        public GameObject prefab;
        public int price;
    }

    public BuildingOption[] buildings;
    public Image buildingImage;
    public Button leftButton;
    public Button rightButton;
    public Button buildingButton;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;

    private int currentIndex = 0;
    private BuildingPlacer buildingPlacer;
    private ClickGranja clickGranja;

    void Start()
    {
        buildingPlacer = FindFirstObjectByType<BuildingPlacer>();
        clickGranja = FindFirstObjectByType<ClickGranja>();

        leftButton.onClick.AddListener(PreviousBuilding);
        rightButton.onClick.AddListener(NextBuilding);
        buildingButton.onClick.AddListener(StartBuildingPlacement);

        UpdateBuildingDisplay();
    }

    void PreviousBuilding()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = buildings.Length - 1;

        UpdateBuildingDisplay();
    }

    void NextBuilding()
    {
        currentIndex++;
        if (currentIndex >= buildings.Length)
            currentIndex = 0;

        UpdateBuildingDisplay();
    }

    void StartBuildingPlacement()
    {
        var currentBuilding = buildings[currentIndex];

        Debug.Log($"Maíz actual: {clickGranja.cantidadMaiz}, Precio: {currentBuilding.price}");

        if (clickGranja != null && clickGranja.cantidadMaiz >= currentBuilding.price)
        {
            clickGranja.cantidadMaiz -= currentBuilding.price;
            clickGranja.SendMessage("ActualizarTextoMaiz");

            buildingPlacer.StartPlacing(currentBuilding.prefab);
            UpdateBuildingDisplay();
        }
    }

    void UpdateBuildingDisplay()
    {
        var currentBuilding = buildings[currentIndex];
        buildingImage.sprite = currentBuilding.image;
        nameText.text = currentBuilding.name;
        priceText.text = $"Costo: {currentBuilding.price} maíz";

        if (clickGranja != null)
        {
            bool puedeComprar = clickGranja.cantidadMaiz >= currentBuilding.price;
            buildingButton.interactable = puedeComprar;

            // Cambiar color del texto si no alcanza
            priceText.color = puedeComprar ? Color.white : Color.red;
        }
    }
}
