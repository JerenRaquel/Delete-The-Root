using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertManager : MonoBehaviour {
    #region Class Instance
    public static AlertManager instance = null;
    private void CreateInstance() {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion
    private void Awake() => CreateInstance();

    private Dictionary<string, int> alertedIps;

    private void Start() {
        this.alertedIps = new Dictionary<string, int>();
    }

    public int RaiseAlertLevel(string ipv6) {
        if (!this.alertedIps.ContainsKey(ipv6)) {
            this.alertedIps.Add(ipv6, 1);
            return 1;
        } else {
            this.alertedIps[ipv6] = Mathf.Min(this.alertedIps[ipv6] + 1, 10);
            if (this.alertedIps[ipv6] == 10) {
                this.alertedIps[ipv6] = 5;
                GameController.instance.DisconnectFromFileDir(ipv6);
            }
            return this.alertedIps[ipv6];
        }
    }

    public int GetAlertLevel(string ipv6) {
        if (!this.alertedIps.ContainsKey(ipv6)) return 0;
        return this.alertedIps[ipv6];
    }

    public void LowerAlertLevels(string ipv6) {
        if (this.alertedIps.ContainsKey(ipv6)) {
            this.alertedIps.Remove(ipv6);
        }

        foreach (KeyValuePair<string, int> item in this.alertedIps) {
            this.alertedIps[item.Key] = Mathf.Max(0, item.Value - 5);
            if (this.alertedIps[item.Key] == 0) {
                this.alertedIps.Remove(item.Key);
            }
        }
    }
}
