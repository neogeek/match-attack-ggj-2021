using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIDynamicFillBarController : MonoBehaviour
{

    private const float fillSpeed = 5;

    [SerializeField]
    private string _valueTemplate = "{0:0}";

    [SerializeField]
    private Text _valueTextComp;

    [SerializeField]
    private Image _fillImage;

    [SerializeField]
    private Transform _shakeTransform;

    private float _currentValue;

    private float _maxValue;

    private float _nextValue;

    private void Update()
    {

        _currentValue = Mathf.Lerp(_currentValue, _nextValue, Time.deltaTime * fillSpeed);

        _valueTextComp.text = string.Format(_valueTemplate, _currentValue);

        _fillImage.fillAmount = _currentValue / _maxValue;

    }

    public void SetMaxValue(float value)
    {

        _maxValue = value;
        _nextValue = value;
        _currentValue = value;

    }

    public void SetValue(float value)
    {

        _nextValue = value;

    }

    public IEnumerator ShakeCoroutine(float duration = 1, float intensity = 20f)
    {

        if (!_shakeTransform)
        {
            yield break;
        }

        var startPosition = _shakeTransform.localPosition;

        while (duration > 0)
        {

            _shakeTransform.localPosition = startPosition + Random.insideUnitSphere * intensity;

            duration -= Time.deltaTime;

            yield return null;

        }

        _shakeTransform.localPosition = startPosition;

    }

}
