using System;

namespace Drawing
{
    public interface ILevelGraphicElement
    {
        float VerticalPosition { get; }
        event Action<ILevelGraphicElement> VerticalPositionChanged;
        void SetDrawingOrder(int order);
    }
}