using Scrum.Api.Exceptions;

namespace Scrum.Api.Application.Commands;

public class UseCaseBase<TEntity>(ScrumDbContext dbContext) where TEntity : class
{
    protected readonly ScrumDbContext DbContext = dbContext;
    protected CancellationToken Ct { get; private set; } = CancellationToken.None;

    private TEntity? _entity;
    protected TEntity Entity => _entity ?? throw new InvalidOperationException(NotLoadedMessage);

    const string NotLoadedMessage = $"{nameof(TEntity)} is not loaded.";

    protected async Task InitializeAndLoadAsync(Guid id, CancellationToken ct)
    {
        if (!await InitializeAndTryLoadAsync(id, ct))
            throw new ScrumDomainException($"{nameof(TEntity)} not found.");
    }
    protected async Task<bool> InitializeAndTryLoadAsync(Guid id, CancellationToken ct)
    {
        Ct = ct;
        _entity = await DbContext.Set<TEntity>().FindAsync(new object[] { id }, Ct);
        return _entity is not null;
    }

    protected ValueTask<TEntity?> FindAsync(Guid id, CancellationToken ct)
        => DbContext.Set<TEntity>().FindAsync(new object[] { id }, ct);

    protected async ValueTask<TEntity> FindOrThrowAsync(Guid id, CancellationToken ct)
    {
        var entity = await FindAsync(id, ct)
            ?? throw new ScrumDomainException($"{nameof(TEntity)} not found.");
        return entity;

    }
}