using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    private Sprite _enemyIdle;

    [SerializeField]
    private Sprite _enemyGrin;

    [SerializeField]
    private Sprite _enemyDamaged;

    [SerializeField]
    private Image _image;

    public void Idle()
    {

        _image.sprite = _enemyIdle;

    }

    public void Grin()
    {

        _image.sprite = _enemyGrin;

    }

    public void Attacked()
    {

        _image.sprite = _enemyDamaged;

    }

}
