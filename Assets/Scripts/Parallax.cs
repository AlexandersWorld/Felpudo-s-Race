using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Material Mat;
    private float Distance;

    [SerializeField]
    [Range(0f, 0.5f)]
    private float Speed = 0.2f;

    void Start()
    {
        Mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        Distance += Time.deltaTime * Speed;

        Mat.SetTextureOffset("_MainTex", Vector2.right * Distance);
    }
}
