using Vlast.Util.Instrumentation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Transactions;

namespace Gamific.Scheduler
{
    /// <summary>
    /// Interface implementada pelos workers
    /// </summary>
    public abstract class ThreadWorker
    {
        private static object _syncRoot = new object();
        private static Random randomGenerator = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Transação configurada especialmente para threads
        /// </summary>
        protected TransactionScope ThreadTransaction
        {
            get
            {
                TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                return scope;
            }
        }

        private bool CanStop { get; set; }

        /// <summary>
        /// Tempo em milisegundos para a thread aguardar para
        /// novas tarefas
        /// </summary>
        protected volatile int SleepTime = 10000;

        /// <summary>
        /// Envia um sinal para parar a thread
        /// </summary>
        public void Stop()
        {
            lock (_syncRoot)
            {
                CanStop = true;
            }
        }

        /// <summary>
        /// Inicia o processamento em lotes da thread
        /// </summary>
        internal void Start()
        {
            CanStop = false;

            while (!CanStop)
            {
                bool processMore = Run();
                if (!processMore)
                {
                    Thread.Sleep(SleepTime);
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// Executa uma tarefa sobre uma lista de items
        /// </summary>
        /// <param name="targetItemsIds"></param>
        /// <returns>True se a thread fes algum trabalho</returns>
        internal abstract bool Run();

    }

    /// <summary>
    /// Factory to create a list of WorkerThreads
    /// </summary>
    public class WorkerFactory<T> where T : ThreadWorker
    {
        private volatile object _syncRoot = new object();
        private volatile List<Thread> workerInstances = new List<Thread>();

        /// <summary>
        /// Inicializa as instancias dos workers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="workersCount"></param>
        public void Init(int workersCount)
        {
            for (int i = 0; i < workersCount; ++i)
            {
                ThreadWorker worker = (ThreadWorker)typeof(T).GetConstructor(Type.EmptyTypes).Invoke(null);
                lock (_syncRoot)
                {
                    workerInstances.Add(new Thread(worker.Start));
                }
            }
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Inicializa os workers
        /// </summary>
        public void Start()
        {
            Thread.Sleep(1000);
            try
            {
                lock (_syncRoot)
                {
                    foreach (Thread worker in workerInstances)
                    {
                        if (!worker.IsAlive)
                        {
                            worker.Start();
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        /// <summary>
        /// Finalza os workers
        /// </summary>
        public void Stop()
        {
            try
            {
                lock (_syncRoot)
                {
                    foreach (Thread worker in workerInstances)
                    {
                        if (!worker.IsAlive)
                        {
                            worker.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }



    }
}
