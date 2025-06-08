using UnityEngine;

public class ColorChange : MonoBehaviour
{
    public GameObject playerToAffect;
    private Material cachedMaterial;

    void Start()
    {
        if (playerToAffect == null)
        {
            enabled = false;
            return;
        }

        Renderer targetRenderer = playerToAffect.GetComponent<Renderer>();
        if (targetRenderer == null)
        {
            enabled = false;
            return;
        }

        cachedMaterial = targetRenderer.material;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (cachedMaterial == null)
        {
            return;
        }

        if (collision.gameObject.CompareTag("yellow"))
        {
            cachedMaterial.color = new Color(1f, 215f / 255f, 0f);
        }
        else if (collision.gameObject.CompareTag("blue"))
        {
            cachedMaterial.color = new Color(0f, 45f / 255f, 1f);
        }
        else if (collision.gameObject.CompareTag("red"))
        {
            cachedMaterial.color = new Color(1f, 0f, 0f);
        }
    }
}