using Audit.Core;
using Audit.Mvc;
using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace CleanDemo.MVC.Audit
{
    public class CustomAuditDataProvider : AuditDataProvider
    {
        private readonly string _logFilePath;
        private readonly object _lock = new object();

        public CustomAuditDataProvider(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public override object InsertEvent(AuditEvent auditEvent)
        {
            var mvcEvent = auditEvent.GetMvcAuditAction();

            var customEvent = new
            {
                Action = new
                {
                    mvcEvent.TraceId,
                    mvcEvent.HttpMethod,
                    mvcEvent.ControllerName,
                    mvcEvent.ActionName,
                    mvcEvent.ViewName,
                    FormVariables = mvcEvent.FormVariables?.ToDictionary(k => k.Key, v => v.Value),
                    mvcEvent.UserName,
                    mvcEvent.RequestUrl,
                    mvcEvent.IpAddress,
                    mvcEvent.ResponseStatus,
                    mvcEvent.ResponseStatusCode,
                    mvcEvent.RedirectLocation,
                    mvcEvent.Exception
                },
                Errors = mvcEvent.ModelStateErrors,
                auditEvent.StartDate,
                auditEvent.EndDate,
                auditEvent.Duration
            };

            var serializedEvent = JsonSerializer.Serialize(customEvent).Trim() + Environment.NewLine;


            // Use a lock to ensure thread-safe writing to the log file
            lock (_lock)
            {
                File.AppendAllText(_logFilePath, serializedEvent, Encoding.UTF8);
            }

            return customEvent;
        }

        public override void ReplaceEvent(object eventId, AuditEvent auditEvent)
        {
            // No implementation required for file-based logging
        }
    }

}
