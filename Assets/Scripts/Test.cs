using UnityEngine;

public class Test : MonoBehaviour
{
    private Animator animator;
    public GameObject web;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            web.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            if(hit.collider.CompareTag("Wall"))
            {
                animator.SetTrigger("Shoot");
            }
        }
    }
}
