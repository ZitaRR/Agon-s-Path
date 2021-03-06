﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CameraBehaviour : MonoBehaviour
{
    public static Camera Camera { get; private set; }
    public static Vector2 MouseWorldPosition
        => Camera.ScreenToWorldPoint(Input.mousePosition);

    private Transform player;
    private Action behaviour;
    private Vector3 target;

    [SerializeField]
    private float transitionDistance = 1;
    [SerializeField]
    private float transitionSmoothening = .1f;

    private void Awake()
    {
        Camera = Camera.main;
    }

    public void Roaming()
    {
        player = GameManager.Player.transform;
        target = new Vector3(player.position.x, player.position.y, Camera.transform.position.z);
        Camera.transform.position = Vector3.Lerp(Camera.transform.position, target, transitionSmoothening * Time.deltaTime);
    }

    public void Combat()
    {
        var entites = CombatSystem.GetPositions();
        entites.Add(player.position);
        var centroid = entites.GetCentroid();
        target = new Vector3(centroid.x, centroid.y, Camera.transform.position.z);
        Camera.transform.position = Vector3.Lerp(Camera.transform.position, target, transitionSmoothening * Time.deltaTime);
    }

    public void Mouse()
    {
        player = GameManager.Player.transform;
        Vector3 midPoint = (player.position + Camera.ScreenToWorldPoint(Input.mousePosition)) / 2;
        Vector3 offset = Vector3.ClampMagnitude(midPoint - player.position, transitionDistance);
        target = new Vector3(player.position.x + offset.x, player.position.y + offset.y, Camera.transform.position.z);
        Camera.transform.position = Vector3.Lerp(Camera.transform.position, target, transitionSmoothening * Time.deltaTime);
    }
}
