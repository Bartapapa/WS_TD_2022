using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler_NorthPole : AnimatorHandler_Tower
{
    public void RPGAnim()
    {
        _anim.SetTrigger("RPG");
    }

    public void GrenadeAnim()
    {
        _anim.SetTrigger("Grenade");
    }

    public void VictoryAnim()
    {
        _anim.SetTrigger("Victory");
    }

    public void DefeatAnim()
    {
        _anim.SetTrigger("Defeat");
    }
}
