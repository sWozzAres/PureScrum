namespace ScrumApp.Templates;

public class CreatedEventArgs<TModel>(HttpResponseMessage response, bool open, TModel model) where TModel : class, new()
{
    public HttpResponseMessage Response { get; private set; } = response;
    public bool Open { get; private set; } = open;
    public TModel Model { get; set; } = model;
}
