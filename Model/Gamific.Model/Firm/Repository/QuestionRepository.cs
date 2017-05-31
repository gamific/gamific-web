using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Properties;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Repository
{

    /// <summary>
    /// Consulta de dados relacionados com o funcionario
    /// </summary>
    public class QuestionRepository : GenericRepository<QuestionEntity>
    {

        ///<summary>
        ///Busca todos os funcionarios do perfil jogador de uma firma
        /// </summary>
        public List<QuestionEntity> GetAllFromFirm(int firmId, string search, int pageIndex, int pageSize)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = (from q in context.QuestionEntity where (q.FirmId == firmId && q.Question.Contains(search)) select q).OrderBy(x => x.Question).Skip(pageIndex * pageSize).Take(pageSize);

                return query.ToList();
            }
        }


        ///<summary>
        ///Busca todos os funcionarios do perfil jogador de uma firma
        /// </summary>
        public int GetCountFromFirm(int firmId, string search)
        {
            using (ModelContext context = new ModelContext())
            {
                if (search != null)
                {
                    return (from q in context.QuestionEntity where (q.FirmId == firmId && q.status == true && q.Question.Contains(search)) select q).Count();
                }
                return (from q in context.QuestionEntity where (q.FirmId == firmId) select q).Count();
            }
        }
    


}
}
