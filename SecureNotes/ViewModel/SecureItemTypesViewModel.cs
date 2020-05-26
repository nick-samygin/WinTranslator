using PasswordBoss.Helpers;
using PasswordBoss.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecureItemsCommon.ViewModels;
using PasswordBoss.DTO;
using System.Windows.Media;
using PasswordBoss;

namespace SecureNotes.ViewModel
{
    public class AlarmCodeSecureItemViewModel: SecureItemWithPasswordViewModel
    {
        private string code;
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                RaisePropertyChanged("Code");
            }
        }

        private string alarmCompany;
        public string AlarmCompany
        {
            get { return alarmCompany; }
            set
            {
                alarmCompany = value;
                RaisePropertyChanged("AlarmCompany");
            }
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                RaisePropertyChanged("PhoneNumber");
            }
        }

        public AlarmCodeSecureItemViewModel()
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_AlarmCode;
        }

        public AlarmCodeSecureItemViewModel(SecureItem item, Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {

            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_AlarmCode;

            Code = item.Data.alarm_code;
            AlarmCompany = item.Data.alarm_company;
            PhoneNumber = item.Data.phoneNumber;

        }

        public AlarmCodeSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PI_PhoneNumber;


        }

        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.Data.alarm_code = Code;
            secureItem.Data.phoneNumber = PhoneNumber;
            secureItem.Data.alarm_company = AlarmCompany;

            return secureItem;
        }

    }

    public class DriversLicenseSecureItemViewModel : SecureItemWithCountryViewModel
    {
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                RaisePropertyChanged("FirstName");
            }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                RaisePropertyChanged("LastName");
            }
        }

        private string number;
        public string Number
        {
            get { return number; }
            set
            {
                number = value;
                ListViewSecondName = value;
                RaisePropertyChanged("Number");
            }
        }

        private string expires;
        public string Expires
        {
            get { return expires; }
            set
            {
                expires = value;
                RaisePropertyChanged("Expires");
            }
        }

        public string[] States
        {
            get
            {
                return new string[]
                {
                    "Alabama" ,
                    "Alaska" ,
                    "American Samoa" ,
                    "Arizona" ,
                    "Arkansas" ,
                    "California" ,
                    "Colorado" ,
                    "Connecticut" ,
                    "Delaware" ,
                    "District Of Columbia" ,
                    "Federated States Of Micronesia" ,
                    "Florida" ,
                    "Georgia" ,
                    "Guam" ,
                    "Hawaii" ,
                    "Idaho" ,
                    "Illinois" ,
                    "Indiana" ,
                    "Iowa" ,
                    "Kansas" ,
                    "Kentucky" ,
                    "Louisiana" ,
                    "Maine" ,
                    "Marshall Islands" ,
                    "Maryland" ,
                    "Massachusetts" ,
                    "Michigan" ,
                    "Minnesota" ,
                    "Mississippi" ,
                    "Missouri" ,
                    "Montana" ,
                    "Nebraska" ,
                    "Nevada" ,
                    "New Hampshire" ,
                    "New Jersey" ,
                    "New Mexico" ,
                    "New York" ,
                    "North Carolina" ,
                    "North Dakota" ,
                    "Northern Mariana Islands" ,
                    "Ohio" ,
                    "Oklahoma" ,
                    "Oregon" ,
                    "Palau" ,
                    "Pennsylvania" ,
                    "Puerto Rico" ,
                    "Rhode Island" ,
                    "South Carolina" ,
                    "South Dakota" ,
                    "Tennessee" ,
                    "Texas" ,
                    "Utah" ,
                    "Vermont" ,
                    "Virgin Islands" ,
                    "Virginia" ,
                    "Washington" ,
                    "West Virginia" ,
                    "Wisconsin" ,
                    "Wyoming"
                };
            }
        }


        private string state;
        public string State
        {
            get { return state; }
            set
            {
                state = value;
                RaisePropertyChanged("State");
            }
        }


        public DriversLicenseSecureItemViewModel()
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_DriverLicense;
        }

        public DriversLicenseSecureItemViewModel(SecureItem item, Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {

            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_DriverLicense;

            FirstName = item.Data.firstName;
            LastName = item.Data.lastName;
            Number = item.Data.driverLicenceNumber;
            Expires = item.Data.expires;
            State = item.Data.state;
        }


        public DriversLicenseSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_DriverLicense;


        }

        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.Data.firstName= FirstName;
            secureItem.Data.lastName= LastName;
            secureItem.Data.driverLicenceNumber=Number;
            secureItem.Data.expires= Expires;
            secureItem.Data.state= State;
            secureItem.Data.country = SelectedCountry!=null ? SelectedCountry.Code : null;

            return secureItem;
        }


    }

    public class EstatePlanSecureItemViewModel : SecureItemViewModel
    {
        private string documentsLocation;
        public string DocumentsLocation
        {
            get { return documentsLocation; }
            set
            {
                documentsLocation = value;
                RaisePropertyChanged("DocumentsLocation");
            }
        }

        private string attorney;
        public string Attorney
        {
            get { return attorney; }
            set
            {
                attorney = value;
                RaisePropertyChanged("Attorney");
            }
        }

        private string executor;
        public string Executor
        {
            get { return executor; }
            set
            {
                executor = value;
                RaisePropertyChanged("Executor");
            }
        }

        private string trustee;
        public string Trustee
        {
            get { return trustee; }
            set
            {
                trustee = value;
                RaisePropertyChanged("Trustee");
            }
        }

        public EstatePlanSecureItemViewModel()
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_EstatePlan;
        }

        public EstatePlanSecureItemViewModel(SecureItem item, Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {

            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_EstatePlan;

            DocumentsLocation = item.Data.location_of_documents;
            Attorney = item.Data.attorney;
            Executor = item.Data.executor;
            Trustee = item.Data.trustee;
        }


        public EstatePlanSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_EstatePlan;


        }

        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.Data.location_of_documents= DocumentsLocation;
            secureItem.Data.attorney= Attorney;
            secureItem.Data.executor= Executor;
            secureItem.Data.trustee= Trustee;

            return secureItem;
        }


    }

    public class FrequentFlyerSecureItemViewModel : SecureItemViewModel
    {
        private string airline;
        public string Airline
        {
            get { return airline; }
            set
            {
                airline = value;
                RaisePropertyChanged("Airline");
            }
        }

        private string frequentFlyerNumber;
        public string FrequentFlyerNumber
        {
            get { return frequentFlyerNumber; }
            set
            {
                frequentFlyerNumber = value;
                ListViewSecondName = value;
                RaisePropertyChanged("FrequentFlyerNumber");
            }
        }

        private string statusLevel;
        public string StatusLevel
        {
            get { return statusLevel; }
            set
            {
                statusLevel = value;
                RaisePropertyChanged("StatusLevel");
            }
        }

        private string airlinePhoneNumber;
        public string AirlinePhoneNumber
        {
            get { return airlinePhoneNumber; }
            set
            {
                airlinePhoneNumber = value;
                RaisePropertyChanged("AirlinePhoneNumber");
            }
        }

        public FrequentFlyerSecureItemViewModel()
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_FrequentFlyer;
        }

        public FrequentFlyerSecureItemViewModel(SecureItem item, Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {

            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_FrequentFlyer;

            Airline = item.Data.airline;
            FrequentFlyerNumber = item.Data.frequent_flyer_number;
            StatusLevel = item.Data.status_level;
            AirlinePhoneNumber = item.Data.phoneNumber;
        }

        public FrequentFlyerSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_FrequentFlyer;


        }


        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.Data.airline = Airline;
            secureItem.Data.frequent_flyer_number = FrequentFlyerNumber;
            secureItem.Data.status_level = StatusLevel;
            secureItem.Data.phoneNumber = AirlinePhoneNumber;

            return secureItem;
        }

    }

    public class NoteSecureItemViewModel : SecureItemViewModel
    {
        public NoteSecureItemViewModel()
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_GenericNote;
            HasNote = true;
        }

        public NoteSecureItemViewModel(SecureItem item, Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {

            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_GenericNote;
            HasNote = true;
        }

        public NoteSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_GenericNote;
            HasNote = true;

        }

    }

        public class HealthInsuranceSecureItemViewModel : SecureItemViewModel
        {
            private string insuranceCompany;
            public string InsuranceCompany
            {
                get { return insuranceCompany; }
                set
                {
                    insuranceCompany = value;
                    RaisePropertyChanged("InsuranceCompany");
                }
            }

            private string memberID;
            public string MemberID
            {
                get { return memberID; }
                set
                {
                    memberID = value;
                    ListViewSecondName = value;
                    RaisePropertyChanged("MemberID");
                }
            }

            private string groupNumber;
            public string GroupNumber
            {
                get { return groupNumber; }
                set
                {
                    groupNumber = value;
                    RaisePropertyChanged("GroupNumber");
                }
            }

            private string prescriptionPlan;
            public string PrescriptionPlan
            {
                get { return prescriptionPlan; }
                set
                {
                    prescriptionPlan = value;
                    RaisePropertyChanged("PrescriptionPlan");
                }
            }

            public HealthInsuranceSecureItemViewModel()
            {
                type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
                subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_HealthInsurance;
            }

            public HealthInsuranceSecureItemViewModel(SecureItem item, Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
            {

                type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
                subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_HealthInsurance;

                InsuranceCompany = item.Data.insurance_company;
                MemberID = item.Data.member_id;
                GroupNumber = item.Data.group_number;
                PrescriptionPlan = item.Data.prescription_plan;
            }


        public HealthInsuranceSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_HealthInsurance;

        }

        public override SecureItem CreateSecureItem()
            {
                var secureItem = base.CreateSecureItem();

                secureItem.Data.insurance_company = InsuranceCompany;
                secureItem.Data.member_id = MemberID;
                secureItem.Data.group_number = GroupNumber;
                secureItem.Data.prescription_plan = PrescriptionPlan;

                return secureItem;
            }

        }

        public class HotelRewardsSecureItemViewModel : SecureItemWithCountryViewModel
        {

            private string hotel;
            public string Hotel
            {
                get { return hotel; }
                set
                {
                    hotel = value;
                    RaisePropertyChanged("Hotel");
                }
            }

            private string membershipNumber;
            public string MembershipNumber
            {
                get { return membershipNumber; }
                set
                {
                    membershipNumber = value;
                    ListViewSecondName = value;
                    RaisePropertyChanged("MembershipNumber");
                }
            }

            private string statusLevel;
            public string StatusLevel
            {
                get { return statusLevel; }
                set
                {
                    statusLevel = value;
                    RaisePropertyChanged("StatusLevel");
                }
            }

            private string phoneNumber;
            public string PhoneNumber
            {
                get { return phoneNumber; }
                set
                {
                    phoneNumber = value;
                    RaisePropertyChanged("PhoneNumber");
                }
            }

            public HotelRewardsSecureItemViewModel()
            {
                type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
                subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_HotelRewards;
            }

            public HotelRewardsSecureItemViewModel(SecureItem item, Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
            {

                type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
                subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_HotelRewards;

                Hotel = item.Data.hotel;
                MembershipNumber = item.Data.membership_number;
                StatusLevel = item.Data.status_level;
                PhoneNumber = item.Data.phoneNumber;
            }

        public HotelRewardsSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_HotelRewards;

        }

        public override SecureItem CreateSecureItem()
            {
                var secureItem = base.CreateSecureItem();

                secureItem.Data.hotel = Hotel;
                secureItem.Data.membership_number = membershipNumber;
                secureItem.Data.status_level = StatusLevel;
                secureItem.Data.phoneNumber = PhoneNumber;

                return secureItem;
            }

        }

        public class InsuranceSecureItemViewModel : SecureItemViewModel
        {
            private string insuranceType;
            public string InsuranceType
            {
                get { return insuranceType; }
                set
                {
                    insuranceType = value;
                    RaisePropertyChanged("InsuranceType");
                }
            }

            private string insuranceCompany;
            public string InsuranceCompany
            {
                get { return insuranceCompany; }
                set
                {
                    insuranceCompany = value;
                    RaisePropertyChanged("InsuranceCompany");
                }
            }

            private string policyNumber;
            public string PolicyNumber
            {
                get { return policyNumber; }
                set
                {
                    policyNumber = value;
                    ListViewSecondName = value;
                    RaisePropertyChanged("PolicyNumber");
                }
            }

            private string agent;
            public string Agent
            {
                get { return agent; }
                set
                {
                    agent = value;
                    RaisePropertyChanged("Agent");
                }
            }

            private string phoneNumber;
            public string PhoneNumber
            {
                get { return phoneNumber; }
                set
                {
                    phoneNumber = value;
                    RaisePropertyChanged("PhoneNumber");
                }
            }


            private string deductible;
            public string Deductible
            {
                get { return deductible; }
                set
                {
                    deductible = value;
                    RaisePropertyChanged("Deductible");
                }
            }

            private string renewalDate;
            public string RenewalDate
            {
                get { return renewalDate; }
                set
                {
                    renewalDate = value;
                    RaisePropertyChanged("RenewalDate");
                }
            }

            public InsuranceSecureItemViewModel()
            {
                type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
                subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_Insurance;
            }

            public InsuranceSecureItemViewModel(SecureItem item, Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
            {

                type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
                subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_Insurance;

                InsuranceType = item.Data.type;
                InsuranceCompany = item.Data.insurance_company;
                PolicyNumber = item.Data.policy_number;
                Agent = item.Data.agent;
                PhoneNumber = item.Data.agent_phone_number;
                RenewalDate = item.Data.renewal_date;
            }

        public InsuranceSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_Insurance;

        }
        public override SecureItem CreateSecureItem()
            {
                var secureItem = base.CreateSecureItem();

                secureItem.Data.type = InsuranceType;
                secureItem.Data.insurance_company = InsuranceCompany;
                secureItem.Data.policy_number = PolicyNumber;
                secureItem.Data.agent = Agent;
                secureItem.Data.agent_phone_number = PhoneNumber;
                secureItem.Data.renewal_date = RenewalDate;

                return secureItem;
            }

        }

        public class MemberIDSecureItemViewModel : SecureItemViewModel
        {
            private string firstName;
            public string FirstName
            {
                get { return firstName; }
                set
                {
                    firstName = value;
                    RaisePropertyChanged("FirstName");
                }
            }

            private string lastName;
            public string LastName
            {
                get { return lastName; }
                set
                {
                    lastName = value;
                    RaisePropertyChanged("LastName");
                }
            }

            private string memberID;
            public string MemberID
            {
                get { return memberID; }
                set
                {
                    memberID = value;
                    ListViewSecondName = value;
                    RaisePropertyChanged("MemberID");
                }
            }


            public MemberIDSecureItemViewModel()
            {
                type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
                subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_MemberIDs;
            }

            public MemberIDSecureItemViewModel(SecureItem item, Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
            {

                type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
                subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_MemberIDs;

                FirstName = item.Data.firstName;
                LastName = item.Data.lastName;
                MemberID = item.Data.memberID;
            }

        public MemberIDSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_MemberIDs;

        }

        public override SecureItem CreateSecureItem()
            {
                var secureItem = base.CreateSecureItem();

                secureItem.Data.firstName = FirstName;
                secureItem.Data.lastName = LastName;
                secureItem.Data.memberID = MemberID;

                return secureItem;
            }

        }

        public class PassportCodeSecureItemViewModel : SecureItemWithCountryViewModel
        {
            private string firstName;
            public string FirstName
            {
                get { return firstName; }
                set
                {
                    firstName = value;
                    RaisePropertyChanged("FirstName");
                }
            }

            private string lastName;
            public string LastName
            {
                get { return lastName; }
                set
                {
                    lastName = value;
                    RaisePropertyChanged("LastName");
                }
            }


            private string number;
            public string Number
            {
                get { return number; }
                set
                {
                    number = value;
                    ListViewSecondName = value;
                    RaisePropertyChanged("Number");
                }
            }

            private Country passportSelectedNationality;
            public Country PassportSelectedNationality
            {
                get
                {
                    if (passportSelectedNationality == null && itemDataCountry != null && Countries != null)
                        passportSelectedNationality = Countries.SingleOrDefault(x => x.Code == itemDataCountry);

                    return passportSelectedNationality;
                }
                set
                {
                    if (value!=null && passportSelectedNationality!=null)
                    {
                        if (passportSelectedNationality.Code != value.Code)
                        {
                            passportSelectedNationality = value;
                            RaisePropertyChanged("PassportSelectedNationality");
                        }
                    }
                    else
                    {
                        passportSelectedNationality = value;
                        RaisePropertyChanged("PassportSelectedNationality");
                    }
                }
            }


            private string issueDate;
            public string IssueDate
            {
                get { return issueDate; }
                set
                {
                    issueDate = value;
                    RaisePropertyChanged("IssueDate");
                }
            }

            private string expires;
            public string Expires
            {
                get { return expires; }
                set
                {
                    expires = value;
                    RaisePropertyChanged("Expires");
                }
            }

            private string dateOfBirth;
            public string DateOfBirth
            {
                get { return dateOfBirth; }
                set
                {
                    dateOfBirth = value;
                    RaisePropertyChanged("DateOfBirth");
                }
            }

            private string placeofIssue;
            public string PlaceofIssue
            {
                get { return placeofIssue; }
                set
                {
                    placeofIssue = value;
                    RaisePropertyChanged("PlaceofIssue");
                }
            }

            public PassportCodeSecureItemViewModel()
            {
                type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
                subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_Passport;
            }

            public PassportCodeSecureItemViewModel(SecureItem item, Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
            {

                type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
                subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_Passport;

                FirstName = item.Data.firstName;
                LastName = item.Data.lastName;
                Number = item.Data.passportNumber;
                itemDataCountry = item.Data.nationality;
                DateOfBirth = item.Data.dateOfBirth;
                IssueDate = item.Data.issueDate;
                Expires = item.Data.expires;
                PlaceofIssue = item.Data.placeOfIssue;
            }

        public PassportCodeSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_Passport;

        }

        public override SecureItem CreateSecureItem()
            {
                var secureItem = base.CreateSecureItem();

                secureItem.Data.firstName = FirstName;
                secureItem.Data.lastName = LastName;
                secureItem.Data.passportNumber = Number;
                secureItem.Data.nationality = PassportSelectedNationality!=null ? PassportSelectedNationality.Code : null;
                secureItem.Data.dateOfBirth = DateOfBirth;
                secureItem.Data.issueDate = IssueDate;
                secureItem.Data.expires = Expires;
                secureItem.Data.placeOfIssue = PlaceofIssue;
                return secureItem;
            }

        }

        public class PrescriptionSecureItemViewModel : SecureItemViewModel
        {
            private string medicine;
            public string Medicine
            {
                get { return medicine; }
                set
                {
                    medicine = value;
                    RaisePropertyChanged("Medicine");
                }
            }

            private string doctor;
            public string Doctor
            {
                get { return doctor; }
                set
                {
                    doctor = value;
                    RaisePropertyChanged("Doctor");
                }
            }


            private string doctorPhone;
            public string DoctorPhone
            {
                get { return doctorPhone; }
                set
                {
                    doctorPhone = value;
                    RaisePropertyChanged("DoctorPhone");
                }
            }

            private string pharmacy;
            public string Pharmacy
            {
                get { return pharmacy; }
                set
                {
                    pharmacy = value;
                    RaisePropertyChanged("Pharmacy");
                }
            }


            private string pharmacyPhone;
            public string PharmacyPhone
            {
                get { return pharmacyPhone; }
                set
                {
                    pharmacyPhone = value;
                    RaisePropertyChanged("PharmacyPhone");
                }
            }

            private string prescriptionNumber;
            public string PrescriptionNumber
            {
                get { return prescriptionNumber; }
                set
                {
                    prescriptionNumber = value;
                    ListViewSecondName = value;
                    RaisePropertyChanged("PrescriptionNumber");
                }
            }


            private string refills;
            public string Refills
            {
                get { return refills; }
                set
                {
                    refills = value;
                    RaisePropertyChanged("Refills");
                }
            }

            public PrescriptionSecureItemViewModel()
            {
                type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
                subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_Prescription;
            }

            public PrescriptionSecureItemViewModel(SecureItem item, Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
            {

                type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
                subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_Prescription;

                Medicine = item.Data.medicine;
                Doctor = item.Data.doctor;
                DoctorPhone = item.Data.doctor_phone;
                Pharmacy = item.Data.pharmacy;
                PharmacyPhone = item.Data.pharmacy_phone;
                PrescriptionNumber = item.Data.prescription_number;
                Refills = item.Data.refills;
            }

        public PrescriptionSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_Prescription;

        }

        public override SecureItem CreateSecureItem()
            {
                var secureItem = base.CreateSecureItem();

                secureItem.Data.medicine = Medicine;
                secureItem.Data.doctor = Doctor;
                secureItem.Data.doctor_phone = DoctorPhone;
                secureItem.Data.pharmacy = Pharmacy;
                secureItem.Data.pharmacy_phone = PharmacyPhone;
                secureItem.Data.refills = Refills;
                secureItem.Data.prescription_number = PrescriptionNumber;
                return secureItem;
            }

        }

        public class SocialSecuritySecureItemViewModel : SecureItemViewModel
        {
            private string firstName;
            public string FirstName
            {
                get { return firstName; }
                set
                {
                    firstName = value;
                    RaisePropertyChanged("FirstName");
                }
            }

            private string lastName;
            public string LastName
            {
                get { return lastName; }
                set
                {
                    lastName = value;
                    RaisePropertyChanged("LastName");
                }
            }


            private string number;
            public string Number
            {
                get { return number; }
                set
                {
                    number = value;
                    ListViewSecondName = value;
                    RaisePropertyChanged("Number");
                }
            }

            private string dateOfBirth;
            public string DateOfBirth
            {
                get { return dateOfBirth; }
                set
                {
                    dateOfBirth = value;
                    RaisePropertyChanged("DateOfBirth");
                }
            }

            public SocialSecuritySecureItemViewModel()
            {
                type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
                subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_SocialSecurity;
            }

            public SocialSecuritySecureItemViewModel(SecureItem item, Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
            {

                type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
                subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_SocialSecurity;

                FirstName = item.Data.firstName;
                LastName = item.Data.lastName;
                Number = item.Data.ssn;
                DateOfBirth = item.Data.dateOfBirth;
            }

        public SocialSecuritySecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_SocialSecurity;

        }


        public override SecureItem CreateSecureItem()
            {
                var secureItem = base.CreateSecureItem();

                secureItem.Data.firstName = FirstName;
                secureItem.Data.lastName = LastName;
                secureItem.Data.ssn = Number;
                return secureItem;
            }
        }

        public class SoftwareLicenseSecureItemViewModel : SecureItemViewModel
        {
            private string licenseKey;
            public string LicenseKey
            {
                get { return licenseKey; }
                set
                {
                    licenseKey = value;
                    RaisePropertyChanged("LicenseKey");
                }
            }

            private string version;
            public string Version
            {
                get { return version; }
                set
                {
                    version = value;
                    RaisePropertyChanged("Version");
                }
            }


            private string publisher;
            public string Publisher
            {
                get { return publisher; }
                set
                {
                    publisher = value;
                    RaisePropertyChanged("Publisher");
                }
            }

            private string price;
            public string Price
            {
                get { return price; }
                set
                {
                    price = value;
                    RaisePropertyChanged("Price");
                }
            }

            private string purchaseDate;
            public string PurchaseDate
            {
                get { return purchaseDate; }
                set
                {
                    purchaseDate = value;
                    RaisePropertyChanged("PurchaseDate");
                }
            }

            private string supportThrough;
            public string SupportThrough
            {
                get { return supportThrough; }
                set
                {
                    supportThrough = value;
                    RaisePropertyChanged("SupportThrough");
                }
            }


            private string orderNumber;
            public string OrderNumber
            {
                get { return orderNumber; }
                set
                {
                    orderNumber = value;
                    RaisePropertyChanged("OrderNumber");
                }
            }

            private string numberOfLicenses;
            public string NumberOfLicenses
            {
                get { return numberOfLicenses; }
                set
                {
                    numberOfLicenses = value;
                    RaisePropertyChanged("NumberOfLicenses");
                }
            }

            public SoftwareLicenseSecureItemViewModel()
            {
                type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
                subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_SoftwareLicense;
            }

            public SoftwareLicenseSecureItemViewModel(SecureItem item, Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
            {

                type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
                subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_SoftwareLicense;

                LicenseKey = item.Data.license_key;
                Version = item.Data.version;
                Publisher = item.Data.publisher;
                Price = item.Data.price;

                PurchaseDate = item.Data.purchase_date;
                SupportThrough = item.Data.support_through;
                OrderNumber = item.Data.order_number;
                NumberOfLicenses = item.Data.number_of_licenses;
            }

        public SoftwareLicenseSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_SecureNotes;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_SN_SoftwareLicense;

        }

        public override SecureItem CreateSecureItem()
            {
                var secureItem = base.CreateSecureItem();

                secureItem.Data.license_key = LicenseKey;
                secureItem.Data.version = Version;
                secureItem.Data.publisher = Publisher;
                secureItem.Data.price = Price;
                secureItem.Data.purchase_date = PurchaseDate;
                secureItem.Data.support_through = SupportThrough;
                secureItem.Data.order_number = OrderNumber;
                secureItem.Data.number_of_licenses = NumberOfLicenses;
                return secureItem;
            }
        }
    
}
