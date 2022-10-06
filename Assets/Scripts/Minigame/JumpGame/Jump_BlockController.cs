using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_BlockController : MonoBehaviour
{
    public float _speed;
    void Start()
    {
        Jump_GameManager.Instance.blocks.Add(gameObject);
    }

    void Update()
    {
        transform.Translate(new Vector2(0, -_speed * Time.deltaTime));
        if(transform.position.y <= -10)
        {
            Jump_GameManager.Instance.blocks.Remove(gameObject);
            Destroy(gameObject);
        }

    }
}
