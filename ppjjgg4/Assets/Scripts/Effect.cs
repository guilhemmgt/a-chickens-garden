using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Effect
{
    public enum Flag { SnailProtection, Score1}
    public Flag flag;
    public ISkill owner;
    private List<Plot> dependencies;

    public Effect(Flag flag, ISkill owner, List<Plot> dependencies)
    {
        this.flag = flag;
        this.owner = owner;
        this.dependencies = dependencies;
    }

}
