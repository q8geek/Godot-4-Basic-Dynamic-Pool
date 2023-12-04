using Godot;
using System.Collections.Generic;


/// <summary>
///     Contains a dictionary of PackedScene and Queue<Node> for objects to disable to be re-used.
/// </summary>

public class Pool
{
    private Dictionary<PackedScene, Queue<Node>> entries;
    private Node poolParent;
    
    public static Pool instance;
    
    /// <summary>
    ///     Pool class' constructor. It receives an array of the PackedScenes to be pooled, and create Queue<Node> for each.
    /// </summary>
   
    /// <param name="packs">The PackedScenes that needs to be queued.</param>
    /// <param name="poolParent">The Node to act as the default parent node for Instantiated Nodes from PackedScenes.</param>
    public Pool(PackedScene[] packs, Node poolParent)
    {
        if (instance != null) return;

        instance = this;
        entries = new Dictionary<PackedScene, Queue<Node>>();

        foreach (PackedScene pack in packs)
        {
            entries.Add(pack, new Queue<Node>());
        }

        this.poolParent = poolParent;
    }

    /// <summary>
    ///     This method stores the unneeded Node in its relative Queue to be reused later.
    /// </summary>
    /// <param name="node">The unneeded Node. (CASE SENSITIVE)</param>
    /// <param name="name">The Node's original name. (The PackedScene's root node's name, basically)</param>
    /// <returns>
    ///     Returns a boolean result whether storing the Node to its relative PackedScene was successful.
    /// </returns>
    public bool Store(Node node, string name)
    {
        try
        {
            foreach (KeyValuePair<PackedScene, Queue<Node>> pair in entries)
            {
                PackedScene packedScene = pair.Key;
                string packName = packedScene.GetState().GetNodeName(0);
                if (packName == name)
                {
                    entries[pair.Key].Enqueue(node);
                    node.Reparent(poolParent);
                    return true;
                }
            }
            
            return false;
        }
        catch
        { 
            return false; 
        }
    }

    /// <summary>
    ///     Return a string info of the pool's entries and counts.
    /// </summary>
    /// <returns>A string of each PackedScene and its Queue count in this format "| NAME:COUNT | NAME:COUNT |".</returns>
    public override string ToString()
    {
        string text = "| ";
        foreach (KeyValuePair<PackedScene,Queue<Node>> pair in entries)
        {
            PackedScene packedScene = pair.Key;
            text += packedScene.GetState().GetNodeName(0) + ":" + pair.Value.Count + " | ";
        }

        return text.Trim();
    }

    /// <summary>
    ///     This method returns a Node from the PackedScene matching the "name" provided. If the PackedScene's queue is empty, it'll Instantiate a new one and return it.
    /// </summary>
    /// <param name="name">The name of the PackedScene's root Node's name required to return. (CASE SENSITIVE)</param>
    /// <returns>A Node dequeued from the pooled PackedScene, or null if PackedScene wasn't found.</returns>
    public Node Get(string name)
    {
        Node node = null;

        try
        {
            GD.Print("Trying " + name);
            foreach (KeyValuePair<PackedScene, Queue<Node>> pair in entries)
            {
                PackedScene packedScene = pair.Key;
                string packName = packedScene.GetState().GetNodeName(0);

                if (node == null && packName == name)
                {
                    if (entries[pair.Key].Count == 0)
                    {
                        node = pair.Key.Instantiate();
                        poolParent.AddChild(node);
                    }
                    
                    node = entries[pair.Key].Dequeue();
                    if (node.GetParent() != poolParent) node.Reparent(poolParent);
                }
            }
            return node;
        }
        catch
        {
            return node;
        }
    }

    /// <summary>
    /// Returns Queue count of a pooled PackedScene, given PackedScene's reference.
    /// </summary>
    /// <param name="packed">PackedScene's reference.</param>
    /// <returns>Queue's count if successful, -1 if not.</returns>
    public int GetCount(PackedScene packed)
    {
        int ret = -1;

        foreach (KeyValuePair<PackedScene,Queue<Node>> pair in entries)
        {
            if (packed == pair.Key)
                return pair.Value.Count;
        }

        return ret;
    }

    /// <summary>
    /// Returns Queue count of a pooled PackedScene, given PackedScene's name.
    /// </summary>
    /// <param name="name">PackedScene's name.</param>
    /// <returns>Queue's count if successful, -1 if not.</returns>
    public int GetCount(string name)
    {
        int ret = -1;

        foreach (KeyValuePair<PackedScene, Queue<Node>> pair in entries)
        {
            PackedScene packedScene = pair.Key;
            string packName = packedScene.GetState().GetNodeName(0);
            if (packName == name)
                return pair.Value.Count;
        }

        return ret;
    }

    /// <summary>
    /// Returns an array of PackedScenes root Node's name.
    /// </summary>
    /// <returns>An array of all pooled PackedScenes root Node's name.</returns>
    public string[] GetNames()
    {
        List<string> names = new List<string>();
        foreach (PackedScene pack in entries.Keys)
        {
            names.Add(pack.GetState().GetNodeName(0));
        }

        return names.ToArray();
    }
}