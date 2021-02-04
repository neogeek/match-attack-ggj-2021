using System;
using System.Collections;
using CandyCoded;
using UnityEngine;
using UnityEngine.Events;

public class TileController : MonoBehaviour
{

    private const float flipSpeed = 0.5f;

    [SerializeField]
    private SpriteRenderer _backBackgroundSpriteRenderer;

    [SerializeField]
    private SpriteRenderer _backItemSpriteRenderer;

    [SerializeField]
    private SpriteRenderer _frontBackgroundSpriteRenderer;

    [SerializeField]
    private SpriteRenderer _frontDisabledOverlayRenderer;

    [SerializeField]
    private SpriteRenderer _frontAttackBackgroundSpriteRenderer;

    [SerializeField]
    private SpriteRenderer _frontItemSpriteRenderer;

    [SerializeField]
    private AudioSource _flipSFX;

    public Tile tile;

    public bool interactable;

    public TileSelectedEvent TileSelected;

    private readonly Quaternion flipRotation = Quaternion.Euler(0, 180, 0);

    private Camera _mainCamera;

    private void Awake()
    {

        _mainCamera = Camera.main;

    }

    private void Start()
    {

        _frontItemSpriteRenderer.sprite = tile.sprite;

        ToggleAttackHighlight(false);

    }

    private void Update()
    {

        if (interactable && gameObject.transform.rotation.eulerAngles.NearlyEqual(Vector3.zero) &&
            gameObject.GetMouseButtonDown(_mainCamera, out RaycastHit _))

        {

            FlipShow();

        }

    }

    public void ToggleAttackHighlight(bool flag)
    {

        _frontAttackBackgroundSpriteRenderer.enabled = flag;
        _frontBackgroundSpriteRenderer.enabled = !flag;

    }

    public void Show()
    {

        gameObject.transform.rotation = flipRotation;

    }

    public void SetSprites(TileSetReference tileSetReference)
    {

        _backBackgroundSpriteRenderer.sprite = tileSetReference.backBackground;
        _backItemSpriteRenderer.sprite = tileSetReference.backItem;
        _frontAttackBackgroundSpriteRenderer.sprite = tileSetReference.frontAttackBackground;
        _frontBackgroundSpriteRenderer.sprite = tileSetReference.frontBackground;
        _frontDisabledOverlayRenderer.sprite = tileSetReference.frontBackground;

    }

    public void FlipShow()
    {

        _flipSFX.PlayOneShot(_flipSFX.clip);

        StartCoroutine(FlipShowCoroutine());

    }

    public IEnumerator FlipShowCoroutine()
    {

        yield return Animate.RotateTo(gameObject, flipRotation, flipSpeed);

        TileSelected?.Invoke(this);

    }

    public void Hide()
    {

        gameObject.transform.rotation = Quaternion.identity;

    }

    public void FlipHide()
    {

        _flipSFX.PlayOneShot(_flipSFX.clip);

        StartCoroutine(FlipHideCoroutine());

    }

    public void MarkAsUsed()
    {

        _frontDisabledOverlayRenderer.enabled = true;

    }

    public IEnumerator FlipHideCoroutine()
    {

        yield return Animate.RotateTo(gameObject, Quaternion.identity, flipSpeed);

    }

    [Serializable]
    public class TileSelectedEvent : UnityEvent<TileController>
    {

    }

}
