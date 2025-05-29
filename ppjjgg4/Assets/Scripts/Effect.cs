using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Effect
{
    public enum Flag { SnailProtection, Score1}
    public Flag flag;
    public ISkill owner;
    public string ownerName;
    public string ownerPlant;
    public string ownerPlot;
    private List<Plot> dependencies;

    public Effect(Flag flag, ISkill owner, List<Plot> dependencies)
    {
        this.flag = flag;
        this.owner = owner;
        this.dependencies = dependencies;
        ownerName = owner.Name;
        ownerPlant = owner.GetOwner().name;
        ownerPlot = owner.GetOwner().plot.name;
    }

}
