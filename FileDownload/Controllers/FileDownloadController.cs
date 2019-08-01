using System.Collections.Generic;
using FileDownload.Helpers;
using FileDownload.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FileDownload.Controllers
{
    [Route("api/file-download")]
    [EnableCors]
    [ApiController]
    public class FileDownloadController : ControllerBase
    {
        AttachmentHelper _attachmentHelper;

        public FileDownloadController()
        {
            _attachmentHelper = new AttachmentHelper();
        }

        [HttpGet]
        [Route("attachments")]
        [EnableCors]
        public ActionResult<IEnumerable<FileDetails>> Get()
        {
            return Ok(_attachmentHelper.GetAttchaments());
        }

        /// <summary>
        /// Get individual file content
        /// </summary>
        /// <param name="attachmentIds"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("attachments/download")]
        public ActionResult<AttachmentModel> Get([FromQuery]List<int> attachmentIds)
        {
            return Ok(_attachmentHelper.DownloadAttchments(attachmentIds));
        }

        /// <summary>
        /// Get files content as zip
        /// </summary>
        /// <param name="attachmentIds"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("attachments/downloadzip")]
        public ActionResult<AttachmentModel> GetZip([FromQuery]List<int> attachmentIds)
        {
            return Ok(_attachmentHelper.DownloadAttchmentsAsZip(attachmentIds));
        }
    }
}
