using System;
using UnityEngine;

public class ConveyorItemRenderer : MonoBehaviour
{
    SpriteRenderer _itemRenderer;
    Conveyor _currentConveyor;

    private int _conveyorPosition = -1;
    private bool _canMove = false;
    private const float MoveDistance = 5.0f; 

    private void Start()
    {
       _itemRenderer = GetComponent<SpriteRenderer>(); 
    }


}
