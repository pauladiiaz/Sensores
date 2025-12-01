using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using InputGyro = UnityEngine.InputSystem.Gyroscope;

public class SensorDisplay : MonoBehaviour
{
    public TextMeshProUGUI sensorText;

    void Start()
    {
        Input.compass.enabled = true;
        Input.location.Start();
        
        // Habilitar todos los sensores disponibles
        EnableAllAvailableSensors();
    }
    
    void EnableAllAvailableSensors()
    {
        foreach (var device in InputSystem.devices)
        {
            // Habilitar todos los sensores detectados
            if (!device.enabled)
            {
                InputSystem.EnableDevice(device);
            }
        }
    }

    void Update()
    {
        string text = "=== Sensores detectados con InputSystem.devices ===\n\n";
        
        // Mostrar todos los dispositivos conectados usando InputSystem.devices
        text += $"Total dispositivos: {InputSystem.devices.Count}\n";
        foreach (var device in InputSystem.devices)
        {
            text += $"• {device.name} ({device.GetType().Name}) - Enabled: {device.enabled}\n";
        }
        text += "\n=== Valores de sensores ===\n\n";

        // ACELERÓMETRO
        if (Accelerometer.current != null && Accelerometer.current.enabled)
            text += $"Acelerómetro: {Accelerometer.current.acceleration.ReadValue()}\n";

        // GIROSCOPIO
        if (InputGyro.current != null && InputGyro.current.enabled)
            text += $"Giroscopio: {InputGyro.current.angularVelocity.ReadValue()}\n";

        // GRAVEDAD
        var gravity = UnityEngine.InputSystem.GravitySensor.current;
        if (gravity != null && gravity.enabled)
            text += $"Gravity: {gravity.gravity.ReadValue()}\n";

        // ACELERACIÓN LINEAL
        var linear = UnityEngine.InputSystem.LinearAccelerationSensor.current;
        if (linear != null && linear.enabled)
            text += $"Aceleración Lineal: {linear.acceleration.ReadValue()}\n";

        // CAMPO MAGNÉTICO
        var magnetic = UnityEngine.InputSystem.MagneticFieldSensor.current;
        if (magnetic != null && magnetic.enabled)
            text += $"Campo Magnético: {magnetic.magneticField.ReadValue()} μT\n";

        // LUZ
        var light = UnityEngine.InputSystem.LightSensor.current;
        if (light != null && light.enabled)
            text += $"Luz: {light.lightLevel.ReadValue()} lux\n";

        // PROXIMIDAD
        var prox = UnityEngine.InputSystem.ProximitySensor.current;
        if (prox != null && prox.enabled)
            text += $"Proximidad: {prox.distance.ReadValue()} cm\n";

        // PRESIÓN (Solo Android)
        var pressure = UnityEngine.InputSystem.PressureSensor.current;
        if (pressure != null && pressure.enabled)
            text += $"Presión: {pressure.atmosphericPressure.ReadValue()} hPa\n";

        // HUMEDAD (Solo Android)
        var humidity = UnityEngine.InputSystem.HumiditySensor.current;
        if (humidity != null && humidity.enabled)
            text += $"Humedad: {humidity.relativeHumidity.ReadValue()}%\n";

        // TEMPERATURA (Solo Android)
        var temp = UnityEngine.InputSystem.AmbientTemperatureSensor.current;
        if (temp != null && temp.enabled)
            text += $"Temperatura: {temp.ambientTemperature.ReadValue()}°C\n";

        // CONTADOR DE PASOS (Solo Android)
        var steps = UnityEngine.InputSystem.StepCounter.current;
        if (steps != null && steps.enabled)
            text += $"Pasos: {steps.stepCounter.ReadValue()}\n";

        // ORIENTACIÓN
        var att = UnityEngine.InputSystem.AttitudeSensor.current;
        if (att != null && att.enabled)
            text += $"Orientación: {att.attitude.ReadValue()}\n";

        // BRÚJULA CLÁSICA (no InputSystem)
        if (Input.compass.enabled)
            text += $"Brújula: {Input.compass.magneticHeading}°\n";

        // GPS (no InputSystem)
        if (Input.location.status == LocationServiceStatus.Running)
        {
            var loc = Input.location.lastData;
            text += $"GPS: Lat {loc.latitude:F6}, Lon {loc.longitude:F6}, Alt {loc.altitude:F1}m\n";
        }
        else
        {
            text += $"GPS: {Input.location.status}\n";
        }

        sensorText.text = text;
    }
}
