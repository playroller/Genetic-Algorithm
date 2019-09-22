using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    // 個体を格納する変数
    [SerializeField] private GameObject[] balls = new GameObject[20];

    // 次世代の個体の遺伝子を格納する変数
    private float[] next_force = new float[20];
    private float[] next_theta = new float[20];

    // トーナメント方式で選択する際に取る遺伝子の個数
    [SerializeField] private int nt = 5;

    // 突然変異が起こる確率
    [SerializeField] private float p = 0.5f;

    private BallControl[] ballControls = new BallControl[20];

    private float maxScore = 0.0f;

    private void Start()
    {
        for(int i = 0; i < 20; ++i)
        {
            ballControls[i] = balls[i].GetComponent<BallControl>();
        }
    }

    private void Update()
    {
        bool finish = true;
        for(int i = 0; i < 20; ++i)
        {
            if(ballControls[i].finish == false) finish = false;
        }

        if(finish == true)
        {
            for(int i = 0; i < 20; ++i)
            {
                maxScore = Mathf.Max(maxScore, ballControls[i].score);
            }
            Debug.Log(maxScore + " " + ballControls[0].force + " " + ballControls[0].theta);

            Crossover();
            Mutation();

            for(int i = 0; i < 20; ++i)
            {
                ballControls[i].Init(next_force[i], next_theta[i]);
            }
        }
    }

    // 選択
    private void Select(ref float force, ref float theta)
    {
        // トーナメント方式で選択する
        int best_num = Random.Range(0, 20);
        for (int j = 1; j < nt; ++j)
        {
            int num = Random.Range(0, 20);
            if (ballControls[best_num].score < ballControls[num].score) best_num = num;
        }
        force = ballControls[best_num].force;
        theta = ballControls[best_num].theta;
    }

    // 交叉
    private void Crossover()
    {
        for(int i = 0; i < 20; i += 2)
        {
            // 個体二つを選択する
            float force_a = 0f, force_b = 0f, theta_a = 0f, theta_b = 0f;
            Select(ref force_a, ref theta_a);
            Select(ref force_b, ref theta_b);

            // 交叉させる
            next_force[i] = force_a; next_theta[i] = theta_b;
            next_force[i + 1] = force_b; next_theta[i + 1] = theta_a;
        }
    }

    // 突然変異
    private void Mutation()
    {
        for(int i = 0; i < 20; ++i)
        {
            float num = Random.value * 100.0f;
            if(num < p) next_force[i] = Random.Range(0.0f, 20.0f);

            num = Random.value * 100.0f;
            if(num < p) next_theta[i] = Random.Range(0.0f, 90.0f);
        }
    }
}
