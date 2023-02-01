using System.Collections;
using System.Collections.Generic;
using System;

public class Bag<T> {
    #region Variables
    // Consts
    public delegate void Enumerator(in T item, in int amount);

    // Publics
    public int ItemCount { get { return this.keys.Count; } }
    public int Total {
        get {
            int total = 0;
            foreach (T key in this.keys) {
                total += this.amounts[key];
            }
            return total;
        }
    }

    // Privates
    private Dictionary<T, int> amounts;
    private HashSet<T> keys;
    private Random rng;
    #endregion Variables

    public Bag() {
        this.amounts = new Dictionary<T, int>();
        this.keys = new HashSet<T>();
        this.rng = new Random();
    }

    #region Operator Overloading
    public int this[T key] {
        get {
            if (key == null) throw new ArgumentNullException("Null Key");
            if (!this.keys.Contains(key)) throw new IndexOutOfRangeException("Invalid Key: " + key);
            return this.amounts[key];
        }
        set {
            if (key == null) throw new ArgumentNullException("Null Key");
            if (!this.keys.Contains(key)) throw new IndexOutOfRangeException("Invalid Key: " + key);
            this.amounts[key] = value;
        }
    }
    #endregion Operator Overloading

    #region Methods
    public void Add(T key) {
        if (key == null) throw new ArgumentNullException("Null Key");
        if (this.keys.Contains(key)) throw new ArgumentException("Key already exists: " + key);

        this.amounts.Add(key, 1);
        this.keys.Add(key);
    }
    public void Add(T key, int amount) {
        if (key == null) throw new ArgumentNullException("Null Key");
        if (this.keys.Contains(key)) throw new ArgumentException("Key already exists: " + key);

        this.amounts.Add(key, amount);
        this.keys.Add(key);
    }

    public void Remove(T key) {
        if (key == null) throw new ArgumentNullException("Null Key");
        if (!this.keys.Contains(key)) throw new IndexOutOfRangeException("Invalid Key: " + key);

        if (this.amounts[key] > 1) {
            this.amounts[key]--;
            return;
        }

        this.amounts.Remove(key);
        this.keys.Remove(key);
    }

    public bool ContainsKey(T key) {
        return this.keys.Contains(key);
    }

    public T PickRandomItem() {
        int rngIndex = this.rng.Next(this.keys.Count);
        int count = 0;
        HashSet<T>.Enumerator e = this.keys.GetEnumerator();
        T value = default(T);
        foreach (T item in this.keys) {
            if (count == rngIndex) {
                value = item;
                break;
            } else {
                count++;
            }
        }
        return value;
    }

    public void Foreach(Enumerator enumerator) {
        foreach (T item in this.keys) {
            enumerator(item, this.amounts[item]);
        }
    }
    #endregion
}