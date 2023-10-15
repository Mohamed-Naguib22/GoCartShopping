using Go_Cart.Models;

namespace Go_Cart.Interfaces
{
    public interface IImageService<T> where T : class
    {
        void SaveImage(IFormFile? imgFile, T model);
        void SetOptionalImage(IFormFile? imgFile, int id);
        void DeleteImage(T model);
    }
}
