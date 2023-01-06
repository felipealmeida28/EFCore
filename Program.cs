using EF_Dev.io.Data;
using EF_Dev.io.Domain;
using EF_Dev.io.ValueObjets;
using Microsoft.EntityFrameworkCore;

namespace EF_Dev.io
{
    class Program
    {
        static void Main()
        {
            RemoverRegistro();

            //AtualizarDados();
            
            Console.WriteLine("deu bom");
        }

        private static void RemoverRegistro()
        {
            using var db = new ApplicationContext();

            var cliente = db.Clientes.Find(1);

            db.Remove(cliente);
            db.SaveChanges();


        }

        private static void AtualizarDados()
        {
            using var db = new ApplicationContext();

            var cliente = db.Clientes.Find(1);

            cliente.Nome = "Cliente foi alteradoo";

            db.SaveChanges();
            
        }
        
        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new ApplicationContext();

            var pedidos = db.Pedidos
                .Include(p => p.Itens)
                .ThenInclude(p => p.Produto)
                .ToList();

            Console.WriteLine(pedidos.Count);
        }
        
        private static void CadastrarPedido()
        {
            using var db = new ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now.AddMinutes(30),
                Observacao = "Pedido teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10,
                    }
                }
            };

            db.Pedidos.Add(pedido);
            db.SaveChanges();
        }
        
        private static void ConsultarDados()
        {
            using var db = new ApplicationContext();


            var produtos = db.Produtos.AsNoTracking().ToList();

            foreach (var produto in produtos)
            {
                Console.WriteLine(produto.Id);
            }

        }
        
        private static void IserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Felipe Almeida",
                CEP = "01233000",
                Cidade = "São Paulo",
                Estado = "SP",
                Telefone = "99000001111"
            };
            using var db = new ApplicationContext();
            
            db.AddRange(produto, cliente);

            var registros = db.SaveChanges();

            Console.WriteLine("registros afetados: " + registros);


        }

        private static void IserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };
            
            
            using var db = new ApplicationContext();

            db.Produtos.Add(produto);
            var num = db.SaveChanges();
            
            Console.WriteLine("Linhas Afetadas: " + num);
        }
    }
    
}