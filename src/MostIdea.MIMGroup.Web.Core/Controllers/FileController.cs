using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AspNetZeroCore.Net;
using Abp.Auditing;
using Abp.Extensions;
using Abp.MimeTypes;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.Web.Controllers
{
    public class FileController : MIMGroupControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IMimeTypeMap _mimeTypeMap;
        private readonly ILogger _logger;

        public FileController(ITempFileCacheManager tempFileCacheManager, IBinaryObjectManager binaryObjectManager, IMimeTypeMap mimeTypeMap, ILogger logger)
        {
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
            _mimeTypeMap = mimeTypeMap;
            _logger = logger;
        }


        [HttpGet]
        public async Task<FileResult> Get(Guid RowId, string TableName, string mimeType = MimeTypeNames.ImagePng)
        {
            var output = await _binaryObjectManager.GetFileWithRowId(RowId, TableName);
            if (output == null)
            {
                _logger.Warn(ControllerContext.HttpContext?.Request?.Body?.ToString());
                return File(Path.Combine("Common", "Images", "default-picture.png"), MimeTypeNames.ImagePng);
            }
            return File(output.Bytes, mimeType);
        }

        [HttpGet]
        public async Task<FileResult> Get(Guid Id, string mimeType = MimeTypeNames.ImagePng)
        {
            var output = await _binaryObjectManager.GetOrNullAsync(Id);
            if (output == null)
            {
                _logger.Warn(ControllerContext.HttpContext?.Request?.Body?.ToString());
                return File(Path.Combine("Common", "Images", "default-picture.png"), MimeTypeNames.ImagePng);
            }
            return File(output.Bytes, mimeType);
        }

        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
            if (fileBytes == null)
            {
                return NotFound(L("RequestedFileDoesNotExists"));
            }

            return File(fileBytes, file.FileType, file.FileName);
        }

        [DisableAuditing]
        public async Task<ActionResult> DownloadBinaryFile(Guid id, string contentType, string fileName)
        {
            var fileObject = await _binaryObjectManager.GetOrNullAsync(id);
            if (fileObject == null)
            {
                return StatusCode((int) HttpStatusCode.NotFound);
            }

            if (fileName.IsNullOrEmpty())
            {
                if (!fileObject.Description.IsNullOrEmpty() &&
                    !Path.GetExtension(fileObject.Description).IsNullOrEmpty())
                {
                    fileName = fileObject.Description;
                }
                else
                {
                    return StatusCode((int) HttpStatusCode.BadRequest);
                }
            }

            if (contentType.IsNullOrEmpty())
            {
                if (!Path.GetExtension(fileName).IsNullOrEmpty())
                {
                    contentType = _mimeTypeMap.GetMimeType(fileName);
                }
                else
                {
                    return StatusCode((int) HttpStatusCode.BadRequest);
                }
            }

            return File(fileObject.Bytes, contentType, fileName);
        }
    }
}
