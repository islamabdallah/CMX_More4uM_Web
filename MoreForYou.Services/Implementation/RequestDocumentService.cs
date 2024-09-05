using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation
{
    public class RequestDocumentService : IRequestDocumentService
    {
        private readonly IRepository<RequestDocument, long> _repository;
        private readonly ILogger<RequestDocumentService> _logger;
        private readonly IMapper _mapper;

        public RequestDocumentService(IRepository<RequestDocument, long> repository,
          ILogger<RequestDocumentService> logger,
          IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
           
        }
        public RequestDocumentModel CreateRequestDocument(RequestDocumentModel model)
        {
            try
            {
                RequestDocument requestDocument = _mapper.Map<RequestDocument>(model);
                var addedRequestDocument = _repository.Add(requestDocument);
                if (addedRequestDocument != null)
                {
                    RequestDocumentModel addedRequestDocumentModel = new RequestDocumentModel();
                    addedRequestDocumentModel = _mapper.Map<RequestDocumentModel>(addedRequestDocument);
                    return addedRequestDocumentModel;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;

            }
        }

        public bool DeleteRequestDocument(int id)
        {
            throw new NotImplementedException();
        }

        public List<RequestDocument> GetAllRequestDocuments()
        {
            throw new NotImplementedException();
        }

        public RequestDocumentModel GetRequestDocument(long requestId)
        {
            var document = _repository.Find(r => r.BenefitRequestId == requestId).FirstOrDefault();

            RequestDocumentModel requestDocumentModel = _mapper.Map<RequestDocumentModel>(document);
           return requestDocumentModel;
        }

        public List<RequestDocumentModel> GetRequestDocuments(long requestId)
        {
           var documents = _repository.Find(r => r.BenefitRequestId == requestId);

            List<RequestDocumentModel> requestDocumentModels = _mapper.Map<List<RequestDocumentModel>>(documents);
            return requestDocumentModels;
        }

        public Task<bool> UpdateRequestDocument(RequestDocumentModel model)
        {
            throw new NotImplementedException();
        }
    }
}
