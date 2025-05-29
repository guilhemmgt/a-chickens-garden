using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Unity.Mathematics;
using Unity.XR.GoogleVr;
using UnityEngine;


[CreateAssetMenu(fileName = "Plant", menuName = "Garden/Plant")]
public class Plant : ScriptableObject
{

    [TextArea(3, 5), SerializeField] private string description;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private int score = 1;
    [SerializeField] private int growthTime = 3;
    [field : SerializeField] public Plot plot { get; private set; }
    [SerializeReference, SubclassPicker] ISkill skill;
    public int day;
    public bool hasMatured;

    public void OnPlanted(Plot plot)
    {
        day = 0;
        this.plot = plot;
        hasMatured = false;
        skill.SetOwner(this);
    }

    public void OnMature()
    {
        hasMatured = true;
        skill.OnMature();
    }

    public void OnRemoved()
    {
        skill.OnRemoved();
    }

    public void EndDay()
    {
        day++;
        if (!hasMatured && day >= growthTime) OnMature();
        skill.OnDayEnd();
    }

    public Sprite Sprite => sprites[Mathf.Min(sprites.Count - 1, day)];


}
