using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Service command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    public abstract class ServiceCommandHandlerBase : CommandHandlerBase
    {
        /// <summary>Initializes a new instance of the <see cref="ServiceCommandHandlerBase"/> class.</summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        protected ServiceCommandHandlerBase(IFileCabinetService fileCabinetService)
        {
            this.Service = fileCabinetService ?? throw new ArgumentNullException($"{nameof(fileCabinetService)} can't be null.");
        }

        /// <summary>Gets the service.</summary>
        /// <value>The service.</value>
        public IFileCabinetService Service { get; private set; }
    }
}
