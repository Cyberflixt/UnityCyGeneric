using UnityEngine;

public class WarningBoxAttribute : PropertyAttribute
{
    public string Message;

    public WarningBoxAttribute(string message)
    {
        Message = message;
    }
}
