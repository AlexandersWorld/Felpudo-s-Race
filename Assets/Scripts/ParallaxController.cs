using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private Transform Cam;
    private Vector3 CamStartPosition;
    private float Distance;

    private GameObject[] Backgrounds;
    Material[] Mat;
    float[] BackSpeed;

    float FarthestBack;

    [SerializeField]
    [Range(0.1f, 0.5f)]
    private float ParallaxSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cam = Camera.main.transform;
        CamStartPosition = Cam.position;

        int backCount = transform.childCount;

        Mat = new Material[backCount];

        BackSpeed = new float[backCount];
        Backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            Backgrounds[i] = transform.GetChild(i).gameObject;
            Mat[i] = Backgrounds[i].GetComponent<Renderer>().material;
        }

        BackSpeedCalculate(backCount);
    }

    void BackSpeedCalculate(int backCount)
    {
        for (int i = 0; i < backCount; i++)
        {
            if ((Backgrounds[i].transform.position.z - Cam.position.z) > FarthestBack)
            {
                FarthestBack = Backgrounds[i].transform.position.z - Cam.position.z;
            }
        }

        for (int i = 0; i < backCount; i++)
        {
            BackSpeed[i] = 1 - (Backgrounds[i].transform.position.z - Cam.position.z) / FarthestBack;
        }
    }

    private void LateUpdate()
    {
        Distance = Cam.position.x - CamStartPosition.x;
        transform.position = new Vector3(Cam.position.x, transform.position.y, 0);

        for (int i = 0; i < Backgrounds.Length; i++)
        {
            float Speed = BackSpeed[i] * ParallaxSpeed;

            Mat[i].SetTextureOffset("_MainText", new Vector2(Distance, 0) * Speed);
        }
    }
}
