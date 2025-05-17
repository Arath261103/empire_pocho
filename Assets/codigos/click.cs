using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.UI;

public class Click : MonoBehaviour
{
    public Material selectedMaterial;
    private Dictionary<GameObject, List<Material>> originalMaterials = new Dictionary<GameObject, List<Material>>();

    private List<GameObject> selectedObjects = new List<GameObject>();
    private Vector3 selectionStartPos;
    private bool isSelecting = false;
    public LayerMask personajesLayer;

    private GameObject enemigoObjetivo = null;
    private bool atacando = false;

    public Button botonAtacar;
    public Button botonCancelar;

    private Rect selectionRect;

    void Start()
    {
        // Ocultar botones al iniciar
        botonAtacar.gameObject.SetActive(false);
        botonCancelar.gameObject.SetActive(false);

        // Asignar eventos a botones
        botonAtacar.onClick.AddListener(ComenzarAtaque);
        botonCancelar.onClick.AddListener(CancelarAtaque);
    }

    void Update()
    {
        DetectClick();
        MoveSelectedCharacters();
        UpdateAnimations();

        // Detectar clic derecho para seleccionar enemigo y mostrar botón atacar
        if (Input.GetMouseButtonDown(1) && selectedObjects.Count > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.SphereCastAll(ray, 2.5f);

            GameObject objetivoCercano = null;
            float distanciaMinima = Mathf.Infinity;

            foreach (var hit in hits)
            {
                GameObject obj = hit.collider.gameObject;
                if (obj.CompareTag("arquero_malo") || obj.CompareTag("barbaro_malo") || obj.CompareTag("espadachin_malo"))
                {
                    float distancia = Vector3.Distance(hit.point, ray.origin);
                    if (distancia < distanciaMinima)
                    {
                        distanciaMinima = distancia;
                        objetivoCercano = obj;
                    }
                }
            }

            if (objetivoCercano != null && !atacando)
            {
                enemigoObjetivo = objetivoCercano;
                botonAtacar.gameObject.SetActive(true);
                botonCancelar.gameObject.SetActive(false);
            }
            else if (Physics.Raycast(ray, out RaycastHit hitSuelo) && hitSuelo.collider.CompareTag("Ground"))
            {
                enemigoObjetivo = null;
                botonAtacar.gameObject.SetActive(false);
                botonCancelar.gameObject.SetActive(false);

                foreach (GameObject obj in selectedObjects)
                {
                    NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
                    if (agent != null)
                    {
                        agent.SetDestination(hitSuelo.point);
                    }
                }
                atacando = false;
            }
            else
            {
                enemigoObjetivo = null;
                botonAtacar.gameObject.SetActive(false);
                botonCancelar.gameObject.SetActive(false);
            }
        }

        // NUEVA LÓGICA: si estamos atacando, perseguir al enemigo
        if (atacando && enemigoObjetivo != null)
        {
            foreach (GameObject obj in selectedObjects)
            {
                if (obj == null) continue;

                NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
                if (agent != null && enemigoObjetivo != null)
                {
                    agent.SetDestination(enemigoObjetivo.transform.position);
                }

                // Aquí puedes agregar lógica para atacar cuando esté cerca (por distancia, animación, etc.)
                float distancia = Vector3.Distance(obj.transform.position, enemigoObjetivo.transform.position);
                if (distancia < 2f)
                {
                    Animator anim = obj.GetComponent<Animator>();
                    if (anim != null)
                    {
                        anim.SetTrigger("atacar"); // Debes tener este trigger en tu animador
                    }
                }
            }
        }
    }

    public void ComenzarAtaque()
    {
        if (enemigoObjetivo == null) return;

        atacando = true;
        botonAtacar.gameObject.SetActive(false);
        botonCancelar.gameObject.SetActive(true);
    }

    public void CancelarAtaque()
    {
        atacando = false;
        enemigoObjetivo = null;
        botonAtacar.gameObject.SetActive(false);
        botonCancelar.gameObject.SetActive(false);
    }

    private void DetectClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectionStartPos = Input.mousePosition;
            isSelecting = true;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, personajesLayer))
            {
                GameObject selectedObject = hit.collider.gameObject;

                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    ToggleSelection(selectedObject);
                }
                else
                {
                    DeselectAll();
                    SelectObject(selectedObject);
                }
            }
            else
            {
                DeselectAll();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;
            SelectObjectsInBox();
        }
    }

    private void ToggleSelection(GameObject obj)
    {
        if (selectedObjects.Contains(obj))
        {
            DeselectObject(obj);
        }
        else
        {
            SelectObject(obj);
        }
    }

    private void SelectObject(GameObject obj)
    {
        if (!originalMaterials.ContainsKey(obj))
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            List<Material> materials = new List<Material>();

            foreach (Renderer renderer in renderers)
            {
                materials.Add(renderer.material);
                renderer.material = selectedMaterial;
            }

            originalMaterials[obj] = materials;
            selectedObjects.Add(obj);
        }
    }

    private void UpdateAnimations()
    {
        for (int i = selectedObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = selectedObjects[i];
            if (obj == null)
            {
                selectedObjects.RemoveAt(i);
                continue;
            }

            NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
            Animator anim = obj.GetComponent<Animator>();

            if (agent != null && anim != null)
            {
                if (agent.hasPath && agent.remainingDistance > agent.stoppingDistance)
                {
                    anim.SetBool("isSprinting", true);
                    anim.SetBool("isIdle", false);
                }
                else
                {
                    anim.SetBool("isSprinting", false);
                    anim.SetBool("isIdle", true);
                }
            }
        }
    }

    private void DeselectObject(GameObject obj)
    {
        if (selectedObjects.Contains(obj))
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            if (originalMaterials.TryGetValue(obj, out List<Material> materials))
            {
                for (int i = 0; i < renderers.Length; i++)
                {
                    if (i < materials.Count)
                    {
                        renderers[i].material = materials[i];
                    }
                }
            }

            originalMaterials.Remove(obj);
            selectedObjects.Remove(obj);
        }
    }

    private void DeselectAll()
    {
        for (int i = selectedObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = selectedObjects[i];
            if (obj == null)
            {
                selectedObjects.RemoveAt(i);
                originalMaterials.Remove(obj);
                continue;
            }

            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            if (originalMaterials.TryGetValue(obj, out List<Material> materials))
            {
                for (int j = 0; j < renderers.Length; j++)
                {
                    if (j < materials.Count)
                    {
                        renderers[j].material = materials[j];
                    }
                }
            }
        }

        originalMaterials.Clear();
        selectedObjects.Clear();
    }

    private void SelectObjectsInBox()
    {
        selectionRect = GetScreenRect(selectionStartPos, Input.mousePosition);

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            Renderer renderer = obj.GetComponentInChildren<Renderer>();

            if (renderer == null) continue;

            Bounds bounds = renderer.bounds;
            Vector3[] screenPoints = new Vector3[]
            {
                Camera.main.WorldToScreenPoint(bounds.min),
                Camera.main.WorldToScreenPoint(bounds.max),
                Camera.main.WorldToScreenPoint(bounds.center)
            };

            foreach (Vector3 screenPoint in screenPoints)
            {
                Vector3 adjustedPoint = screenPoint;
                adjustedPoint.y = Screen.height - adjustedPoint.y;

                if (selectionRect.Contains(adjustedPoint))
                {
                    SelectObject(obj);
                    break;
                }
            }
        }
    }

    private Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        float x = Mathf.Min(screenPosition1.x, screenPosition2.x);
        float y = Mathf.Min(screenPosition1.y, screenPosition2.y);
        float width = Mathf.Abs(screenPosition1.x - screenPosition2.x);
        float height = Mathf.Abs(screenPosition1.y - screenPosition2.y);
        return new Rect(x, Screen.height - y - height, width, height);
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            Rect rect = GetScreenRect(selectionStartPos, Input.mousePosition);
            GUI.color = new Color(0, 1, 0, 0.3f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.green;
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, 2), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height - 2, rect.width, 2), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.x, rect.y, 2, rect.height), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.x + rect.width - 2, rect.y, 2, rect.height), Texture2D.whiteTexture);
        }
    }

    private void MoveSelectedCharacters()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Ground") && !atacando)
                {
                    foreach (GameObject obj in selectedObjects)
                    {
                        NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
                        if (agent != null)
                        {
                            agent.SetDestination(hit.point);
                        }
                    }

                    botonAtacar.gameObject.SetActive(false);
                    botonCancelar.gameObject.SetActive(false);
                    enemigoObjetivo = null;
                }
            }
        }
    }
}
