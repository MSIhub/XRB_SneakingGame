using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class KeyCardHolder : MonoBehaviour
{
    public string uniqueIdentifierKeyCard = "xrb1";
    [ShowInInspector, ReadOnly] public Vector3 initPositionKeyCard;
    [ShowInInspector, ReadOnly] public Quaternion initRotationKeyCard;

    private void Awake()
    {
        initPositionKeyCard = transform.position;
        initRotationKeyCard = transform.rotation;
    }
}
