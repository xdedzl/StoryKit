using System.Collections.Generic;
using XFramework.UI;

public class NodeData
{
    [ElementIngore]
    public int id;
    public string name;
    [ElementIngore]
    public List<int> nextNodes;
}