using UnityEngine;
using UnityEngine.InputSystem;

public class Controlcamara : MonoBehaviour
{
    // Velocidades de movimiento y rotación
    public float velocidadMovimiento = 10;
    public float velocidadRotacion = 100; // Nueva variable para la velocidad de rotación

    private InputAction movimiento;
    private InputAction rotacion;
    private Transform yaw;

    void Start()
    {
        movimiento = InputSystem.actions.FindAction("Movimiento");
        rotacion = InputSystem.actions.FindAction("Rotation");
        yaw = transform.Find("Yaw");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            Vector2 vectorMovimiento = movimiento.ReadValue<Vector2>();
            float cambioRotacion = rotacion.ReadValue<float>();

            // Rotación de la cámara utilizando la velocidad de rotación
            yaw.Rotate(0, cambioRotacion * velocidadRotacion * Time.deltaTime, 0);

            // Movimiento de la cámara
            Vector3 movimientoRotado = yaw.rotation * new Vector3(vectorMovimiento.x, 0, vectorMovimiento.y);
            transform.Translate(movimientoRotado * velocidadMovimiento * Time.deltaTime);
        }
    }
}
