using System;
using UI.Enums;

namespace UI.Core
{
    public interface IScreenController
    {
        event Action CloseRequested;
        event Action<ScreenType> OpenScreenRequested;
        void Initialize();
        void Complete();
    }
}