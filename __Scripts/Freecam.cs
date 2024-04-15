using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class Freecam : MonoBehaviour
{
    public Camera cam;
    public EventSystem events;

    public Vector2 xBounds;
    public Vector2 yBounds;
    private Vector2 bottomLeft;
    private Vector2 topRight;

    public float sens;
    public float speed;
    public float accel;

    public float rate;
    public float min;
    public float max;

    public bool blocked;
    private Vector2 prevMousePos;
    private float velocity;

    void Update()
    {
        MouseZoom();
        RecalculateBounds();
        TryFixing();
        MousePan();
        KeypadMove();
    }

    void MouseZoom()
    {
        if(events.IsPointerOverGameObject()){ return; }
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - Input.mouseScrollDelta.y * Time.deltaTime * rate, min, max);
    }

    void RecalculateBounds()
    {
        bottomLeft = (Vector2) cam.transform.position - new Vector2(cam.orthographicSize * 16f / 9f, cam.orthographicSize);
        topRight = (Vector2) cam.transform.position + new Vector2(cam.orthographicSize * 16f / 9f, cam.orthographicSize);
    }

    void TryFixing()
    {
        int crashPrevention = 0;

        while(bottomLeft.x < xBounds.x && crashPrevention < 99){ transform.Translate(Vector2.right * 0.1f); RecalculateBounds(); crashPrevention++; }
        while(topRight.x > xBounds.y && crashPrevention < 99){ transform.Translate(-Vector2.right * 0.1f); RecalculateBounds(); crashPrevention++; }
        while(bottomLeft.y < yBounds.x && crashPrevention < 99){ transform.Translate(Vector2.up * 0.1f); RecalculateBounds(); crashPrevention++; }
        while(topRight.y > yBounds.y && crashPrevention < 99){ transform.Translate(-Vector2.up * 0.1f); RecalculateBounds(); crashPrevention++; }
    }
    
    void MousePan()
    {
        Vector2 mousePos = (Vector2) Input.mousePosition / new Vector2(Screen.width, Screen.width);
        mousePos.x = Mathf.Clamp(mousePos.x, 0f, 1f);
        mousePos.y = Mathf.Clamp(mousePos.y, 0f, 1f);

        if((Input.GetMouseButtonDown(0) && CanDrag()) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)){ blocked = events.IsPointerOverGameObject(); }
        if((!Input.GetMouseButton(0) && CanDrag()) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2)){ blocked = false; }

        if(((Input.GetMouseButton(0) && CanDrag()) || Input.GetMouseButton(1) || Input.GetMouseButton(2)) && !blocked)
        {
            Vector2 deltaMouse = prevMousePos - mousePos;
            transform.Translate(deltaMouse * sens * cam.orthographicSize / 5f);
        }
        prevMousePos = mousePos;
    }

    void KeypadMove()
    {
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(inputVector == Vector2.zero){ velocity = 8; return; }
        velocity = Mathf.Min(velocity + accel * Time.deltaTime, speed);
        transform.Translate(inputVector.normalized * Time.deltaTime * velocity);
    }

    bool CanDrag()
    {
        return (!ManaManager.Instance.spiritMenu.activeInHierarchy || ManaManager.Instance.selectedType == Element.None) && !SpiritController.Instance.isHovering && !UpgradeManager.Instance.upgradeMenu.activeInHierarchy;
    }
}
