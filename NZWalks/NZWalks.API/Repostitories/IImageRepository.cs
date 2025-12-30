using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repostitories
{
    public interface IImageRepository
    {
        Task<Image> UploadImage(Image image);
    }
}
