using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Properties;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Repository
{

    public class AnswerRepository : GenericRepository<AnswersEntity>
    {
        public List<AnswersEntity> GetAllFromFirm(int firmId, string search,int pageIndex,int pageSize)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = (from q in context.AnswersEntity where (q.FirmId == firmId && (q.Name.Contains(search) || q.Answer.Contains(search))) select q).OrderBy(x => x.Name).Skip(pageIndex * pageSize).Take(pageSize);

                return query.ToList();
            }
        }
        public int GetCountFromFirm(int firmId, string search)
        {
            using (ModelContext context = new ModelContext())
            {
                if (search != null)
                {
                    return (from q in context.AnswersEntity where (q.FirmId == firmId && q.Name.Contains(search)) select q).Count();
                }
                return (from q in context.AnswersEntity where (q.FirmId == firmId) select q).Count();
            }
        }
    }
}
