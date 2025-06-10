using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrappleHandPosition : MonoBehaviour
{
    public Transform handStartFacing;
    public Transform handStartNormal;
    public Sprite left;
    public Sprite right;
    public Sprite normal;
    public SpriteRenderer body;





    public Transform getCurrPosition()
    {
        if (body.sprite == left || body.sprite == right)
        {
            return handStartFacing;
        }
        else
        {
            return handStartNormal;
        }
    }
    public int getDrawingLayer()
    {
        if (body.sprite == left)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
}