using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class ScreenMaxDistanceKeeper : MonoBehaviour, IPointerMoveHandler
{
    public OnScreenStick stick;
    RectTransform rectTr;

    private void Awake()
    {
        rectTr = GetComponent<RectTransform>();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (Vector3.Distance(eventData.position, transform.position) > stick.movementRange)
        {
            Vector2 fromPointerToPos = (Vector2)rectTr.position - eventData.position;
            rectTr.position = eventData.position + fromPointerToPos.normalized * (stick.movementRange - 1);
        }
    }

    private void Update()
    {
    }
}
