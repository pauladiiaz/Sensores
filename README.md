# Práctica de Sensores

## Scripts Principales
- **SensorManager.cs**: Gestión y habilitación automática de sensores
- **SensorDisplay.cs**: Visualización en tiempo real de valores
- **WarriorController.cs**: Control de personaje con acelerómetro y brújula

## Jerarquía de Escena
```
SampleScene
├── Main Camera
├── Directional Light  
├── Guerrero → WarriorController.cs
├── SensorManager (Empty) → SensorManager.cs y SensorDisplay.cs
└── Canvas → SensorText (TextMeshPro)
```

## 📊 Medidas

### Laboratorio ESIT
```
Medidas GPS: 28.483083, -16.321388

```

### Jardín ESIT
```
Medidas GPS: 28.482601, -16.322206

```

## Vídeo demostrativo
[![Vídeo Demostrativo](https://github.com/user-attachments/assets/021cb59f-f243-4ccb-b3d1-f74da7d3b374)](https://github.com/user-attachments/assets/021cb59f-f243-4ccb-b3d1-f74da7d3b374)