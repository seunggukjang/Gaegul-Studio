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
    private int frogsCount = 0;
    
    void Start()
    {
        frogLayer = 1 << LayerMask.NameToLayer("Frog");
        halfSize = transform.lossyScale * 0.5f;
        isPlay = false;
        
        Player[] frogs = FindObjectsOfType(typeof(Player), false) as Player[];
        frogsCount = frogs.Length;
        Debug.Log("FrogsCount : " + frogsCount);
    }

    private void FixedUpdate()
    {
        Collider2D[] frogsCollider = Physics2D.OverlapAreaAll(transform.position - halfSize, transform.position + halfSize, frogLayer);
        if (frogsCollider.Length == frogsCount && !isPlay)
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
