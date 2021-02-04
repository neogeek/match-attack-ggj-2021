using System.Collections;
using UnityEngine;
using CandyCoded;

public class InstructionsController : MonoBehaviour
{

    [SerializeField]
    private GameObject _dialog;

    [SerializeField]
    private Vector3AnimationCurve _scaleAnimationCurve;

    public IEnumerator ShowDialog()
    {

        _dialog.SetActive(true);

        yield return Animate.Scale(_dialog, _scaleAnimationCurve);

        yield return new WaitForSeconds(1);

        _dialog.SetActive(false);

    }

}
