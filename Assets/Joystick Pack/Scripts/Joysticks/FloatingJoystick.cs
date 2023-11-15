using UnityEngine.EventSystems;

public class FloatingJoystick : MyJoystick
{
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnEndDrag(eventData);
    }
}