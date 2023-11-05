using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class PoolEntry
{
    public string name;
    public PackedScene pack;
    public Queue<Node> queue;
}

public class Pool
{
    public PackedScene[] templates;
    public Node parentNode;
    public List<PoolEntry> entries;

    public static Pool instance;
    public bool debug = false;

    /// <summary>
    /// Gets a PackedScene providing name.
    /// </summary>
    /// <param name="name">PackedScene's name (Case sensitive).</param>
    /// <returns>PackedScene if there's a match, or null if there isn't.</returns>
    public PackedScene GetPackFromName(string name)
    {
        foreach (PackedScene p in templates)
        {
            if (GetNameFromPack(p) == name) return p;
        }

        return null;
    }

    /// <summary>
    /// Get's a pool's entry providing name.
    /// </summary>
    /// <param name="name">PackedScene's name.</param>
    /// <returns>PoolEntry if it exist, null if it doesn't.</returns>
    public PoolEntry GetEntryFromName(string name)
    {
        foreach (PoolEntry ent in entries)
        {
            if (ent.name == name) return ent;
        }

        return null;
    }

    /// <summary>
    /// Extract PackedScene's root node's name providing the PackedScene itself.
    /// </summary>
    /// <param name="pack">PackedScene to extract the name from.</param>
    /// <returns>String of the PackedScene's root node's name.</returns>
    public string GetNameFromPack(PackedScene pack)
    {
        RegEx reg = new RegEx();
        reg.Compile("\"(.*?)\""); string packName = pack._Bundled.Values.ToArray()[0].ToString().Trim();

        return reg.Search(packName).Strings[0].Replace("\"", "");
    }

    /// <summary>
    /// Initialize the pool providing templates/scenes as an array, and the node to hold all instantiated objects.
    /// </summary>
    /// <param name="temps">Templates/Scenes as PackedScene.</param>
    /// <param name="parent">The node that will be used to parent instantiated nodes.</param>
    /// <returns>True/False of the initializatoin's success.</returns>
    public bool Innit(PackedScene[] temps, Node parent)
    {
        if (debug) GD.Print("... Initializing pool...");
        
        if (instance == null)
        {
            instance = this;
            if (debug) GD.Print("... Assigned instance.");
            templates = temps;
            if (debug) GD.Print("... Assigned templates.");
            parentNode = parent;
            if (debug) GD.Print("... Assigned parent node.");
            entries = new List<PoolEntry>();
            foreach (PackedScene p in templates)
            {
                Register(p);
            }
            if (debug) GD.Print("Pool is ready.");
            return true;
        }
        if (debug) GD.PrintErr("Failed to assign instance: Pool.instance != null!");
        return false;
    }

    /// <summary>
    /// Registers and adds a PackedScene to the pool's templates.
    /// </summary>
    /// <param name="pack">PackedScene to register in the pool.</param>
    /// <returns>True/False of the registration's success.</returns>
    public bool Register(PackedScene pack)
    {
        try
        {
            PoolEntry entry = new PoolEntry();
            entry.name = GetNameFromPack(pack);
            entry.pack = pack;
            entry.queue = new Queue<Node>();
            if (debug) GD.Print("... Attempting to register " + entry.name);

            entries.Add(entry);
            if (debug) GD.Print("Added \"" + entry.name + "\" in the pool.");
            return true;
        }
        catch (Exception e)
        {
            if (debug) GD.PrintErr("Failed to register " + pack.ResourceName + "!: " +  e.Message);
            return false;
        }
    }


    /// <summary>
    /// Get's an object from the pool providing the object's name.
    /// </summary>
    /// <param name="nodeName">Object's name.</param>
    /// <returns>Node of the object from the pool, or null if the operation failed.</returns>
    /// <remarks>If you must rename the Node, make sure that you don't change the first part up to the first ":"</remarks>

    public Node GetPooled(string nodeName)
    {
        if (debug) GD.Print("... Attempting to get " +  nodeName + " from pool");
        foreach(PoolEntry entry in entries)
        {
            if (entry.name == nodeName)
            {
                if (entry.queue.Count > 0) return entry.queue.Dequeue();
                else
                {
                    foreach(PackedScene pack in templates)
                    {
                        if (pack == entry.pack)
                        {
                            Node n = pack.Instantiate();
                            parentNode.AddChild(n);
                            n.Name = entry.name + ":";
                            if (debug) GD.Print(nodeName + " has been successfully retreived.");
                            return n;
                        }
                    }
                    if (debug) GD.PrintErr("Failed to get " + nodeName + " from pool!");
                    return null;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Used to store the unwanted Node inside the pool given the Node's PackedScene is already registered
    /// </summary>
    /// <param name="node">Registered node</param>
    /// <returns>True/False of the operation's success</returns>
    /// <remarks>Make sure the node's name starts with the original PackedScene's original name followed by ":"</remarks>
    
    public bool StoreObject(Node node)
    {
        if (debug) GD.Print("... Attempting to store " + node.Name + " in pool.");
        string oName = node.Name.ToString().Split(':')[0];
        foreach(PoolEntry entry in entries)
        {
            if (oName.Contains(entry.name))
            {
                if (debug) GD.Print("Successfully stored " + node.Name + " in pool.");
                entry.queue.Enqueue(node);
                return true;
            }
        }
        if (debug) GD.PrintErr("Failed to store " + node.Name + " in pool!");

        return false;
    }

}
