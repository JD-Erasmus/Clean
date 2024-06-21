/*using Audit.Core;
using Audit.Mvc;
using Audit.SqlServer.Providers;
using System.Linq;
using System.Threading.Tasks;

namespace CleanDemo.MVC.Audit
{
    public class CustomSqlDataProvider : SqlDataProvider
    {
        public CustomSqlDataProvider(string connectionString)
        {
            ConnectionString = connectionString;
            TableName = "AuditLogs";
            IdColumnName = "EventId";
            
        }

        public override object InsertEvent(AuditEvent auditEvent)
        {
            var mvcEvent = auditEvent.GetMvcAuditAction();

            var parameters = new
            {
                TraceId = mvcEvent.TraceId,
                HttpMethod = mvcEvent.HttpMethod,
                ControllerName = mvcEvent.ControllerName,
                ActionName = mvcEvent.ActionName,
                ViewName = mvcEvent.ViewName,
                FormVariables = mvcEvent.FormVariables?.ToDictionary(k => k.Key, v => v.Value),
                UserName = mvcEvent.UserName,
                RequestUrl = mvcEvent.RequestUrl,
                IpAddress = mvcEvent.IpAddress,
                ResponseStatus = mvcEvent.ResponseStatus,
                ResponseStatusCode = mvcEvent.ResponseStatusCode,
                RedirectLocation = mvcEvent.RedirectLocation,
                Exception = mvcEvent.Exception,
                Errors = mvcEvent.ModelStateErrors,
                auditEvent.StartDate,
                auditEvent.EndDate,
                auditEvent.Duration
            };

            return base.InsertEvent(parameters);
        }

        // Implement the async version without overriding
        public async Task<object> InsertEventAsync(AuditEvent auditEvent)
        {
            var mvcEvent = auditEvent.GetMvcAuditAction();

            var parameters = new
            {
                TraceId = mvcEvent.TraceId,
                HttpMethod = mvcEvent.HttpMethod,
                ControllerName = mvcEvent.ControllerName,
                ActionName = mvcEvent.ActionName,
                ViewName = mvcEvent.ViewName,
                FormVariables = mvcEvent.FormVariables?.ToDictionary(k => k.Key, v => v.Value),
                UserName = mvcEvent.UserName,
                RequestUrl = mvcEvent.RequestUrl,
                IpAddress = mvcEvent.IpAddress,
                ResponseStatus = mvcEvent.ResponseStatus,
                ResponseStatusCode = mvcEvent.ResponseStatusCode,
                RedirectLocation = mvcEvent.RedirectLocation,
                Exception = mvcEvent.Exception,
                Errors = mvcEvent.ModelStateErrors,
                auditEvent.StartDate,
                auditEvent.EndDate,
                auditEvent.Duration
            };

            return await Task.FromResult(base.InsertEvent(parameters));
        }

        public override void ReplaceEvent(object eventId, AuditEvent auditEvent)
        {
            // No implementation required for now
        }

        public Task ReplaceEventAsync(object eventId, AuditEvent auditEvent)
        {
            // No implementation required for now
            return Task.CompletedTask;
        }
    }
}
*/