﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Interface
{
    public interface IResponseCasheService
    {
        Task CasheResponseAsync(string cashekey,object response,TimeSpan timeToLive );
        Task<string> GetCashedResponse(string cashekey);
    }
}
