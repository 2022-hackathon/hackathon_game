using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jump_PlayerController : MonoBehaviour
{

    public Rigidbody2D rigid;
    public float _speed = 5;


    public bool _isGround;
    public int jumpCnt;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");

        transform.Translate(new Vector2(h * Time.deltaTime * _speed, 0));

        if (Input.GetButtonDown("Jump") && jumpCnt < 1)
        {
            rigid.AddForce(new Vector2(0, 350));
            jumpCnt++;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {

        }


        if(transform.position.y <= -10)
        {
            Jump_GameManager.Instance.jumpGameover();
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position + (Vector3)(Vector2.down * 0.7f), Vector2.down, .02f);

        if (hitinfo)
            jumpCnt = 0;
        
    }


}
