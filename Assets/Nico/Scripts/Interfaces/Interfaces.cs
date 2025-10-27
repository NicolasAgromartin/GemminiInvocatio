using UnityEngine;




public interface IInteractable
{
    public void Interact(GameObject interactor);
}


public interface IDamageable
{
    protected virtual void RecieveDamage(int damage) { }
}