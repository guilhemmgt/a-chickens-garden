using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[Serializable]
public class Effect
{
    public enum Flag { SnailProtection, Score10, Score20, Score40, Score100, ScoreM10, ScoreM5}
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
        Debug.Log("---------------------");
        Debug.Log(owner);
        Debug.Log(owner.GetOwner());
        ownerPlant = owner.GetOwner().name;
        ownerPlot = owner.GetOwner().plot.name;
        foreach (Plot dep in dependencies)
        {
            dep.OnPlantExit += (_) => owner.RemoveEffect(this);
        }
    }

}
