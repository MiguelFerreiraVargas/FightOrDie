using UnityEngine;
using UnityEngine.InputSystem;

public class DragonAnimation : MonoBehaviour
{
    public Animator animator;
    public string nomeAnimacao = "NomeDaAnimacao";

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            animator.Play(nomeAnimacao, 0, 0f);
        }
    }
}