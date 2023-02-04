using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashObject {
    public GameObject obj;
    public float speed;
    public Vector3 resetPosition;

    public HashObject(
        GameObject obj, float speed, float resetPosition,
        string hashString, Vector3 playerResetPosition) {
        this.obj = obj;
        this.speed = speed;
        this.resetPosition = new Vector3(
            obj.transform.position.x,
            resetPosition,
            obj.transform.position.z
        );
        this.obj.GetComponent<HashElement>().Initialize(hashString, playerResetPosition);
    }

    public void Move() {
        this.obj.transform.position
            = this.obj.transform.position + new Vector3(0, -this.speed, 0);
    }

    public void Reset() {
        this.obj.transform.position = this.resetPosition;
    }
}

public class MatrixCyper : MonoBehaviour {
    [SerializeField] private float range;
    [SerializeField] private Vector2 speedRange;
    [SerializeField] private int length;
    [SerializeField] private int amount;
    [SerializeField] private Vector2 resetYLevel;
    [SerializeField] private GameObject hashPrefab;
    [SerializeField] private Transform playerResetPosition;

    private List<HashObject> hashObjs;

    private void Start() {
        this.hashObjs = new List<HashObject>();
        for (int i = 0; i < this.amount; i++) {
            string hash = GenerateHash();
            this.hashObjs.Add(new HashObject(
                Instantiate(
                    this.hashPrefab,
                    new Vector3(
                        Random.Range(-this.range, this.range),
                        this.resetYLevel.x,
                        0
                    ),
                    Quaternion.identity,
                    transform
                ),
                Random.Range(this.speedRange.x, this.speedRange.y),
                this.resetYLevel.x,
                hash,
                this.playerResetPosition.position
            ));
        }
    }

    private void FixedUpdate() {
        foreach (HashObject obj in this.hashObjs) {
            obj.Move();
            if (obj.obj.transform.position.y <= this.resetYLevel.y) {
                obj.Reset();
            }
        }
    }

    private string GenerateHash() {
        Hash128 hash = new Hash128();
        hash.Append(Random.Range(0, 1000));
        return hash.ToString().Substring(0, length);
    }
}
