using Repository.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUrlClickService
    {
        Task<IEnumerable<UrlClickEntity>> GetAll();
        Task<UrlClickEntity> Add(string url, string os, string browser);
    }
}
