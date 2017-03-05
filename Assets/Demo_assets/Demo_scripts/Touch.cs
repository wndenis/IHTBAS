using UnityEngine;
using System.Collections;

public class Touch : MonoBehaviour
{
    private Controls player;


    void Start()
    {
        player = FindObjectOfType<Controls>();
    }

    public void Left()
    {
        //player.moving = "left";
    }
    public void Right()
    {
        //player.moving = "right";
    }
    public void stop()
    {
        //player.moving = "";
    }
    public void Jump()
    {
        player.makeJump(player.jumpheight);
    }

    public void Z()
    {
        player.switchMode('z');
    }
    public void X()
    {
        player.switchMode('x');
    }
    public void C()
    {
        player.switchMode('c');
    }
    public void V()
    {
        player.switchMode('v');
    }

}
