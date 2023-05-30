using Core.Enums;
using UnityEngine;

namespace Drawing.Data
{
    [CreateAssetMenu(fileName = nameof(LevelDrawingDataStorage), menuName = "Drawing/LevelDrawingDataStorage")]
    public class LevelDrawingDataStorage : ScriptableObject
    {
        [field: SerializeField] public LevelId LevelId { get; private set; }
        [field: SerializeField] public float MovementLayerStep { get; private set; }
        [field: SerializeField] public int OrdersPerLayer { get; private set; }
        [field: SerializeField] public float MaxVerticalPosition { get; private set; }
        [field: SerializeField] public float MinVerticalPosition { get; private set; }
    }
}