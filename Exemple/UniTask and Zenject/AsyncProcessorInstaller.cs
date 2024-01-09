using Zenject;

public class AsyncProcessorInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<AsyncProcessor>().AsSingle();
    }
}
