using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SPD.Model.Util
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class Transactional : Attribute
    {
        private const int CURRENT = 1;
        public bool AutoSave { get; set; }
        public bool UseTransactionScope { get; set; }
        public Enums.SPD_Enums.Propagation Propagation { get; }

        public Transactional()
        {
            this.AutoSave = false;
            this.UseTransactionScope = true;
            this.Propagation = Enums.SPD_Enums.Propagation.Required;
        }

        public Transactional(Enums.SPD_Enums.Propagation propagation)
            : base()
        {
            this.Propagation = propagation;
        }
        
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static MethodBase GetMethod(int level)
        {
            StackTrace stackTrace = new StackTrace();

            StackFrame stackFrame = stackTrace.GetFrame(level);

            return stackFrame.GetMethod();
        }
        
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Transactional ExtractTransactionalFromFrame(IDictionary<string, object> internalTransactionalMaps, string internalTransactionalMapProperty)
        {
            StackTrace stackTrace = new StackTrace();

            foreach (var frame in stackTrace.GetFrames())
            {
                object value;

                if (internalTransactionalMaps.TryGetValue(internalTransactionalMapProperty, out value))
                {
                    var transactionalMap = value as Dictionary<string, Transactional>;

                    Transactional transactional;

                    if (transactionalMap.TryGetValue(frame.GetMethod().ToString(), out transactional))
                    {
                        return transactional;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Método responsável por operar a transação
        /// </summary>
        /// <param name="transactional"></param>
        public static implicit operator TransactionScope(Transactional transactional)
        {
            return new TransactionScope(transactional.ToTransactionScopeOption());
        }

        /// <summary>
        /// Método responsável por ir para a opção de escopo de transação
        /// </summary>
        /// <returns></returns>
        public TransactionScopeOption ToTransactionScopeOption()
        {
            switch (this.Propagation)
            {
                case Enums.SPD_Enums.Propagation.Required:
                    return TransactionScopeOption.Required;

                case Enums.SPD_Enums.Propagation.RequiresNew:
                    return TransactionScopeOption.RequiresNew;

                case Enums.SPD_Enums.Propagation.Suppress:
                    return TransactionScopeOption.Suppress;

                default:
                    // ToDo: Prepare it to translation.
                    throw new Exception("Invalid option for TransactionScopeOption.");
            }
        }
        
        public static Transactional GetTransactional(Type type)
        {
            var exportedType = Assembly.GetCallingAssembly().GetExportedTypes().Where(internalType => internalType.Equals(type)).FirstOrDefault();

            if (exportedType != null)
            {
                foreach (var method in exportedType.GetMethods())
                {
                    var transactional = method.GetCustomAttribute<Transactional>();

                    if (transactional != null)
                    {
                        if (String.Equals(method.Name, Transactional.GetMethod(Transactional.CURRENT).Name))
                        {
                            return transactional;
                        }
                    }
                }
            }

            return null;
        }
        
        public static Dictionary<string, Transactional> GetTransactionalMap(Type type)
        {
            var transactionalMap = new Dictionary<string, Transactional>();

            var exportedType = Assembly.GetCallingAssembly().GetExportedTypes().Where(internalType => internalType.Equals(type)).FirstOrDefault();

            if (exportedType != null)
            {
                foreach (var method in exportedType.GetMethods())
                {
                    var transactional = method.GetCustomAttribute<Transactional>();

                    if (transactional != null)
                    {
                        transactionalMap.Add(method.ToString(), transactional);
                    }
                }
            }

            return transactionalMap;
        }
        
        public static Transactional ExtractTransactional(dynamic transactionalMaps)
        {
            IDictionary<string, object> internalTransactionalMaps = transactionalMaps;

            foreach (var internalTransactionalMapProperty in internalTransactionalMaps.Keys)
            {
                if ((internalTransactionalMapProperty.Equals("FromApplicationService", StringComparison.InvariantCulture)) ||
                    (internalTransactionalMapProperty.Equals("FromService", StringComparison.InvariantCulture)) ||
                    (internalTransactionalMapProperty.Equals("FromRepository", StringComparison.InvariantCulture)))
                {
                    return Transactional.ExtractTransactionalFromFrame(internalTransactionalMaps, internalTransactionalMapProperty);
                }
            }

            return null;
        }
    }
}
