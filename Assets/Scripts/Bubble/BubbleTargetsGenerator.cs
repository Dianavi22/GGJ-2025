using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BubbleTargetsGenerator : MonoBehaviour {

    public List<GameObject> bubbleTargetPoints = new();

    [SerializeField] private bool IsDebug = false;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(ComputeTargets());
    }

    IEnumerator ComputeTargets() {
        while (true) {
            bubbleTargetPoints.ForEach(p => {
                Destroy(p.gameObject);
            });
            bubbleTargetPoints.Clear();

            MeshFilter meshFilter = GetComponentInChildren<MeshFilter>();

            int count = meshFilter.mesh.vertices.Count();

            for (int i = 0; i < count; i++) {
                Vector3 currentVerticeVector3 = meshFilter.mesh.vertices[i];
                if (Mathf.Abs(currentVerticeVector3.z) < 0.01f) {
                    GameObject obj = IsDebug ? GameObject.CreatePrimitive(PrimitiveType.Sphere) : CreateEmptyGameObject();
                    obj.GetComponent<Renderer>().material.color = Color.red;
                    obj.transform.position = transform.position +
                        new Vector3(transform.localScale.x * currentVerticeVector3.x,
                                    transform.localScale.y * currentVerticeVector3.y,
                                    transform.localScale.z * currentVerticeVector3.z);
                    obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    bubbleTargetPoints.Add(obj);
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    public GameObject CreateEmptyGameObject() {
        //spawn object
        GameObject gameobject = new GameObject("Bubble target point");
        //Add Components
        gameobject.AddComponent<Rigidbody>();
        gameobject.AddComponent<MeshFilter>();
        gameobject.AddComponent<BoxCollider>();
        gameobject.AddComponent<MeshRenderer>();
        gameobject.AddComponent<BubbleTargetPoint>();

        return gameobject;
    }

    private void CreateMesh() {
        //Mesh mesh = new Mesh();


        //List<System.Numerics.Vector3> allVertices = new();

        //for(int i = 0; i < 60; i++) {
        //    float angle = 2 * Mathf.PI * i / 60;
        //    System.Numerics.Vector3 vector = new() {
        //        X = Mathf.Cos(angle),
        //        Y = Mathf.Sin(angle),
        //        Z = 0
        //    };

        //    allVertices.Add(vector);
        //}

        //mesh.vertices.AddRange(allVertices);

        //GetComponent<MeshFilter>().mesh = mesh;
    }
}
