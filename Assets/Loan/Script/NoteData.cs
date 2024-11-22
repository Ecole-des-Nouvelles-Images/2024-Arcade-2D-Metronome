using UnityEngine;

public class NoteData
{
    public GameObject NoteObject;
    public RectTransform NoteRect;
    public float SpeedNote;
    public Vector3 StartPosition;
    public Vector3 EndPosition;

    private float _progress;

    public NoteData(GameObject note, float movementSpeed, Vector3 start, Vector3 end)
    {
        
        NoteObject = note;
        NoteRect = note.GetComponent<RectTransform>();
        SpeedNote = movementSpeed;
        StartPosition = start;
        EndPosition = end;
        _progress = 0f;
    }

    public float GetLerpProgress(float deltaTime)
    {
        _progress += SpeedNote * deltaTime / Vector2.Distance(StartPosition, EndPosition);
        return Mathf.Clamp01(_progress);
    }
}
