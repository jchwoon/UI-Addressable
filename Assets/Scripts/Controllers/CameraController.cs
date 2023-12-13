using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode
{
    QuaterView,
}

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CameraMode _mode = CameraMode.QuaterView;
    [SerializeField]
    private GameObject _player = null;
    [SerializeField]
    private Vector3 _dir = new Vector3(0, 4.0f, -6.0f);

    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        RaycastHit hit;
        //스타트 기준 second포지션위치와 연결해서 쭉 이어준게 레이케스트
        Debug.DrawRay(_dir, _player.transform.position, Color.blue);
        if (Physics.Raycast(_player.transform.position + Vector3.up, _dir, out hit, 100.0f, LayerMask.GetMask("Wall")))
        {
            Vector3 playerToHitDir = hit.point - _player.transform.position + Vector3.up;
            transform.position = _player.transform.position + playerToHitDir.normalized * (playerToHitDir.magnitude * 0.9f);
        }
        else
        {
            transform.position = _player.transform.position + _dir;
            transform.LookAt(_player.transform);
        }
        
    }
}
