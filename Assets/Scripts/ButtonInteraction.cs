using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonInteraction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField]
    private RectTransform _textTransform;

    private Vector3 _startPosition;

    private void Awake()
    {

        _startPosition = _textTransform.localPosition;

    }

    public void OnPointerDown(PointerEventData eventData)
    {

        _textTransform.localPosition = _startPosition + new Vector3(0, -10, 0);

    }

    public void OnPointerUp(PointerEventData eventData)
    {

        _textTransform.localPosition = _startPosition;

    }

}
