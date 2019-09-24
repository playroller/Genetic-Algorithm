using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    private Rigidbody rg;

    // ボールを飛ばす力
    [System.NonSerialized] public float force;

    // ボールを飛ばす角度
    [System.NonSerialized] public float theta;

    // ボールの初期位置
    private Vector3 pos;

    // 評価値
    [System.NonSerialized] public float score = 0f;

    // ボールが地面に落ちたかどうか
    [System.NonSerialized] public bool finish = false;

    public void MyStart()
    {
        rg = GetComponent<Rigidbody>();

        pos = transform.position;
    }

    public void Init(float _force, float _theta)
    {
        rg.velocity = Vector3.zero;
        finish = false;
        score = 0f;
        transform.position = pos;

        force = _force;
        theta = _theta;

        Vector3 direction = new Vector3(Mathf.Cos(theta * Mathf.Deg2Rad), Mathf.Sin(theta * Mathf.Deg2Rad), 0f);

        rg.AddForce(direction * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ground" && finish == false)
        {
            // 評価値はボールのx座標
            score = transform.position.x;
            finish = true;
        }
    }
}
