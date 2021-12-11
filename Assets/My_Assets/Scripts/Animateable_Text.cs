using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(Animator))]
public class Animateable_Text : MonoBehaviour
{
    Animator animator;
    private Text text;
    private int _val;
    private void Start()
    {
        text = GetComponent<Text>();
        animator = GetComponent<Animator>();
    }
    public int Value
    {
        get { return _val; }
        set { 
            _val = value;

            text.text = Value.ToString();
            animator.SetTrigger("scale");
        }
    }
    
   
}

 