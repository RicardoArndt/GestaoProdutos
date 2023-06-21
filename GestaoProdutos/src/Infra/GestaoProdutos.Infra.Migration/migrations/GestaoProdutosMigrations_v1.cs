using FluentMigrator;

namespace GestaoProdutos.Infra.Migration.migrations;

[FluentMigrator.Migration(1)]
public class GestaoProdutosMigrations_v1 : FluentMigrator.Migration
{
    public override void Up()
    {
    	Create.Table("Produtos")
	        .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("Codigo").AsInt32().Identity().Unique().NotNullable()
            .WithColumn("Descricao").AsString(20).NotNullable()
            .WithColumn("Situacao").AsBoolean().NotNullable()
            .WithColumn("DataFabricacao").AsDate().NotNullable()
            .WithColumn("DataValidade").AsDate()
            .WithColumn("FornecedorId").AsGuid().ForeignKey().NotNullable();

    	Create.Table("Fornecedores")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("Codigo").AsInt32().Identity().Unique().NotNullable()
            .WithColumn("Descricao").AsString(20).NotNullable()
            .WithColumn("Cnpj").AsString(14).Unique().NotNullable();

        Create.ForeignKey("FK_Produto_Fornecedor")
            .FromTable("Produtos").ForeignColumn("FornecedorId")
            .ToTable("Fornecedores").PrimaryColumn("Id");
        
        InsertInitialData();
    }

    public override void Down()
    {
        Delete.Table("Produtos");
    }

    private void InsertInitialData()
    {
        // Inclusao de Fornecedores
        Guid fornecedorId1 = Guid.NewGuid();
        InsertFornecedor(fornecedorId1, "Ricardo LTDA", "12345678912345");
       
        Guid fornecedorId2 = Guid.NewGuid();
        InsertFornecedor(fornecedorId2, "Bruno LTDA", "12345678912346");

        // Inclusao de Produtos
        InsertProduto(
            Guid.NewGuid(),
            "Guaraná", 
            true, 
            DateTime.Now, 
            DateTime.Now.AddYears(1), 
            fornecedorId1);
        
        InsertProduto(
            Guid.NewGuid(),
            "Azeitona", 
            false, 
            DateTime.Now, 
            DateTime.Now.AddMonths(1), 
            fornecedorId2);
    }
    
    private void InsertProduto(
        Guid id,
        string descricao, 
        bool situacao, 
        DateTime dataFabricacao, 
        DateTime dataValidade, 
        Guid fornecedorId)
    {
        Insert.IntoTable("Produtos").Row(new
        {
            Id = id,
            Descricao = descricao,
            Situacao = situacao,
            DataFabricacao = dataFabricacao,
            DataValidade = dataValidade,
            FornecedorId = fornecedorId
        });
    }        

    private void InsertFornecedor(
        Guid id,
        string descricao, 
        string cnpj)
    {
        Insert.IntoTable("Fornecedores").Row(new
        {
            Id = id,
            Descricao = descricao,
            Cnpj = cnpj
        });
    }        
}