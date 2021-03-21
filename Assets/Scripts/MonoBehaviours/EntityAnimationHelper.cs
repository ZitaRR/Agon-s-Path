using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimationHelper : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void RunAnimation()
    {
        entity.Attack();
    }
}
