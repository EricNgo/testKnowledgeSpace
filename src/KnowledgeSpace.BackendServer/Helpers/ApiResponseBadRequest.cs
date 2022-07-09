using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System;
using System.Linq;

namespace KnowledgeSpace.BackendServer.Helpers
{
    public class ApiResponseBadRequest : ApiResponse

    {
        public IEnumerable<string> Errors { get; }

        public ApiResponseBadRequest(ModelStateDictionary modelState)
            : base(400)
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException("ModelState must be invalid", nameof(modelState));
            }

            Errors = modelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToArray();
        }

        public ApiResponseBadRequest(IdentityResult identityResult)
           : base(400)
        {
            Errors = identityResult.Errors
                .Select(x => x.Code + " - " + x.Description).ToArray();
        }

        public ApiResponseBadRequest(string message)
           : base(400, message)
        {
        }


    }
}
