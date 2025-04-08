using Microsoft.AspNetCore.Mvc;
using Rookies_ASP.NETCoreAPI.API.Dtos.RequestDtos;
using Rookies_ASP.NETCoreAPI.API.Dtos.ResponseDtos;
using Rookies_ASP.NETCoreAPI.API.Services;
using Rookies_ASP.NETCoreAPI.Common;

namespace Rookies_ASP.NETCoreAPI.API.Controllers
{
    [Route("tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        [HttpGet]
        public ApiResponse GetTasks()
        {
            return new ApiResponse
            {
                Message = "Get Tasks Successfully!",
                StatusCode = 200,
                Data = _taskService.GetTasks()
            };
        }
        [HttpGet("{id}")]
        public ApiResponse GetTask(Guid id)
        {
            var task = _taskService.GetTask(id);
            if (task != null)
                return new ApiResponse
                {
                    StatusCode = 200,
                    Message = "Get Task Successfully!",
                    Data = _taskService.GetTasks()
                };
            return new ApiResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Get Task Failed!"
            };
        }
        [HttpPost]
        public ApiResponse Add(RequestTaskDto taskDto)
        {
            int status = _taskService.Add(taskDto);
            if (status == ConstantsStatus.Success)
            {
                return new ApiResponse
                {                    
                    StatusCode = 200,
                    Message = "Add Task Successfully!",
                    Data = taskDto
                };
            }
            return new ApiResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Add Task Failed!"
            };
        }
        [HttpPut("{id}")]
        public ApiResponse Update(Guid id, [FromBody] RequestTaskDto taskDto)
        {
            int status = _taskService.Update(id, taskDto);
            if (status == ConstantsStatus.Success)
            {
                return new ApiResponse
                {
                    StatusCode = 200,
                    Message = "Update Task Successfully!",
                    Data = taskDto
                };
            }
            return new ApiResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Update Task Successfully!"
            };
        }
        [HttpDelete("{id}")]
        public ApiResponse Delete(Guid id)
        {
            int status = _taskService.Delete(id);
            if (status == ConstantsStatus.Success)
            {
                return new ApiResponse
                {      
                    StatusCode = 200,
                    Message = "Delete Tasks Successfully!",
                    Data = _taskService.Delete(id)
                };
            }
            return new ApiResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Delete Tasks Failed!"
            };
        }
        [HttpPost("/bulk")]
        public ApiResponse BulkAdd(List<RequestTaskDto> taskDtos)
        {
            int status = _taskService.BulkAdd(taskDtos);
            if (status == ConstantsStatus.Success)
            {
                return new ApiResponse
                {
                    StatusCode = 200,
                    Message = "Bulk Add Tasks Successfully!",
                    Data = taskDtos
                };
            }
            return new ApiResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Bulk Add Tasks Failed!"
            };
        }
        [HttpDelete("/bulk")]
        public ApiResponse BulkDelete(List<Guid> ids)
        {
            int status = _taskService.BulkDelete(ids);
            if (status == ConstantsStatusBulkDelete.AllRemoved)
            {
                return new ApiResponse
                {
                    StatusCode = 200,
                    Message = "Bulk Delete Tasks Successfully!",
                    Data = _taskService.BulkDelete(ids)
                };
            }
            else if (status == ConstantsStatusBulkDelete.OnlyValidRemoved)
            {
                return new ApiResponse
                {
                    Message = "Bulk Delete Tasks Successfully! Bulk Delete Only Existed Ids"
                };
            }
            return new ApiResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Bulk Delete Tasks Failed!"
            };
        }
    }
}
