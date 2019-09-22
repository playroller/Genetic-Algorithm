using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    private Rigidbody rg;
    [SerializeField] private GameObject startline = null;

    // ボールを飛ばす力
    public float force;

    // ボールを飛ばす角度
    public float theta;

    // ボールの初期位置
    private Vector3 pos;

    // 評価値
    public float score = 0f;

    // ボールが地面に落ちたかどうか
    public bool finish = false;

    private void Start()
    {
        rg = GetComponent<Rigidbody>();

        pos = transform.position;

        // ボールを飛ばす力,角度共にランダムに初期化する
        force = Random.Range(0.0f, 20.0f);
        theta = Random.Range(0.0f, 90.0f);

        Vector3 direction = new Vector3(Mathf.Cos(theta * Mathf.Deg2Rad), Mathf.Sin(theta * Mathf.Deg2Rad), 0f);

        rg.AddForce(direction * force, ForceMode.Impulse);
        startline.SetActive(false);
    }

    public void Init(float _force, float _theta)
    {
        startline.SetActive(true);
        rg.velocity = Vector3.zero;
        finish = false;
        score = 0f;
        transform.position = pos;

        force = _force;
        theta = _theta;

        Vector3 direction = new Vector3(Mathf.Cos(theta * Mathf.Deg2Rad), Mathf.Sin(theta * Mathf.Deg2Rad), 0f);

        rg.AddForce(direction * force, ForceMode.Impulse);
        startline.SetActive(false);
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
