
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileLookArea : MonoBehaviour,
    IPointerDownHandler, IDragHandler
{
    public playerscript player;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Start tracking the finger.
    }

    public void OnDrag(PointerEventData eventData)
    {
        player.AddTouchLook(eventData.delta);
    }
}
