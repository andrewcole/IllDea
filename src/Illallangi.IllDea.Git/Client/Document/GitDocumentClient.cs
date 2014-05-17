namespace Illallangi.IllDea.Client.Document
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using AutoMapper;

    using Illallangi.IllDea.Model;

    public sealed class GitDocumentClient : BaseClient, ICrudClient<IDocument>
    {
        #region Fields

        private readonly GitDeaClient currentClient;

        #endregion

        #region Constructor

        public GitDocumentClient(GitDeaClient client)
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

        #region Methods

        public IDocument Create(Guid companyId, IDocument document, string log = null)
        {
            return this.CreateDocument(companyId, Mapper.DynamicMap<GitDocument>(document), log);
        }

        public IEnumerable<IDocument> Retrieve(Guid companyId)
        {
            return this.RetrieveDocument(companyId).OrderBy(d => d.Date).ThenBy(d => d.Title);
        }

        public IDocument Update(Guid companyId, IDocument document, string log = null)
        {
            return this.UpdateDocument(
                companyId, 
                this.RetrieveDocument(companyId: companyId, id: document.Id).Single(),
                document.Title,
                document.Date,
                document.Compilation,
                log);
        }

        public void Delete(Guid companyId, IDocument document, string log = null)
        {
            this.DeleteDocument(
                this.RetrieveDocument(companyId: companyId, id: document.Id).Single(),
                log);
        }

        private GitDocument CreateDocument(Guid companyId, GitDocument document, string log = null)
        {
            var index = this.Client.Retrieve(companyId: companyId).Single();
            var period = this.Client.Period.Retrieve(companyId).Single(p => (p.Start <= document.Date) && (p.End >= document.Date));

            document.Period = period.Id;

            using (var atomic = index.Atomic(log ?? "Adding Document {0}", document.Title))
            {
                index.Documents.Add(document.Id);
                atomic.Download(document.Uri, string.Format("{0}.pdf", document.Id));
                atomic.Save(document);
            }

            return document;
        }

        private IEnumerable<GitDocument> RetrieveDocument(Guid companyId, Guid? id = null, string title = null)
        {
            var index = this.Client.Retrieve(companyId: companyId).Single();

            foreach (var documentId in index.Documents)
            {
                GitDocument document;
                try
                {
                    document = index.Load<GitDocument>(documentId);
                }
                catch (Exception e)
                {
                    this.OnError(e);
                    continue;
                }

                document.Uri = new Uri(Path.Combine(index.RootPath, string.Format("{0}.pdf", document.Id)));

                if ((null == id || document.Id.Equals(id)) &&
                    (null == title || document.Title.Equals(title)))
                {
                    yield return document;
                }
            }
        }

        private GitDocument UpdateDocument(Guid companyId, GitDocument document, string title, DateTime date, bool compilation, string log)
        {
            document.Title = title;
            document.Date = date;
            document.Compilation = compilation;

            var period = this.Client.Period.Retrieve(companyId).Single(p => (p.Start <= document.Date) && (p.End >= document.Date));

            document.Period = period.Id;

            using (var atomic = this.Client.Retrieve(id: document.Index).Single().Atomic(log ?? "Updating Document"))
            {
                return atomic.Save(document);
            }
        }

        private void DeleteDocument(GitDocument document, string log)
        {
            var index = this.Client.Retrieve(id: document.Index).Single();
            index.Documents.Remove(document.Id);

            using (var atomic = index.Atomic(log ?? "Removing Document"))
            {
                atomic.Delete(document);
            }
        }

        #endregion
    }
}