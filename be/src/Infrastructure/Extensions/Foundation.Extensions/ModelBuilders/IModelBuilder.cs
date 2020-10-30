namespace Foundation.Extensions.ModelBuilders
{
    public interface IModelBuilder<out T> where T : class
    {
        T BuildModel();
    }
}
