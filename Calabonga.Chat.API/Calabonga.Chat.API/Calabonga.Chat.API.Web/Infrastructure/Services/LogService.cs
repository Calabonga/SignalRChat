﻿using Calabonga.Chat.API.Data;
using Calabonga.Chat.API.Entities;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;

namespace Calabonga.Chat.API.Web.Infrastructure.Services
{
    /// <summary>
    /// Business logic logger can save messages to database
    /// </summary>
    public class LogService : ILogService
    {
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

        public LogService(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Log information message
        /// </summary>
        /// <param name="message"></param>
        public void LogInformation(string message)
        {
            Log(LogLevel.Information, message);
        }

        /// <summary>
        /// Allows to save data logs to the database Logs table
        /// </summary>
        private void Log(LogLevel level, string message, Exception exception = null)
        {
            var logs = _unitOfWork.GetRepository<Log>();
            var log = new Log
            {
                CreatedAt = DateTime.UtcNow,
                Level = level.ToString(),
                Logger = GetType().Name,
                Message = message,
                ThreadId = "0",
                ExceptionMessage = exception?.Message
            };
            logs.Insert(log);
            _unitOfWork.SaveChanges();
            if (!_unitOfWork.LastSaveChangesResult.IsOk)
            {
                throw new MicroserviceInvalidOperationException();
            }
        }
    }
}