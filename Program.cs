/*
  Copyright © 2018 ASCON-Design Systems LLC. All rights reserved.
  This sample is licensed under the MIT License.
*/
using System;
using System.Configuration;
using Ascon.Pilot.Common.DataProtection;
using Ascon.Pilot.DataClasses;
using Ascon.Pilot.DataModifier;
using Ascon.Pilot.Server.Api;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using RabbitMQ;
using MRabbitMQ.Consumer.PilotICE;
//using log4net;
//using System.Net.NetworkInformation;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

using Serilog;



namespace RabbitMQ.Consumer.PilotICE
{
    class Program
    {
        private static void InitLogger(string logfile)
        {



            var configuration_loki = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings_loki.json")
            .Build();


            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration_loki)
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(logfile, rollingInterval: RollingInterval.Day)
                //       .WriteTo.GrafanaLoki("http://localhost:3100")
                .CreateLogger();

            Log.Information("RabbitMQ.Consumer.PilotICE started");

        }

        static void Main(string[] args)
        {

            InitLogger("logs/myapp.txt");

            const string config_path = @"C:\Develop\RabbitMQ\RabbitMQ.Consumer\consummersettings.json";
            
            IConfiguration config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();

            // Get a configuration section
            IConfigurationSection section = config.GetSection("Settings");

            var RabbitMQClientConfig = section.Get<RabbitMQConsumerConfig>();

            Log.Information("------RabbitMQ.Consumer.PilotICE started---------");

          

            // Log.Information( string.Format("Can't connect RabbitMQ  Host: {0} Exchange: {1} Queue: {1}", HostName, Exchange, Queue));

           

            string UserName = RabbitMQClientConfig.RabbitMQUser,
                   Password = RabbitMQClientConfig.RabbitMQPassword,
                   HostName = RabbitMQClientConfig.RabbitMQUrl,
                   Exchange = RabbitMQClientConfig.RabbitMQExchange,
                   Queue = RabbitMQClientConfig.RabbitMQQueue;


            try
            {
                var connectionFactory = new RabbitMQ.Client.ConnectionFactory()
                {
                    UserName = RabbitMQClientConfig.RabbitMQUser,
                    Password = RabbitMQClientConfig.RabbitMQPassword,
                    HostName = RabbitMQClientConfig.RabbitMQUrl
                };

                var connection = connectionFactory.CreateConnection();
                var channel = connection.CreateModel();

                  Log.Information(string.Format("Connected RabittMQ: {0} ", RabbitMQClientConfig.PilotICE_URL));
                Log.Information(string.Format("RabbitMQ  Host: {0} Exchange: {1} Queue: {1}", HostName, Exchange, Queue));
                //Log.Information("RabbitMQ.Produser started");

                TopicExchangeConsumer.Consume(channel, RabbitMQClientConfig);


            }
            catch (Exception ex)
            {

                //throw;
                 Log.Error(ex, string.Format("Can't connect RabbitMQ  Host: {0} Exchange: {1} Queue: {1}", HostName, Exchange, Queue));


            }

            finally
            {
                Log.Information("RabbitMQ.Consumer.PilotICE stoped");
                Log.CloseAndFlush();
            }




            //var connectionFactory = new RabbitMQ.Client.ConnectionFactory()
            //{
            //    UserName = UserName,
            //    Password = Password,
            //    HostName = HostName
            //};

            //var connection = connectionFactory.CreateConnection();
            //var channel = connection.CreateModel();
            //Console.WriteLine("Creating Exchange");

            //TopicExchangeConsumer.Consume(channel, RabbitMQClientConfig);


            Log.Information("------RabbitMQ.Consumer.PilotICE stoped---------");


        }
    }
}
