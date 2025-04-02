/*
  Copyright © 2018 ASCON-Design Systems LLC. All rights reserved.
  This sample is licensed under the MIT License.
*/
using System;
using Ascon.Pilot.DataModifier;
using Ascon.Pilot.Server.Api;
using Ascon.Pilot.Server.Api.Contracts;
using Ascon.Pilot.Transport;

namespace RabbitMQ.Consumer.PilotICE
{
    public class RemoteApiProvider : IConnectionLostListener
    {
        private Backend _backend;
        private IServerApi _serverApi;
        private FileSystemStorageProvider _fileStorageProvider;

        public void ConnectToPilotServer(ConnectionCredentials credentials)
        {
            var httpClient = new HttpPilotClient(credentials.GetConnectionString(), credentials.GetConnectionProxy());
            httpClient.SetConnectionLostListener(this);
            // Set isCheckedClientVersion to true if you need to check if the versions of the client and server match
            httpClient.Connect(false);
            
            // get apis
            var authApi = httpClient.GetAuthenticationApi();
            // if you need to handle callbacks from the server, please implement the IServerCallback interface
            _serverApi = httpClient.GetServerApi(new NullableServerCallback());
            // if you need to handle messages callbacks from the server, please implement the IMessageCallback interface
            var messageApi = httpClient.GetMessagesApi(new NullableMessagesCallback());
            var fileApi = httpClient.GetFileArchiveApi();

            // auth
            // License code for pilot-ice (100) use your license code here
            authApi.Login(credentials.DatabaseName, credentials.Username, credentials.ProtectedPassword, false, 103);

            // create instances, you can implement interfaces by yourself or use default from Server.Api
            _fileStorageProvider = new FileSystemStorageProvider(DirectoryHelper.GetTempPath()); // Default IFileStorageProvider implementation
            var changSetUploader = new ChangesetUploader(fileApi, _fileStorageProvider, null); // Default IChangesetUploader implementation, set logger if needed
            _backend = new Backend(_serverApi, messageApi, changSetUploader); // Default IBackend implementation
        }

        public IModifier GetNewModifier()
        {
            var modifier = new Modifier(_backend);
            return modifier;
        }

        public IBackend GetBackend()
        {
            return _backend;
        }

        public void ConnectionLost(Exception ex = null)
        {
            // todo
            //try to reconnect if needed
        }

        public FileSystemStorageProvider GetStorage()
        {
            return _fileStorageProvider;
        }
    }
}
