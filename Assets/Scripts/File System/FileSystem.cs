using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSystem {
    public delegate void Callback();
    public bool IsUnlocked {
        get {
            return this.Keys == 3 && this.currentDirectory.IsRoot;
        }
    }
    public bool HasVisited { get { return this.currentDirectory.HasVisited; } }
    public string DirectoryName { get { return this.currentDirectory.Name; } }
    public int Keys { get; private set; } = 0;
    public bool IsRoot { get { return this.currentDirectory.IsRoot; } }
    public bool HasKey { get { return this.currentDirectory.HasKey; } }

    private Dictionary<string, Directory> directories;
    private Directory currentDirectory = null;
    private string rootDirectory;

    public FileSystem(DirectoryData[] data, string root) {
        this.directories = new Dictionary<string, Directory>();
        foreach (DirectoryData item in data) {
            this.directories.TryAdd(item.name, new Directory(item));
        }
        this.rootDirectory = root;
        this.currentDirectory = this.directories[this.rootDirectory];
        this.currentDirectory.SetAsRoot();
        GenerateKeys();
    }

    public void FoundKey() {
        this.Keys++;
        this.currentDirectory.HasKey = false;
    }

    public string GetParent() {
        if (!this.currentDirectory.HasParent) return null;
        return this.currentDirectory.GoUp();
    }

    public string GetChild(int index) {
        if (!this.currentDirectory.HasChildren) return null;
        return this.currentDirectory.GoDown(index);
    }

    public bool CheckIfFile(string name) {
        if (name == null || name == "") return false;
        if (!this.directories.ContainsKey(name)) return true;
        return !this.directories[name].HasChildren;
    }

    public void ChangeDirectory(string name, Callback callback) {
        if (name == null || name == "") return;
        if (!this.directories.ContainsKey(name)) return;
        this.currentDirectory.Visited();
        this.currentDirectory = this.directories[name];
        callback();
    }

    public void MarkAsVisited() {
        this.currentDirectory.Visited();
    }

    public void Foreach(Directory.Enumerator func) {
        this.currentDirectory.Foreach(func);
    }

    private void GenerateKeys() {
        List<string> dirs = LocateAllPossibleKeyDirectories();
        for (int i = 0; i < 3; i++) {
            int rng = Random.Range(0, dirs.Count);
            this.directories[dirs[rng]].HasKey = true;
            dirs.Remove(dirs[rng]);
        }
    }

    private List<string> LocateAllPossibleKeyDirectories() {
        List<string> dirs = new List<string>();
        foreach (var item in this.directories.Keys) {
            if (this.directories[item].IsRoot) continue;
            if (!this.directories[item].HasChildren) continue;
            dirs.Add(item);
        }
        return dirs;
    }
}