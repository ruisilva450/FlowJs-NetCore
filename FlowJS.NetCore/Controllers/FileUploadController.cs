using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ngFlowSample.Core.Services;
using ngFlowSample.Core.Utilities;
using NgFlowSample.Models;
using System.Threading.Tasks;

namespace ngFlowSample.Core.Controllers
{
    [Route("api/Upload")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        IWebHostEnvironment environment;

        public FileUploadController(IWebHostEnvironment _environment)
        {
            this.environment = _environment;
        }

        // POST: api/Upload
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return StatusCode(StatusCodes.Status415UnsupportedMediaType);
            }

            var uploadProcessor = new FlowUploadProcessorCore(environment, "Tmp/FileUploads");

            await uploadProcessor.ProcessUploadChunkRequest(Request.ToHttpRequestMessage());

            if (uploadProcessor.IsComplete)
            {
                // Do post processing here:
                // - Move the file to a permanent location
                // - Persist information to a database
                // - Raise an event to signal it was completed (if you are really feeling up to it)
                //      - http://www.udidahan.com/2009/06/14/domain-\events-salvation/
                //      - http://msdn.microsoft.com/en-gb/magazine/ee236415.aspx#id0400079
            }

            return Ok();
        }

        // GET: api/Upload
        [HttpGet]
        public IActionResult Get([FromQuery]FlowMetaData flowMeta)
        {
            if (FlowUploadProcessorCore.HasRecievedChunk(flowMeta))
            {
                return Ok();
            }

            return NoContent();
        }
    }
}
