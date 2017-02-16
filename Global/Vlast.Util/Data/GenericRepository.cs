using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vlast.Util.Data
{
    /// <summary>
    /// Repositório genérico 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="KeyType"></typeparam>
    public abstract class GenericRepository<T, KeyType>
    {

        /// <summary>
        /// Recupera a entidade relacionada pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract T GetById(KeyType id);

        /// <summary>
        /// Cria uma nova entidade
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public abstract T Create(T newEntity);

        /// <summary>
        /// Atualiza a entidade
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public abstract T Update(T updatedEntity);

        /// <summary>
        /// Deleta uma entidade pelo seu id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract bool DeleteById(KeyType id);

        /// <summary>
        /// Retorna um objeto para queries externas
        /// </summary>
        /// <returns></returns>
        public abstract IQueryable<T> GetAll();

        /// <summary>
        /// Retorna a hora atual no time zone de São paulo
        /// </summary>
        protected DateTime CurrentSaoPauloDate
        {
            get { 
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
            }
        }

      
    }
}
