using System.Collections.Generic;

public class NodeData
{
    public int id;
    [TextField]
    public string name;
    public List<int> nextNodes;
}

public class NodeElemnetAttribute : System.Attribute { }
public class TextFieldAttribute : NodeElemnetAttribute { }
public class TextureAttribute : NodeElemnetAttribute { }