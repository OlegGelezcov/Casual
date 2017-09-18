using UnityEngine;

namespace Casual
{
    public interface ICanvasSerive : IService {
        void Add(Transform view);
        void AddToFirstGroup(Transform view);
        void AddToLastGroup(Transform view);
        void RestoreSiblings();
        Vector2 WorldToCanvasPoint(Vector3 position, Camera camera = null);
        Canvas Canvas { get; }
        Vector2 TouchPositionToCanvasPosition(Vector2 touchPosition);
    }
}
