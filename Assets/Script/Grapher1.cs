using UnityEngine;
using System.Collections;

public class Grapher1 : MonoBehaviour
{

    public enum FunctionOption
    {
        Linear,
        Exponential,
        Parabola,
        Sine
    }

    private delegate float FunctionDelegate(float x);
    private static FunctionDelegate[] functionDelegates = {
        Linear,
        Exponential,
        Parabola,
        Sine
    };

    public FunctionOption function;

    [Range(10, 100)]
    // 解像度の定義
    public int resolution = 10;
    // 現解像度
    private int currentResolution;
    // パーティクルの座標の配列定義
    private ParticleSystem.Particle[] points;

    private void CreatePoints()
    {
        // 解像度の条件
        currentResolution = resolution;
        points = new ParticleSystem.Particle[resolution];
        float increment = 1f / (resolution - 1);
        // 座標と色の設定
        for (int i = 0; i < resolution; i++)
        {
            float x = i * increment;
            points[i].position = new Vector3(x, 0f, 0f);
            points[i].color = new Color(x, 0f, 0f);
            points[i].size = 0.1f;
        }
    }

    void Update()
    {
        if (currentResolution != resolution || points == null)
        {
            CreatePoints();
        }
        FunctionDelegate f = functionDelegates[(int)function];
        for (int i = 0; i < resolution; i++)
        {
            Vector3 p = points[i].position;
            p.y = f(p.x);
            points[i].position = p;
            Color c = points[i].color;
            c.g = p.y;
            points[i].color = c;
        }
        GetComponent<ParticleSystem>().SetParticles(points, points.Length);
    }

    /*
        数式定義
    */
    // 直線
    private static float Linear(float x)
    {
        return x;
    }

    // 放物線(s^2)
    private static float Exponential(float x)
    {
        return x * x;
    }

    // 放物線(2x-1)^2
    private static float Parabola(float x)
    {
        x = 2f * x - 1f;
        return x * x;
    }

    // (sin(2πx + Δ) + 1) / 2
    private static float Sine(float x)
    {
        return 0.5f + 0.5f * Mathf.Sin(2 * Mathf.PI * x + Time.timeSinceLevelLoad);
    }
}