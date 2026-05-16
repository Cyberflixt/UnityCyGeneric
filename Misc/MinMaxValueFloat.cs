
using System;
using UnityEngine;

public class MinMaxValueFloat
{
    private float _min;
    private float _max;
    private float _value;

    public Action onChanged;

    public float min
    {
        get => _min;
        set
        {
            _min = value;
            onChanged?.Invoke();
        }
    }

    public float max
    {
        get => _max;
        set
        {
            _max = value;
            onChanged?.Invoke();
        }
    }

    public float value
    {
        get => _value;
        set
        {
            if (value > _max)
                _value = _max;
            else if (value < _min)
                _value = _min;
            else
                _value = value;
            onChanged?.Invoke();
        }
    }

    public float valueNormalized
    {
        get => (value - _min) / (_max - _min);
        set => this.value = _min + (_max - _min) * value;
    }

    public MinMaxValueFloat(float Min, float Max, float Value)
    {
        _min = Min;
        _max = Max;
        _value = Value;
        onChanged = null;
    }
}
