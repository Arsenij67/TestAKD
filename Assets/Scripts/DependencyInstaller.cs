using UnityEngine;
using Zenject;

public class DependencyInstaller : MonoInstaller
{
    [SerializeField] private DynamicJoystick DynamicJoystick;
    [SerializeField] private CharacterController charController;
    public override void InstallBindings()
    {
        // Остальные привязки
        Container.Bind<Bag>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<ListPool<Item>>().FromNew().AsSingle();
        Container.Bind<DynamicJoystick>().FromInstance(DynamicJoystick).AsSingle();
        Container.Bind<CharacterController>().FromInstance(charController).AsSingle();

    }

}