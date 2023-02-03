using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DirectoryData {
    public string name;
    public string parent;
    public string[] children;
}

public class Directory
{
    public delegate void Enumerator(in string name, int index);
    public bool IsRoot { get { 
        if(this.rootOverride) return true;
        return (this.parent == null || this.parent == ""); 
    } }
    public int Count { get { 
        return this.children.Length > 0 ? this.children.Length: 0; 
    } }
    public bool HasVisited { get; private set; } = false;
    public string Name { get { 
        if (this.rootOverride) return "Root";
        return this.name;
    }}
    public bool HasParent { get {
        return !IsRoot;
    } }

    private string name;
    private string parent;
    private string[] children;
    private bool rootOverride = false;

    public Directory(DirectoryData data) {
        this.name = data.name;
        this.parent = data.parent;
        this.children = data.children;
    }

    public void SetAsRoot() {
        this.rootOverride = true;
    }

    public void Reset() {
        this.rootOverride = false;
    }

    public string GoUp() {
        if (IsRoot) return null;
        this.HasVisited = true;
        return this.parent;
    }

    public string GoDown(int index) {
        if(this.children == null || index >= this.children.Length) return null;
        this.HasVisited = true;
        if (this.children[index] == "") return null;
        return this.children[index];
    }

    public bool HasChildren() {
        return (this.children != null && this.children.Length > 0);
    }

    public void Foreach(Enumerator enumerator) {
        for(int i = 0; i < this.Count; i++) {
            enumerator(this.children[i], i);
        }
    }
}
