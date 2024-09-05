using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IRequestDocumentService
    {
        List<RequestDocument> GetAllRequestDocuments();
        RequestDocumentModel CreateRequestDocument(RequestDocumentModel model);
        Task<bool> UpdateRequestDocument(RequestDocumentModel model);
        bool DeleteRequestDocument(int id);
        List<RequestDocumentModel> GetRequestDocuments(long requestId);
        RequestDocumentModel GetRequestDocument(long requestId);
    }
}
