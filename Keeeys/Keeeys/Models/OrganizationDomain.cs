using Keeeys.Common.Network;
using System;
using System.Collections.Generic;

namespace Keeeys.Common.Models
{
    public class OrganizationDomain : ICustomListItem
    {
        public Organization OrganizationInstance;
        public Domain DomainInstance;

        public static List<OrganizationDomain> BuildFromOrganizationList(IEnumerable<Organization> organizations)
        {
            List<OrganizationDomain> newList = new List<OrganizationDomain>();
            foreach (Organization org in organizations)
            {
                if (org.Domains.Count == 0)
                    continue;
                foreach (Domain domain in org.Domains)
                {
                    newList.Add(new OrganizationDomain() { OrganizationInstance = org, DomainInstance = domain });
                }
            }

            return newList;
        }

        public string ToLabelString()
        {
            return String.Format("{0} / {1}", OrganizationInstance.Name, DomainInstance.Name);
        }

        public int GetId()
        {
            return DomainInstance.Id.HasValue ? (int)DomainInstance.Id.Value : 0;
        }
    }
}
