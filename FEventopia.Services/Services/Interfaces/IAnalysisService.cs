using FEventopia.Services.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services.Interfaces
{
    public interface IAnalysisService
    {
        public Task<AnalysisModel> GetEventAnalysis(string eventId);
    }
}
