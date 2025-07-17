using UnityEngine;

public class WindController : MonoBehaviour
{
    public ParticleSystem windBurst;

    public void PlayWindBurst()
    {
        if (windBurst != null)
        {
            windBurst.Play();
        }
    }
}
