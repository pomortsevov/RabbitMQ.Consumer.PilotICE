using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ascon.Pilot.Server.Api.Contracts;
using Ascon.Pilot.DataClasses;
using Ascon.Pilot.Transport;

namespace RabbitMQ.Consumer.PilotICE
{
    public class SearchResultCallBack : IServerCallback
    {
        private readonly Action<DSearchResult> _actionWithResult;

        public SearchResultCallBack(Action<DSearchResult> actionWithResult)
        {
            _actionWithResult = actionWithResult;
        }

        public void NotifySearchResult(DSearchResult searchResult)
        {
            _actionWithResult(searchResult);
        }
        #region NotImplemented

        public void NotifyChangeset(DChangeset changeset)
        {
        }

        public void NotifyOrganisationUnitChangeset(OrganisationUnitChangeset changeset)
        {
        }

        public void NotifyPersonChangeset(PersonChangeset changeset)
        {
        }

        public void NotifyDMetadataChangeset(DMetadataChangeset changeset)
        {
        }

        public void NotifyGeometrySearchResult(DGeometrySearchResult searchResult)
        {
        }

        public void NotifyDNotificationChangeset(DNotificationChangeset changeset)
        {
        }

        public void NotifyCommandResult(Guid requestId, byte[] data, ServerCommandResult result)
        {
        }

        public void NotifyChangeAsyncCompleted(DChangeset changeset)
        {
        }

        public void NotifyChangeAsyncError(Guid identity, ProtoExceptionInfo exception)
        {
        }

        public void NotifyCustomNotification(string name, byte[] data)
        {
        }

        void IServerCallback.NotifyAccessChangeset(Guid objectId)
        {
            //throw new NotImplementedException();
            //закоментил тк выпадает в эксепшен
        }

        void IServerCallback.NotifySettingsChangeset(string settingKey)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
