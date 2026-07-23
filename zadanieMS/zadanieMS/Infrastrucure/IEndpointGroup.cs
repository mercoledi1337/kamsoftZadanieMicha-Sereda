namespace zadanieMS.Infrastrucure
{
    public interface IEndpointGroup
    {
        static virtual string? RoutePrefix => null;

        static abstract void Map(RouteGroupBuilder groupBuilder);
    }
}
