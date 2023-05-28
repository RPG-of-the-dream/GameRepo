using System;
using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using Drawing.Data;
using UnityEngine;

namespace Drawing
{
    public class LevelDrawer : IDisposable
    {
        private readonly LevelsDrawingDataStorage _levelsDrawingDataStorage;
        private readonly Dictionary<float, GraphicGameLayer> _gameLayers;
        private readonly List<ILevelGraphicElement> _graphicElements;

        private LevelDrawingDataStorage _levelDrawingData;

        public LevelDrawer(LevelId levelId)
        {
            _levelsDrawingDataStorage = Resources.Load<LevelsDrawingDataStorage>($"LevelsData/{nameof(LevelsDrawingDataStorage)}");
            _gameLayers = new Dictionary<float, GraphicGameLayer>();
            _graphicElements = new List<ILevelGraphicElement>();
            Initialize(levelId);
        }

        private void Initialize(LevelId levelId)
        {
            _levelDrawingData = 
                _levelsDrawingDataStorage.LevelsData.Find(element => element.LevelId == levelId);

            var currentLayerStartPos = _levelDrawingData.MaxVerticalPosition;
            while (currentLayerStartPos > _levelDrawingData.MinVerticalPosition)
            {
                var layer = new GraphicGameLayer(_levelDrawingData.OrdersPerLayer * _gameLayers.Count);
                _gameLayers.Add(currentLayerStartPos, layer);
                currentLayerStartPos -= _levelDrawingData.MovementLayerStep;
            }
            _gameLayers.Add(_levelDrawingData.MinVerticalPosition, new GraphicGameLayer(_levelDrawingData.OrdersPerLayer * _gameLayers.Count));
        }

        public void RedrawStaticEnvironment(Transform staticElementsContainer)
        {
            var elements = staticElementsContainer.GetComponentsInChildren<ILevelGraphicElement>();
            foreach (var element in elements)
                UpdateElement(element);
        }

        public void RegisterElement(ILevelGraphicElement element)
        {
            _graphicElements.Add(element);
            element.VerticalPositionChanged += UpdateElement;
            UpdateElement(element);
        }

        public void UnregisterElement(ILevelGraphicElement element)
        {
            _graphicElements.Remove(element);
            var prevLayer = _gameLayers.Values.FirstOrDefault(layer => layer.ContainsElement(element));
            element.VerticalPositionChanged -= UpdateElement;
            prevLayer?.RemoveElement(element);
        }

        public void Dispose()
        {
            foreach(var layer in _gameLayers.Values)
                layer.Clear();
            _gameLayers.Clear();
            foreach (var element in _graphicElements)
                element.VerticalPositionChanged -= UpdateElement;
        }

        private void UpdateElement(ILevelGraphicElement element)
        {
            var position = element.VerticalPosition;
            var layerValue = _gameLayers.Keys.LastOrDefault(value => value > position + _levelDrawingData.MovementLayerStep);

            if(!_gameLayers.TryGetValue(layerValue, out var currentLayer) || currentLayer.ContainsElement(element))
                return;

            var prevLayer = _gameLayers.Values.FirstOrDefault(layer => layer.ContainsElement(element));
            prevLayer?.RemoveElement(element);
            _gameLayers[layerValue].AddElement(element);
        }
    }
}