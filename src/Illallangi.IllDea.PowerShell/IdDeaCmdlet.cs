namespace Illallangi.IllDea.PowerShell
{
    using System;

    public abstract class IdDeaCmdlet : DeaCmdlet
    {
        private Guid? currentId;

        public Guid Id
        {
            get
            {
                return (this.currentId.HasValue ? this.currentId : (this.currentId = Guid.NewGuid())).Value;
            }
            set
            {
                this.currentId = value;
            }
        }
    }
}