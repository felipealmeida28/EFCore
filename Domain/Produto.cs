using EF_Dev.io.ValueObjets;

namespace EF_Dev.io.Domain;

public class Produto
{
    public int Id { get; set; }
    public string CodigoBarras { get; set; }
    public string Descricao { get; set; }
    public Decimal Valor { get; set; }
    public TipoProduto TipoProduto { get; set; }
    public bool Ativo { get; set; }
}