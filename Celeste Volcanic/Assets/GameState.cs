using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameState : MonoBehaviour
{
    int _score = 0;
    
    [SerializeField] GameObject _scoreText;
    [SerializeField] GameObject _level1;
    [SerializeField] GameObject _level2;
    GameObject _currentLevel;
    int _currentLevelIndex = 0;
    GameObject[] _levels;

    public static GameState Instance;

    void Awake() {
        Instance = this;
        GameEvents.ScoreIncreased += OnScoreIncreased;
        GameEvents.LevelIncreased += OnLevelIncreased;
    }

    void Start() {
        _levels = new GameObject[2];
        _levels[0] = _level1;
        _levels[1] = _level2;

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
        
    }

    public void IncreaseScore(int amount) {
        _score += amount;
        _scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + _score;
    }

    void OnScoreIncreased(object sender, ScoreEventArgs args) {
        IncreaseScore(args.score);
    }

    void OnLevelIncreased(object sender, EventArgs args) {
        foreach (Transform child in _currentLevel.transform) {
            child.gameObject.SetActive(false);
        }
        _currentLevelIndex++;
        _currentLevel = _levels[_currentLevelIndex];
        foreach (Transform child in _currentLevel.transform) {
            child.gameObject.SetActive(true);
        }
    }

}
