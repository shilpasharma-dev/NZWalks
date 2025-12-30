using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repostitories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IImageRepository imageRepository;

        public ImagesController(IMapper mapper, IImageRepository imageRepository)
        {
            this.mapper = mapper;
            this.imageRepository = imageRepository;
        }


        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> UploadImage([FromForm]UploadFileDto uploadImageDto)
        {
            ValidateFile(uploadImageDto);
               
            if (ModelState.IsValid)
            {
                var image = mapper.Map<Image>(uploadImageDto);
                image.FileExtension = Path.GetExtension(uploadImageDto.File.FileName);
                image.FileSizeInBytes = uploadImageDto.File.Length;

                await imageRepository.UploadImage(image);

                return StatusCode(StatusCodes.Status201Created, image);
            }
            return BadRequest(ModelState);
        }

        private void ValidateFile(UploadFileDto uploadImageDto)
        {
            var allowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            var fileExtension = Path.GetExtension(uploadImageDto.File.FileName);
            if (!allowedFileExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("File", "Invalid file extension. Only .jpg, .jpeg, .png are allowed.");
            }

            if (uploadImageDto.File.Length > 10485760)// 10 MB
            {
                ModelState.AddModelError("File", "File size exceeds the 10 MB limit.");
            }
        }
    }
}
