using UnityEngine;
using UnityEngine.InputSystem;

public class Controlcamara : MonoBehaviour
{
    // Velocidades de movimiento y rotaci�n
    public float velocidadMovimiento = 10;
    public float velocidadRotacion = 100; // Nueva variable para la velocidad de rotaci�n

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

            // Rotaci�n de la c�mara utilizando la velocidad de rotaci�n
            yaw.Rotate(0, cambioRotacion * velocidadRotacion * Time.deltaTime, 0);

            // Movimiento de la c�mara
            Vector3 movimientoRotado = yaw.rotation * new Vector3(vectorMovimiento.x, 0, vectorMovimiento.y);
            transform.Translate(movimientoRotado * velocidadMovimiento * Time.deltaTime);
        }
    }
}
