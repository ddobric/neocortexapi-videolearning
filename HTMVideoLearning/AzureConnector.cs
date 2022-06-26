using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace HTMVideoLearning
{
    internal class AzureConnector
    {
        /// <summary>
        /// This is the string to connect to the storage account
        /// </summary>
        private string queConnectionString { get; set; }

        /// <summary>
        /// Constructor to create the azure account connector
        /// </summary>
        /// <param name="queConnectionString">The string to connect to the que storage</param>
        public AzureConnector(string queConnectionString)
        {
            this.queConnectionString = queConnectionString; 
        }

        public async Task CreateQueue(string queueName = "queue-name")
        {
            QueueClient queue = new QueueClient(queConnectionString, queueName);
            await queue.CreateIfNotExistsAsync();
            await queue.CreateAsync();
        }
    }
}
