using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public Grid _grid;
    Vector3Int _cellPos = Vector3Int.zero;
    MoveDir _dir;


    public float _speed;
    public bool _isMoving;

    public PhotonView PV;

    public Canvas personalCanvas;
    public TMP_Text nicknameText;
    public MoveDir Dir
    {
        get { return _dir; }
        set { _dir = value; }
    }
    private void Awake()
    {
        nicknameText.text = GameManager.userData.nickName;
    }
    void Start()
    {
        PV = GetComponent<PhotonView>();
        personalCanvas.worldCamera = Camera.main;
        StartCoroutine(tempCo());
    }

    IEnumerator tempCo()
    {
        yield return new WaitForSeconds(0.5f);
        _grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        Vector3 pos = _grid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f);
        transform.position = pos;
    }
    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        UpdateIsMoving();
    
        if(PV.IsMine)
         GetDirInput();

    }

    void GetDirInput()
    {
        if (IngameManager.isChat)
            return;

        if (Input.GetKey(KeyCode.W))
        {
            Dir = MoveDir.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Dir = MoveDir.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Dir = MoveDir.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Dir = MoveDir.Right;
        }
            else
            {
                Dir = MoveDir.None;
            }
    }

    void UpdatePosition()
    {
        if (_isMoving == false)
            return;

        if (_grid == null)
            return;
        Vector3 destPos = _grid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f);
        Vector3 moveDir = destPos - transform.position;

        // 도착 여부 체크
        float dist = moveDir.magnitude;
        if (dist < _speed * Time.deltaTime)
        {
            transform.position = destPos;
            _isMoving = false;
        }
        else
        {
            transform.position += moveDir.normalized * _speed * Time.deltaTime;
            _isMoving = true;
        }
    }

    void UpdateIsMoving()
    {
        if (_isMoving == false)
        {
            switch (_dir)
            {
                case MoveDir.Up:
                    if (CollisionCheck(Vector2.up))
                    {
                        _cellPos += Vector3Int.up;
                        _isMoving = true;
                    }
                    break;
                case MoveDir.Down:
                    if (CollisionCheck(Vector2.down))
                    {
                        _cellPos += Vector3Int.down;
                        _isMoving = true;
                    }
                    break;
                case MoveDir.Left:
                    if (CollisionCheck(Vector2.left))
                    {
                        _cellPos += Vector3Int.left;
                        _isMoving = true;
                    }
                    break;
                case MoveDir.Right:
                    if (CollisionCheck(Vector2.right))
                    {
                        _cellPos += Vector3Int.right;
                        _isMoving = true;
                    }
                    break;
            }
        }
    }

    bool CollisionCheck(Vector2 dir)
    {
        RaycastHit2D hitinfo =
        Physics2D.Raycast(transform.position, dir, 1);


        return !hitinfo;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //stream.SendNext(Dir);
            //stream.SendNext(_isMoving);
        }
        else
        {
            //Dir = (MoveDir)stream.ReceiveNext();
            //_isMoving = (bool)stream.ReceiveNext();
        }
    }
}
