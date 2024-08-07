public class TheDetectorFactory : KillerFactory
{
    protected override IKiller CreateProduct()
    {
        return new TheDetector();
    }
}
