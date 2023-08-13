using CloudinaryDotNet.Actions;

namespace DatingAppService.API.Interfaces
{
	public interface IPhotoService
	{
		Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
		Task<DeletionResult> DeletePhotoAsync(string publicId);
	}
}
