using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneChangeTrigger : MonoBehaviour
{
    [SerializeField]
    private string sceneName;
    [SerializeField]
    private Vector2 playerSpawnPosition;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Entity entity = collider.GetComponent<Entity>();
        if (!(entity is PlayerEntity))
            return;

        GameManager.Instance.LoadScene(sceneName, playerSpawnPosition);
    }
}
