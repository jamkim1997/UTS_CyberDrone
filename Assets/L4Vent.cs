using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L4Vent : MonoBehaviour
{
    public Text text;

    Player player;
    Animator animator;
    bool existDriver;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, player.GetPosition()) < 1.5f) {
            text.gameObject.SetActive(true);

            if(existDriver && Input.GetKeyDown(KeyCode.E))
            {
                OpenVent();
            }
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }

    public void TakeScrewDriver()
    {
        existDriver = true;
        text.text = "'E' to Open";
    }
    public void OpenVent()
    {
        Destroy(text.transform.parent.gameObject);
        animator.enabled = true;
        Destroy(this);
    }
}