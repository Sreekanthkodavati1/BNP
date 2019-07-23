using System;
using System.Collections;
using Bnp.Core.Tests.API.Enums;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods
{
    public class CDIS_Service_Methods
    {
        public Common common;
        string output;
        double time = 0;
        LWClientException exception = null;

        public CDIS_Service_Methods(Common common)
        {
            this.common = common;
        }


        /// <summary>
        /// This method is to change the card expiration date by providing Card Id from ChangeCardExpirationDate service call
        /// </summary>
        /// <param name="cardid">Specifies the Card Id</param>
        /// <Param name="new Date">Specifies the new Date</Param>
        /// <param name="Elapsed Time">Specifies the Elapsed Time</param>
        /// <returns></returns>
        public DateTime ChangeCardExpirationDate(String cardid, DateTime date, out double time1)
        {


            ProjectTestBase.lWIntegrationSvcClientManager.ChangeCardExpirationDate(cardid, date, string.Empty, out time1);
            return date;
        }


        /// <summary>
        /// This method is to change the card expiration date by providing Card Id from ChangeCardExpirationDate(Negative) service call
        /// </summary>
        /// <param name="cardid">Specifies the Card Id</param>
        /// <Param name="new Date">Specifies the new Date</Param>
        /// <param name="Elapsed Time">Specifies the Elapsed Time</param>
        /// <returns></returns>
        public string ChangeCardExpirationDate_Negative(String cardid, DateTime date)
        {
            string errorcode = null;

            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.ChangeCardExpirationDate(cardid, date, string.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }

            if (errorcode != null)
            {
                return errorcode;
            }
            return date.ToString();

        }



        /// <summary>
        /// This method is to replace the card by providing the Old Card Id and the New Card Id from ReplaceCard service call
        /// </summary>
        /// <param name="oldcardID">Specifies the Old Card Id</param>
        /// <param name="newcardID">Specifies the New Card Id</param>
        /// /// <param name="OutTime">Specifies the OutTime</param>
        /// <returns></returns>
        public Member ReplaceCard(string oldcardID, string newcardID, bool? transferPoints, DateTime Datenew, out double time)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.ReplaceCard(oldcardID, newcardID, transferPoints, Datenew, "SOAP Automation", string.Empty, out time);
        }

        /// <summary>
        /// This method is to replace the card by providing the Old Card Id and the New Card Id from ReplaceCard service call
        /// </summary>
        /// <param name="oldcardID">Specifies the Old Card Id</param>
        /// <param name="newcardID">Specifies the New Card Id</param>
        /// /// <param name="OutTime">Specifies the OutTime</param>
        /// <returns></returns>
        public object ReplaceCardNegative(String oldcardID, string newcardID, bool? transferPoints, DateTime Datenew)
        {
            string errorcode = null;
            try
            {

                return ProjectTestBase.lWIntegrationSvcClientManager.ReplaceCard(oldcardID, newcardID, transferPoints, Datenew, "SOAP Automation", String.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }

            if (errorcode != null)
            {
                return errorcode;
            }
            return output;

        }

        /// <summary>
        /// This Member is to transfer the card from one member to another member.
        /// </summary>
        public Member TransferCard(string loyaltyIdNumber1, string loyaltyIdNumber2, bool? makePrimary, bool? deactivateMember, double time)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.TransferCard(loyaltyIdNumber1, loyaltyIdNumber2, makePrimary, deactivateMember, string.Empty, out time);

        }

        /// <summary>
        /// Retrieving the member using CDIS call
        /// </summary>
        /// <returns>Member</returns>
        public Member GetCDISMemberGeneral()
        {
            Member output = ProjectTestBase.lWIntegrationSvcClientManager.AddMember(common.GenerateAddMemberForSOAP(), new string[0], System.Guid.NewGuid().ToString(), out time);
            return output;
        }


        


        /// <summary>
        /// This Member is to transfer the card from one member to another member.
        /// </summary>
        public string TransferCardNegative(string loyaltyIdNumber1, string loyaltyIdNumber2, bool makePrimary, bool DeactivateMember, double elapsedtime)
        {
            string errorcode = null;
            string msg = null; 
            try
            {
               ProjectTestBase.lWIntegrationSvcClientManager.TransferCard(loyaltyIdNumber1, loyaltyIdNumber2, makePrimary, DeactivateMember, string.Empty, out elapsedtime);
                msg = "TransferCard Succssful; Card being transfered is: "+ loyaltyIdNumber1 + " and the member identity of the member to which card is been transferred is: "+ loyaltyIdNumber2;
                return msg;  
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }

            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }


        /// <summary>
        /// Retrieving the member using CDIS call
        /// </summary>
        /// <returns>Member</returns>
        public Member CDISMemberGeneral(out double time1)
        {
            Member output = ProjectTestBase.lWIntegrationSvcClientManager.AddMember(common.GenerateAddMemberForSOAP(), new string[0], System.Guid.NewGuid().ToString(), out time1);
            return output;
        }

        /// <summary>
        /// Adding the member using CDIS call by passing a member
        /// </summary>
        /// <returns>Member</returns>
        public Member AddSoapMember(Member member)
        {
            Member output = ProjectTestBase.lWIntegrationSvcClientManager.AddMember(member, new string[0], System.Guid.NewGuid().ToString(), out time);
            return output;
        }

        /// <summary>
        ///  Adding the member using CDIS call by passing all the attributes with negative scenario
        /// </summary>
        /// <returns></returns>
        public Object GetCDISMemberNegative(Member member)
        {
            Member output = null;
            string errorcode = null;
            try
            {
                output = ProjectTestBase.lWIntegrationSvcClientManager.AddMember(member, new string[0], System.Guid.NewGuid().ToString(), out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }
        /// <summary>
		/// Retrieving the pre enrolled  member using CDIS call
		/// </summary>
		/// <returns>Member</returns>
        public Member GetCDISMemberPreEnrolled()
        {
            Member output = ProjectTestBase.lWIntegrationSvcClientManager.AddMember(common.GenerateAddMemberPreEnrolledForSOAP(), new string[0], System.Guid.NewGuid().ToString(), out time);
            return output;

        }

        /// <summary>
        /// Retrieving member with non member status using CDIS call
        /// </summary>
        /// <returns>Member</returns>
        public Member GetCDISMemberNonMember()
        {
            Member output = ProjectTestBase.lWIntegrationSvcClientManager.AddMember(common.GenerateAddNonMemberForSOAP(), new string[0], System.Guid.NewGuid().ToString(), out time);
            return output;

        }

        /// <summary>
        /// Retrieving the member with mandatory fields using CDIS call
        /// </summary>
        /// <returns>Member</returns>
        public Member GetCDISMemberMandatory()
        {
            Member output = ProjectTestBase.lWIntegrationSvcClientManager.AddMember(common.GenerateaddMemberForSOAPMandatory(), new string[0], System.Guid.NewGuid().ToString(), out time);
            return output;
        }

        /// <summary>
        /// Adding the member with all fields using CDIS call
        /// </summary>
        /// <returns></returns>
        public Member AddCDISMemberWithAllFields()
        {
            Member output = ProjectTestBase.lWIntegrationSvcClientManager.AddMember(common.AddMemberWithAllFields(), new string[0], System.Guid.NewGuid().ToString(), out time);
            return output;
        }

        /// <summary>
        /// Adding the member with all fields using CDIS call
        /// </summary>
        /// <returns></returns>
        public Member AddMemberWithDiffMetadata()
        {
            Member member = new Member();
            member.FirstName = "CDIS_" + common.RandomString(6);
            member.LastName = "CDIS_" + common.RandomString(6);
            member.Username = member.FirstName;
            member.Password = "Password1*";
            Logger.Info(member.FirstName);
            VirtualCard vc = new VirtualCard();
            vc.LoyaltyIdNumber = common.RandomNumber(8);
            member.Add(vc);
            Member newMember = new Member();
            member.FirstName = "CDIS_" + common.RandomString(6);
            member.BirthDate = System.DateTime.Now.AddYears(-20);
            member.Add(newMember);
            return member;
        }

        /// <summary>
        /// This method is to Deactivate a member
        /// </summary>
        /// <param name="loyaltyIdNumber"></param>
        public void DeactivateMember_negative(string loyaltyIdNumber, bool deactivateCards)
        {
            double time = 0;
            ProjectTestBase.lWIntegrationSvcClientManager.DeactivateMember(loyaltyIdNumber, DateTime.Now.AddHours(-12), "CDIS Service", deactivateCards, null, null, null, null, null, null, String.Empty, out time);
        }

        /// <summary>
        /// This method is to Deactivate a member
        /// </summary>
        /// <param name="loyaltyIdNumber"></param>
        /// <param name="StatusReason"></param>
        public Object DeactivateMember_negative(string loyaltyIdNumber, string StatusReason)
        {
            double time = 0;
            Member output = null;
            string errorcode = null;
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.DeactivateMember(loyaltyIdNumber, DateTime.Now.AddHours(-12), StatusReason, true, true, true, true, true, true, true, String.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }


        /// <summary>
        /// Retrieving the member with no VCKey using CDIS call
        /// </summary>
        /// <returns>Memeber</returns>
        public Object GetCDISMemberNoVC()
        {
            Member output = null;
            string errorcode = null;
            try
            {
                output = ProjectTestBase.lWIntegrationSvcClientManager.AddMember(common.GenerateAddMemberForSOAPNoVC(), new string[0], System.Guid.NewGuid().ToString(), out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }
        /// <summary>
        ///  Locking down the member using CDIS call by passing all the attributes with negative scenario
        /// </summary>
        /// <param name="loyaltyId">Loyalty Id of the member is passed as parameter</param>
        /// <returns></returns>
        //public Object LockDownMemberNegative(string loyaltyId)
        //{
        //    Member output = null;
        //    string errorcode = null;
        //    try
        //    {
        //        ProjectTestBase.lWIntegrationSvcClientManager.LockdownMember(loyaltyId, DateTime.UtcNow, "CDIS Automation", String.Empty, out time);
        //    }
        //    catch (LWClientException e)
        //    {
        //        Logger.Info(e.ErrorCode);
        //        errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Info(e.Message);
        //        Logger.Info(e.GetType());
        //    }
        //    if (errorcode != null)
        //    {
        //        return errorcode;
        //    }
        //    return output;
        //}
        /// <summary>
        /// This method is to activate the card by using the Loyalty Id from ActivateCardUsingLoyaltyIDNegative service call for Negative scenario.
        /// </summary>
        /// <param name="cardid">Loyalty Id of the member is passed as parameter</param>
        /// <returns></returns>
        //public Object ActivateCardUsingLoyaltyIDNegative(string cardid)
        //{
        //    Member output = null;
        //    string errorcode = null;
        //    try
        //    {
        //        ProjectTestBase.lWIntegrationSvcClientManager.ActivateCard(cardid, System.DateTime.Now, "Automation - Activate Card", string.Empty, out time);
        //    }
        //    catch (LWClientException e)
        //    {
        //        Logger.Info(e.ErrorCode);
        //        errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Info(e.Message);
        //        Logger.Info(e.GetType());
        //    }
        //    if (errorcode != null)
        //    {
        //        return errorcode;
        //    }
        //    return output;
        //}

        /// <summary>
        /// Locking down the member using CDIS call by passing Updated Member Status Reason more than 255 characters with negative scenario
        /// </summary>
        /// <param name="UpdatedMemberStatusReason">UpdatedMemberStatusReason is passed aas parameter</param>
        /// <param name="loyaltyId">Loyalty Id is passed as a parameter</param>
        /// <returns></returns>
        //public Object LockDownMemberUpdatedMemberStatusReason(string UpdatedMemberStatusReason, string loyaltyId)
        //{
        //    Member output = null;
        //    string errorcode = null;
        //    try
        //    {
        //        ProjectTestBase.lWIntegrationSvcClientManager.LockdownMember(loyaltyId, DateTime.Now, UpdatedMemberStatusReason, String.Empty, out time);
        //    }
        //    catch (LWClientException e)
        //    {
        //        Logger.Info(e.ErrorCode);
        //        errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Info(e.Message);
        //        Logger.Info(e.GetType());
        //    }
        //    if (errorcode != null)
        //    {
        //        return errorcode;
        //    }
        //    return output;
        //}

        /// <summary>
        /// This Method is used to Cancel Card
        /// </summary>
        /// <returns></returns>

        public void CancelCard(string loyaltyIdNumber, DateTime? effectivedate, string updatecardstatusreason, bool? deactivatemember, out double time)
        {
            ProjectTestBase.lWIntegrationSvcClientManager.CancelCard(loyaltyIdNumber, effectivedate, updatecardstatusreason, deactivatemember, string.Empty, out time);
        }

        /// <summary>
        /// This Method is used to Cancel Card
        /// </summary>
        /// <param name="loyaltyIdNumber"></param>
        public void CancelCard(string loyaltyIdNumber, out double time)
        {
            ProjectTestBase.lWIntegrationSvcClientManager.CancelCard(loyaltyIdNumber, null, "CDIS Service", false, string.Empty, out time);
        }

        /// <summary>
        /// This method is used to cancel a card with invalid data
        /// </summary>
        /// <param name="loyaltyIdNumber"></param>
        /// <param name="effectivedate"></param>
        /// <param name="updatecardstatusreason"></param>
        /// <param name="deactivatemember"></param>
        /// <param name="time"></param>
        public string CancelCard_Invalid(string loyaltyIdNumber, DateTime? effectivedate, string updatecardstatusreason, bool? deactivatemember)
        {
            string message = "";
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.CancelCard(loyaltyIdNumber, effectivedate, updatecardstatusreason, deactivatemember, string.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                message = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return message;
        }

        /// <summary>
        /// This method is to Deactivate a member
        /// </summary>
        /// <param name="loyaltyIdNumber"></param>
        public void DeactivateMember(string loyaltyIdNumber)
        {
            ProjectTestBase.lWIntegrationSvcClientManager.DeactivateMember(loyaltyIdNumber, DateTime.Now, "SOAP_AUTOMATION", true, false, false, false, false, false, false, String.Empty, out time);
        }

        /// <summary>
        /// this method is to return the member tier array for the given loyalty id
        /// </summary>
        /// <param name="loyaltyID">This specifies the loyalty id of the member</param>
        /// <returns>array of memberTierStruct</returns>
        public MemberTierStruct[] GetMemberTier(String loyaltyID)
        {
            MemberTierStruct[] output = null;
            output = ProjectTestBase.lWIntegrationSvcClientManager.GetMemberTiers(loyaltyID, null, null, 0, 10, string.Empty, out time);
            return output;
        }

        /// <summary>
        /// Retrieving bonus definition using CDIS service for regression
        /// </summary>
        /// <param name="BonusDefId"></param>
        /// <param name="Channel"></param>
        /// <param name="Language"></param>
        /// <param name="ReturnAttributes"></param>
        /// <param name="ExternalId"></param>
        /// <param name="time"></param>
        /// <returns>Bonus Definition of a particular bonus</returns>
        public BonusDefinitionStruct GetBonusDefinition(string bonusDefId, string channel, string language, bool? returnAttributes, string externalId, double time)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetBonusDefinition(Convert.ToInt64(bonusDefId), channel, language, returnAttributes, externalId, out time);
        }

        /// <summary>
        /// Retrieving bonus definitions for all bonuses using CDIS service for regression
        /// </summary>
        /// <param name="Language"></param>
        /// <param name="Channel"></param>
        /// <param name="ActiveOnly"></param>
        /// <param name="contentSearchAttributes"></param>
        /// <param name="ReturnAttributes"></param>
        /// <param name="StartIndex"></param>
        /// <param name="BatchSize"></param>
        /// <param name="ExternalId"></param>
        /// <param name="time"></param>
        /// <returns>Bonus Definitions of all bonuses</returns>
        public BonusDefinitionStruct[] GetBonusDefinitions(string language, string channel, bool? activeOnly, ContentSearchAttributesStruct[] contentSearchAttributes, bool? returnAttributes, int startIndex, int batchSize, string externalId, out double time)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetBonusDefinitions(language, channel, activeOnly, contentSearchAttributes, returnAttributes, startIndex, batchSize, externalId, out time);
        }

        /// <summary>
        /// Retrieving bonus definition count using CDIS service for regression
        /// </summary>
        /// <param name="ActiveOnly"></param>
        /// <param name="contentSearchAttributes"></param>
        /// <param name="ExternalId"></param>
        /// <param name="time"></param>
        /// <returns>Bonus Definition Count</returns>
        public int GetBonusDefinitionCount(bool? ActiveOnly, ContentSearchAttributesStruct[] contentSearchAttributes, String ExternalId, out double time)
        {
            int output = ProjectTestBase.lWIntegrationSvcClientManager.GetBonusDefinitionCount(ActiveOnly, contentSearchAttributes, ExternalId, out time);
            return output;
        }

        /// <summary>
        /// Trying to retrieve bonus definition by passing null valuess for all fields using CDIS service for regression
        /// </summary>
        /// <returns>Error code</returns>
        public Object GetBonusDefinitionsNegative()
        {
            string errorcode = null;
            try
            {
                return ProjectTestBase.lWIntegrationSvcClientManager.GetBonusDefinition(Convert.ToInt64(null), null, null, null, null, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }

        /// <summary>
        /// Retrieving coupon definitions using CDIS service
        /// </summary>
        /// <returns>Coupon Definition</returns>
        public GetCouponDefinitionsOut GetCouponDefinitions()
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetCouponDefinitions(null, null, null, null, false, 1, 10, String.Empty, out time);
        }

        /// <summary>
        /// Retrieving coupon definitions using CDIS service
        /// </summary>
        /// <returns>Coupon Definition</returns>
        public GetCouponDefinitionsOut GetCouponDefinitions(string language, string channel, ContentSearchAttributesStruct[] contentsearchattributes, ActiveCouponOptionsStruct activecouponoptions, bool? returnattributes, int? pagenumber, int? resultsperpage, out double elapsedTime)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetCouponDefinitions(language, channel, contentsearchattributes, activecouponoptions, returnattributes, pagenumber, resultsperpage, String.Empty, out elapsedTime);
        }


        /// <summary>
        /// Retrieving coupon definitions using CDIS service
        /// </summary>
        /// <returns>Coupon Definition</returns>
        public CouponDefinitionStruct GetCouponDefinition(long couponID, string channel, string language, bool? returnAttributes, string externalID, out double elapsedTime)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetCouponDefinition(couponID, channel, language, returnAttributes, externalID, out elapsedTime);
        }

        /// <summary>
        /// Adding the coupon to a member using CDIS service
        /// </summary>
        /// <param name="loyaltyId">This specifies the loyalty id of the member</param>
        /// <param name="couponDefinitionId">This specifies the coupon definition id</param>
        /// <returns></returns>
        public long AddMemberCoupon(string loyaltyId, long couponDefinitionId)
        {
            long output = 0;
            output = ProjectTestBase.lWIntegrationSvcClientManager.AddMemberCoupon(loyaltyId, couponDefinitionId, DateTime.Now.AddMilliseconds(2000), null, null, null, string.Empty, out time);
            return output;
        }

        /// <summary>
        /// Adding promotion to a member using CDIS service
        /// </summary>
        /// <param name="loyaltyId">This specifies the loyalty id of the member</param>
        /// <param name="promotionCode">This specifies the promotion code of the promotion</param>
        public MemberPromotionStruct AddMemberPromotion(string loyaltyId, string promotionCode)
        {
            MemberPromotionStruct output = null;
            output = ProjectTestBase.lWIntegrationSvcClientManager.AddMemberPromotion(loyaltyId, promotionCode, null, true, null, "Web", false, string.Empty, out time);
            return output;
        }

        /// <summary>
        /// Adding promotion to a member 
        /// </summary>
        /// <param name="loyaltyId"></param>
        /// <param name="promotionCode"></param>
        /// <param name="certificatenmbr"></param>
        /// <param name="returndefinition"></param>
        /// <param name="language"></param>
        /// <param name="channel"></param>
        /// <param name="returnattributes"></param>
        /// <param name="externalId"></param>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>

        public MemberPromotionStruct AddMemberPromotion(string loyaltyId, string promotionCode, string certificatenmbr, bool? returndefinition, string language, string channel, bool? returnattributes, string externalId, out double elapsedTime)
        {
            MemberPromotionStruct output = null;
            output = ProjectTestBase.lWIntegrationSvcClientManager.AddMemberPromotion(loyaltyId, promotionCode, certificatenmbr, returndefinition, language, channel, returnattributes, externalId, out elapsedTime);
            return output;
        }
        ///<summary>
        ///This Method is Used to get the details of the Promotion count for the Members.
        ///</summary>
        /// <returns> promotions count for that number</returns>

        public int GetPromotionMembersCount(string LoyalityID)
        {
            int Output = ProjectTestBase.lWIntegrationSvcClientManager.GetMemberPromotionsCount(LoyalityID, null, String.Empty, out time);
            return Output;
        }

        /// <summary>
        /// updating the member using CDIS service by adding transaction  for requireddate
        /// </summary>
        /// <param name="member">Specifies the member</param>
        /// <param name="date">Specifies the date</param>
        /// <returns>txnHeader</returns>
        public String UpdateMember_AddTransactionRequiredDate(Member member, DateTime date)
        {
            Member imember = new Member();
            imember.AlternateId = member.AlternateId;
            imember.BirthDate = member.BirthDate;
            imember.ChangedBy = member.ChangedBy;
            imember.FirstName = member.FirstName;
            imember.IpCode = member.IpCode;
            imember.IsEmployee = member.IsEmployee;
            imember.LastActivityDate = member.LastActivityDate;
            imember.LastName = member.LastName;
            imember.MemberCloseDate = member.MemberCloseDate;
            imember.MemberCreateDate = member.MemberCreateDate;
            imember.MemberStatus = member.MemberStatus;
            imember.MiddleName = member.MiddleName;
            imember.NamePrefix = member.NamePrefix;
            imember.NameSuffix = member.NameSuffix;
            imember.Password = member.Password;
            imember.PreferredLanguage = member.PreferredLanguage;
            imember.PrimaryEmailAddress = member.PrimaryEmailAddress;
            imember.PrimaryPhoneNumber = member.PrimaryPhoneNumber;
            imember.PrimaryPostalCode = member.PrimaryPostalCode;
            imember.Username = member.Username;
            VirtualCard card = member.GetLoyaltyCard(member.GetLoyaltyCards()[0].LoyaltyIdNumber);
            int amount = Convert.ToInt32(common.RandomNumber(3));
            TxnHeader txnHeader = new TxnHeader()
            {
                TxnHeaderId = common.RandomNumber(7),
                TxnDate = date,
                TxnRegisterNumber = "123",
                TxnStoreId = Convert.ToInt32(common.RandomNumber(3)),
                TxnTypeId = 0,
                TxnAmount = amount
            };
            TxnDetailItem detail = new TxnDetailItem()
            {
                TxnHeaderId = txnHeader.TxnHeaderId,
                TxnDate = txnHeader.TxnDate,
                TxnStoreId = txnHeader.TxnStoreId,
                TxnDetailId = common.RandomNumber(6),
                DtlProductId = 1,
                DtlRetailAmount = amount,
                DtlSaleAmount = amount,
                DtlQuantity = 2,
                DtlDiscountAmount = 0,
                DtlClearanceItem = 0
            };
            txnHeader.Add(detail);
            card.Add(txnHeader);
            imember.Add(card);

            ProjectTestBase.lWIntegrationSvcClientManager.UpdateMember(imember, null, String.Empty, out time);
            return txnHeader.TxnHeaderId;
        }

        /// <summary>
        /// updating the member using CDIS service by adding transaction  for requireddate
        /// </summary>
        /// <param name="member">Specifies the member</param>
        /// <param name="date">Specifies the date</param>
        /// <returns>txnHeader object</returns>
        public TxnHeader UpdateMember_PostTransactionRequiredDate(Member member, DateTime date)
        {
            Member imember = new Member();
            imember.AlternateId = member.AlternateId;
            imember.BirthDate = member.BirthDate;
            imember.ChangedBy = member.ChangedBy;
            imember.FirstName = member.FirstName;
            imember.IpCode = member.IpCode;
            imember.IsEmployee = member.IsEmployee;
            imember.LastActivityDate = member.LastActivityDate;
            imember.LastName = member.LastName;
            imember.MemberCloseDate = member.MemberCloseDate;
            imember.MemberCreateDate = member.MemberCreateDate;
            imember.MemberStatus = member.MemberStatus;
            imember.MiddleName = member.MiddleName;
            imember.NamePrefix = member.NamePrefix;
            imember.NameSuffix = member.NameSuffix;
            imember.Password = member.Password;
            imember.PreferredLanguage = member.PreferredLanguage;
            imember.PrimaryEmailAddress = member.PrimaryEmailAddress;
            imember.PrimaryPhoneNumber = member.PrimaryPhoneNumber;
            imember.PrimaryPostalCode = member.PrimaryPostalCode;
            imember.Username = member.Username;
            VirtualCard card = member.GetLoyaltyCard(member.GetLoyaltyCards()[0].LoyaltyIdNumber);
            int amount = Convert.ToInt32(common.RandomNumber(3));
            TxnHeader txnHeader = new TxnHeader()
            {
                TxnHeaderId = common.RandomNumber(7),
                TxnDate = date,
                TxnRegisterNumber = "123",
                TxnStoreId = Convert.ToInt32(common.RandomNumber(3)),
                TxnTypeId = 0,
                TxnAmount = amount
            };
            TxnDetailItem detail = new TxnDetailItem()
            {
                TxnHeaderId = txnHeader.TxnHeaderId,
                TxnDate = txnHeader.TxnDate,
                TxnStoreId = txnHeader.TxnStoreId,
                TxnDetailId = common.RandomNumber(6),
                DtlProductId = 1,
                DtlRetailAmount = amount,
                DtlSaleAmount = amount,
                DtlQuantity = 2,
                DtlDiscountAmount = 0,
                DtlClearanceItem = 0
            };
            txnHeader.Add(detail);
            card.Add(txnHeader);
            imember.Add(card);

            ProjectTestBase.lWIntegrationSvcClientManager.UpdateMember(imember, null, String.Empty, out time);
            return txnHeader;
        }

        ///<summary>
        ///Retrieving Coupn Definition with Attributes
        ///</summary>

        public GetCouponDefinitionsOut GetCouponDefinitionsWithAttributes()
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetCouponDefinitions(null, null, null, null, true, 1, 5, String.Empty, out time);
        }

        /// <summary>
        /// This method is to service call the promotion definitions with return attributes
        /// </summary>
        /// <returns>Array of PromotionDefinitionStruct</returns>
        public PromotionDefinitionStruct[] GetPromotionDefinitionsWithReturnAttributes()
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetPromotionDefinitions(null, "Web", true, null, true, 0, 5, new string[0], null, out time);
        }


        /// <summary>
        /// Updating the member with updated details
        /// </summary>
        /// <param name="member">Required MemberObject</param>
        /// <returns>updated member Object</returns>
        public Member UpdateMemberGeneral(Member member)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.UpdateMember(member, null, String.Empty, out time);
        }

        /// <summary>
        /// This Method is for getting the Reward Catalog
        /// </summary>
        /// <returns>returns an array of reward catalog items based on number specified in batchsize</returns>
        public RewardCatalogSummaryStruct[] GetRewardCatalog()
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetRewardCatalog(true, null, null, "Web", null, true, null, 0, 10, null, null, string.Empty, out time);

        }
        /// <summary>
        /// This Method is for getting the Reward Catalog Count
        /// </summary>
        /// <returns></returns>

        public int GetRewardCatalogCount()
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetRewardCatalogCount(true, null, null, null, string.Empty, out time);

        }
        /// <summary>
        /// This method is to return the total number of promotions from GetPromotionDefinitionsCount service call
        /// </summary>
        /// <returns>An integer value of promotions count</returns>
        public int GetTotalPromotionsDefinitionsCount()
        {
            int output = ProjectTestBase.lWIntegrationSvcClientManager.GetPromotionDefinitionsCount(false, null, null, null, out time);
            return output;
        }
        /// <summary>
        /// Following method is to return only promotions count from GetActivePromotionsDefinitionsCount service call
        /// </summary>
        /// <returns>An integer value of promotions count of only active promotions</returns>
        public int GetActivePromotionsDefinitionsCount()
        {
            int output = ProjectTestBase.lWIntegrationSvcClientManager.GetPromotionDefinitionsCount(true, null, null, null, out time);
            return output;
        }

        /// <summary>
        /// This method provides the promotion definitions without return attributes.
        /// </summary>
        /// <returns>promotion definitions struct array</returns>
        public PromotionDefinitionStruct[] GetPromotionDefinitions()
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetPromotionDefinitions(null, "Web", true, null, false, 0, 10, new string[0], null, out time);
        }

        /// <summary>
        /// This method is to fetch the recent 5 promotion definitions 
        /// </summary>
        /// <param name="StartIndex">From which index u need to search</param>
        /// <returns>promotion definition array</returns>
        public PromotionDefinitionStruct[] GetPromotionDefinitionsRecent(int StartIndex)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetPromotionDefinitions(null, "Web", true, null, false, StartIndex, 10, new string[0], null, out time);
        }
        ///<summary>
        ///This Method is Used to get the details of the Enrolled Promotion Members.
        ///</summary>
        ///

        public MemberPromotionStruct EnrolledPromotionMember(string LoyalityID, string PromotionCode)
        {
            MemberPromotionStruct Output = ProjectTestBase.lWIntegrationSvcClientManager.EnrollMemberPromotion(LoyalityID, PromotionCode, false, false, null, "Web", true, String.Empty, out time);
            return Output;
        }

        /// <summary>
        /// This Method is to get Promotion list of particular member
        /// </summary>
        /// <param name="loyaltyIdNumber">This Specifies the loyality id of the member</param>
        public GetMemberPromotionsOut GetMemberPromotions(string loyaltyIdNumber)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetMemberPromotions(loyaltyIdNumber, 0, 10, false, false, null, "Web", false, false, null, null, string.Empty, out time);
        }

        /// <summary>
        /// This method is to return the member with existing VCkey from GetCDISMemberExistingVC service call
        /// </summary>
        /// <returns>IPCode and the member details</returns>
        public Object GetCDISMemberExistingVC()
        {
            Member output = null;
            string errorcode = null;
            try
            {
                output = ProjectTestBase.lWIntegrationSvcClientManager.AddMember(common.GenerateAddMemberForSOAPExistingVC(), new string[0], System.Guid.NewGuid().ToString(), out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }

        /// <summary>
        /// This method is to return the member with an invalid First Name from GetCDISMemberInvalidFN service call
        /// </summary>
        /// <returns>IPCode and the member details</returns>
        public Object GetCDISMemberInvalidFN()
        {
            Member output = null;
            string errorcode = null;
            try
            {
                Member member = AddCDISMemberWithAllFields();
                member.FirstName = common.RandomString(51);
                output = ProjectTestBase.lWIntegrationSvcClientManager.AddMember(member, new string[0], System.Guid.NewGuid().ToString(), out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;

            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }

        internal void DeactivateMember(string loyaltyIdNumber, string statusReason)
        {
            ProjectTestBase.lWIntegrationSvcClientManager.DeactivateMember(loyaltyIdNumber, DateTime.Now.AddHours(-12), statusReason, true, true, true, true, true, true, true, String.Empty, out time);
        }

        /// <summary>
        /// This method is to return the member details by providing member id from GetCDISMemberById service call
        /// </summary>
        /// <param name="Id">Specifies the Card Id</param>
        /// <returns>IPCode and the member details</returns>
        public Member[] GetCDISMemberById(String Id)
        {
            String[] type = new string[] { MemberSearchIdentity.MemberID.ToString() };
            string[] value = new string[] { Id };
            Member[] output = ProjectTestBase.lWIntegrationSvcClientManager.GetMembers(type, value, null, null, String.Empty, out time);
            return output;
        }

        /// <summary>
        /// This method is to return the member details by providing card id from GetCDISMemberByCardId service call
        /// </summary>
        /// <param name="Id">Specifies the Card Id</param>
        /// <returns>IPCode and the member details</returns>
        public Member[] GetCDISMemberByCardId(String Id)
        {
            String[] type = new string[] { MemberSearchIdentity.CardID.ToString() };
            string[] value = new string[] { Id };
            Member[] output = ProjectTestBase.lWIntegrationSvcClientManager.GetMembers(type, value, null, null, String.Empty, out time);
            return output;
        }

        /// <summary>
        /// This method is to return the member details by providing email address from GetCDISMemberByEmailAddress service call
        /// </summary>
        /// <param name="Email">Specifies the Email Id</param>
        /// <returns>IPCode and the member details</returns>
        public Member[] GetCDISMemberByEmailAddress(String Email)
        {
            String[] type = new string[] { MemberSearchIdentity.EmailAddress.ToString() };
            string[] value = new string[] { Email };
            Member[] output = ProjectTestBase.lWIntegrationSvcClientManager.GetMembers(type, value, null, null, String.Empty, out time);
            return output;
        }

        /// <summary>
        /// This method is to return the member details by providing phone number from GetCDISMemberByPhoneNumber service call
        /// </summary>
        /// <param name="Pno">Specifies the Phone number/param>
        /// <returns>IPCode and the member details</returns>
        public Member[] GetCDISMemberByPhoneNumber(String Pno)
        {
            String[] type = new string[] { MemberSearchIdentity.PhoneNumber.ToString() };
            string[] value = new string[] { Pno };
            Member[] output = ProjectTestBase.lWIntegrationSvcClientManager.GetMembers(type, value, null, null, String.Empty, out time);
            return output;
        }

        /// <summary>
        /// This method is to return the member details by providing last name from GetCDISMemberByLastName service call
        /// </summary>
        /// <param name="Name">Specifies the Last Name</param>
        /// <returns>IPCode and the member details</returns>
        public Member[] GetCDISMemberByLastName(String Name)
        {
            String[] type = new string[] { MemberSearchIdentity.LastName.ToString() };
            string[] value = new string[] { Name };
            Member[] output = ProjectTestBase.lWIntegrationSvcClientManager.GetMembers(type, value, null, null, String.Empty, out time);
            return output;
        }

        /// <summary>
        /// This method is to return the member details by providing the username from GetCDISMemberByUsername service call
        /// </summary>
        /// <param name="Name">Specifies the Username</param>
        /// <returns>IPCode and the member details</returns>
        public Member[] GetCDISMemberByUsername(String Name)
        {
            String[] type = new string[] { MemberSearchIdentity.Username.ToString() };
            string[] value = new string[] { Name };
            Member[] output = ProjectTestBase.lWIntegrationSvcClientManager.GetMembers(type, value, null, null, String.Empty, out time);
            return output;
        }

        /// <summary>
        /// This method is to return the member details by providing the Postal Code from GetCDISMemberByPostalCode service call
        /// </summary>
        /// <param name="Pcd">Specifies the Postal Code</param>
        /// <returns>IPCode and the member details</returns>
        public Member[] GetCDISMemberByPostalCode(String Pcd)
        {
            String[] type = new string[] { MemberSearchIdentity.PostalCode.ToString() };
            string[] value = new string[] { Pcd };
            Member[] output = ProjectTestBase.lWIntegrationSvcClientManager.GetMembers(type, value, null, null, String.Empty, out time);
            return output;
        }

        ///// <summary>
        ///// This method is to deactivate the card by using the Loyalty Id from DeActivateCardUsingLoyaltyID service call
        ///// </summary>
        ///// <param name="cardid">Specifies the Card Id</param>
        //public void DeActivateCardUsingLoyaltyID(String cardid)
        //{
        //    ProjectTestBase.lWIntegrationSvcClientManager.DeactivateCard(cardid, System.DateTime.Now, "Automation - Deactivate Card", string.Empty, out time);
        //}

        /// <summary>
        /// This methods deactivates a card using Loyalty Id
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="date"></param>
        /// <param name="cardStatusReason"></param>
        /// <returns>If there is not exception then it returns 'pass' else returns error code and message</returns>
        public string DeActivateCardUsingLoyaltyID(string cardId, DateTime? date, string cardStatusReason)
        {
            string status = "fail";
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.DeactivateCard(cardId, date, cardStatusReason, string.Empty, out time);
                status = "pass";
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                status = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return status;
        }

        ///// <summary>
        ///// This method is to deactivate the card by using the Loyalty Id where date is null from DeActivateCardUsingLoyaltyIDWhereDateIsNull service call
        ///// </summary>
        ///// <param name="cardid">Specifies the Card Id</param>
        //public void DeActivateCardUsingLoyaltyIDWhereDateIsNull(String cardid)
        //{
        //    ProjectTestBase.lWIntegrationSvcClientManager.DeactivateCard(cardid, null, "Automation - Deactivate Card", string.Empty, out time);
        //}

        ///// <summary>
        ///// This method is to deactivate the card by using the Loyalty Id where Update Status Reason is null from DeActivateCardUsingLoyaltyIDWhereUpdateStatusReasonIsNull service call
        ///// </summary>
        ///// <param name="cardid">Specifies the Card Id</param>
        //public void DeActivateCardUsingLoyaltyIDWhereUpdateStatusReasonIsNull(String cardid)
        //{
        //    ProjectTestBase.lWIntegrationSvcClientManager.DeactivateCard(cardid, System.DateTime.Now, null, string.Empty, out time);
        //}

        ///// <summary>
        ///// This method is to deactivate the card 
        ///// </summary>
        ///// <param name="cardid"></param>
        ///// <param name="date"></param>
        ///// <param name="reason"></param>
        //public void DeActivateCard(string cardid, DateTime? date, string reason)
        //{
        //    reason = (reason == null) ? null : "SOAP-Automation";
        //    ProjectTestBase.lWIntegrationSvcClientManager.DeactivateCard(cardid, date, reason, string.Empty, out time);
        //}

        /// <summary>
        /// This method is to activate the card by using the Loyalty Id from ActivateCardUsingLoyaltyID service call
        /// </summary>
        /// <param name="cardid">Specifies the card id</param>
        public string ActivateCardUsingLoyaltyID(string cardid)
        {
            string msg = string.Empty;
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.ActivateCard(cardid, System.DateTime.Now, "Automation - Activate Card", string.Empty, out time);
                msg = "The Loyalty Card [" + cardid + "] has been activated";
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                msg = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return msg;
        }

        /// <summary>
        /// This method is to activate the card by using the Loyalty Id from ActivateCardUsingLoyaltyID service call
        /// </summary>
        /// <param name="cardid">Specifies the card id</param>
        /// <param name="time1">Elapsed time</param>
        public void ActivateCardUsingLoyaltyIDToVerifyElapsedTime(string cardid, out double time)
        {
            ProjectTestBase.lWIntegrationSvcClientManager.ActivateCard(cardid, System.DateTime.Now, "SOAP - Automation_Activate Card", string.Empty, out time);
        }

        /// <summary>
        /// This method is to change the card expiration date by providing Card Id from ChangeCardExpirationDate service call
        /// </summary>
        /// <param name="cardid">Specifies the Card Id</param>
        /// <returns></returns>
        public DateTime ChangeCardExpirationDate(String cardid)
        {
            DateTime newDate = DateTime.Now.AddYears(1);
            ProjectTestBase.lWIntegrationSvcClientManager.ChangeCardExpirationDate(cardid, newDate, string.Empty, out time);
            return newDate;
        }

        /// <summary>
        /// This method is to change the card expiration date by providing Card Id from ChangeCardExpirationDate service call
        /// </summary>
        /// <param name="cardid">Specifies the Card Id</param>
        /// <returns></returns>
        public DateTime ChangeCardExpirationDateToNow(string cardid)
        {
            DateTime newDate = DateTime.Now;
            ProjectTestBase.lWIntegrationSvcClientManager.ChangeCardExpirationDate(cardid, newDate, string.Empty, out time);
            return newDate;
        }

        /// <summary>
        /// This method is to lock down the member's card by providing Card Id from LockDownMember service call
        /// </summary>
        /// <param name="cardid">Specifies the Card Id</param>
        //public void LockDownMember(string cardid)
        //{
        //    ProjectTestBase.lWIntegrationSvcClientManager.LockdownMember(cardid, DateTime.Now, "SOAP_Automation", string.Empty, out time);
        //}

        public string LockDownMember(string cardId, DateTime? date, string updateMemberStatusReason, string externalId, out double time)
        {
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.LockdownMember(cardId, date, updateMemberStatusReason, externalId, out time);
                output = "pass";
            }
            catch (LWClientException e)
            {
                time = -1;
                Logger.Info(e.ErrorCode);
                output = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                time = -1;
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return output;
        }

        /// <summary>
        /// This method is to activate the member's card by providing Card Id from ActivateMember service call
        /// </summary>
        /// <param name="cardid">Specifies the Card Id</param>
        public void ActivateMember(String cardid)
        {
            ProjectTestBase.lWIntegrationSvcClientManager.ActivateMember(cardid, DateTime.Now, "SOAP_Automation", true, string.Empty, out time);
        }



        /// <summary>
        /// To activate member with Invalid member Identity
        /// </summary>
        /// <returns></returns>
        public string ActivateMemberInvalidMemberIdentity()
        {
            string memberIdentity = "";
            try
            {
                memberIdentity = common.RandomNumber(7);
                ProjectTestBase.lWIntegrationSvcClientManager.ActivateMember(memberIdentity, null, null, true, null, out time);
            }
            catch (LWClientException e)
            {
                exception = e;
                Logger.Info(e.ErrorCode);
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            finally
            {
                if (exception.ErrorCode == 3302 && exception.Message.Contains("Unable to find member with identity = " + memberIdentity))
                {
                    output = "Error code received as expected: " + exception.ErrorCode + " Error message is: " + exception.Message;
                }
                else
                {
                    throw new Exception("Invalid response received" + exception.ErrorCode);
                }
            }
            return output;
        }

        /// <summary>
        /// This Method is for getting the Active Reward Catalog Count
        /// </summary>
        /// <returns></returns>

        public int GetActiveRewardCatalogCount()
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetRewardCatalogCount(true, null, null, null, string.Empty, out time);

        }
        /// <summary>
        /// This Method is for getting the Recent Reward Catalog
        /// </summary>
        /// <returns></returns>
        public RewardCatalogSummaryStruct[] GetRecentRewardCatalog(int startIndex, decimal? currencyToEarnLow, decimal? currencyToEarnHigh)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetRewardCatalog(true, null, null, "Web", null, true, null, startIndex, 10, currencyToEarnLow, currencyToEarnHigh, string.Empty, out time);

        }

        /// <summary>
        /// This Method is for getting the Add Memeber Reward
        /// </summary>
        /// <returns>object</returns>
        public Object AddMemberRewards(string loyalityID, string CardID, RewardCatalogSummaryStruct reward)
        {
            string errorcode = null;
            AddMemberRewardsOut output = null;
            try
            {
                RewardOrderInfoStruct[] order = new RewardOrderInfoStruct[1];
                if (reward == null)
                {
                    order = null;
                }
                else if (reward.RewardName.Equals("blank"))
                {
                    order = new RewardOrderInfoStruct[1];
                    order[0] = new RewardOrderInfoStruct();
                    order[0].RewardName = "";
                    order[0].CertificateNumber = "";
                    order[0].TypeCode = "";
                    order[0].ExpirationDate = null;
                    order[0].VariantPartNumber = "";
                }
                else
                {
                    order[0] = new RewardOrderInfoStruct();
                    order[0].RewardName = reward.RewardName;
                    order[0].TypeCode = reward.TypeCode;
                }
                output = ProjectTestBase.lWIntegrationSvcClientManager.AddMemberRewards(loyalityID, CardID, null, null, null, null, null, null, null, null, null, null, null, null, "Web", "CDISAutomation", order, string.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }

        /// <summary>
        /// This method is for getting add member rewards
        /// </summary>
        /// <param name="loyalityID">Loyalty Id of the member</param>
        /// <param name="reward">Reward of the member</param>
        /// <param name="time1">Elapsed Time</param>
        /// <returns>Object</returns>
        public AddMemberRewardsOut AddMemberRewardsWithElapsedTime(string loyalityID, RewardCatalogSummaryStruct reward, out double time1)
        {
            RewardOrderInfoStruct[] order = new RewardOrderInfoStruct[1];
            order[0] = new RewardOrderInfoStruct();
            order[0].RewardName = reward.RewardName;
            order[0].TypeCode = reward.TypeCode;
            return ProjectTestBase.lWIntegrationSvcClientManager.AddMemberRewards(loyalityID, loyalityID, null, null, null, null, null, null, null, null, null, null, null, null, "Web", "CDISAutomation", order, string.Empty, out time1);
        }

        /// <summary>
        /// This Method is for getting Rewards Summary
        /// </summary>
        /// <param name="loyaltyIdNumber"></param>
        /// <returns></returns>

        public MemberRewardSummaryStruct[] GetMemberRewardsSummary(string loyaltyIdNumber)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetMemberRewardsSummary(loyaltyIdNumber, null, null, null, 0, 10, true, true, null, "Web", string.Empty, out time);
        }

        /// <summary>
        /// This Method is for getting members Rewards Summary
        /// </summary>
        /// <param name="loyaltyIdNumber"></param>
        /// <returns>returns MemberRewardOrderStruct[]</returns>
        public MemberRewardOrderStruct[] GetMemberRewards(string loyaltyIdNumber)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetMemberRewards(loyaltyIdNumber, null, null, null, false, false, 0, 10, string.Empty, out time);

        }

        /// <summary>
        /// This Method is used to get account summary
        /// </summary>
        /// <returns>returns account summary</returns>

        public GetAccountSummaryOut GetAccountSummary(string LoyaltyIDCard)
        {
            GetAccountSummaryOut getAccountSummary;
            getAccountSummary = ProjectTestBase.lWIntegrationSvcClientManager.GetAccountSummary(LoyaltyIDCard, string.Empty, out time);
            return getAccountSummary;
        }

        /// <summary>
        /// This Method is for getting the Reward Catalog Item
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public RewardCatalogItemStruct GetRewardCatalogItem(long ID)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetRewardCatalogItem(ID, null, "Web", true, string.Empty, out time);
        }

        /// <summary>
        /// This Method is used to get Account Activity Details 
        /// </summary>
        /// <returns>returns AccountActivityDetailsStruct[]</returns>

        public AccountActivityDetailsStruct[] GetAccountActivityDetails(String txnHeaderId)
        {
            AccountActivityDetailsStruct[] k;
            k = ProjectTestBase.lWIntegrationSvcClientManager.GetAccountActivityDetails(txnHeaderId, true, string.Empty, out time);
            return k;
        }

        /// <summary>
        /// This Method is used to get Loyalty Currency
        /// </summary>
        /// <returns>returns arrey of LoyaltyCurrencyStruct with currency types</returns>

        public LoyaltyCurrencyStruct[] GetLoyalityCurrencies()
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetLoyaltyCurrencies(string.Empty, out time);
        }

        /// <summary>
        /// This Method is for Checking MemeberCoupon is Redeemable
        /// </summary>
        /// <param name="id">memberCouponid</param>
        /// <returns></returns>
        public IsMemberCouponRedeemableOut IsMemberCouponRedeemable(long id)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.IsMemberCouponRedeemable(id, null, null, string.Empty, out time);
        }

        /// <summary>
        /// This Method is used to get Redeem Member reward
        /// </summary>
        /// <returns></returns>
        public void RedeemMemberReward(string MemberRewardID)
        {
            ProjectTestBase.lWIntegrationSvcClientManager.RedeemMemberReward(Convert.ToInt64(MemberRewardID), 50, null, System.DateTime.Now.AddDays(-1), string.Empty, out time);
        }

        /// <summary>
        /// This Method is used to get Redeem Member reward
        /// </summary>
        /// <param name="MemberRewardID"></param>
        /// <param name="AvailableBalance"></param>
        /// <param name="ExpirationDate"></param>
        /// <param name="RedemptionDate"></param>
        /// <param name="time1"></param>
        public void RedeemMemberReward(string MemberRewardID, decimal? availableBalance, DateTime expirationDate, DateTime? redemptionDate, out double time1)
        {
            ProjectTestBase.lWIntegrationSvcClientManager.RedeemMemberReward(Convert.ToInt64(MemberRewardID), availableBalance, expirationDate, redemptionDate, string.Empty, out time1);
        }

        /// <summary>
        /// This Method is used to get Redeem Member reward with null available balance
        /// </summary>
        /// <param name="MemberRewardID"></param>
        /// <param name="time1"></param>
        //public void RedeemMemberRewardWithNullAvailableBalance(string MemberRewardID, out double time1)
        //{
        //    ProjectTestBase.lWIntegrationSvcClientManager.RedeemMemberReward(Convert.ToInt64(MemberRewardID), null, null, System.DateTime.Now.AddDays(-1), null, string.Empty, out time1);
        //}

        /// <summary>
        /// This Method is used to get Redeem Member reward with null redemption balance
        /// </summary>
        /// <param name="MemberRewardID"></param>
        /// <param name="time1"></param>
        //public void RedeemMemberRewardWithNullRedemptionDate(string MemberRewardID, out double time1)
        //{
        //    ProjectTestBase.lWIntegrationSvcClientManager.RedeemMemberReward(Convert.ToInt64(MemberRewardID), Convert.ToInt32(common.RandomNumber(3)), System.DateTime.Today.AddYears(5), null, null, string.Empty, out time1);
        //}

        /// <summary>
        /// This Method is used to get Redeem Member reward with decimal available balance
        /// </summary>
        /// <param name="MemberRewardID"></param>
        /// <param name="time1"></param>
        //public void RedeemMemberRewardWithDecimalAvailableBalance(string MemberRewardID, out double time1)
        //{
        //    string Value = common.RandomNumber(2) + "." + common.RandomNumber(2);
        //    decimal DecimalValue = decimal.Parse(Value);
        //    ProjectTestBase.lWIntegrationSvcClientManager.RedeemMemberReward(Convert.ToInt64(MemberRewardID), DecimalValue, null, System.DateTime.Now.AddDays(-1), null, string.Empty, out time1);
        //}

        /// <summary>
        /// This method is used to redeem member reward by providing invalid reward id
        /// </summary>
        /// <returns></returns>
        public Object RedeemMemberRewardNegative()
        {
            string errorcode = null;
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.RedeemMemberReward(Convert.ToInt32(common.RandomNumber(4)), Convert.ToInt32(common.RandomNumber(3)), System.DateTime.Today.AddYears(5), System.DateTime.Now.AddDays(-1), string.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }

        /// <summary>
        /// This method is used to unredeem member reward by providing invalid certificate number 
        /// </summary>
        /// <returns></returns>
        public Object UnredeemMemberCouponByCertNmbrNegative()
        {
            string errorcode = null;
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.UnredeemMemberCouponByCertNmbr(common.RandomNumber(8), string.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }

        /// <summary>
        /// This Method is used to get Member reward Redeemed
        /// </summary>
        /// <returns>true if reward is redeemed else false</returns>

        public bool IsMemberRewardRedeemed(string ID, out double time1)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.IsMemberRewardRedeemed(Convert.ToInt64(ID), string.Empty, out time1);
        }

        /// <summary>
        /// This method is to verify whether the member reward is redeemed or not by providing invalid reward id
        /// </summary>
        /// <returns>Error code</returns>
        public Object IsMemberRewardRedeemedIDNegative()
        {
            string errorcode = null;
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.IsMemberRewardRedeemed(Convert.ToInt32(common.RandomNumber(4)), string.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }

        /// <summary>
        /// This method is used to trigger an event manually
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>returns userEventOut object with details of triggerevent</returns>
        public TriggerUserEventOut UserTriggerEvent(string ID)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.TriggerUserEvent(ID, "IssueMessage", "WEB", null, null, null, null, null, null, null, null, true, null, null, string.Empty, out time);
        }

        /// <summary>
		/// This Method is used to get Member Messages
		/// </summary>
		/// <returns>returns members message details</returns>
        public GetMemberMessagesOut GetMemberMessages(string ID)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetMemberMessages(ID, null, "WEB", null, null, null, null, 1, 10, null, string.Empty, out time);
        }

        /// <summary>
        /// This Method is used to get Account Activity Summary
        /// </summary>
        /// <returns></returns>

        public GetAccountActivitySummaryOut GetAccountActivitySummary(string loyaltyIdNumber)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetAccountActivitySummary(loyaltyIdNumber, null, null, true, true, 0, 5, 0, 5, string.Empty, out time);
        }
        /// <summary>
        /// This Method is to get all coupons of the members
        /// </summary>

        public MemberCouponStruct[] GetMemberCoupons(string LoyalityID)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetMemberCoupons(LoyalityID, false, null, "WEB", null, 0, 10, true, String.Empty, out time);
        }
        /// <summary>
        /// This Method is to Redeem Member Coupon 
        /// </summary>
        /// <param name="Id">Member Coupon ID</param>
        /// <returns></returns>
        public RedeemMemberCouponByIdOut RedeemMemberCouponById(long id)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.RedeemMemberCouponById(id, "Web", string.Empty, null, null, true, true, string.Empty, out time);
        }

        /// <summary>
        /// This Method is to Redeem Member Coupon only using the memberCouponId
        /// </summary>
        /// <param name="Id">Member Coupon ID</param>
        /// <returns></returns>
        public RedeemMemberCouponByIdOut RedeemMemberCouponByIdx(long Id)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.RedeemMemberCouponById(Id, null, null, null, null, null, null, null, out time);
        }




        /// <summary>
        /// This Method is to UnRedeem Member Coupon 
        /// </summary>
        /// <param name="Id">Member Coupon ID</param>
        /// <returns></returns>
        public void UnRedeemMemberCouponById(long id)
        {
            ProjectTestBase.lWIntegrationSvcClientManager.UnredeemMemberCouponById(id, string.Empty, out time);
        }


        /// <summary>
        /// This Method is to UnRedeem Member Coupon Negative
        /// </summary>
        /// <param name="Id">Member Coupon ID</param>
        /// <returns></returns>
        public object UnRedeemMemberCouponByIdNegative(long id)
        {
            string errorcode = null;
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.UnredeemMemberCouponById(id, string.Empty, out time);

            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }

        /// <summary>
        /// This Method is to UnRedeem Member Coupon Negative
        /// </summary>
        /// <param name="Id">Member Coupon ID</param>
        /// <returns></returns>
        public object RedeemMemberCouponByIdNegative(long id)
        {
            string errorcode = null;
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.RedeemMemberCouponById(id, null, null, null, null, null, null, null, out time);

            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }

        /// <summary>
        /// This Method is used to get Loyalty Events
        /// </summary>
        /// <returns></returns>
        public LoyaltyEventStruct[] GetLoyalityEvents()
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetLoyaltyEvents(string.Empty, out time);
        }

        /// <summary>
        /// This Method is for getting the Loyalty Currency Balance
        /// </summary>
        /// <returns></returns>
        public decimal GetLoyaltyCurrencyBalance(string LoyaltyID)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetLoyaltyCurrencyBalance(LoyaltyID, null, null, null, null, null, null, null, null, null, null, null, null, false, false, string.Empty, out time);
        }

        /// <summary>
        /// Method is used to get the Passwordreset options
        /// </summary>
        /// <param name="identityType"></param>
        /// <param name="loyaltyID"></param>
        /// <returns>PasswordResetOptionsOut object with password reset options</returns>
        public PasswordResetOptionsOut GetPasswordResetOptionsUsingLoyaltyID(string loyaltyID)
        {
            string identityType = "loyaltyidnumber";
            return ProjectTestBase.lWIntegrationSvcClientManager.PasswordResetOptions(identityType, loyaltyID, null, out time);
        }

        /// <summary>
        /// The method is used to award loyalty currency to a member
        /// </summary>
        /// <param name="LoyaltyNumber"></param>
        /// <param name="eventType"></param>
        /// <param name="currencyType"></param>
        /// <returns>loyalty currency</returns>
        public AwardLoyaltyCurrencyOut AwardLoyaltyCurrency(string LoyaltyNumber, LoyaltyEvents eventType, LoyaltyCurrency currencyType)
        {
            decimal amount = Convert.ToDecimal(common.RandomNumber(3));
            DateTime lastDateOfYear = new DateTime(DateTime.Now.Year, 12, 31);
            return ProjectTestBase.lWIntegrationSvcClientManager.AwardLoyaltyCurrency(LoyaltyNumber, eventType.ToString(), currencyType.ToString(), amount, DateTime.Now.AddDays(-1), lastDateOfYear, null, "CDISService", string.Empty, out time);
        }

        //public AwardLoyaltyCurrencyOut AwardLoyaltyCurrency(string LoyaltyNumber)
        //{
        //    double time = 0;
        //    return ProjectTestBase.lWIntegrationSvcClientManager.AwardLoyaltyCurrency(LoyaltyNumber, "PurchaseActivity", "BasePoints", 400, DateTime.Now.AddDays(-1), DateTime.Now.AddYears(10), null, "CDISService", string.Empty, out time);
        //}


        /// This method is used to return the categories based on parentCategoryID
        /// </summary>
        /// <param name="parentCategoryID"></param>
        /// <returns>returns RewardCategoryStruct with category names and id's</returns>
        public RewardCategoryStruct[] GetRewardCategories(long parentCategoryID)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetRewardCategories(parentCategoryID, true, null, null, null, out time);
        }

        /// <summary>
        /// This Method is for to merge the members
        /// </summary>
        /// <returns></returns>

        public Member MergeMembers(string loyaltyIdNumber1, string loyaltyIdNumber2)
        {
            MemberMergeOptionsStruct MemberMergeOption = new MemberMergeOptionsStruct();
            MemberMergeOption.PointBalance = true;
            MemberMergeOption.MemberRewards = true;
            MemberMergeOption.MemberTiers = true;
            return ProjectTestBase.lWIntegrationSvcClientManager.MergeMembers(loyaltyIdNumber1, loyaltyIdNumber2, "MergeActivity", "BasePoints", DateTime.Now.AddYears(2), MemberMergeOption, string.Empty, out time);
        }
        /// <summary>
        /// This Method to cancel reward for the member through service
        /// </summary>
        /// <param name="Id"></param>
        public decimal CancelMemberReward(string Id, out double time)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.CancelMemberReward(Convert.ToInt64(Id), string.Empty, out time);
        }

        /// <summary>
        /// This Method is to cancel reward for the member by providing invalid reward id
        /// </summary>
        /// <returns></returns>
        public object CancelMemberRewardNegative()
        {
            string errorcode = null;
            try
            {
                return ProjectTestBase.lWIntegrationSvcClientManager.CancelMemberReward(Convert.ToInt32(common.RandomNumber(4)), String.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }


        /// <summary>
        /// This Method is for getting the Loyalty Currency Award
        /// </summary>
        /// <returns></returns>
        public AwardLoyaltyCurrencyOut AwardLoyaltyCurrency(string LoyaltyNumber)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.AwardLoyaltyCurrency(LoyaltyNumber, "PurchaseActivity", "BasePoints", 400, DateTime.Now.AddDays(-1), DateTime.Now.AddYears(10), null, "CDISService", string.Empty, out time);
        }


        /// <summary>
        /// This method is used to redeem member coupon by using certnumber
        /// </summary>
        /// <param name="certNumber"></param>
        /// <returns>RedeemMemberCouponByCertNmbrOut</returns>
        public RedeemMemberCouponByCertNmbrOut RedeemMemberCouponByCertNumber(string certNumber)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.RedeemMemberCouponByCertNmbr(certNumber, "Web", "en", System.DateTime.Now, null, false, false, string.Empty, out time);
        }

        /// <summary>
        /// This method is used to redeem membercoupon by using certnumber
        /// </summary>
        /// <param name="certnmbr"></param>
        /// <param name="channel"></param>
        /// <param name="language"></param>
        /// <param name="redemptiondate"></param>
        /// <param name="timesused"></param>
        /// <param name="returnattributes"></param>
        /// <param name="ignoreviolations"></param>
        /// <param name="externalId"></param>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        public RedeemMemberCouponByCertNmbrOut RedeemMemberCouponByCertNumber(string certnmbr, string channel, string language, DateTime? redemptiondate, int? timesused, bool? returnattributes, bool? ignoreviolations, string externalId, out double elapsedTime)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.RedeemMemberCouponByCertNmbr(certnmbr, channel, language, redemptiondate, timesused, returnattributes, ignoreviolations, string.Empty, out elapsedTime);
        }

        /// <summary>
        /// To Redeem member coupon based on Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="channel"></param>
        /// <param name="language"></param>
        /// <param name="redemptiondate"></param>
        /// <param name="timesused"></param>
        /// <param name="returnattributes"></param>
        /// <param name="ignoreviolations"></param>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        public RedeemMemberCouponByIdOut RedeemMemberCouponById(long id, string channel, string language, DateTime? redemptiondate, int? timesused, bool? returnattributes, bool? ignoreviolations, out double elapsedTime)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.RedeemMemberCouponById(id, channel, language, redemptiondate, timesused, returnattributes, ignoreviolations, string.Empty, out elapsedTime);
        }

        /// <summary>
        /// To Redeem member coupon based on Id with invalid data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="channel"></param>
        /// <param name="language"></param>
        /// <param name="redemptiondate"></param>
        /// <param name="timesused"></param>
        /// <param name="returnattributes"></param>
        /// <param name="ignoreviolations"></param>
        /// <returns></returns>
        public string RedeemMemberCouponById_Invalid(long id, string channel, string language, DateTime? redemptiondate, int? timesused, bool? returnattributes, bool? ignoreviolations)
        {
            string message = "";
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.RedeemMemberCouponById(id, channel, language, redemptiondate, timesused, returnattributes, ignoreviolations, string.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                message = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return message;
        }

        /// <summary>
        /// To Redeem member coupon based on Certificate number with invalid data
        /// </summary>
        /// <param name="certnmbr"></param>
        /// <param name="channel"></param>
        /// <param name="language"></param>
        /// <param name="redemptiondate"></param>
        /// <param name="timesused"></param>
        /// <param name="returnattributes"></param>
        /// <param name="ignoreviolations"></param>
        /// <param name="externalId"></param>
        /// <returns></returns>
        public string RedeemMemberCouponByByCertNumber_Invalid(string certnmbr, string channel, string language, DateTime? redemptiondate, int? timesused, bool? returnattributes, bool? ignoreviolations, string externalId)
        {
            string message = "";
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.RedeemMemberCouponByCertNmbr(certnmbr, channel, language, redemptiondate, timesused, returnattributes, ignoreviolations, string.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                message = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return message;
        }

        /// <summary>
        /// This method is used to unredeem membercoupon by using certnumber
        /// </summary>
        /// <param name="certNumber"></param>
        public void UnRedeemMemberCouponByCertNumber(string certNumber, out double time)
        {
            ProjectTestBase.lWIntegrationSvcClientManager.UnredeemMemberCouponByCertNmbr(certNumber, string.Empty, out time);
        }

        /// <summary>
        /// To active the member whose status is Terminated
        /// </summary>
        /// <param name="memberIdentity"></param>
        /// <returns></returns>
        public string ActivateMemberWithTerminatedStatus(String memberIdentity)
        {
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.ActivateMember(memberIdentity, null, null, null, null, out time);
            }
            catch (LWClientException e)
            {
                exception = e;
                Logger.Info(e.ErrorCode);
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            finally
            {
                if (exception.ErrorCode == 3303 && exception.Message.Contains("This member is in terminated status.  It cannot be re-activated"))
                {
                    output = "Error code received as expected: " + exception.ErrorCode + " Error message is: " + exception.Message;
                }
                else
                {
                    throw new Exception("Invalid response received" + exception.ErrorCode);
                }
            }
            return output;
        }

        /// <summary>
        /// This method is used to activate member with the mandatory field Member Identity
        /// </summary>
        /// <param name="cardid"></param>
        public void ActivateMemberWithMandatoryFields(String cardid)
        {
            DateTime? date = null;
            ProjectTestBase.lWIntegrationSvcClientManager.ActivateMember(cardid, date, String.Empty, true, String.Empty, out time);
        }
        /// <summary>
        /// To Terminate a team member
        /// </summary>
        /// <param name="memberIdentity"></param>
        //public void TerminateMember(String memberIdentity)
        //{
        //    double time = 0;
        //    ProjectTestBase.lWIntegrationSvcClientManager.TerminateMember(memberIdentity, DateTime.Now, "SOAP_Automation", String.Empty, out time);
        //}

        /// <summary>
        /// To Terminate a team member
        /// </summary>
        /// <param name="memberIdentity"></param>
        public string TerminateMember(String memberIdentity, DateTime? date, string updateMemberStatusReason, string externalId, out double time)
        {
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.TerminateMember(memberIdentity, date, updateMemberStatusReason, externalId, out time);
                output = "pass";
            }
            catch (LWClientException e)
            {
                time = -1;
                Logger.Info(e.ErrorCode);
                output = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                time = -1;
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
                output = "Error Message=" + e.Message;
            }
            return output;
        }

        /// <summary>
        /// To activate member with null values
        /// </summary>
        /// <returns></returns>
        public string ActivateMemberWithNullFields()
        {
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.ActivateMember(null, null, null, null, null, out time);
            }
            catch (LWClientException e)
            {
                exception = e;
                Logger.Info(e.ErrorCode);
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            finally
            {
                if (exception.ErrorCode == 2003 && exception.Message.Equals("MemberIdentity of ActivateMemberIn is a required property.  Please provide a valid value."))
                {
                    output = "Error code received as expected: " + exception.ErrorCode + " Error message is: " + exception.Message;
                }
                else
                {
                    throw new Exception("Invalid response received" + exception.ErrorCode);
                }
            }
            return output;
        }

        /// <summary>
        /// To activate a member whose status is merged
        /// </summary>
        /// <param name="memberIdentity"></param>
        /// <returns></returns>
        public string ActivateMemberWhoseStatusIsMerged(String memberIdentity)
        {
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.ActivateMember(memberIdentity, null, null, null, null, out time);
            }
            catch (LWClientException e)
            {
                exception = e;
                Logger.Info(e.ErrorCode);
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            finally
            {
                if (exception.ErrorCode == 3392 && exception.Message.Contains("This member is in merged status.  It cannot be activated anymore"))
                {
                    output = "Error code received as expected: " + exception.ErrorCode + " Error message is: " + exception.Message;
                }
                else
                {
                    throw new Exception("Invalid response received" + exception.ErrorCode);
                }
            }
            return output;
        }

        /// <summary>
        /// To deactivate the member with member identity as null
        /// </summary>
        /// <returns></returns>
        public string DeactivateMemberwithMemberIdentityNull()
        {
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.DeactivateMember(null, null, null, null, null, null, null, null, null, null, String.Empty, out time);
            }
            catch (LWClientException e)
            {
                exception = e;
                Logger.Info(e.ErrorCode);
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            finally
            {
                if (exception.ErrorCode == 2003 && exception.Message.Equals("MemberIdentity of DeactivateMemberIn is a required property.  Please provide a valid value."))
                {
                    output = "Error code received as expected: " + exception.ErrorCode + " Error message is: " + exception.Message;
                }
                else
                {
                    throw new Exception("Invalid response received" + exception.ErrorCode);
                }
            }
            return output;
        }

        /// <summary>
        /// To deactivate the member whose status is Terminated
        /// </summary>
        /// <param name="memberIdentity"></param>
        /// <returns></returns>
        public string DeactivateMemberWithTerminatedStatus(String memberIdentity)
        {
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.DeactivateMember(memberIdentity, null, null, null, null, null, null, null, null, null, String.Empty, out time);
            }
            catch (LWClientException e)
            {
                exception = e;
                Logger.Info(e.ErrorCode);
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            finally
            {
                if (exception.ErrorCode == 3303 && exception.Message.Contains("This member is already in terminated status.  It cannot be deactivated"))
                {
                    output = "Error code received as expected: " + exception.ErrorCode + " Error message is: " + exception.Message;
                }
                else
                {
                    throw new Exception("Invalid response received" + exception.ErrorCode);
                }
            }
            return output;
        }

        /// <summary>
        /// To deactivate the member whose status is merged
        /// </summary>
        /// <param name="memberIdentity"></param>
        /// <returns></returns>
        public string DeactivateMemberWhoseStatusIsMerged(String memberIdentity)
        {
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.DeactivateMember(memberIdentity, null, null, null, null, null, null, null, null, null, String.Empty, out time);
            }
            catch (LWClientException e)
            {
                exception = e;
                Logger.Info(e.ErrorCode);
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            finally
            {
                if (exception.ErrorCode == 3392 && exception.Message.Contains("This member is in merged status.  It cannot be deactivated"))
                {
                    output = "Error code received as expected: " + exception.ErrorCode + " Error message is: " + exception.Message;
                }
                else
                {
                    throw new Exception("Invalid response received" + exception.ErrorCode);
                }
            }
            return output;
        }

        /// <summary>
        /// To deactivate a member whose status is a non member
        /// </summary>
        /// <param name="memberIdentity"></param>
        /// <returns></returns>
        public string DeactivateMemberWhoseStatusIsNonMember(String memberIdentity)
        {
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.DeactivateMember(memberIdentity, null, null, null, null, null, null, null, null, null, String.Empty, out time);
            }
            catch (LWClientException e)
            {
                exception = e;
                Logger.Info(e.ErrorCode);
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            finally
            {
                if (exception.ErrorCode == 3393 && exception.Message.Contains("This entity is a non-member.  It cannot be deactivated"))
                {
                    output = "Error code received as expected: " + exception.ErrorCode + " Error message is: " + exception.Message;
                }
                else
                {
                    throw new Exception("Invalid response received" + exception.ErrorCode);
                }
            }
            return output;
        }

        /// <summary>
        /// This Method is used to authenticate member
        /// </summary>
        /// <param name="identityType"></param>
        /// <param name="identity"></param>
        /// <param name="password"></param>
        /// <param name="resetCode"></param>
        /// <returns>AuthenticateMemberOut</returns>
        public AuthenticateMemberOut AuthenticateMember(string identityType, string identity, string password, string resetCode, out double time)
        {
            //double time = 0;
            return ProjectTestBase.lWIntegrationSvcClientManager.AuthenticateMember(identityType, identity, password, resetCode, string.Empty, out time);
        }


        /// <summary>
        /// To deactivate the member with member identity not exists in Database
        /// </summary>
        /// <returns></returns>
        public string DeactivateMemberWithMemberIdentityNotExistsInDb()
        {
            string memberIdentity = "";
            try
            {
                memberIdentity = common.RandomNumber(7);
                string value = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", memberIdentity, "LOYALTYIDNUMBER", string.Empty);
                while (value == memberIdentity)
                {
                    memberIdentity = common.RandomNumber(7);
                    value = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", memberIdentity, "LOYALTYIDNUMBER", string.Empty);
                }
                ProjectTestBase.lWIntegrationSvcClientManager.DeactivateMember(memberIdentity, null, null, null, null, null, null, null, null, null, String.Empty, out time);
            }
            catch (LWClientException e)
            {
                exception = e;
                Logger.Info(e.ErrorCode);
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            finally
            {
                if (exception.ErrorCode == 3302 && exception.Message.Contains("Unable to find member with identity = " + memberIdentity))
                {
                    output = "Error code received as expected: " + exception.ErrorCode + " Error message is: " + exception.Message;
                }
                else
                {
                    throw new Exception("Invalid response received" + exception.ErrorCode);
                }
            }
            return output;
        }

        /// <summary>
        /// To add Member promotions with Invalid data
        /// </summary>
        /// <param name="loyaltyId"></param>
        /// <param name="promotionCode"></param>
        /// <param name="certificatenmbr"></param>
        /// <param name="returndefinition"></param>
        /// <param name="language"></param>
        /// <param name="channel"></param>
        /// <param name="returnattributes"></param>
        /// <param name="externalId"></param>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        public string AddMemberPromotionInvalid(string loyaltyId, string promotionCode, string certificatenmbr, bool returndefinition, string language, string channel, bool returnattributes, string externalId, out double elapsedTime)
        {
            string message = "";
            elapsedTime = 0;
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.AddMemberPromotion(loyaltyId, promotionCode, certificatenmbr, returndefinition, language, channel, returnattributes, externalId, out elapsedTime);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                message = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return message;
        }

        /// <summary>
        /// To associate social handles data for a member
        /// </summary>
        /// <param name="memberIdentity"></param>
        /// <param name="providerType"></param>
        /// <param name="providerUID"></param>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        public string AssociateMemberSocialHandles(string memberIdentity, string providerType, string providerUID, out double elapsedTime)
        {
            elapsedTime = 0;
            string msg = "Social Handles associated for a member added successfully with member identity: " + memberIdentity + " ,providerType: " + providerType + " and providerUID: " + providerUID;
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.AssociateMemberSocialHandles(memberIdentity, CDIS_DataGenerator.generateMemberSocialHandleStruct(providerType, providerUID), System.Guid.NewGuid().ToString(), out elapsedTime);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                msg = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return msg;
        }

        /// <summary>
        /// To get Social handles data for a Member
        /// </summary>
        /// <param name="memberIdentity"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public MemberSocialHandleStruct[] GetMemberSocialHandles(string memberIdentity, out string message)
        {
            MemberSocialHandleStruct[] socialHandlesData = null;
            message = "";
            try
            {
                socialHandlesData = ProjectTestBase.lWIntegrationSvcClientManager.GetMemberSocialHandles(memberIdentity, System.Guid.NewGuid().ToString(), out time);
                message = "Social handles for a member are retrived successfully with memberIdentity" + memberIdentity;
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                message = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return socialHandlesData;
        }


        /// <summary>
        /// To get a promotion code
        /// </summary>
        /// <returns></returns>
        public string getPromotionCode()
        {
            PromotionDefinitionStruct[] def = GetPromotionDefinitions();
            PromotionDefinitionStruct promot = new PromotionDefinitionStruct();
            foreach (PromotionDefinitionStruct pd in def)
            {
                if (pd.Targeted)
                {
                    promot = pd;
                    break;
                }
            }
            return promot.Code;
        }


        /// <summary>
        /// To get a Reward
        /// </summary>
        /// <returns></returns>
        public CouponDefinitionStruct getCoupon()
        {
            GetCouponDefinitionsOut def = GetCouponDefinitions();
            CouponDefinitionStruct[] coupondefintions = def.CouponDefinition;
            CouponDefinitionStruct reqCoupon = null;
            foreach (var coupon in coupondefintions)
            {
                if (coupon.UsesAllowed > 10)
                {
                    reqCoupon = coupon;
                    break;
                }
            }
            return reqCoupon;
        }


        /// <summary>
        /// To get a Reward
        /// </summary>
        /// <returns></returns>
        public RewardCatalogSummaryStruct getReward()
        {
            RewardCatalogSummaryStruct[] rewardCatalog = GetRecentRewardCatalog(0, 0, 0);
            RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
            foreach (RewardCatalogSummaryStruct r in rewardCatalog)
            {
                if (r.CurrencyToEarn == 0 && !r.RewardName.Contains("Welcome"))
                {
                    reward = r;
                    break;
                }
            }
            return reward;
        }

        /// <summary>
        /// To get a Reward
        /// </summary>
        /// <returns></returns>
        public RewardCatalogSummaryStruct getRewardWithTypeCode()
        {
            RewardCatalogSummaryStruct[] rewardCatalog = GetRecentRewardCatalog(0, 0, 0);
            RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
            foreach (RewardCatalogSummaryStruct r in rewardCatalog)
            {
                if (r.CurrencyToEarn == 0 && !string.IsNullOrEmpty(r.TypeCode))
                {
                    reward = r;
                    break;
                }
            }
            return reward;
        }

        /// <summary>
        /// This Method is for getting the Member Reward error code by providing reward order info as null
        /// </summary>
        /// <param name="loyalityID"> Loyalty id of the member</param>
        /// <param name="reward">Reward id of the member</param>
        /// <returns>error code</returns>
        //public Object AddMemberRewardWhereRewardOrderInfoIsNull(string loyalityID, RewardCatalogSummaryStruct reward)
        //{
        //    string errorcode = null;
        //    AddMemberRewardsOut output = null;
        //    try
        //    {
        //        RewardOrderInfoStruct[] order;
        //        if (reward == null)
        //        {
        //            order = null;

        //        }
        //        else
        //        {
        //            order = new RewardOrderInfoStruct[1];
        //            order[0] = new RewardOrderInfoStruct();
        //            order[0].RewardName = reward.RewardName;
        //            order[0].TypeCode = reward.TypeCode;
        //        }
        //        output = ProjectTestBase.lWIntegrationSvcClientManager.AddMemberRewards(loyalityID, loyalityID, null, null, null, null, null, null, null, null, null, null, null, null, "Web", "CDISAutomation", order, string.Empty, out time);
        //    }
        //    catch (LWClientException e)
        //    {
        //        Logger.Info(e.ErrorCode);
        //        errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Info(e.Message);
        //        Logger.Info(e.GetType());
        //    }
        //    if (errorcode != null)
        //    {
        //        return errorcode;
        //    }
        //    return output;
        //}

        /// <summary>
        /// This Method is for getting the Member Reward error code by providing non existing card id
        /// </summary>
        /// <param name="loyalityID">Loyalty id of the member</param>
        /// <param name="reward">Reward id of the member</param>
        /// <returns>error code</returns>
        public Object AddMemberRewardswithNonExistingCard(string loyalityID, RewardCatalogSummaryStruct reward)
        {
            string errorcode = null;
            AddMemberRewardsOut output = null;
            string cardIdentity = "";
            try
            {
                RewardOrderInfoStruct[] order = new RewardOrderInfoStruct[1];
                order[0] = new RewardOrderInfoStruct();
                order[0].RewardName = reward.RewardName;
                order[0].TypeCode = reward.TypeCode;
                cardIdentity = common.RandomNumber(13);
                output = ProjectTestBase.lWIntegrationSvcClientManager.AddMemberRewards(loyalityID, cardIdentity, null, null, null, null, null, null, null, null, null, null, null, null, "Web", "CDISAutomation", order, string.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }

        /// <summary>
        /// This Method is for getting the Member Reward error code by providing non existing reward name
        /// </summary>
        /// <param name="loyalityID">Loyalty id of the member</param>
        /// <param name="reward">Reward id of the member</param>
        /// <returns>error code</returns>
        public Object AddMemberRewardswithNonExistingRewardName(string loyalityID, RewardCatalogSummaryStruct reward)
        {
            string errorcode = null;
            AddMemberRewardsOut output = null;
            reward.RewardName = common.RandomString(12);
            try
            {
                RewardOrderInfoStruct[] order = new RewardOrderInfoStruct[1];
                order[0] = new RewardOrderInfoStruct();
                order[0].RewardName = reward.RewardName;
                order[0].TypeCode = reward.TypeCode;
                output = ProjectTestBase.lWIntegrationSvcClientManager.AddMemberRewards(loyalityID, loyalityID, null, null, null, null, null, null, null, null, null, null, null, null, "Web", "CDISAutomation", order, string.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }

        /// <summary>
        /// This Method is for getting the Member Reward error code by providing blank reward name
        /// </summary>
        /// <param name="loyalityID">Loyalty id of the member</param>
        /// <param name="reward">Reward id of the member</param>
        /// <returns>error code</returns>
        //public Object AddMemberRewardswithBlankRewardName(string loyalityID, RewardCatalogSummaryStruct reward)
        //{
        //    string errorcode = null;
        //    AddMemberRewardsOut output = null;
        //    reward.RewardName = "";
        //    try
        //    {
        //        RewardOrderInfoStruct[] order = new RewardOrderInfoStruct[1];
        //        order[0] = new RewardOrderInfoStruct();
        //        order[0].RewardName = reward.RewardName;
        //        order[0].TypeCode = reward.TypeCode;
        //        output = ProjectTestBase.lWIntegrationSvcClientManager.AddMemberRewards(loyalityID, loyalityID, null, null, null, null, null, null, null, null, null, null, null, null, "Web", "CDISAutomation", order, string.Empty, out time);
        //    }
        //    catch (LWClientException e)
        //    {
        //        Logger.Info(e.ErrorCode);
        //        errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Info(e.Message);
        //        Logger.Info(e.GetType());
        //    }
        //    if (errorcode != null)
        //    {
        //        return errorcode;
        //    }
        //    return output;
        //}

        /// <summary>
        /// This Method is for getting the Member Reward error code by providing blank reward info
        /// </summary>
        /// <param name="loyalityID">Loyalty id of the member</param>
        /// <param name="reward">Reward id of the member</param>
        /// <returns>error code</returns>
        public Object AddMemberRewardswithBlankRewardInfo(string loyalityID, RewardCatalogSummaryStruct reward)
        {
            string errorcode = null;
            AddMemberRewardsOut output = null;
            RewardOrderInfoStruct[] order;
            try
            {
                if (reward != null)
                {
                    order = new RewardOrderInfoStruct[1];
                    order[0] = new RewardOrderInfoStruct();
                    order[0].RewardName = reward.RewardName;
                    order[0].TypeCode = reward.TypeCode;
                }
                else
                {
                    order = new RewardOrderInfoStruct[1];
                    order[0] = new RewardOrderInfoStruct();
                    order[0].RewardName = "";
                    order[0].CertificateNumber = "";
                    order[0].TypeCode = "";
                    order[0].ExpirationDate = null;
                    order[0].VariantPartNumber = "";
                }
                output = ProjectTestBase.lWIntegrationSvcClientManager.AddMemberRewards(loyalityID, loyalityID, null, null, null, null, null, null, null, null, null, null, null, null, "Web", "CDISAutomation", order, string.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }

        /// <summary> 
        /// To apply credit to a member's account 
        /// </summary> 
        /// <param name="memberidentity"></param> 
        /// <param name="cardid"></param> 
        /// <param name="txnheaderid"></param> 
        /// <param name="note"></param> 
        /// <returns></returns> 
        public decimal ApplyTxnCredit(string memberidentity, string cardid, string txnheaderid, string note, out double time)
        {
            decimal pointsEarned = ProjectTestBase.lWIntegrationSvcClientManager.ApplyTxnCredit(memberidentity, cardid, txnheaderid, note, System.Guid.NewGuid().ToString(), out time);
            return pointsEarned;
        }

        /// <summary> 
        /// To apply credit to a member's account with Invalid data 
        /// </summary> 
        /// <param name="memberidentity"></param> 
        /// <param name="cardid"></param> 
        /// <param name="txnheaderid"></param> 
        /// <param name="note"></param> 
        /// <returns></returns> 
        public string ApplyTxnCreditInvalid(string memberidentity, string cardid, string txnheaderid, string note)
        {
            string message = "";
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.ApplyTxnCredit(memberidentity, cardid, txnheaderid, note, System.Guid.NewGuid().ToString(), out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                message = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return message;
        }

        /// <summary>
        /// Authenticate Member invalid case
        /// </summary>
        /// <param name="identityType"></param>
        /// <param name="identity"></param>
        /// <param name="password"></param>
        /// <param name="resetCode"></param>
        /// <returns></returns>
        public string AuthenticateMemberInvalid(string identityType, string identity, string password, string resetCode)
        {
            string message = "";
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.AuthenticateMember(identityType, identity, password, resetCode, string.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                message = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return message;
        }


        /// <summary>
        /// Negative method for GetCsNotes to fetch error codes
        /// </summary>
        /// <param name="memberidentity"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public string getCsNotesNegative(string memberidentity, DateTime? startDate, DateTime? endDate)
        {
            string message = "";
            double elapsedTime = 0;
            try
            {
                var getCsNotes = ProjectTestBase.lWIntegrationSvcClientManager.GetCSNotes(memberidentity, startDate, endDate, string.Empty, out elapsedTime);
                message = getCsNotes[0].Note;
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                message = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return message;
        }



        /// <summary>
        /// To create CsNote for a member
        /// </summary>
        /// <param name="memberidentity"></param>
        /// <param name="note"></param>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        public long CreateCsNote(string memberidentity, string note, out double elapsedTime)
        {
            long value = ProjectTestBase.lWIntegrationSvcClientManager.CreateCSNote(memberidentity, note, 0, System.Guid.NewGuid().ToString(), out elapsedTime);
            return value;
        }

        /// <summary>
        /// This method is used to get csnotes of a member
        /// </summary>
        /// <param name="memberidentity"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        public CSNoteStruct[] GetCsNotes(string memberidentity, DateTime? startDate, DateTime? endDate, out double elapsedTime)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetCSNotes(memberidentity, startDate, endDate, string.Empty, out elapsedTime);
        }


        /// <summary>
        /// To create CsNote for a member with invalid data
        /// </summary>
        /// <param name="memberidentity"></param>
        /// <param name="note"></param>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        public string CreateCsNoteInvalid(string memberidentity, string note, out double elapsedTime)
        {
            string message = "";
            elapsedTime = 0;
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.CreateCSNote(memberidentity, note, 0, System.Guid.NewGuid().ToString(), out elapsedTime);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                message = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return message;
        }

        /// <summary>
        /// To award Loyalty currency for a member
        /// </summary>
        /// <param name="LoyaltyNumber"></param>
        /// <param name="eventType"></param>
        /// <param name="currencyType"></param>
        /// <param name="amount"></param>
        /// <param name="transactiondate"></param>
        /// <param name="expirationdate"></param>
        /// <param name="note"></param>
        /// <param name="changedby"></param>
        /// <returns></returns>
        public AwardLoyaltyCurrencyOut AwardLoyaltyCurrency(string LoyaltyNumber, LoyaltyEvents eventType, LoyaltyCurrency currencyType, decimal? amount, DateTime? transactiondate, DateTime expirationdate, string note, string changedby)
        {

            return ProjectTestBase.lWIntegrationSvcClientManager.AwardLoyaltyCurrency(LoyaltyNumber, eventType.ToString(), currencyType.ToString(), amount, DateTime.Now.AddDays(-1), expirationdate, null, "CDISService", string.Empty, out time);
        }

        /// <summary>
        /// To award Loyalty currency for a member with invalid data
        /// </summary>
        /// <param name="LoyaltyNumber"></param>
        /// <param name="eventType"></param>
        /// <param name="currencyType"></param>
        /// <param name="amount"></param>
        /// <param name="transactiondate"></param>
        /// <param name="expirationdate"></param>
        /// <param name="note"></param>
        /// <param name="changedby"></param>
        /// <returns></returns>
        public string AwardLoyaltyCurrencyInvalid(string LoyaltyNumber, LoyaltyEvents eventType, LoyaltyCurrency currencyType, decimal? amount, DateTime? transactiondate, DateTime expirationdate, string note, string changedby)
        {

            string message = "";
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.AwardLoyaltyCurrency(LoyaltyNumber, eventType.ToString(), currencyType.ToString(), amount, DateTime.Now.AddDays(-1), expirationdate, null, "CDISService", string.Empty, out time);

            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                message = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return message;
        }

        /// <summary>
        /// To get Tier Names
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string[] GetTierNames(out double time)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetTierNames(string.Empty, out time);
        }

        /// <summary>
        /// To get Tiers information
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public TierDefStruct[] GetTiers(out double time)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetTiers(string.Empty, out time);
        }

        /// <summary>
        /// To get Message definitions with invalid data
        /// </summary>
        /// <param name="messagedefid"></param>
        /// <param name="language"></param>
        /// <param name="channel"></param>
        /// <param name="returnattributes"></param>
        /// <param name="externalId"></param>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        public string GetMessageDefinition_Invalid(long messagedefid, string language, string channel, bool? returnattributes)
        {
            string message = "";
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.GetMessageDefinition(messagedefid, language, channel, returnattributes, string.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                message = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return message;
        }

        /// <summary>
        /// To get Message definition
        /// </summary>
        /// <param name="messagedefid"></param>
        /// <param name="language"></param>
        /// <param name="channel"></param>
        /// <param name="returnattributes"></param>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        public MessageDefinitionStruct GetMessageDefinition(long messagedefid, string language, string channel, bool? returnattributes, out double elapsedTime)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetMessageDefinition(messagedefid, language, channel, returnattributes, string.Empty, out elapsedTime);
        }

        /// <summary>
        /// To get Message definitions with invalid data
        /// </summary>
        /// <param name="messagedefid"></param>
        /// <param name="language"></param>
        /// <param name="channel"></param>
        /// <param name="returnattributes"></param>
        /// <param name="externalId"></param>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        /// </summary>
        /// <param name="language"></param>
        /// <param name="channel"></param>
        /// <param name="contentsearchattributes"></param>
        /// <param name="activecouponoptions"></param>
        /// <param name="returnattributes"></param>
        /// <param name="pagenumber"></param>
        /// <param name="resultsperpage"></param>
        /// <returns></returns>
        public string GetCouponDefinitions_Invalid(string language, string channel, ContentSearchAttributesStruct[] contentsearchattributes, ActiveCouponOptionsStruct activecouponoptions, bool? returnattributes, int? pagenumber, int? resultsperpage)
        {
            string message = "";
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.GetCouponDefinitions(language, channel, contentsearchattributes, activecouponoptions, returnattributes, pagenumber, resultsperpage, String.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                message = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return message;
        }

        /// <summary>
        /// To add Rewards to a member
        /// </summary>
        /// <param name="loyalityID"></param>
        /// <param name="CardID"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="emailaddress"></param>
        /// <param name="addresslineone"></param>
        /// <param name="addresslinetwo"></param>
        /// <param name="addresslinethree"></param>
        /// <param name="addresslinefour"></param>
        /// <param name="city"></param>
        /// <param name="stateorprovince"></param>
        /// <param name="ziporpostalcode"></param>
        /// <param name="county"></param>
        /// <param name="country"></param>
        /// <param name="channel"></param>
        /// <param name="changedby"></param>
        /// <param name="reward"></param>
        /// <returns></returns>
        public Object AddMemberRewards(string loyalityID, string CardID, string firstname, string lastname, string emailaddress, string addresslineone, string addresslinetwo, string addresslinethree, string addresslinefour, string city, string stateorprovince, string ziporpostalcode, string county, string country, string channel, string changedby, RewardCatalogSummaryStruct reward)
        {
            string errorcode = null;
            AddMemberRewardsOut output = null;
            try
            {
                RewardOrderInfoStruct[] order = new RewardOrderInfoStruct[1];
                if (reward == null)
                {
                    order = null;
                }
                else if (reward.RewardName.Equals("blank"))
                {
                    order = new RewardOrderInfoStruct[1];
                    order[0] = new RewardOrderInfoStruct();
                    order[0].RewardName = "";
                    order[0].CertificateNumber = "";
                    order[0].TypeCode = "";
                    order[0].ExpirationDate = null;
                    order[0].VariantPartNumber = "";
                }
                else
                {
                    order[0] = new RewardOrderInfoStruct();
                    order[0].RewardName = reward.RewardName;
                    order[0].TypeCode = reward.TypeCode;
                }
                output = ProjectTestBase.lWIntegrationSvcClientManager.AddMemberRewards(loyalityID, CardID, firstname, lastname, emailaddress, addresslineone, addresslinetwo, addresslinethree, addresslinefour, city, stateorprovince, ziporpostalcode, county, country, channel, changedby, order, string.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                errorcode = "Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            if (errorcode != null)
            {
                return errorcode;
            }
            return output;
        }

        /// <summary>
        /// To get all rewards orders shipping address
        /// </summary>
        /// <param name="memberidentity"></param>
        public ShippingAddressStruct[] GetAllRewardOrderShippingAddresses(string memberidentity)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetAllRewardOrderShippingAddresses(memberidentity, String.Empty, out time);
        }

        /// <summary>
        /// To get all reward orders shipping Addresses with invalid data
        /// </summary>
        /// <param name="memberidentity"></param>
        /// <returns></returns>
        public string GetAllRewardOrderShippingAddresses_Invalid(string memberidentity)
        {
            string message = "";
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.GetAllRewardOrderShippingAddresses(memberidentity, String.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                message = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return message;
        }

        /// <summary>
        /// To set Shipping address for Reward Orders
        /// </summary>
        /// <returns></returns>
        public Hashtable SetRewardOrderShippingAddress()
        {
            Hashtable addressData = new Hashtable();
            addressData.Add("addresslineone", "testAdressLine1");
            addressData.Add("addresslinetwo", "testAddressLine2");
            addressData.Add("addresslinethree", "testAddressLine3");
            addressData.Add("addresslinefour", "testAddressLine4");
            addressData.Add("city", "testCity");
            addressData.Add("state", "testState");
            addressData.Add("ziporpostalcode", "121121");
            addressData.Add("county", "testCounty");
            addressData.Add("country", "testCountry");
            return addressData;
        }

        /// <summary>
        /// Get Service Info
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public GetServiceInfoOut GetServiceInfo(out double time)
        {
            return ProjectTestBase.lWIntegrationSvcClientManager.GetServiceInfo(string.Empty, out time);
        }

        /// <summary>
        /// This method is used for member return
        /// </summary>
        /// <param name="ordernumber"></param>
        /// <param name="time"></param>
        /// <returns>Returns null if it is executed successfully otherwise returns the message</returns>
        public string ReturnMemberRewardOrder(string ordernumber, out double time)
        {
            string status = "fail";
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.ReturnMemberRewardOrder(ordernumber, false, string.Empty, out time);
                status = "pass";
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                time = -1;
                status = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                time = -1;
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return status;
        }

        /// <summary>
        /// To convert a non member to a member
        /// </summary>
        /// <param name="memberidentity"></param>
        /// <param name="effectivedate"></param>
        /// <param name="externalId"></param>
        /// <param name="time"></param>
        public string ConvertToMember(string memberidentity, DateTime? effectivedate, out double time)
        {
            string message = "";
            time = 0;
            try
            {
                ProjectTestBase.lWIntegrationSvcClientManager.ConvertToMember(memberidentity, effectivedate, string.Empty, out time);
            }
            catch (LWClientException e)
            {
                Logger.Info(e.ErrorCode);
                message = "Received an Exception Error code=" + e.ErrorCode + ";Error Message=" + e.Message;
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                Logger.Info(e.GetType());
            }
            return message;
        }
    }
}
