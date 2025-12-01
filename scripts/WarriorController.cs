using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Android;

public class WarriorController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotationSpeed = 1f;
    
    // Rango GPS (Madrid por defecto)
    public float latitudeMin = 27.5f;
    public float latitudeMax = 29.5f;
    public float longitudeMin = -17.5f;
    public float longitudeMax = -15.5f;
    
    private Accelerometer accelerometer;

    void Start()
    {
        // Habilitar sensores con InputSystem (como requiere el enunciado)
        accelerometer = Accelerometer.current;
        if (accelerometer != null) 
        {
            InputSystem.EnableDevice(accelerometer);
        }
        
        Input.compass.enabled = true;
        Input.location.Start();
    }

    void Update()
    {
    // 1. Orientar hacia el norte
    float heading = Input.compass.magneticHeading;
    Quaternion northRotation = Quaternion.Euler(0, heading - 90, 0);
    transform.rotation = Quaternion.Slerp(transform.rotation, northRotation, rotationSpeed * Time.deltaTime);

    // 2. Comprobar GPS listo
    if (Input.location.status != LocationServiceStatus.Running)
        return;

    // 3. Verificar rango
    var loc = Input.location.lastData;
    bool inRange = loc.latitude >= latitudeMin && loc.latitude <= latitudeMax &&
                   loc.longitude >= longitudeMin && loc.longitude <= longitudeMax;

    if (!inRange)
        return;

    // 4. Movimiento con acelerómetro
    if (accelerometer != null)
    {
        Vector3 accel = accelerometer.acceleration.ReadValue();
        float forwardSpeed = -accel.z;

        if (Mathf.Abs(forwardSpeed) < 0.15f)
            forwardSpeed = 0f;

        Vector3 movement = transform.forward * forwardSpeed * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }
    }


    void OnDisable()
    {
        // Deshabilitar sensores con InputSystem (como requiere el enunciado)
        if (accelerometer != null) 
        {
            InputSystem.DisableDevice(accelerometer);
        }
        Input.compass.enabled = false;
        Input.location.Stop();
    }
}
