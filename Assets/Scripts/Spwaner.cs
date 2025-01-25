using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;

public class Spwaner : MonoBehaviour {
    private bool _isActive = false;

    void Start() {

    }

    public void Spawn(MonsterType monsterType) {
        if (!_isActive) {
            return;
        }

        string monsterToLoad = monsterType == MonsterType.BubbleAttacker ? "Prefabs/BubbleAttacker" : "Prefabs/PlayerAttacker";

        var myGameObject = Resources.Load(monsterToLoad) as GameObject;
        myGameObject.transform.position = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.name == "Bubble") {
            _isActive = true;
        }
    }
}
