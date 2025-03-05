using UnityEngine;
using Zenject;

public class DependencyInstaller : MonoInstaller
{
    [SerializeField] private DynamicJoystick dynamicJoystick;
    [SerializeField] private CharacterController characterController;

    public override void InstallBindings()
    {
        BindBag();
        BindListPool();
        BindDynamicJoystick();
        BindCharacterController();
    }

    private void BindBag()
    {
        Container.Bind<Bag>()
                 .FromComponentsInHierarchy()
                 .AsSingle();
    }

    private void BindListPool()
    {
        Container.Bind<ListPool<Item>>()
                 .FromNew()
                 .AsSingle();
    }

    private void BindDynamicJoystick()
    {
        Container.Bind<DynamicJoystick>()
                 .FromInstance(dynamicJoystick)
                 .AsSingle();
    }

    private void BindCharacterController()
    {
        Container.Bind<CharacterController>()
                 .FromInstance(characterController)
                 .AsSingle();
    }
}