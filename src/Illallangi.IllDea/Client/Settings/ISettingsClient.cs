namespace Illallangi.IllDea.Client.Settings
{
    using System;
    using System.Collections.Generic;

    using Illallangi.IllDea.Logging;
    using Illallangi.IllDea.Model;

    public interface ISettingsClient : ILogging
    {
        IEnumerable<ISettings> Retrieve(Guid companyId);

        ISettings Update(Guid companyId, ISettings companySettings);
    }
}
