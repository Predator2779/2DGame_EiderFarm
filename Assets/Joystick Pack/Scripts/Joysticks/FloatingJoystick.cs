using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : MyJoystick
{
    [SerializeField] private CanvasGroup _canvasGroup;
    
    protected override void Start()
    {
        _canvasGroup = transform.parent.GetComponent<CanvasGroup>();
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
        background.gameObject.SetActive(false);
        base.OnEndDrag(eventData);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnBeginDrag(eventData);
    }
}