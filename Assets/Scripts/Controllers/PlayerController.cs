using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;
    private Vector3 _destinationPos;
    private bool isMove;
    private Animator _anim;
    private float waitRunRaitio;
    private void Start()
    {
        Managers.InputManager.KeyAction -= OnKeyBoardInput;
        Managers.InputManager.KeyAction += OnKeyBoardInput;
        Managers.InputManager.MouseAction -= OnMouseClick;
        Managers.InputManager.MouseAction += OnMouseClick;

        //Managers.UIManager.ShowPopupUI<UIButton>("UI_Button");


        _anim = GetComponent<Animator>();
    }

    private void Update()
    {   
        if (isMove)
        {
            Vector3 direction = _destinationPos - transform.position;
            if (direction.magnitude < 0.0001f)
            {
                isMove = false;
            }
            else
            {
                float fixedSpeed = Mathf.Clamp(_speed * Time.deltaTime, 0, direction.magnitude);

                transform.position += direction.normalized * fixedSpeed;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.2f);
            }
        }
        
        if (isMove)
        {
            _anim.SetFloat("speed", _speed);
        }
        else
        {
            _anim.SetFloat("speed", 0);
        }
    }

    private void OnKeyBoardInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += Vector3.forward * Time.deltaTime * _speed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += Vector3.back * Time.deltaTime * _speed;
        }

        if ( Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += Vector3.right * Time.deltaTime * _speed;
        }

        if ( Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += Vector3.left * Time.deltaTime * _speed;
        }

        isMove = false;
    }

    private void OnMouseClick(Define.MouseEvent mouseEvent)
    {
        if (mouseEvent != Define.MouseEvent.Click) return;

        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Vector3 dir = (mousePos - cameraPos).normalized;

        Debug.DrawRay(cameraPos, dir * 100.0f, Color.red, 1.0f); ;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 100.0f, LayerMask.GetMask("Wall")))
        {
            _destinationPos = hit.point;
            isMove = true;
        }
    }
}
