using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float speed = 0.1f;
    private Material skyMaterial;
    private Vector2 offset;

    void Start()
    {
        // Obtiene el material del SpriteRenderer o del MeshRenderer
        if (GetComponent<SpriteRenderer>())
            skyMaterial = GetComponent<SpriteRenderer>().material;
        else if (GetComponent<MeshRenderer>())
            skyMaterial = GetComponent<MeshRenderer>().material;

        offset = skyMaterial.mainTextureOffset;
    }

    void Update()
    {
        offset.x += speed * Time.deltaTime;
        skyMaterial.mainTextureOffset = offset;
    }
}
