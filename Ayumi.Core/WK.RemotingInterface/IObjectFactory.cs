namespace WK.RemotingInterface
{
    public interface IObjectFactory
    {
        object CreateObject(string assemblyName, string fqn);
    }
}
