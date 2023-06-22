using Dapper;
using GestaoProdutos.Infra.Database.Abstraction;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GestaoProdutos.Infra.Database;

internal sealed class SqlContext : IContext
{
    private readonly string _connectionString;

    public SqlContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("GestaoProdutosDB")!;
    }

    public async Task ExecuteAsync(string sql, object? parameters = null)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(sql, parameters);
    }

    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst,TSecond,TReturn>(
        string query, 
        Func<TFirst,TSecond,TReturn> map, 
        object? parameters = null, 
        string splitOn = "Id")
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        return await connection.QueryAsync(query, map, parameters, splitOn: "Id");
    }

    public async Task<TReturn> QueryFirstOrDefaultAsync<TReturn>(string query, object? parameters = null)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        return await connection.QueryFirstOrDefaultAsync<TReturn>(query, parameters);
    }
}