using UnityEngine;

public struct SBlockStats
{
    public PrimitiveType PrimitiveType { get; set; }
    public int Value { get; set; }
    public SBlockStats(PrimitiveType primitiveType, int value)
    {
        this.PrimitiveType = primitiveType;
        this.Value = value;
    }
}
public class BlockStats : MonoBehaviour
{
    public bool OnTable { get; set; }
    public int Merges { get; set; }
    public SBlockStats blockStats;
    
    private float MOVEMENT_THRESHOLD = 0.05f;
    private float FADE_SPEED = 8f;
    private float fadeProgress = 0f;
    private Rigidbody rb;
    private MeshRenderer mr;
    private Color originalColor;
    
    public BlockStats(PrimitiveType primitiveType, int value)
    {
        this.OnTable = false;
        this.Merges = 0;
        blockStats = new SBlockStats(primitiveType, value);
    }

    void OnEnable()
    {
        rb = this.GetComponentInParent<Rigidbody>();
        mr = this.GetComponentInParent<MeshRenderer>();
        originalColor = mr.material.color;
        fadeProgress = 0f;
    }


    void Update()
    {
        if (rb.linearVelocity.magnitude <= MOVEMENT_THRESHOLD)
        {
            fadeProgress += Time.deltaTime*.002f;
            Color current = mr.material.color;
            Color newColor = Color.Lerp(
                current,
                Color.gray,
                fadeProgress
            );
            mr.material.color = newColor;
        }
        else
        {
            mr.material.color = originalColor;
            fadeProgress = 0;
        }
    }
   
}
