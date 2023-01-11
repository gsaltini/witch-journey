using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    // Create references to two background sprites and float variable for movement speed
    [SerializeField] Transform _first, _second, _third, _fourth;
    [SerializeField] private float _scrollSpeed, _zOffset;
    [SerializeField] private float _maximumDistance = 65.00f;

    private float _horizontalDifference = 35.00f; 

    private Vector3 newPosition;

    // Start is called before the first frame update
    void Start()
    {
        _zOffset += _first.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Wrap();
    }

    void Movement()
    {
        _first.position = new Vector3(_first.position.x + Time.deltaTime * _scrollSpeed, _first.position.y, _zOffset);
        _second.position = new Vector3(_second.position.x + Time.deltaTime * _scrollSpeed, _second.position.y, _zOffset);
        _third.position = new Vector3(_third.position.x + Time.deltaTime * _scrollSpeed, _third.position.y, _zOffset);
        _fourth.position = new Vector3(_fourth.position.x + Time.deltaTime * _scrollSpeed, _fourth.position.y, _zOffset);
    }

    void Wrap()
    {
        if (_first.localPosition.x > _maximumDistance)
        {
            newPosition = new Vector3(_second.position.x - _horizontalDifference, _second.position.y, _zOffset);
            _first.position = newPosition;
        }

        if (_second.localPosition.x > _maximumDistance)
        {
            newPosition = new Vector3(_third.position.x - _horizontalDifference, _third.position.y, _zOffset);
            _second.position = newPosition;
        }

        if (_third.localPosition.x > _maximumDistance)
        {
            newPosition = new Vector3(_fourth.position.x - _horizontalDifference, _fourth.position.y, _zOffset);
            _third.position = newPosition;
        }

        if (_fourth.localPosition.x > _maximumDistance)
        {
            newPosition = new Vector3(_first.position.x - _horizontalDifference, _first.position.y, _zOffset);
            _fourth.position = newPosition;
        }
    }


}
