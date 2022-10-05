using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_PlayerController : MonoBehaviour
{

    public Rigidbody2D rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");

        transform.Translate(new Vector2(h, 0));

        if (Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(new Vector2(0, 600));
        }
    }
}
