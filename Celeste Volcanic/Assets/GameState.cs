using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameState : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] GameObject _scoreText;
    [SerializeField] GameObject _winText1;
    [SerializeField] GameObject _winText2;
    [SerializeField] GameObject _level1;
    [SerializeField] GameObject _level2;
    [SerializeField] GameObject _level3;
    [SerializeField] GameObject _levelEnd;
    [SerializeField] GameObject _crystalPrefab;
    [SerializeField] GameObject _chestPrefab;
    [SerializeField] GameObject _breakablePrefab;
    GameObject _currentPlayer;
    float _xSpawn = -3.4f;
    float _ySpawn = -3.3f;
    int _score = 0;
    float _xCrystalSpawn = -3.54634f;
    float _yCrystalSpawn = -0.4100001f;
    float _xChestSpawn = 4.399894f;
    float _yChestSpawn = 1.52f;
    GameObject _currentLevel;
    int _currentLevelIndex = 0;
    GameObject[] _levels;
    bool _newLevel = false;
    bool _isGameOver = false;

    public static GameState Instance;

    void Awake() {
        Instance = this;
        GameEvents.ScoreIncreased += OnScoreIncreased;
        GameEvents.LevelIncreased += OnLevelIncreased;
        GameEvents.ResetPlayer += OnResetPlayer;
        GameEvents.InstantiateBreakable += OnInstantiateBreakable;
    }

    void Start() {
        _winText1.SetActive(false);
        _winText2.SetActive(false);
        _levels = new GameObject[4];
        _levels[0] = _level1;
        _levels[1] = _level2;
        _levels[2] = _level3;
        _levels[3] = _levelEnd;

        _currentLevel = _level1;

        for (int i = 0; i != _levels.Length; i++) {
            foreach (Transform child in _levels[i].transform) {
                child.gameObject.SetActive(false);
            }
        }

        foreach (Transform child in _currentLevel.transform) {
            child.gameObject.SetActive(true);
        }
    }

    void Update() {
        if (Input.GetButtonDown("Submit") && _isGameOver) {
            _isGameOver = false;
            _currentLevel = _level1;
            _currentLevelIndex = 0;
            _score = 0;

            for (int i = 0; i != _levels.Length; i++) {
                foreach (Transform child in _levels[i].transform) {
                    child.gameObject.SetActive(false);
                }
            }

            foreach (Transform child in _currentLevel.transform) {
                child.gameObject.SetActive(true);
            }
            _scoreText.SetActive(true);
            _winText1.SetActive(false);
            _winText2.SetActive(false);
            IncreaseScore(0);
            Destroy(_currentPlayer);
            GameEvents.InvokeResetPlayer();
            GameObject chest = Instantiate(_chestPrefab, new Vector3(_xChestSpawn, _yChestSpawn, 0f), Quaternion.identity);
            chest.transform.parent = _levelEnd.transform;
            chest.SetActive(false);
        }
    }

    public void IncreaseScore(int amount) {
        _score += amount;
        _scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + _score;
    }

    void OnScoreIncreased(object sender, ScoreEventArgs args) {
        IncreaseScore(args.score);
    }

    void OnLevelIncreased(object sender, EventArgs args) {
        if (_currentLevelIndex < _levels.Length - 1) {
            foreach (Transform child in _currentLevel.transform) {
                child.gameObject.SetActive(false);
            }
            _currentLevelIndex++;
            _currentLevel = _levels[_currentLevelIndex];
            foreach (Transform child in _currentLevel.transform) {
                child.gameObject.SetActive(true);
            }
        }
        else {
            _scoreText.SetActive(false);
            _winText1.SetActive(true);
            _winText2.SetActive(true);
            _winText2.GetComponent<TextMeshProUGUI>().text = "Your score was " + _score + "\nPress Enter to Restart";
            _isGameOver = true;
        }
        _newLevel = true;
    }

    void OnResetPlayer(object sender, EventArgs args) {
        if (_currentLevel == _level2) {
            GameObject crystal = Instantiate(_crystalPrefab, new Vector3(_xCrystalSpawn, _yCrystalSpawn, 0f), Quaternion.identity);
            crystal.transform.parent = _level2.transform;
        }
        _currentPlayer = Instantiate(_playerPrefab, new Vector3(_xSpawn, _ySpawn, -1.6f), Quaternion.identity);
    }


    IEnumerator BreakableInstantiation(object sender, BreakableEventArgs args) {
        yield return new WaitForSeconds(1.5f);
        if (! _newLevel) {
            GameObject breakable = Instantiate(_breakablePrefab, new Vector3(args.x, args.y, args.z), Quaternion.identity);
            breakable.transform.parent = _currentLevel.transform;
        }
        else {
            GameObject breakable = Instantiate(_breakablePrefab, new Vector3(args.x, args.y, args.z), Quaternion.identity);
            breakable.transform.parent = _level3.transform;
            breakable.SetActive(false);
        }
    }

    void OnInstantiateBreakable(object sender, BreakableEventArgs args) {
        _newLevel = false;
        StartCoroutine(BreakableInstantiation(sender, args)); 
    }

}
