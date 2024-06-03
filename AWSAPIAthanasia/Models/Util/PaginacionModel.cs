namespace ApiAthanasia.Models.Util
{
    public class PaginacionModel <T>
    {
        public List<T> Lista { get; set; }
        public int NumeroPaginas { get; set; }
    }
}
