using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
public static class Helpers {
    ///<summary> Get and store the maincamera</summary>
    private static Camera _camera;
    public static Camera Camera {
        get {
            if (_camera == null) _camera = Camera.main;
            return _camera;
        }
    }

    /// <summary>
    /// Store all used Wait for seconds, so don't need to create more trash
    /// </summary>
    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWait(float time) {
        if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }


    ///<summary> Destroy all childrens of this gameobject</summary>
    public static void DestroyChildren(this Transform t) {
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }


    /// <summary>
    /// Check if your mouse can click in this position or sth blocked the raycast
    /// </summary>
    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _result;
    public static bool IsOverUi() {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        _result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _result);
        return _result.Count > 0;
    }
    /// <summary>
    /// Get World Position Of Canvas Element. Ex: When you want object follow an rect object
    /// </summary>
    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(element, element.position, Camera, out var _result);
        return _result;
    }

}
