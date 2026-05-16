using System;

[Serializable]
[AddTypeMenu("Flow/Header")]
public class AnimationElementHeader : AnimationElementBase
{
    [BigHeader(24)]
    public string header;
}
