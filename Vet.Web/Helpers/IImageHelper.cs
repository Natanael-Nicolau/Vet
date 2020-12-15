using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Helpers
{
    public interface IImageHelper
    {
        /// <summary>
        /// Uploads an image file to a specified server folder
        /// </summary>
        /// <param name="imageFile"></param>
        /// <param name="folder"></param>
        /// <returns>path to the image in the server</returns>
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);
    }
}
