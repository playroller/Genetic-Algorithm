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

            Select();
            Crossover();
            Mutation();

            for(int i = 0; i < 20; ++i)
            {
                ballControls[i].Init(next_force[i], next_theta[i]);
            }
        }
    }

    // 選択
    private void Select()
    {
        // トーナメント方式で次世代に残す個体を10体選択する
        for (int i = 0; i < 10; ++i)
        {
            int best_num = Random.Range(0, 20);
            for(int j = 1; j < nt; ++j)
            {
                int num = Random.Range(0, 20);
                if(ballControls[best_num].score < ballControls[num].score) best_num = num;
            }
            next_force[i] = ballControls[best_num].force;
            next_theta[i] = ballControls[best_num].theta;
        }
    }

    // 交叉
    private void Crossover()
    {
        for(int i = 10; i < 20; i += 2)
        {
            // 評価値の高い個体の中からランダムに二つ個体を選ぶ
            int a = Random.Range(0, 10);
            int b = Random.Range(0, 10);

            next_force[i] = next_force[a]; next_theta[i] = next_theta[b];
            next_force[i + 1] = next_force[b]; next_theta[i + 1] = next_theta[a];
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
