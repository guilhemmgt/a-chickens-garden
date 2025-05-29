using UnityEngine;

public interface ISkill
{
    string Name { get; }
    string Description { get; }
    void OnDayEnd() { }
    void OnMature() { }
    void OnRemoved() { }
    void SetOwner(Plant plant);
}
