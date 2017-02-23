using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Services.Engine
{
    public class ItemEngineService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile ItemEngineService instance;

        private ItemEngineService() : base(ENGINE_API + "item/") { }

        public static ItemEngineService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new ItemEngineService();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Services

        public ItemEngineDTO CreateOrUpdate(ItemEngineDTO item)
        {
            return PostDTO<ItemEngineDTO>(ref item);
        }

        public ItemEngineDTO GetById(string itemId)
        {
            return GetDTO<ItemEngineDTO>(itemId);
        }

        public void DeleteById(string id)
        {
            Delete(id);
        }

        #endregion
    }
}