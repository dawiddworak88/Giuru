namespace Foundation.Extensions.ModelBuilders
{
    public interface IModelBuilder<T> where T : class
    {
        T BuildModel();
    }
}
