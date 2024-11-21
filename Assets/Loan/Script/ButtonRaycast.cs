using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ButtonRaycast : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private RectTransform _notePanel;
    [SerializeField] private GameObject _notePrefabs;

    private RectTransform _buttonRect;

    private void Start()
    {
        _buttonRect = _notePanel.GetComponent<RectTransform>();
    }

    private void Update()
    {
        CheckIfNoteUnderButtom();
    }

    private void CheckIfNoteUnderButtom()
    {
        Vector3[] buttonCorners = new Vector3[4];
        _buttonRect.GetWorldCorners(buttonCorners);

        Rect buttonRect = new Rect(buttonCorners[0].x, buttonCorners[0].y,
            buttonCorners[2].x - buttonCorners[0].x,
            buttonCorners[2].y - buttonCorners[0].y);

        foreach (Transform note in _notePanel)
        {
            RectTransform noteRect = note.GetComponent<RectTransform>();

            if (buttonRect.Overlaps(noteRect.rect))
            {
                Debug.Log("Note Under Button");
            }
        }
    }
}
