using UnityEngine;

public interface IExplodable
{
    float Adjust(float health);
    void Explode();
}