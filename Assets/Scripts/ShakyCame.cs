using System.Collections;
using UnityEngine;

// Ce script est a placer sur une camera
// Il prend directement son transform ccomme point de depart de la shaky came
public class ShakyCame : MonoBehaviour
{
    private Transform _pointToShake;
    private float _speed = 0; 
    private Vector3 _offset;

    [Header("Configuration de la duree et de la distance de secousse")]
    private float _duration = 1f; 
    private float _radius = 1; 

    public bool isShaking = false; 

    Vector3 center = Vector3.zero; 

    private void Start()
    {
        _pointToShake = GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _pointToShake.position + _offset, _speed * Time.deltaTime);
        if (isShaking)
        {
            isShaking = false;
            StartCoroutine(Shaking());
        }
    }

    public void ShakyCameCustom(float duration, float radius)
    {
        _radius = radius;
        _duration = duration;
        isShaking = true;
    }
    IEnumerator Shaking() 
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = startPosition + Random.insideUnitSphere * _radius + center;
            yield return null;
        }
        transform.position = startPosition;
    }
}
