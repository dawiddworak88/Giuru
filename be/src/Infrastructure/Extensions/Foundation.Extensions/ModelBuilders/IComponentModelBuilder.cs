namespace Foundation.Extensions.ModelBuilders
{
    public interface IComponentModelBuilder<in T, out S> where T : class where S : class
    {
        S BuildModel(T componentModel);
    }
}
