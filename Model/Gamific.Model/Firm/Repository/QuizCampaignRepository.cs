﻿using System;
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
    public class QuizCampaignRepository : GenericRepository<QuizCampaignEntity>
    {


        public List<QuizCampaignEntity> getAllByAssociated(int idAssociated)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = (from q in context.QuizCampaignEntity where (q.IdQuiz == idAssociated) select q);

                return query.ToList();
            }
        }

    }
}
