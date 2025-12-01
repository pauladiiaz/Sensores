using UnityEngine;
using UnityEngine.InputSystem;  // Solo para Accelerometer y Gyroscope del nuevo Input System
using UnityEngine.Android; // Para permisos de ubicación en Android

public class SensorManager : MonoBehaviour
{
    private Accelerometer accelerometer;
    private UnityEngine.InputSystem.Gyroscope gyroscope;

    void Start()
    {
        // Habilitar todos los sensores disponibles usando InputSystem.devices
        EnableAllDetectedSensors();

        // Activar brújula y arrancar el GPS de forma robusta (coroutine)
        Input.compass.enabled = true;
        StartCoroutine(InitializeLocationService());
    }
    
    void EnableAllDetectedSensors()
    {
        // Recorrer todos los dispositivos y habilitar los sensores
        foreach (var device in InputSystem.devices)
        {
            if (!device.enabled)
            {
                InputSystem.EnableDevice(device);
            }
            
            // Guardar referencias de sensores específicos
            if (device is Accelerometer && accelerometer == null)
                accelerometer = device as Accelerometer;
            
            if (device is UnityEngine.InputSystem.Gyroscope && gyroscope == null)
                gyroscope = device as UnityEngine.InputSystem.Gyroscope;
        }
    }

    System.Collections.IEnumerator InitializeLocationService()
    {
    #if UNITY_ANDROID && !UNITY_EDITOR
        // Solicitar permiso en tiempo de ejecución en Android
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);

            float permissionWait = 0f;
            // esperar hasta 10s a que el usuario responda
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation) && permissionWait < 10f)
            {
                yield return new WaitForSeconds(0.5f);
                permissionWait += 0.5f;
            }
        }
    #endif

        if (!Input.location.isEnabledByUser)
        {
            yield break;
        }

        // Inicia el servicio de localización con cierta precisión y distancia mínima
        Input.location.Start(1f, 0.1f);

        int maxWait = 20; // segundos
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0)
        {
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            yield break;
        }

        yield break;
    }

    void OnDisable()
    {
        // Deshabilitar todos los sensores usando InputSystem.devices
        foreach (var device in InputSystem.devices)
        {
            if (device.enabled)
            {
                InputSystem.DisableDevice(device);
            }
        }

        Input.compass.enabled = false;

        if (Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running)
            Input.location.Stop();
    }

    void Update()
    {
    }
}
