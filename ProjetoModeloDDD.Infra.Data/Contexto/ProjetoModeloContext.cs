using ProjetoModeloDDD.Domain.Entities;
using ProjetoModeloDDD.Infra.Data.EntityConfig;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace ProjetoModeloDDD.Infra.Data.Contexto
{
    public class ProjetoModeloContext : DbContext        
    {
        public ProjetoModeloContext()
            : base("ProjetoModeloDDD")
        {

        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); // Não plurlizar as tabelas
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>(); // Não deletar em cascatas
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>(); // Não deletar relacionamento N para N

            modelBuilder.Properties()
                .Where(p => p.Name == p.ReflectedType.Name + "Id")
                .Configure(p => p.IsKey()); //Aqui configuramos para sempre que achar o nomeId, ele seta que isso é a chave primária da tabela

            modelBuilder.Properties<string>()
                .Configure(p => p.HasColumnType("varchar"));

            modelBuilder.Properties<string>()
                .Configure(p => p.HasMaxLength(100)); // Nos dois comandos acima, digo para o EF que quando tiver um tipo string, 
                                                      //ele deve criar como sendo varchar de tamanho 100.       

            modelBuilder.Configurations.Add(new ClienteConfiguration());
            modelBuilder.Configurations.Add(new ProdutoConfiguration());
        }

        public override int SaveChanges()
        {
            //Aqui sempre quando salvar a entidade, verifico se existe a propriedade data cadastro, se quando estiver inserindo um
            //novo registro, seto a data dela para o datetime now.
            //Setar dados padrões em algmas propriedades.
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }

            return base.SaveChanges();
        }
    }
}
