using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DirectoryData", menuName = "Delete The Root/DirectoryData", order = 0)]
public class DirectoryData : ScriptableObject {
    public new string name;
    public DirectoryData parent;
    public DirectoryData[] children;
}

public class Directory
{
    public delegate void Enumerator(in string name, int index);
    public bool IsRoot { get { return this.parent == null; } }
    public int Count { get { 
        return this.children.Length > 0 ? this.children.Length: 0; 
    } }
    public bool HasVisited { get; private set; } = false;

    private string name;
    private Directory parent;
    private Directory[] children;

    public Directory(DirectoryData data, bool isRoot = false) {
        if(isRoot){
            this.name = "Root";
        } else {
            this.name = data.name;
            this.parent = new Directory(data.parent);
        }
        this.children = new Directory[data.children.Length];
        for(int i = 0; i < data.children.Length; i++) {
            this.children[i] = new Directory(data.children[i]);
        }
    }

    public Directory GoUp() {
        if (IsRoot) return null;
        this.HasVisited = true;
        return this.parent;
    }

    public Directory GoDown(int index) {
        if(this.children == null || index >= this.children.Length) return null;
        this.HasVisited = true;
        return this.children[index];
    }

    public void Foreach(Enumerator enumerator) {
        for(int i = 0; i < this.Count; i++) {
            enumerator(this.children[i].name, i);
        }
    }
}
