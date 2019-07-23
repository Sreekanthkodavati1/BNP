using Bnp.Core.Tests.API.Validators;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods
{
    public class CDIS_DataGenerator
    {
        public Common com;

        public CDIS_DataGenerator(Common common)
        {
            this.com = common;
        }

        /// <summary>
        /// To generate Social handles for a Member
        /// </summary>
        /// <param name="providerType"></param>
        /// <param name="providerUID"></param>
        /// <returns></returns>
        public static MemberSocialHandleStruct[] generateMemberSocialHandleStruct(string providerType, string providerUID)
        {
            MemberSocialHandleStruct memberSocialHandleStruct = new MemberSocialHandleStruct();
            memberSocialHandleStruct.ProviderType = providerType;
            memberSocialHandleStruct.ProviderUID = providerUID;
            MemberSocialHandleStruct[] membersocialhandles = new MemberSocialHandleStruct[1];
            membersocialhandles[0] = memberSocialHandleStruct;
            return membersocialhandles;
        }
    }
}
