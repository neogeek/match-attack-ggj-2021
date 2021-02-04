using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CandyCoded;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private TileSetReference _tileSetReference;

    [SerializeField]
    private GameObject _tilePrefab;

    [SerializeField]
    private Vector3 _gridSize = new Vector2(5, 5);

    [SerializeField]
    private Vector3 _gridSpacing = new Vector2(0.15f, 0.15f);

    [SerializeField]
    private UIDynamicFillBarController _uiTimerBar;

    [SerializeField]
    private AudioSource _timeTickSFX;

    [SerializeField]
    private UIDynamicFillBarController _uiHealthBar;

    [SerializeField]
    private EnemyController _enemyController;

    [SerializeField]
    private GameObject _gameOverDialog;

    [SerializeField]
    private GameObject _gameWonDialog;

    [SerializeField]
    private GameObject _nextLevelDialog;

    [SerializeField]
    private InstructionsController _matchInstructionsDialog;

    [SerializeField]
    private InstructionsController _attackInstructionsDialog;

    private readonly List<Tuple<TileController, TileController>> _inventory =
        new List<Tuple<TileController, TileController>>();

    private readonly List<TileController> _tileControllers = new List<TileController>();

    private Transform _spawnTransform;

    private float _levelEnemyHealth;

    private TileController _previousTileControllerSelected;

    private int _secondsRemaining;

    private void Awake()
    {

        _spawnTransform = gameObject.transform;

    }

    private IEnumerator Start()
    {

        var totalLevelEnemyHealth = _tileSetReference.tiles.Sum(tile => tile.damage);

        _levelEnemyHealth = totalLevelEnemyHealth;

        _secondsRemaining = _tileSetReference.timer;

        _uiTimerBar.gameObject.SetActive(true);
        _uiHealthBar.gameObject.SetActive(false);

        _uiTimerBar.SetMaxValue(_secondsRemaining);
        _uiHealthBar.SetMaxValue(_levelEnemyHealth);

        yield return PlaceTilesCoroutine();

        ShowAllTiles();

        yield return new WaitForSeconds(2);

        yield return FlipHideAllTilesCoroutine();

        _enemyController.Grin();

        yield return _matchInstructionsDialog.ShowDialog();

        ToggleInteractionOnAllTiles(true);

        yield return new WaitForSeconds(1);

        var delayInSecondsRealtime = new WaitForSecondsRealtime(1);

        while (_secondsRemaining > 0 && _inventory.Count < _tileControllers.Count / 2)
        {

            _secondsRemaining--;

            _uiTimerBar.SetValue(_secondsRemaining);

            _timeTickSFX.PlayOneShot(_timeTickSFX.clip);

            yield return delayInSecondsRealtime;

        }

        ToggleInteractionOnAllTiles(false);

        _uiTimerBar.gameObject.SetActive(false);
        _uiHealthBar.gameObject.SetActive(true);

        yield return _attackInstructionsDialog.ShowDialog();

        yield return AttackSequenceCoroutine();

        if (_levelEnemyHealth > totalLevelEnemyHealth * _tileSetReference.enemyMinDamagePercentage)
        {

            _gameOverDialog.SetActive(true);

        }
        else if (!_tileSetReference.isLastLevel)
        {

            _nextLevelDialog.SetActive(true);

        }
        else
        {

            _gameWonDialog.SetActive(true);

        }

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        var bounds = _tilePrefab.GetComponentInChildren<Renderer>().bounds;

        var size = new Vector3((bounds.size.x + _gridSpacing.x) * _gridSize.x,
            (bounds.size.y + _gridSpacing.y) * _gridSize.y) - _gridSpacing;

        Gizmos.DrawWireCube(gameObject.transform.position, size);

    }
#endif

    private IEnumerator PlaceTilesCoroutine()
    {

        var delayBetweenPlacingTiles = new WaitForSeconds(0.05f);

        var tiles = _tileSetReference.tiles.Concat(_tileSetReference.tiles).Shuffle();

        var bounds = _tilePrefab.GetComponentInChildren<Renderer>().bounds;

        var offset = new Vector3((bounds.size.x + _gridSpacing.x) * _gridSize.x / 2,
            (bounds.size.y + _gridSpacing.y) * _gridSize.y / 2) - bounds.extents - _gridSpacing / 2;

        for (var row = _gridSize.y - 1; row >= 0; row -= 1)
        {

            for (var col = 0; col < _gridSize.x; col += 1)
            {

                if (tiles.Count <= 0)
                {
                    continue;
                }

                var position =
                    new Vector3((bounds.size.x + _gridSpacing.x) * col, (bounds.size.y + _gridSpacing.y) * row) -
                    offset + gameObject.transform.position;

                var spawnedTile = Instantiate(_tilePrefab, position, Quaternion.identity, _spawnTransform);
                var tileController = spawnedTile.GetComponent<TileController>();

                tileController.TileSelected.AddListener(HandleTileSelected);

                tileController.SetSprites(_tileSetReference);

                tileController.Show();

                tileController.tile = tiles.Pop();

                _tileControllers.Add(tileController);

                yield return delayBetweenPlacingTiles;

            }

        }

    }

    private void HandleTileSelected(TileController tileController)
    {

        if (_previousTileControllerSelected == null)
        {

            _previousTileControllerSelected = tileController;

        }
        else if (!_previousTileControllerSelected.tile.Equals(tileController.tile))
        {

            _previousTileControllerSelected.FlipHide();
            tileController.FlipHide();

            _previousTileControllerSelected = null;

        }
        else
        {

            _inventory.Add(
                new Tuple<TileController, TileController>(_previousTileControllerSelected, tileController));

            _previousTileControllerSelected = null;

        }

    }

    private void ShowAllTiles()
    {

        foreach (var tileController in _tileControllers)
        {
            tileController.Show();
        }

    }

    private void ToggleInteractionOnAllTiles(bool flag)
    {

        foreach (var tileController in _tileControllers)
        {
            tileController.interactable = flag;
        }

    }

    private IEnumerator FlipHideAllTilesCoroutine()
    {

        var delayBetweenFlippingTiles = new WaitForSeconds(0.1f);

        foreach (var tileController in _tileControllers)
        {
            tileController.FlipHide();

            yield return delayBetweenFlippingTiles;

        }

    }

    private IEnumerator AttackSequenceCoroutine()
    {

        var delayBetweenAttacks = new WaitForSeconds(1);

        var weapons = _inventory.Where(tuple => tuple.Item1.tile.type.Equals(TileType.Weapon));
        var ammo = _inventory.Where(tuple => tuple.Item1.tile.type.Equals(TileType.Ammo)).ToList();
        var specialAmmo = _inventory.Where(tuple => tuple.Item1.tile.type.Equals(TileType.SpecialAmmo)).ToList();

        foreach (var weapon in weapons)
        {

            weapon.Item1.ToggleAttackHighlight(true);
            weapon.Item2.ToggleAttackHighlight(true);

            var damage = 0;

            if (weapon.Item1.tile.requiresSpecialAmmo && specialAmmo.Count > 0)
            {

                damage = specialAmmo.Pop().Item1.tile.damage;

            }
            else if (weapon.Item1.tile.requiresAmmo && ammo.Count > 0)
            {

                damage = ammo.Pop().Item1.tile.damage;

            }
            else if (weapon.Item1.tile.damage > 0)
            {

                damage = weapon.Item1.tile.damage;

            }

            if (damage > 0)
            {

                _levelEnemyHealth -= damage;

                _uiHealthBar.SetValue(_levelEnemyHealth);

                _enemyController.Attacked();

                yield return _uiHealthBar.ShakeCoroutine();

                _enemyController.Grin();

                Debug.Log(
                    $"{weapon.Item1.tile.name} attacked for {damage} damage! Current enemy health: {_levelEnemyHealth}");

            }

            weapon.Item1.ToggleAttackHighlight(false);
            weapon.Item2.ToggleAttackHighlight(false);

            weapon.Item1.MarkAsUsed();
            weapon.Item2.MarkAsUsed();

            yield return delayBetweenAttacks;

        }

    }

}
