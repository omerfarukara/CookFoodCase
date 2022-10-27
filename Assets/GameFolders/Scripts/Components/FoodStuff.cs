using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class FoodStuff : MonoBehaviour
{
    [SerializeField] private FoodStuffType foodStuffType;
    
    private EventData _eventData;
    private Camera _camera;
    private Rigidbody _rigidbody;

    private Vector3 offset;
    private float _zOffset;
    private float yBorder;
    private bool _isDoubleTapped;

    public bool IsDoubleTapped => _isDoubleTapped;

    public FoodStuffType FoodStuffType => foodStuffType;
    
    private void Awake()
    {
        _eventData = Resources.Load("EventData") as EventData;
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _eventData.OnSetBorderPosition += SetYBorder;
    }
    
   private void OnMouseDown()
    {
        SetOffset();
        if (Pan.Instance.ClickPlayability)
        {
            _eventData.OnSelectFoodStuff?.Invoke(this);
        }
    }
    
    private void OnMouseDrag()
    {
        Move();
    }

    private void OnDisable()
    {
        _eventData.OnSetBorderPosition -= SetYBorder;
    }

    private void OnValidate()
    {
        if (transform.childCount > 0)
        {
            gameObject.name = $"Food : {transform.GetChild(0).name}";
        }
    }

    private void Move()
    {
        Vector3 screenPosition = new Vector3(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width), Mathf.Clamp(Input.mousePosition.y, yBorder, Screen.height), _zOffset);

        Vector3 newWorldPosition = _camera.ScreenToWorldPoint(screenPosition);
        transform.position = newWorldPosition + offset;
    }

    private void SetOffset()
    {
        _zOffset = _camera.WorldToScreenPoint(transform.position).z;
        Vector3 screenPosition = new Vector3(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width), Mathf.Clamp(Input.mousePosition.y, yBorder, Screen.height), _zOffset);
        offset = transform.position - _camera.ScreenToWorldPoint(screenPosition); // mouse position ile transform.position farkını aldıkki tıkladığımız yer offsetimiz olsun
    }

    private void SetYBorder(Vector3 borderPosition)
    {
        yBorder = _camera.WorldToScreenPoint(borderPosition).y;
    }

    public void FallDown(Vector3 velocity)
    {
        _rigidbody.velocity = velocity;
    }

    public void FreezeConstraints(RigidbodyConstraints constraints)
    {
        _rigidbody.constraints = constraints;
    }
    
    public void ScaleUp()
    {
        transform.DOScale(Vector3.one * 1.15f, 0.5f).SetEase(Ease.OutElastic); 
       _isDoubleTapped = true;
    }

    public void ScaleDown()
    {
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic);
        _isDoubleTapped = false;
    }

    public void MoveBackFoodArea(Transform backTransform)
    {
        transform.DOMove(backTransform.position, 1f).OnComplete(()=> Pan.Instance.ClickPlayability = true);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
    
}