using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionDictionary : MonoBehaviour
{
    public Dictionary<int, GameObject> selectedTable = new Dictionary<int, GameObject>();

    public void AddSelected(GameObject go) {
        int id = go.GetInstanceID();
        if (!selectedTable.ContainsKey(id)) {
            selectedTable.Add(id, go);
            go.AddComponent<SelectionComponent>();
            Debug.Log("selected: " + id);
        }
    }

    public void Deselect(int id) {
        Destroy(selectedTable[id].GetComponent<SelectionComponent>());
        selectedTable.Remove(id);
    }

    public void DeselectAll() {
        foreach(KeyValuePair<int,GameObject> pair in selectedTable) {
            if(pair.Value != null) {
                Destroy(selectedTable[pair.Key].GetComponent<SelectionComponent>());
            }
        }
        selectedTable.Clear();
    }
}
