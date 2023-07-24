using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDEnemyScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Sprite[] spriteArray;
    [SerializeField] private AudioClip pop;

    private int spriteIdx;
    private float speed = 7f;
    private bool hasDied = false;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        // Disable animator until animation because it will overwrite the balloon sprite
        animator.enabled = false;
        // Choose a sprite for the spawned enemy
        spriteIdx = Random.Range(0, spriteArray.Length);
        GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIdx];
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasDied)
        {
            // Move enemy towards player in a line
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    public void SetSpeed(float spd)
    {
        speed = spd;
    }

    public void Die()
    {
        hasDied = true;
        GetComponent<BoxCollider2D>().enabled = false;
        AudioSource.PlayClipAtPoint(pop, transform.position);
        animator.enabled = true;
        Destroy(gameObject, 0.25f);
    }
}
