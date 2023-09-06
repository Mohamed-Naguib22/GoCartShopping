namespace Go_Cart.Interfaces
{
    public interface IDateService<T> where T : class
    {
        void SetDatesToNow(T model);
        void SetUpdateDateToNow(T model);
    }
}
