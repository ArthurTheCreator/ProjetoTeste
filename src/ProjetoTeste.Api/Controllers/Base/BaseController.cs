﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjetoTeste.Arguments.Arguments.Base;
using ProjetoTeste.Infrastructure.Interface.Service.Base;
using ProjetoTeste.Infrastructure.Interface.UnitOfWork;
using ProjetoTeste.Infrastructure.Persistence.Entity.Base;

namespace ProjetoTeste.Api.Controllers.Base;

[Route("api/v1/[controller]")]
[ApiController]
public abstract class BaseController<TService, TEntity, TInputCreateDTO, TInputIndetityUpdate, TInputIndetityDelete, TInputIndeityViewDTO, TOutputDTO> : Controller
    where TService : IBaseService<TEntity, TInputCreateDTO, TInputIndetityUpdate, TInputIndetityDelete, TInputIndeityViewDTO, TOutputDTO>
    where TEntity : BaseEntity
    where TInputCreateDTO : BaseInputCreate<TInputCreateDTO>
    where TInputIndetityUpdate : BaseInputIdentityUpdate<TInputIndetityUpdate>
    where TInputIndetityDelete : BaseInputIdentityDelete<TInputIndetityDelete>
    where TInputIndeityViewDTO : BaseInputIdentityView<TInputIndeityViewDTO>, IBaseIdentity
    where TOutputDTO : BaseOutput<TOutputDTO>
{
    #region Dependecy Injection
    private readonly IUnitOfWork unitOfWork;
    private readonly TService service;

    public BaseController(IUnitOfWork unitOfWork, TService service)
    {
        this.unitOfWork = unitOfWork;
        this.service = service;
    }
    #endregion

    #region Transacation
    public override void OnActionExecuting(ActionExecutingContext context) //overide substitui o comportamento padrão do controller
    {
        unitOfWork.BeginTransaction();
        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        unitOfWork.Commit();
        base.OnActionExecuted(context);
    }
    #endregion

    #region Get
    [HttpGet("Get/All")]
    public virtual async Task<ActionResult<List<TOutputDTO>>> GetAll()
    {
        var getAll = await service.GetAll();
        return Ok(getAll);
    }

    [HttpPost("Get/Id")]
    public virtual async Task<ActionResult<TOutputDTO>> Get(TInputIndeityViewDTO inputIdentifyView)
    {
        var get = await service.Get(inputIdentifyView);
        return Ok(get);
    }

    [HttpPost("Get/ListByListId")]
    public virtual async Task<ActionResult<TOutputDTO>> GetListByListId(List<TInputIndeityViewDTO> listInputIdentifyView)
    {
        var getListByListId = await service.GetListByListId(listInputIdentifyView);
        return Ok(getListByListId);
    }
    #endregion

    #region Create
    [HttpPost("Create")]
    public virtual async Task<ActionResult<BaseResponse<List<TOutputDTO>>>> Create(TInputCreateDTO inputCreateDTO)
    {
        var create = await service.Create(inputCreateDTO);
        if (create.Success == false)
        {
            return BadRequest(create);
        }
        return Ok(create);
    }

    [HttpPost("Create/Multiple")]
    public virtual async Task<ActionResult<BaseResponse<List<TOutputDTO>>>> Create(List<TInputCreateDTO> listTInputCreateDTO)
    {
        var create = await service.CreateMultiple(listTInputCreateDTO);
        if (create.Success == false)
        {
            return BadRequest(create);
        }
        return Ok(create);
    }
    #endregion

    #region Update
    [HttpPut("Update")]
    public virtual async Task<ActionResult<BaseResponse<bool>>> Update(TInputIndetityUpdate inputIdentityUpdateDTO)
    {
        var update = await service.Update(inputIdentityUpdateDTO);
        if (!update.Success)
        {
            return BadRequest(update);
        }
        return Ok(update);
    }

    [HttpPut("Update/Multiple")]
    public virtual async Task<ActionResult<BaseResponse<bool>>> Update(List<TInputIndetityUpdate> listTInputIndetityUpdate)
    {
        var listUpdate = await service.UpdateMultiple(listTInputIndetityUpdate);
        if (!listUpdate.Success)
        {
            return BadRequest(listUpdate);
        }
        return Ok(listUpdate);
    }
    #endregion

    #region Delete
    [HttpDelete("Delete")]
    public virtual async Task<ActionResult<BaseResponse<bool>>> Delete(TInputIndetityDelete inputIdentifyDeleteDTO)
    {
        var delete = await service.Delete(inputIdentifyDeleteDTO);
        if (!delete.Success)
        {
            return BadRequest(delete);
        }
        return Ok(delete);
    }

    [HttpDelete("Delete/Multiple")]
    public virtual async Task<ActionResult<BaseResponse<bool>>> Delete(List<TInputIndetityDelete> listInputIdentifyDeleteDTO)
    {
        var listDelete = await service.DeleteMultiple(listInputIdentifyDeleteDTO);
        if (!listDelete.Success)
        {
            return BadRequest(listDelete);
        }
        return Ok(listDelete);
    }
    #endregion
}