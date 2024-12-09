using System.Collections.Generic;
using UnityEngine;

public class NoteControler : MonoBehaviour
{
    [SerializeField] GameObject _noteImage;
    [SerializeField] private GameObject _noteFake;
    
    private RectTransform _scroll;
    [SerializeField]private float _speedScroll = 10f;
    private float _nextFakeSpawnTime = 0f;
    private float _nextRealSpawnTime = 0f;
    
    [SerializeField] private List<float> realSpawnTimes = new List<float>{0.2f,0.4f,0.6f,0.8f,1f,1.2f,1.4f,1.6f,1.8f,2f};
    [SerializeField] private List<float> fakeSpawnTimes = new List<float> { 1f };


    private void Start()
    {
        _scroll = GetComponent<RectTransform>();
        AddNote();
        SetNextRealNoteTime();
    }

    private void Update()
    {
        if (_scroll != null)
        {
            Vector2 pivot = _scroll.anchoredPosition;
            
            pivot.x -= _speedScroll * Time.deltaTime;
            
            _scroll.anchoredPosition = pivot;
        }

        if (Time.time >= _nextFakeSpawnTime)
        {
                AddFakeNote();
                SetNextFakeNoteTime();
        }
        
        if (Time.time >= _nextRealSpawnTime)
        {
            AddNote();
            SetNextRealNoteTime();
        }
    }

    private void SetNextRealNoteTime()
    {
        if (realSpawnTimes.Count > 0)
        {
            float randomRealInterval = realSpawnTimes[Random.Range(0, realSpawnTimes.Count)];
            _nextRealSpawnTime = Time.time + randomRealInterval;
        }
    }
    
    private void SetNextFakeNoteTime()
    {
        if (fakeSpawnTimes.Count > 0)
        {
            float randomInterval = fakeSpawnTimes[Random.Range(0, fakeSpawnTimes.Count)];
            _nextFakeSpawnTime = Time.time + randomInterval;
        }
    }
    
    private void AddFakeNote()
    {
        if (_noteFake!= null && _scroll != null )
        {
            GameObject fakeNote = Instantiate(_noteFake, _scroll);
            fakeNote.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    private void AddNote()
    {
        if (_noteImage != null && _scroll != null )
        {
            
            GameObject newNote = Instantiate(_noteImage, _scroll);
            newNote.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}
