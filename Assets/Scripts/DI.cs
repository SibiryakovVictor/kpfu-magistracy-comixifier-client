using Comixification.ApiClient.V1;
using Comixification.Comixifier;
using Comixification.Command.ComixifyImage;
using TopRightMenu.Events;
using Zenject;

public class DI : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<VanceAI>().AsSingle();
        Container.Bind<Face2Comics>().AsSingle();
        Container.Bind<CutOut>().AsSingle();
        
        Container.Bind<EventBus>().AsSingle();

        // Container.Bind<MapperFactory>().AsSingle();
        // Container.Bind<ParametersPreparer>().AsSingle().NonLazy();
        
        Container.Bind<ApiClient>().AsSingle();
        Container.Bind<ComixifyImageHandler>().AsSingle();
        Container.Bind<ComixifyImageInterceptor>().AsSingle().NonLazy();
    }
}
