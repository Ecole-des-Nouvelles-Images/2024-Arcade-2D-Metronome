using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultiplePlayerCamera : MonoBehaviour
{
   public List<Transform> players;

   [SerializeField]
   private Vector3 _offset = new Vector3(0f, 0f, -10f);
   [SerializeField]
   private float _smoothTime = 0.5f;
   [SerializeField]
   private float _minZoom = 5f;
   [SerializeField]
   private float _maxZoom = 20f;
   [SerializeField]
   private float _zoomLimit = 50f;
   
   
   private Vector3 _velocity;
   private Camera _camera;
   private float _speedCamera = 7f;

   private void Start()
   {
      _camera = GetComponent<Camera>();
      
   }

   private void LateUpdate()
   {
      if (players.Count == 0)
         return;
      
      Move();
      Zoom();

   }

   private void Zoom()
   {
      float greatestDistance = GetGreatestDistance();
      float newZoom = Mathf.Lerp(_maxZoom, _minZoom, greatestDistance / _zoomLimit);

      _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, newZoom, Time.deltaTime * _speedCamera);
   }

   void Move()
   {
      Vector3 centerPoint = GetCenterPoint();
      
      Vector3 newPosition = centerPoint + _offset;
      
      transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, _smoothTime * Time.deltaTime);
   }

   float GetGreatestDistance()
   {
      var bounds = new Bounds(players[0].position, Vector3.zero);
      for (int i = 0; i < players.Count; i++)
      {
         bounds.Encapsulate(players[i].position);
      }

      return Mathf.Max(bounds.size.x, bounds.size.y);
   }

   private Vector3 GetCenterPoint()
   {
      if (players.Count == 1)
      {
         return players[0].position;
      }
      
      var bounds = new Bounds(players[0].position, Vector3.zero);
      for (int i = 0; i < players.Count; i++)
      {
         bounds.Encapsulate(players[i].position);
      }
      return bounds.center;
   }

   public void AddPlayer(Transform player)
   {
      if (!players.Contains(player))
      {
         players.Add(player);
      }
   }

   public void RemovePlayer(Transform player)
   {
      if (players.Contains(player))
      {
         players.Remove(player);
      }
   }
}
