using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public static Camera Camera { get; private set; }

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

    private void Start()
    {
        player = GameManager.Player.transform;
        GameManager.OnStateChange += OnStateChanged;
        GameManager.SetState(GameManager.GameState.Idle);
    }

    private void Update()
    {
        behaviour();
    }

    private void Roaming()
    {
        target = new Vector3(player.position.x, player.position.y, Camera.transform.position.z);
        Camera.transform.position = Vector3.Lerp(Camera.transform.position, target, transitionSmoothening * Time.deltaTime);
    }

    private void Mouse()
    {
        Vector3 midPoint = (player.position + Camera.ScreenToWorldPoint(Input.mousePosition)) / 2;
        Vector3 offset = Vector3.ClampMagnitude(midPoint - player.position, transitionDistance);
        target = new Vector3(player.position.x + offset.x, player.position.y + offset.y, Camera.transform.position.z);
        Camera.transform.position = Vector3.Lerp(Camera.transform.position, target, transitionSmoothening * Time.deltaTime);
    }

    private void OnStateChanged(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Idle:
                behaviour = Roaming;
                break;
            default:
                behaviour = Roaming;
                break;
        }
    }
}
