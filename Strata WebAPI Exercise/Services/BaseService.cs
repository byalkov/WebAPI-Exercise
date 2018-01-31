using Strata_WebAPI_Exercise.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Strata_WebAPI_Exercise.Services
{
    public abstract class BaseService
    {
        protected readonly IRepositoryService _repositoryService;

        protected BaseService(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }
    }
}