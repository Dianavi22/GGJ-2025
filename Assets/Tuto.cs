using Assets.Scripts;
using Player;
using System.Collections;
using TMPro;
using UnityEngine;

public class Tuto : MonoBehaviour
{
    [SerializeField] TMP_Text _tuto;
    [SerializeField] TypeSentence _typeSentence;
    [SerializeField] ShootBehaviour _player;
    private GameManager _gameManager = GameManager.Instance;

    public bool isInTuto = true;
    private bool _isReadyToClick = false;

    void Start()
    {
        StartCoroutine(TutoTxt());
    }

    private void Update() {
        if(Input.GetMouseButton(0) && _isReadyToClick) {

            isInTuto = false;
            _player.enabled = true;
            _tuto.text = "";

        }
    }

    private IEnumerator TutoTxt() {

        _tuto.text = "";
        _typeSentence.WriteMachinEffect("Make the biggest bubble by shooting, by clicking with the mouse", _tuto, 0.01f);
        yield return new WaitForSeconds(2f);
        _isReadyToClick = true;
        print("_isReadyToClick " + _isReadyToClick);
    }

  

    
}
