using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] _cutScenes;
    [SerializeField] private string _nextLoadedScene;

    private GameObject[] _slides;
    private bool _isShowed;
    private int _currentCutscene;
    private int _currentSlide;
    private bool _final;

    private void Start() => InitializeCutscene();

    private void Update()
    {
        if (!_isShowed) return;

        if (Input.anyKeyDown) NextScene();
    }

    private void InitializeCutscene()
    {
        _isShowed = true;
        _slides = GetAllChilds(_cutScenes[_currentCutscene]);
        _currentSlide = 1;
        _cutScenes[_currentCutscene].SetActive(true);
        _slides[_currentSlide].SetActive(true);
    }

    private void NextScene()
    {
        _slides[_currentSlide].SetActive(false);

        if (_slides.Length > _currentSlide + 1)
        {
            _currentSlide++;
            _slides[_currentSlide].SetActive(true);
            return;
        }

        FinilizeCutscene();
    }

    private GameObject[] GetAllChilds(GameObject g)
    {
        List<GameObject> gObjs = new();

        int count = g.transform.childCount;

        for (int i = 0; i < count; i++)
            gObjs.Add(g.transform.GetChild(i).gameObject);

        return gObjs.ToArray();
    }

    private void FinilizeCutscene()
    {
        _cutScenes[_currentCutscene].SetActive(false);
        SceneManager.LoadScene(_nextLoadedScene);
    }
}