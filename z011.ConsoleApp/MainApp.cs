// <copyright file="MainApp.cs" company="None">
// Free and open source code.
// </copyright>

namespace Z011.ConsoleApp
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using MediatR;
    using Z011.Application;

    /// <summary>
    /// Main application class.
    /// </summary>
    internal class MainApp
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainApp"/> class.
        /// </summary>
        /// <param name="mediator">IMediator.</param>
        public MainApp(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// The running program.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Run()
        {
            var result = await this.mediator.Send(new StockChangeGrid.Query());
            Console.WriteLine($"There are {result.Rows.Count()} rows.");
        }
    }
}