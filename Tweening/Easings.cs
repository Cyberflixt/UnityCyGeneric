using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Eases
{
    // Quadratic
    public static float InQuad(float x)
    {
        return x * x;
    }
    public static float OutQuad(float x)
    {
        return 1f - (1f - x) * (1f - x);
    }
    public static float InOutQuad(float x)
    {
        return x < 0.5f ? 2f * x * x : 1f - Mathf.Pow(-2f * x + 2f, 2f) / 2f;
    }

    // Cubic
    public static float InCubic(float x)
    {
        return x * x * x;
    }
    public static float OutCubic(float x)
    {
        return 1f - (1f - x) * (1f - x) * (1f - x);
    }
    public static float InOutCubic(float x)
    {
        return x < 0.5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;
    }

    // Quartic
    public static float InQuart(float x)
    {
        float xx = x * x;
        return xx * xx;
    }
    public static float OutQuart(float x)
    {
        float xx = (1f - x) * (1f - x);
        return 1f - xx * xx;
    }
    public static float InOutQuart(float x)
    {
        return x < 0.5f ? 8f * x * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 4f) / 2f;
    }

    // Quintic
    public static float InQuint(float x)
    {
        float xx = x * x;
        return xx * xx * x;
    }
    public static float OutQuint(float x)
    {
        float xx = (1f - x) * (1f - x);
        return 1f - xx * xx * x;
    }
    public static float InOutQuint(float x)
    {
        return x < 0.5f ? 16f * x * x * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 5f) / 2f;
    }

    // X^N: Generalised form
    public static float InPower(float x, int pow = 2)
    {
        return Mathf.Pow(x, pow);
    }
    public static float OutPower(float x, int pow = 2)
    {
        return 1f - Mathf.Pow(1f - x, pow);
    }
    public static float InOutPower(float x, int pow = 2)
    {
        return x < 0.5f ? Mathf.Pow(2, pow) * Mathf.Pow(x, pow) : 1f - Mathf.Pow(-2f * x + 2f, pow) / 2f;
    }

    // Sinus
    public static float InSine(float x)
    {
        return 1f - Mathf.Cos(x * Mathf.PI / 2f);
    }
    public static float OutSine(float x)
    {
        return Mathf.Sin(x * Mathf.PI / 2f);
    }
    public static float InOutSine(float x)
    {
        return (Mathf.Cos(x * Mathf.PI) - 1f) / -2f;
    }

    // Circle
    public static float InCircle(float x)
    {
        return 1 - Mathf.Sqrt(1 - x * x);
    }
    public static float OutCircle(float x)
    {
        return Mathf.Sqrt(1 - (1 - x) * (1 - x));
    }
    public static float InOutCircle(float x)
    {
        return x < 0.5f
            ? (1f - Mathf.Sqrt(1f - 4f * x * x)) / 2f
            : (Mathf.Sqrt(1f - (2f - 2f * x) * (2f - 2f * x)) + 1f) / 2f;
    }

    // Bounce
    public static float InBounce2(float x)
    {
        return 1 - OutBounce2(1 - x);
    }
    public static float OutBounce2(float x)
    {
        float x0 = 2.5f; // Sharpness of 1st fall
        float h1 = 0.7f; // Height of first bounce

        float root_x0 = Mathf.Sqrt(x0);
        float b0 = root_x0 / x0; // X of first bounce's start

        // 1st fall
        if (x < b0)
            return x0 * x * x;

        // 1st bounce
        float c1 = 0.5f + root_x0 / (2 * x0); // x center of first bounce
        float a1 = (1f - h1) * (4f * x0 / (1f - 2f * root_x0 + root_x0)); // aX² correction for first bounce
        return (x - c1) * (x - c1) * a1 + h1;
    }

    // Back
    public static float OutBack(float x)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;

        float xx = (x - 1) * (x - 1);
        return 1 + c3 * xx * x + c1 * xx;
    }

    public static Func<float, float> GetFunc(EasingStyle style, EasingDirection direction)
    {
        switch (style)
        {
            case EasingStyle.Quad:
                switch (direction)
                {
                    case EasingDirection.In:
                        return InQuad;
                    case EasingDirection.Out:
                        return OutQuad;
                    case EasingDirection.InOut:
                        return InOutQuad;
                }
                break;
            case EasingStyle.Cubic:
                switch (direction)
                {
                    case EasingDirection.In:
                        return InCubic;
                    case EasingDirection.Out:
                        return OutCubic;
                    case EasingDirection.InOut:
                        return InOutCubic;
                }
                break;
            case EasingStyle.Quart:
                switch (direction)
                {
                    case EasingDirection.In:
                        return InQuart;
                    case EasingDirection.Out:
                        return OutQuart;
                    case EasingDirection.InOut:
                        return InOutQuart;
                }
                break;
            case EasingStyle.Quint:
                switch (direction)
                {
                    case EasingDirection.In:
                        return InQuint;
                    case EasingDirection.Out:
                        return OutQuint;
                    case EasingDirection.InOut:
                        return InOutQuint;
                }
                break;
            case EasingStyle.Sine:
                switch (direction)
                {
                    case EasingDirection.In:
                        return InSine;
                    case EasingDirection.Out:
                        return OutSine;
                    case EasingDirection.InOut:
                        return InOutSine;
                }
                break;
            case EasingStyle.Circle:
                switch (direction)
                {
                    case EasingDirection.In:
                        return InCircle;
                    case EasingDirection.Out:
                        return OutCircle;
                    case EasingDirection.InOut:
                        return InOutCircle;
                }
                break;
            case EasingStyle.Bounce2:
                switch (direction)
                {
                    case EasingDirection.In:
                        return InBounce2;
                    case EasingDirection.Out:
                        return OutBounce2;
                }
                break;
        }
        throw new NotImplementedException("Easing function not implemented");
    }
}
