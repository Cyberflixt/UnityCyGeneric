using UnityEngine;

public class BigHeaderAttribute : PropertyAttribute
{
    public readonly int fontSize;

    public BigHeaderAttribute(int fontSize = 20)
    {
        this.fontSize = fontSize;
    }
}