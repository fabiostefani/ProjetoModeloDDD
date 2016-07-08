﻿using System.Collections.Generic;
using ProjetoModeloDDD.Domain.Entities;
using System.Linq;
using ProjetoModeloDDD.Domain.Interfaces.Repositories;

namespace ProjetoModeloDDD.Infra.Data.Repositories
{
    public class ProdutoRepository : RepositoryBase<Produto>, IProdutoRepository
    {
        public IEnumerable<Produto> BuscarPorNome(string nome)
        {
            return Db.Produtos.Where(p => p.Nome.ToUpper() == nome.ToUpper());
        }
    }
}
