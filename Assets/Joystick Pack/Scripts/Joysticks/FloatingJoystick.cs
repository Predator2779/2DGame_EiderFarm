using UnityEngine.EventSystems;

public class FloatingJoystick : MyJoystick
{
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnEndDrag(eventData);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnBeginDrag(eventData);
    }
}