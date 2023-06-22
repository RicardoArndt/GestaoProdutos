namespace GestaoProdutos.Infra.Database.Abstraction;

public interface IContext
{
    Task ExecuteAsync(string sql, object? parameters = null);

    Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(
        string query,
        Func<TFirst, TSecond, TReturn> map,
        object? parameters = null,
        string splitOn = "Id");

    Task<TReturn> QueryFirstOrDefaultAsync<TReturn>(string query, object? parameters = null);
}