using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;

namespace ImanHangfireSample.Filters
{
    public class LogJobAttribute : JobFilterAttribute, IClientFilter,IServerFilter,IElectStateFilter,IApplyStateFilter
    {
        private static readonly ILog _logger = LogProvider.GetCurrentClassLogger();
        
        //ایجاد شد
        public void OnCreated(CreatedContext context)
        {
            _logger.InfoFormat($"------------------- OnCreated {context.Job.Method.Name} , id = {context.JobId}");
        }

        //در حال ایجاد شدن
        public void OnCreating(CreatingContext context)
        {
            _logger.InfoFormat($"------------------- OnCreating {context.Job.Method.Name}");
        }

        //درحال انجام
        public void OnPerformed(PerformedContext context)
        {
            _logger.InfoFormat($"------------------- OnPerformed {context.JobId} , ExceptionHandled = {context.ExceptionHandled} , result {context.Result}");
        }

        //انجام شد
        public void OnPerforming(PerformingContext context)
        {
            _logger.InfoFormat($"------------------- OnPerforming {context.JobId}");
        }

        //وقتی یک وضعیت برای جاب مشخص میشود
        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            _logger.InfoFormat($"------------------- OnPerforming {context.JobId} , old state = {context.OldStateName} , new State = {context.NewState}" );
        }

        //وقتی وضعیت برای جاب انتخاب مشخص میشود
        public void OnStateElection(ElectStateContext context)
        {
            _logger.InfoFormat($"------------------- OnStateElection {context.JobId} CandidateState = {context.CandidateState} CurrentState = {context.CurrentState}");
        }

        //وقتی یک وضعیت از یک جاب گرفته میشود
        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            _logger.InfoFormat($"------------------- OnStateUnapplied {context.JobId} old state {context.OldStateName} , new State = {context.NewState}");
        }
    }
}
