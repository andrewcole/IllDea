namespace Illallangi.IllDea.Client.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Illallangi.IllDea.Client.Settings;
    using Illallangi.IllDea.Model;

    public sealed class GitSettingsClient : BaseClient, ISettingsClient
    {
        #region Fields

        private readonly GitDeaClient currentClient;

        #endregion

        #region Constructor

        public GitSettingsClient(GitDeaClient client)
        {
            this.currentClient = client;
        }

        #endregion

        #region Properties

        private GitDeaClient Client
        {
            get
            {
                return this.currentClient;
            }
        }

        #endregion

        public IEnumerable<ISettings> Retrieve(Guid companyId)
        {
            return this.Client.Retrieve(companyId: companyId);
        }

        public ISettings Update(Guid companyId, ISettings companySettings)
        {
            var result = this.Client.Retrieve(companyId: companyId).Single();
            result.AuthorEmail = companySettings.AuthorEmail ?? result.AuthorEmail;
            result.AuthorName = companySettings.AuthorName ?? result.AuthorName;
            return result;
        }
    }
}