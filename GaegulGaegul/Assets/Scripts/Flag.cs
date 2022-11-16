using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Flag : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private string next_scene_name;
    private Vector3 halfSize;
    private LayerMask frogLayer;
    private bool isPlay = false;
    
    void Start()
    {
        frogLayer = 1 << LayerMask.NameToLayer("Frog");
        halfSize = transform.lossyScale * 0.5f;
        isPlay = false;
    }

        private void FixedUpdate()
    {
        Collider2D frogCollider = Physics2D.OverlapArea(transform.position - halfSize, transform.position + halfSize, frogLayer);
        if(frogCollider && !isPlay)
        {
            particles.Play();
            isPlay = true;
            StartCoroutine(NextScene());
        }
    }
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(2);
        if (next_scene_name != "" && !SceneManager.GetSceneByName(next_scene_name).IsValid())
            SceneManager.LoadScene(next_scene_name);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
            
    }
}
