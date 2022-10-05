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
    public SpriteRenderer SR;


    public Canvas personalCanvas;
    public TMP_Text nicknameText;
    Camera cam;

    public Animator anim;
    readonly int h_AnimParam = Animator.StringToHash("h");
    readonly int v_AnimParam = Animator.StringToHash("v");
    readonly int IsMoving_AnimParam = Animator.StringToHash("isMoving");
    public MoveDir Dir
    {
        get { return _dir; }
        set { _dir = value; }
    }
    private void Awake()
    {
        
    }
    void Start()
    {
        cam = Camera.main;
        PV = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        personalCanvas.worldCamera = Camera.main;
        _grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        Vector3 pos = _grid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f);
        transform.position = pos;

        nicknameText.text = (PV.IsMine) ? PhotonNetwork.NickName : PV.Owner.NickName;


    }

    IEnumerator tempCo()
    {
        yield return new WaitForSeconds(0.5f);
        
    }
    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        UpdateIsMoving();

        if (PV.IsMine)
        {
            GetDirInput();
            cam.transform.position = new Vector3(transform.position.x,transform.position.y,-10);
        }
    }

    void GetDirInput()
    {
        anim.SetBool(IsMoving_AnimParam, _isMoving);
        if (IngameManager.isChat)
            return;

        if (Input.GetKey(KeyCode.W))
        {
            Dir = MoveDir.Up;
            anim.SetInteger(v_AnimParam, 1);
            PV.RPC("FlipX", RpcTarget.AllBuffered, false);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Dir = MoveDir.Down;
            anim.SetInteger(v_AnimParam, -1);
            PV.RPC("FlipX", RpcTarget.AllBuffered, false);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Dir = MoveDir.Left;
            anim.SetInteger(h_AnimParam, -1);
            PV.RPC("FlipX", RpcTarget.AllBuffered, false);
            
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Dir = MoveDir.Right;
            anim.SetInteger(h_AnimParam, 1);
            PV.RPC("FlipX", RpcTarget.AllBuffered, true);
        }
        else
        {
            Dir = MoveDir.None;
            anim.SetInteger(h_AnimParam, 0);
            anim.SetInteger(v_AnimParam, 0);
        }
    }

    [PunRPC]
    void FlipX(bool b)
    {
        SR.flipX = b;
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
