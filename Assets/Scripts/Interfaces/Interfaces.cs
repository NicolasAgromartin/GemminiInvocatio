using System.Threading.Tasks;
using UnityEngine;



public interface ICommand
{
    public Task Execute();
}

public interface IInteractable
{
    public void Interact(GameObject interactor);
}
public interface IDisectable
{
    public void Disect(GameObject unit);
}
public interface IDamageable
{
    public void RecieveDamage(int damage);
}
public interface IPlayable
{
    public int Lives { get; protected set; }

    public void LostLife();
    public void GainLife();
}
