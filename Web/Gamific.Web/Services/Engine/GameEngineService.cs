using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Services.Engine
{
    public class GameEngineService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile GameEngineService instance;

        private GameEngineService() : base(ENGINE_API + "game/") { }

        public static GameEngineService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new GameEngineService();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Services

        public GameEngineDTO CreateOrUpdate(GameEngineDTO game)
        {
            return PostDTO<GameEngineDTO>(ref game);
        }

        public GameEngineDTO CreateOrUpdate(GameEngineDTO game, string email)
        {
            return PostDTO<GameEngineDTO>(ref game, email);
        }

        #endregion
    }
}