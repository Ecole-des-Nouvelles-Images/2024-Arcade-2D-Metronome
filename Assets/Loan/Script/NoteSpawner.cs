using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class NoteSpawner : MonoBehaviour
{
  [SerializeField] private GameObject _notePrefab;
  [SerializeField] private List<float> _spawnIntervals = new List<float>{0.2f,0.4f,0.6f,0.8f,1f,1.2f,1.4f,1.6f,1.8f,2f};
  [SerializeField] private List<float> _speedScroll = new List<float>(){10000f};
  [SerializeField] private RectTransform _startNote;
  [SerializeField] private RectTransform _endNote;
  [SerializeField] private RectTransform _rectTransformPanel;
  [SerializeField] private RectTransform _detectionImage;
  
  private List<NoteData> _notesData = new List<NoteData>();

  private void Start()
  {
    StartCoroutine(SpawnNoteRandom());
  }

  private void Update()
  {
    MoveNotes();
  }

  private IEnumerator SpawnNoteRandom()
  {
    while (true)
    {
      float randomSpawn = _spawnIntervals[Random.Range(0, _spawnIntervals.Count)];
      float randomSpeed = _speedScroll[Random.Range(0, _speedScroll.Count)];
      
      yield return new WaitForSeconds(randomSpawn);

      SpawnNote(randomSpeed);
    }
  }

  private void SpawnNote(float speed)
  {
    GameObject newNote = Instantiate(_notePrefab, _rectTransformPanel);
    
    RectTransform noteRect = newNote.GetComponent<RectTransform>();
    noteRect.position = _startNote.position;
    
    _notesData.Add(new NoteData(newNote, speed, _startNote.position, _endNote.position));
  }

  private void MoveNotes()
  {
    for (int i = _notesData.Count -1 ; i >= 0 ; i--)
    {
      NoteData noteData = _notesData[i];

      if (noteData.NoteObject != null)
      {
        float progress = noteData.GetLerpProgress(Time.deltaTime);
        noteData.NoteRect.position = Vector3.Lerp(noteData.StartPosition, noteData.EndPosition, progress);

        if (progress >= 1.0f)
        {
          Destroy(noteData.NoteObject);
          _notesData.RemoveAt(i);
        }
      }
    }
  }

  private bool IsRectOverlapping(RectTransform rect1, RectTransform rect2)
  {
    Rect rect1World = GetworldRect(rect1);
    Rect rect2World = GetworldRect(rect2);
    
    return rect1World.Overlaps(rect2World);
  }

  private Rect GetworldRect(RectTransform rectTransform)
  {
    Vector3[] corners = new Vector3[4];
    rectTransform.GetWorldCorners(corners);
    
    Vector3 bottomLeft = corners[0];
    Vector3 topRight = corners[2];

    return new Rect(bottomLeft, topRight - bottomLeft);
  }

  public bool CheckNoteUnderImage()
  {
    foreach (NoteData note in _notesData)
    {
      if (note.NoteObject != null && IsRectOverlapping(_detectionImage,note.NoteRect))
      {
        Destroy(note.NoteObject);
        _notesData.Remove(note);
        return true;
      }
    }

    return false;
  }
}
